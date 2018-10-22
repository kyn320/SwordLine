using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    protected MonsterBehaviour monster;

    protected Coroutine attackWaitTimer = null;

    protected float attackCurrentTime;
    [Header("공격 시간")]
    public float attackTime = 1;
    [Header("타겟팅 된 오브젝트")]
    public Transform target;
    [Header("넉백 파워")]
    public float knockBackPower = 10;

    protected virtual void Awake()
    {
        monster = GetComponent<MonsterBehaviour>();
    }

    public virtual void Attack()
    {

    }

    public virtual void SetTarget(Transform _target)
    {
        target = _target;
    }

    public virtual IEnumerator AttackWaitTimer()
    {
        yield return null;
    }

    public virtual void Damage(GameObject _object)
    {

    }



}
