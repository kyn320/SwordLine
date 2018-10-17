using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBulletAttack : AIAttack
{

    public string bulletObjectName;

    public float bulletSpeed = 1f;
    public Vector3 aimPosition;

    public override void Attack()
    {
        if (attackCurrentTime > 0)
            return;

        attackCurrentTime = attackTime;
        //ani.SetTrigger("Attack");
        monster.UpdateState(MonsterState.Attack, true);

        GameObject g = ObjectPoolManager.Instance.Get(bulletObjectName);
        g.transform.position = transform.position;

        Vector3 dir = transform.position - aimPosition;
        g.GetComponent<Bullet>().SetBullet(dir.normalized, bulletSpeed);

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
