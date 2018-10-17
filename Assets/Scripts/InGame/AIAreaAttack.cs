using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAreaAttack : AIAttack
{
    public string areaObjectName;

    public float castTime;

    public override void Attack()
    {
        if (attackCurrentTime > 0)
            return;

        attackCurrentTime = attackTime;
        //ani.SetTrigger("Attack");
        monster.UpdateState(MonsterState.Attack, true);

        GameObject g = ObjectPoolManager.Instance.Get(areaObjectName);
        g.transform.position = transform.position;

        g.GetComponent<AreaAttack>().Cast(castTime, attackTime);

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
