using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBulletAttack : AIAttack
{
    [Header("투사체 오브젝트")]
    public string bulletObjectName;
    [Header("투사체 이동 속도")]
    public float bulletSpeed = 1f;

    public override void Attack()
    {
        if (attackCurrentTime > 0)
            return;

        attackCurrentTime = attackTime;
        //ani.SetTrigger("Attack");
        monster.UpdateState(MonsterState.Attack, true);

        GameObject g = ObjectPoolManager.Instance.Get(bulletObjectName);
        g.transform.position = transform.position;

        Vector3 dir = target.position - transform.position;
        g.GetComponent<Bullet>().SetBullet(dir.normalized, bulletSpeed, Damage);

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

    public override void Damage(GameObject _object)
    {
        PlayerBehaviour player = _object.GetComponent<PlayerBehaviour>();
        player.Damage(monster.GetDamage());
        player.KnockBack(knockBackPower, player.GetDirectionToVector3(transform.position));
    }



}
