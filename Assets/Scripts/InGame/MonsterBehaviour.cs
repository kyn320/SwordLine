﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : MonoBehaviour
{

    public int hp;
    public MonsterState state;


    private MonsterAI ai;
    private Rigidbody2D ri;


    private void Awake()
    {
        ri = GetComponent<Rigidbody2D>();
        ai = GetComponent<MonsterAI>();
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
        state = MonsterState.Death;
        //TODO :: 사망 판정 구현

    }
    #endregion


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