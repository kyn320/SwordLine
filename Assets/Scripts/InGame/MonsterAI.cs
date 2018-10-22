using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : EntityAI
{
    private MonsterBehaviour monster;

    [Header("추적 가능 거리")]
    public int traceDistance;
    [Header("공격 가능 거리")]
    public float attackDistance;
    [Header("기존 스폰 장소")]
    public Vector3 orignSpawnPoint;

    protected override void Awake()
    {
        base.Awake();
        monster = GetComponent<MonsterBehaviour>();
    }

    protected override void Start()
    {
        base.Start();
        orignSpawnPoint = transform.position;
        sightChecker.sightEnterWithGameObject += SightEnter;
        sightChecker.sightExitWithGameObject += SightExit;
    }

    Coroutine movement = null;

    IEnumerator MoveToOriginSpawnPoint()
    {
        countPath.FindPath(transform, orignSpawnPoint);

        while (monster.state == MonsterState.Return)
        {
            if (monster.state == MonsterState.Damage)
            {
                break;
            }

            yield return null;
            if ((orignSpawnPoint - transform.position).sqrMagnitude <= 0.1f)
            {
                monster.UpdateState(MonsterState.End);
                break;
            }
        }

        movement = null;
    }

    IEnumerator UpdateTraceWithTarget()
    {
        while (target != null)
        {
            if (monster.state == MonsterState.Damage || monster.state == MonsterState.Hacking || monster.state == MonsterState.Death)
            {
                break;
            }

            float targetDistance = (target.position - transform.position).sqrMagnitude;

//                      print("거리 = " + targetDistance);

            if (monster.state == MonsterState.Attack)
            {
                yield return null;
                continue;
//                if (targetDistance > attackDistance * attackDistance)
//                {
//                    //                    print("공격 이후 재 추격");
//                    monster.UpdateState(MonsterState.Trace);
//                    RestartMovement();
//                }
            }

            if (target != null)
                countPath.FindPath(transform, target.position);

            if (countPath.GetPathLenght() > traceDistance)
            {
                //추적
                //                print("추적 해제");
                monster.UpdateState(MonsterState.Return);
                StopMovement();
                target = null;
                StartMovementToOriginSpawnPoint();
                break;
            }

            if (targetDistance <= attackDistance * attackDistance)
            {
                monster.UpdateState(MonsterState.Attack);
                //               print("공격 거리 진입");
            }

            yield return new WaitForSeconds(intervalTime);
        }

        movement = null;
    }

    public void RestartMovement()
    {
        countPath.RestartMovement();
    }

    public override void StartMovement()
    {
        if (movement != null)
            StopCoroutine(movement);

        movement = StartCoroutine(UpdateTraceWithTarget());
    }

    public void StartMovementToOriginSpawnPoint()
    {
        if (movement != null)
            StopCoroutine(movement);

        movement = StartCoroutine(MoveToOriginSpawnPoint());
    }

    public override void StopMovement()
    {
        base.StopMovement();
    }

    public void SightEnter(GameObject _gameObject)
    {
        //TODO :: 인식 범위내 들어옴을 알림

        target = _gameObject.transform;
        monster.UpdateState(MonsterState.Trace);
    }

    public void SightExit(GameObject _gameObject)
    {
        //if (target != null && monster.state != MonsterState.Damage)
        //{
        //    StopMovement();
        //}
    }

}
