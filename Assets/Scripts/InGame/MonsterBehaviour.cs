using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : MonoBehaviour
{
    [Header("공격력")]
    public int damage;

    [Header("현재 체력")]
    public int hp;
    [Header("최대 체력")]
    public int maxHP;

    [Header("몬스터 크기")]
    public MonsterSizeType sizeType;
    [Header("몬스터 상태")]
    public MonsterState state;

    [Header("이동 속도")]
    public float moveSpeed = 1f;

    [Header("무적 상태")]
    public bool isSuperPower = false;

    [Header("렌더링 오브젝트")]
    public GameObject monsterRenderer;
    private SpriteRenderer spriteRenderer;

    private Animator ani;
    private MonsterAI ai;
    private Rigidbody2D ri;
    private AIAttack aiAttack;

    [Header("데미지 텍스트")]
    public string damageTextPrefab;

    private string damageEffect;

    private void Awake()
    {
        ri = GetComponent<Rigidbody2D>();
        ai = GetComponent<MonsterAI>();
        ani = monsterRenderer.GetComponent<Animator>();
        spriteRenderer = monsterRenderer.GetComponent<SpriteRenderer>();
        aiAttack = GetComponent<AIAttack>();
    }

    private void Start()
    {
        ai.SetMoveSpeed(moveSpeed);
        hp = maxHP;
    }

    public void UpdateState(MonsterState _state, bool _isOn = true)
    {
        switch (_state)
        {
            case MonsterState.Idle:
                state = MonsterState.Idle;
                //TODO :: LED 초록색으로 변경
                break;
            case MonsterState.Trace:
                state = MonsterState.Trace;
                ri.velocity = Vector2.zero;
                ai.StartMovement();
                //TODO :: LED 빨간색으로 변경
                break;
            case MonsterState.Attack:
                if (_isOn)
                {
                    ri.velocity = Vector2.zero;
                    ai.StopMovement();
                    state = MonsterState.Attack;
                    //TODO :: 공격 구현
                    Attack();
                }
                else
                {
                    state = MonsterState.Idle;
                }
                break;
            case MonsterState.Damage:
                state = MonsterState.Damage;
                break;
            case MonsterState.Hacking:
                state = MonsterState.Hacking;
                ai.StopMovement();
                switch (sizeType)
                {
                    case MonsterSizeType.Normal:
                        Hacking(2f);
                        break;
                    case MonsterSizeType.Boss:
                        Hacking(1f);
                        break;
                }
                //TODO :: 스킬 사용 불가능
                //TODO :: LED 보라색으로 변경
                //TODO :: 해킹 이펙트 출력
                break;
            case MonsterState.Reset:
                state = MonsterState.Reset;
                //TODO ::  ??
                break;
            case MonsterState.End:
                state = MonsterState.End;
                isSuperPower = false;
                ai.SetMoveSpeed(moveSpeed);
                UpdateState(MonsterState.Idle);
                break;
            case MonsterState.Return:
                state = MonsterState.Return;
                //TODO :: 최초 스폰 장소로 귀환
                //TODO :: 도착 할때까지 무적 상태, 이동속도 2배
                hp = maxHP;
                isSuperPower = true;
                ai.SetMoveSpeed(moveSpeed * 2f);
                break;
            case MonsterState.Death:
                state = MonsterState.Death;
                ai.StopMovement();
                Death();
                //TODO :: 사망 애니메이션 출력
                //TODO :: 3초 동안 불투명도 (Alpha) n 만큼 값 하락
                //TODO :: 불투명도가 0이 되면 삭제
                break;
        }

    }

    #region 공격
    public void Attack()
    {
        //TODO ::  공격 구현
        aiAttack.SetTarget(ai.target);
        aiAttack.Attack();
        //ani.SetTrigger("Attack");
    }


    public int GetDamage()
    {
        return damage;
    }

    #endregion

    #region HP 제어
    public void Heal(int _value)
    {
        hp += _value;
    }

    public void Damage(int _value)
    {
        if (isSuperPower)
            return;

        UpdateState(MonsterState.Damage);

        hp -= _value;

        GameObject g = ObjectPoolManager.Instance.Get(damageTextPrefab);
        g.transform.position = transform.position;
        g.GetComponent<UIDamageText>().SetText(_value.ToString(), Color.white);


        if (hp < 1)
            UpdateState(MonsterState.Death);
    }

    public void Death()
    {
        //TODO :: 사망 판정 구현
        StartCoroutine(DeathEffect(3f));
    }
    #endregion

    #region 넉백효과
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
        ri.constraints = RigidbodyConstraints2D.FreezeRotation;
        ai.StopMovement();

        ri.velocity = Vector2.zero;
        ri.drag = _power * 0.8f;
        ri.AddForce(_power * _dir, ForceMode2D.Impulse);

        while (ri.velocity.sqrMagnitude > 0.1f)
        {
            yield return null;
        }

        ri.drag = 0f;
        ri.velocity = Vector2.zero;
        knockBack = null;
        ri.constraints = RigidbodyConstraints2D.FreezeAll;

        if (state == MonsterState.Damage)
        {
            UpdateState(MonsterState.Trace);
            ai.RestartMovement();
        }
    }

    #endregion

    #region 해킹효과

    public void Hacking(float _hackingTime)
    {
        print("Hacking");

        if (hacking != null)
            StopCoroutine(hacking);

        hacking = StartCoroutine(HackingEffect(_hackingTime));
    }

    Coroutine hacking = null;

    IEnumerator HackingEffect(float _hackingTime)
    {
        float hackingTime = _hackingTime;

        while (hackingTime > 0)
        {

            hackingTime -= Time.deltaTime;
            yield return null;
        }

        //TODO ::  일반 상태로 전환 
        UpdateState(MonsterState.Idle);
    }

    #endregion

    IEnumerator DeathEffect(float _time)
    {
        float time = _time;
        Color color = spriteRenderer.color;

        while (time > 0)
        {
            color.a -= (1 / _time);
            time -= Time.deltaTime;
            spriteRenderer.color = color;
            yield return null;
        }

        gameObject.SetActive(false);
    }


}

public enum MonsterState
{
    /// <summary>
    /// 비전투 상태
    /// </summary>
    Idle,
    /// <summary>
    /// 인식 범위에 들어와 추적하는 상태
    /// </summary>
    Trace,
    /// <summary>
    /// 공격 범위에 들어와 공격하는 상태
    /// </summary>
    Attack,
    /// <summary>
    /// 공격 받는 상태
    /// </summary>
    Damage,
    /// <summary>
    /// 해킹 당한 상태
    /// </summary>
    Hacking,
    /// <summary>
    /// 추적거리를 넘어선 상태
    /// </summary>
    Reset,
    /// <summary>
    /// 전투 종료 상태
    /// </summary>
    End,
    /// <summary>
    /// 초기 위치로 이동하는 상태
    /// </summary>
    Return,
    /// <summary>
    /// 사망 상태
    /// </summary>
    Death
}

public enum MonsterSizeType
{
    /// <summary>
    /// 일반 몬스터
    /// </summary>
    Normal,
    /// <summary>
    /// 보스 몬스터
    /// </summary>
    Boss
}