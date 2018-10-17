using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropHologram : PropAttack
{
    //TODO :: 홀로그램 구현 방식에 대해...
    //TODO :: 공격 후 0.15초 이후에 N% 로 공격이 한번 더 들어감

    public override void Attack(MonsterBehaviour _monster)
    {
        base.Attack(_monster);
        StartCoroutine(HologramEffect(_monster));
    }

    IEnumerator HologramEffect(MonsterBehaviour _monster)
    {
        float hologramTime = 0.15f;
        yield return new WaitForSeconds(hologramTime);
        float rand = Random.Range(0f, 1f) * 100;
        float successPercent = prop.levelToPercent[weapon.propLevel];

        if (rand <= successPercent)
        {
            base.Attack(_monster);
            _monster.Damage((int)(weapon.OperateDamage() * prop.levelToMultipleDamage[weapon.propLevel]));
        }
    }


}
