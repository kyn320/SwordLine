using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropOverDrive : PropAttack
{
    
    public override float OperateDamage(float _damage)
    {
        return _damage * prop.levelToMultipleDamage[weapon.propLevel];
    }

        
}
