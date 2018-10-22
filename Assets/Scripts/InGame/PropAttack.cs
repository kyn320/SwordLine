using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropAttack : MonoBehaviour
{

    protected WeaponBehaviour weapon;
    public Prop prop;

    protected float currentCoolTime;

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


    public virtual void StartCoolTime()
    {

        currentCoolTime = prop.levelToCoolTime[weapon.propLevel];

        if (coolTimeTimer != null)
        {
            StopCoroutine(coolTimeTimer);
        }
        coolTimeTimer = StartCoroutine(CoolTimeTimer());

    }

    Coroutine coolTimeTimer;

    public virtual IEnumerator CoolTimeTimer()
    {
        while (currentCoolTime > 0)
        {
            currentCoolTime -= Time.deltaTime;
            yield return null;
        }

        currentCoolTime = 0;
    }

    public virtual bool RandomToPercent()
    {
        float rand = Random.Range(0f, 1f) * 100;
        float successPercent = prop.levelToPercent[weapon.propLevel];

        if (rand <= successPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual float OperateDamage(float _damage)
    {
        return _damage;
    }

}
