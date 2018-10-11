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

    public float intervalTime = 0.4f;

    [Header("타겟")]
    public Transform target;

    protected override void Awake()
    {
        base.Awake();
        monster = GetComponent<MonsterBehaviour>();
    }

    protected override void Start()
    {
        base.Start();
        sightChecker.sightEnterWithGameObject += SightEnter;
        sightChecker.sightExitWithGameObject += SightExit;
    }

    IEnumerator UpdateTrace() {

        while (target != null) {
            countPath.FindPath(transform, target.position);

            if (countPath.GetPathLenght() > traceDistance)
            {
                //추적
                print("추적 해제");
                monster.UpdateState(MonsterState.Idle);
                countPath.StopMovement();
                target = null;
                break;
            }

            if ((target.position - transform.position).sqrMagnitude <= attackDistance * attackDistance)
            {
                monster.UpdateState(MonsterState.Attack);
                print("공격 거리 진입");
                break;
            }
            else
                monster.UpdateState(MonsterState.Trace);

            yield return new WaitForSeconds(intervalTime);
        }
               
    }


    public void SetMoveSpeed(float _moveSpeed)
    {
        countPath.moveSpeed = _moveSpeed;
    }

    public void StopMovement()
    {
        countPath.StopMovement();
    }

    public void SightEnter(GameObject _gameObject)
    {
        //TODO :: 인식 범위내 들어옴을 알림

        target = _gameObject.transform;
        StartCoroutine(UpdateTrace());
    }

    public void SightExit(GameObject _gameObject)
    {
        if (target != null)
            StopMovement();
    }

}
