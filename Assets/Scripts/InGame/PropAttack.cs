using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropAttack : MonoBehaviour
{

    protected WeaponBehaviour weapon;
    public Prop prop;

    protected virtual void Awake()
    {
        weapon = GetComponent<WeaponBehaviour>();
    }

    protected virtual void Start()
    {

    }

    public virtual void Attack(MonsterBehaviour _monster)
    {
        GameObject g = ObjectPoolManager.Instance.Get(prop.effectName);
        g.transform.position = _monster.transform.position;
    }

    public virtual float OperateDamage(float _damage)
    {
        return _damage;
    }

}
