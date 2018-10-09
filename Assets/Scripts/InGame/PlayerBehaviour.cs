using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public PlayerState state;

    PlayerController controller;
    Rigidbody2D ri;

    public int hp;
    public float attackDamage;
    public float attackSpeed;
    public float moveSpeed;

    public Vector3 dir;
    public Vector3 lookDir;

    public GameObject afterImageEffect;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        ri = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (state != PlayerState.Stand && state != PlayerState.Move)
            return;

        state = PlayerState.Move;

        ri.velocity = dir * moveSpeed;
    }

    #region HP 제어
    public void Heal(int _value)
    {
        hp += _value;
    }

    public void Damage(int _value)
    {
        hp -= _value;

        if (hp < 1)
            Death();
    }

    public void Death()
    {
        state = PlayerState.Death;
        //TODO :: 사망 판정 구현

    }
    #endregion

    public void SetDirection(Vector3 _dir)
    {
        if (_dir != Vector3.zero)
            lookDir = _dir;

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
        state = PlayerState.Stand;
        evasionEffect = null;

    }
    #endregion


    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (state == PlayerState.Evasion)
            state = PlayerState.Stand;
    }


    public Vector3 GetDirectionToVector3(Vector3 _pos)
    {
        return (transform.position - _pos).normalized;
    }

}

[System.Serializable]
public enum PlayerState
{
    Stand,
    Move,
    Attack,
    Evasion,
    Death
}