using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropHacking : PropAttack
{

    public override void Attack(MonsterBehaviour _monster)
    {
        if (currentCoolTime > 0)
            return;

        base.Attack(_monster);
        _monster.UpdateState(MonsterState.Hacking);

        StartCoolTime();
    }

}
