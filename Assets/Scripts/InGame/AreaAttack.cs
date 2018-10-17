using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    public int damage;

    public float castCurrentWaitTime;
    public float castWaitTime;

    public float attackCurrentWaitTime;
    public float attackWaitTime;

    public bool isStayAttack = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Cast(float _castTime, float _attackTime)
    {
        castWaitTime = _castTime;
        attackWaitTime = _attackTime;

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

        attackCurrentWaitTime = 0;
        attackWaitTimer = null;

        //TODO :: 파괴
        ObjectPoolManager.Instance.Free(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        //TODO :: 데미지 피해 입히기
    }

    private void OnTriggerStay2D(Collider2D _collision)
    {
        if (!isStayAttack)
            return;

        //TODO :: 데미지 피해 입히기
    }

}
