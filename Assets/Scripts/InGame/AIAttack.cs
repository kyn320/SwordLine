using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    protected MonsterBehaviour monster;

    protected Coroutine attackWaitTimer = null;

    public float attackCurrentTime;
    public float attackTime;

    protected virtual void Awake()
    {
        monster = GetComponent<MonsterBehaviour>();
    }

    public virtual void Attack()
    {

    }

    public virtual IEnumerator AttackWaitTimer()
    {
        yield return null;
    }





}
