using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropHacking : PropAttack
{

    public override void Attack(MonsterBehaviour _monster)
    {
        _monster.UpdateState(MonsterState.Hacking);
        base.Attack(_monster);
    }
    
}
