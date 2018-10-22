using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAreaAttack : AIAttack
{
    [Header("범위기 오브젝트")]
    public string areaObjectName;
    [Header("캐스팅 시간")]
    public float castTime = 1;

    [Header("타겟을 추적하는 공격")]
    public bool isTargetAttack = false;

    public override void Attack()
    {
        if (attackCurrentTime > 0)
            return;

        attackCurrentTime = attackTime;
        //ani.SetTrigger("Attack");
        monster.UpdateState(MonsterState.Attack, true);

        GameObject g = ObjectPoolManager.Instance.Get(areaObjectName);

        if (isTargetAttack)
            g.transform.position = target.position;
        else
            g.transform.position = transform.position;

        g.GetComponent<AreaAttack>().Cast(castTime, attackTime, Damage);

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
