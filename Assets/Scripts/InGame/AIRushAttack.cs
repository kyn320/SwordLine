using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRushAttack : AIAttack
{
    [Header("돌진 속도")]
    public float rushSpeed = 10f;

    protected Animator ani;

    //public Transform attackDirection;
    //public AttackCollider attackCollider;

    protected override void Awake()
    {
        base.Awake();
        //ani = attackCollider.GetComponent<Animator>();
    }

    private void Start()
    {
        //attackCollider.damageAction += Damage;
    }

    public void SetRotate()
    {
        Vector3 rot = target.position - transform.position;
        rot.Normalize();

        float degree = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
        //attackDirection.localRotation = Quaternion.Euler(0, 0, degree);
    }

    public override void Attack()
    {
        if (attackCurrentTime > 0)
            return;

        attackCurrentTime = attackTime;
        //attackCollider.SetAttack(true);
        monster.UpdateState(MonsterState.Attack, true);

        if (rush != null)
        {
            StopCoroutine(rush);
        }
        rush = StartCoroutine(Rush());

    }

    Coroutine rush = null;

    public IEnumerator Rush()
    {
        Vector3 rushPosition = target.position;

        while ((rushPosition - transform.position).sqrMagnitude > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, rushPosition, Time.deltaTime * rushSpeed);
            yield return null;
        }

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
        //attackCollider.SetAttack(false);
        attackCurrentTime = 0;
    }

    public override void Damage(GameObject _object)
    {
        PlayerBehaviour player = _object.GetComponent<PlayerBehaviour>();
        player.Damage(monster.GetDamage());
        player.KnockBack(knockBackPower, player.GetDirectionToVector3(transform.position));
    }

}
