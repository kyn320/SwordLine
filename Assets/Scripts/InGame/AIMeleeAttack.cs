using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeAttack : AIAttack
{

    public override void Attack()
    {
        if (attackCurrentTime > 0)
            return;

        attackCurrentTime = attackTime;
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
        
        while (attackCurrentTime > 0)
        {
            attackCurrentTime -= Time.deltaTime;
            yield return null;
        }

        monster.UpdateState(MonsterState.Attack, false);
        attackCurrentTime = 0;
    }

}
