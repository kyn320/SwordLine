using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public PlayerState state;

    private BoxCollider2D footCollider;
    private PlayerController controller;
    private Rigidbody2D ri;
    private Animator ani;

    public int hp;
    public float attackDamage;
    public float attackSpeed;
    public float moveSpeed;

    public Vector3 dir;
    public Vector3 lookDir;

    public GameObject playerRenderer;
    public GameObject afterImageEffect;

    private void Awake()
    {
        footCollider = GetComponent<BoxCollider2D>();
        controller = GetComponent<PlayerController>();
        ri = GetComponent<Rigidbody2D>();
        ani = playerRenderer.GetComponent<Animator>();
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

    public void UpdateState(PlayerState _state, bool _isOn = true)
    {
        switch (_state)
        {
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
    }

    public void Damage(int _value)
    {
        hp -= _value;
        ani.SetTrigger("NuckBack");
        if (hp < 1)
            Death();
    }

    public void Death()
    {
        state = PlayerState.Death;
        ani.SetTrigger("Death");
        //TODO :: 사망 판정 구현

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
        footCollider.isTrigger = true;
        afterImageEffect.SetActive(true);
        ani.SetBool("Dash", true);
        float time = _time;

        while (time >= 0 && state == PlayerState.Evasion)
        {
            ri.velocity = lookDir * moveSpeed * 2f;
            time -= Time.deltaTime;
            yield return null;
        }

        ri.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.2f);
        footCollider.isTrigger = false;
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
        ri.velocity = Vector2.zero;
        ri.drag = _power * 0.7f;
        ri.AddForce(_power * _dir, ForceMode2D.Impulse);

        while (ri.velocity.sqrMagnitude > 0.1f)
        {
            yield return null;
        }

        ri.drag = 0f;
        ri.velocity = Vector2.zero;
        knockBack = null;

    }
    #endregion

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
            controller.isInput = true;
            ani.SetBool("Dash", false);
            state = PlayerState.Idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (state == PlayerState.Evasion && _collision.CompareTag("Fall"))
        {
            //TODO::낙사 액션 구현
            print("Player is Falling.. Death");
            state = PlayerState.Death;
            ani.SetTrigger("Fall");
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

}

[System.Serializable]
public enum PlayerState
{
    Idle,
    Move,
    KnockBack,
    Attack,
    Evasion,
    Death
}