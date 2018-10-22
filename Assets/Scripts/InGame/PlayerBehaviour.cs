using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("플레이어 상태")]
    public PlayerState state;

    private BoxCollider2D footCollider;
    private PlayerController controller;
    private Rigidbody2D ri;
    private Animator ani;
    private SightChecker sightChecker;

    [HideInInspector]
    public PropBehaviour propBehaviour;

    [Header("현재 체력")]
    public int hp;
    [Header("최대 체력")]
    public int maxHp;

    [Header("현재 스테미너")]
    public int sp;
    [Header("최대 스테미너")]
    public int maxSp;

    [Header("이동 속도")]
    public float moveSpeed;

    [Header("입력 방향")]
    public Vector3 dir;
    [Header("시점 방향")]
    public Vector3 lookDir;

    [Header("렌더러 오브젝트")]
    public GameObject playerRenderer;
    [Header("잔상 이펙트")]
    public GameObject afterImageEffect;
    [Header("발 먼지 이펙트")]
    public string footStepDustEffectPrefab;
    [Header("발 위치 트랜스폼")]
    public Transform footTransform;
    [Header("발 먼지 생성 거리 비율")]
    public float stepDistance;


    private Vector3 oldPosition;

    private float evasionDistance = 1f;

    [Header("데미지 텍스트")]
    public string damageTextPrefab;
    [Header("피격 이펙트")]
    public string damageEffect;

    private void Awake()
    {
        footCollider = GetComponent<BoxCollider2D>();
        controller = GetComponent<PlayerController>();
        ri = GetComponent<Rigidbody2D>();
        sightChecker = GetComponentInChildren<SightChecker>();
        ani = playerRenderer.GetComponent<Animator>();
        propBehaviour = GetComponent<PropBehaviour>();
    }

    private void Start()
    {
        hp = maxHp;
        StartCoroutine(DustEffect());
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (state != PlayerState.Idle && state != PlayerState.Move)
        {
            ani.SetBool("Move", false);
            return;
        }

        state = PlayerState.Move;
        ani.SetBool("Move", true);
        ri.velocity = dir * moveSpeed;
    }

    IEnumerator DustEffect()
    {
        while (true)
        {
            if ((oldPosition - transform.position).sqrMagnitude >= stepDistance)
            {
                GameObject g = ObjectPoolManager.Instance.Get(footStepDustEffectPrefab);
                oldPosition = g.transform.position = footTransform.position;
            }
            yield return null;
        }
    }

    public void UpdateState(PlayerState _state, bool _isOn = true)
    {
        switch (_state)
        {
            case PlayerState.Interactive:
                state = PlayerState.Interactive;
                break;

            case PlayerState.Attack:
                if (_isOn)
                {
                    ri.velocity = Vector2.zero;
                    state = PlayerState.Attack;
                }
                else
                {
                    state = PlayerState.Idle;
                }
                ani.SetBool("Attack", _isOn);
                break;
        }

    }

    #region HP 제어
    public void Heal(int _value)
    {
        hp += _value;

        hp = Mathf.Clamp(hp, 0, maxHp);
    }

    public void Damage(int _value)
    {
        hp -= _value;

        ani.SetTrigger("NuckBack");

        GameObject g = ObjectPoolManager.Instance.Get(damageTextPrefab);
        g.transform.position = transform.position;
        g.GetComponent<UIDamageText>().SetText(_value.ToString(), Color.red);

        g = ObjectPoolManager.Instance.Get(damageEffect);
        g.transform.position = transform.position;

        if (hp < 1)
            Death();
    }

    public void Death()
    {
        state = PlayerState.Death;
        ri.velocity = Vector2.zero;
        ani.SetTrigger("Death");
        controller.isInput = false;
        //최근 세이브 포인트로 이동
        transform.position = SavePointManager.Instance.GetSavePoint().transform.position;
        Heal(10);

    }
    #endregion

    public void SetDirection(Vector3 _dir)
    {
        if (_dir != Vector3.zero && state != PlayerState.Attack)
        {
            lookDir = _dir;
            ani.SetFloat("X", lookDir.x);

            if (lookDir.x < 0)
                playerRenderer.transform.localScale = new Vector3(-1, 1, 1);
            else
                playerRenderer.transform.localScale = new Vector3(1, 1, 1);

            ani.SetFloat("Y", lookDir.y);
        }
        else
        {
            if (state == PlayerState.Move)
                state = PlayerState.Idle;

            ani.SetBool("Move", false);
        }

        dir = _dir;

    }

    #region 회피 효과

    public int CheckEvasion(float _evasionDistance)
    {
        //Debug.DrawRay(footTransform.position + lookDir * _evasionDistance, Vector3.forward, Color.red, 1f);
        RaycastHit2D hit = Physics2D.Raycast(footTransform.position + lookDir * _evasionDistance, Vector3.forward, 3f, LayerMask.GetMask("UnWalkable"));
        if (hit.collider != null)
        {
            print(hit.collider.gameObject.tag);
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                print("Wall Evasion");
                return 1;
            }
            else if (hit.collider.gameObject.CompareTag("Fall"))
            {
                print("Fall evaion");
                return 2;
            }
            else
            {
                print("ground evasion");
                return 0;
            }
        }
        return 0;
    }

    public void Evasion(float _time)
    {
        state = PlayerState.Evasion;

        if (evasionEffect != null)
        {
            StopCoroutine(evasionEffect);
        }

        evasionEffect = StartCoroutine(EvasionEffect(_time));
    }

    Coroutine evasionEffect = null;

    IEnumerator EvasionEffect(float _time)
    {
        afterImageEffect.SetActive(true);
        ani.SetBool("Dash", true);
        float time = _time;
        int evasionChecker = -1;
        while (time >= 0 && state == PlayerState.Evasion)
        {

            evasionChecker = CheckEvasion(evasionDistance);

            if (evasionChecker == 2)
            {
                footCollider.isTrigger = true;
            }
            else
                footCollider.isTrigger = false;

            ri.velocity = lookDir * moveSpeed * 2f;
            time -= Time.deltaTime;
            yield return null;
        }

        ri.velocity = Vector3.zero;

        print("evasion end");

        if (CheckEvasion(0) == 2)
        {
            Fall();
        }

        yield return new WaitForSeconds(0.2f);
        controller.isInput = true;
        ani.SetBool("Dash", false);
        state = PlayerState.Idle;
        evasionEffect = null;

    }
    #endregion

    #region 넉백 효과
    public void KnockBack(float _power, Vector3 _dir)
    {

        if (knockBack != null)
        {
            StopCoroutine(knockBack);
        }

        knockBack = StartCoroutine(KnockBackEffect(_power, _dir));

    }

    Coroutine knockBack = null;

    IEnumerator KnockBackEffect(float _power, Vector3 _dir)
    {
        state = PlayerState.KnockBack;
        print("knockBack");
        controller.isInput = false;
        ri.velocity = Vector2.zero;
        ri.drag = _power * 0.8f;

        ri.AddForce(_power * _dir, ForceMode2D.Impulse);

        while (ri.velocity.sqrMagnitude > 0.1f)
        {
            yield return null;
        }

        ri.drag = 0f;
        ri.velocity = Vector2.zero;
        controller.isInput = true;
        state = PlayerState.Idle;
        knockBack = null;

    }
    #endregion

    private void Fall()
    {
        print("Player is Falling.. Death");
        ri.velocity = Vector3.zero;
        state = PlayerState.Death;
        ani.SetTrigger("Fall");
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (state == PlayerState.Evasion)
        {
            if (evasionEffect != null)
            {
                StopCoroutine(evasionEffect);
                evasionEffect = null;
            }

            afterImageEffect.SetActive(false);
            ri.velocity = Vector3.zero;
            if (CheckEvasion(0) == 2)
            {
                print("collisoin");
                Fall();
                return;
            }
            controller.isInput = true;
            ani.SetBool("Dash", false);
            state = PlayerState.Idle;

            return;
        }

    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (state == PlayerState.Evasion && _collision.CompareTag("Fall"))
        {
            //TODO::낙사 액션 구현
            print("trigger");
            Fall();
            return;
        }

    }

    public Vector3 GetDirectionToVector3(Vector3 _pos)
    {
        return (transform.position - _pos).normalized;
    }

    public Animator GetAnimator()
    {
        return ani;
    }

    public SightChecker GetSightChecker()
    {
        return sightChecker;
    }

}

[System.Serializable]
public enum PlayerState
{
    Idle,
    Move,
    KnockBack,
    Attack,
    Evasion,
    Interactive,
    Death
}