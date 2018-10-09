using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public PlayerState state;

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
        if (_dir != Vector3.zero)
        {
            lookDir = _dir;
            ani.SetFloat("X", lookDir.x);
            ani.SetFloat("Y", lookDir.y);
        }
        else
        {
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
        controller.isInput = true;
        ani.SetBool("Dash", false);
        state = PlayerState.Idle;
        evasionEffect = null;

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
    Attack,
    Evasion,
    Death
}