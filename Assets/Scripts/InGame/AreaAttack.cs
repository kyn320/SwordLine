using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaAttack : MonoBehaviour
{
    private AttackCollider[] attackCollider;
    private Animator ani;
    private SpriteRenderer spriteRenderer;

    [Header("공격력")]
    public int damage;

    private float castCurrentWaitTime;
    [Header("캐스팅 시간")]
    public float castWaitTime;

    private float attackCurrentWaitTime;
    [Header("공격 시간")]
    public float attackWaitTime;

    UnityAction<GameObject> damageAction;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackCollider = GetComponentsInChildren<AttackCollider>();
    }

    public void Cast(float _castTime, float _attackTime, UnityAction<GameObject> _damageAction)
    {
        castWaitTime = _castTime;
        attackWaitTime = _attackTime;

        for (int i = 0; i < attackCollider.Length; ++i)
        {
            attackCollider[i].damageAction = _damageAction;
        }

        ani.SetTrigger("Cast");

        if (castWaitTimer != null)
            StopCoroutine(castWaitTimer);

        castWaitTimer = StartCoroutine(CastWaitTimer());
    }

    Coroutine castWaitTimer = null;

    IEnumerator CastWaitTimer()
    {
        castCurrentWaitTime = castWaitTime;

        while (castCurrentWaitTime > 0)
        {
            castCurrentWaitTime -= Time.deltaTime;
            yield return null;
        }

        castCurrentWaitTime = 0;
        castWaitTimer = null;
        Attack();
    }

    public void Attack()
    {
        SetAttack(true);
        ani.SetTrigger("Attack");

        if (castWaitTimer != null)
            StopCoroutine(attackWaitTimer);

        attackWaitTimer = StartCoroutine(AttackWaitTimer());


    }

    Coroutine attackWaitTimer;

    IEnumerator AttackWaitTimer()
    {
        attackCurrentWaitTime = attackWaitTime;

        while (attackCurrentWaitTime > 0)
        {
            attackCurrentWaitTime -= Time.deltaTime;
            yield return null;
        }

        SetAttack(false);
        attackCurrentWaitTime = 0;
        attackWaitTimer = null;

        //TODO :: 파괴
        ObjectPoolManager.Instance.Free(this.gameObject);
    }

    public void SetAttack(bool _isAttack)
    {
        for (int i = 0; i < attackCollider.Length; ++i)
        {
            attackCollider[i].SetAttack(_isAttack);
        }
    }

}
