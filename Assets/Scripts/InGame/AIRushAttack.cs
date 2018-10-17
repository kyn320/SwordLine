using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRushAttack : AIAttack
{
    public float rushSpeed = 10f;
    public Vector3 rushPosition;

    public override void Attack()
    {
        if (attackCurrentTime > 0)
            return;

        //ani.SetTrigger("Attack");
        monster.UpdateState(MonsterState.Attack, true);

        if (attackWaitTimer != null)
        {
            StopCoroutine(attackWaitTimer);
        }
        attackWaitTimer = StartCoroutine(AttackWaitTimer());

    }

    public override IEnumerator AttackWaitTimer()
    {
        attackCurrentTime = 1;

        while ((transform.position - rushPosition).sqrMagnitude < 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, rushPosition, Time.deltaTime * rushSpeed);
            yield return null;
        }

        monster.UpdateState(MonsterState.Attack, false);
        attackCurrentTime = 0;
    }



}
