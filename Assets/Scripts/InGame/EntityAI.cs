using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Astar2DPathFinding.Mika;

public class EntityAI : MonoBehaviour
{
    [Header("시야 인식 범위")]
    public float sightRange = 1f;
    [Header("타겟")]
    public Transform target;
    [Header("타겟 위치 갱신 시간")]
    public float intervalTime = 0.4f;

    protected CountPath countPath;
    protected SightChecker sightChecker;

    protected virtual void Awake()
    {
        sightChecker = GetComponentInChildren<SightChecker>();
        countPath = GetComponent<CountPath>();
    }

    protected virtual void Start() {
        sightChecker.SetSight(sightRange);
    }

    public void SetMoveSpeed(float _moveSpeed)
    {
        countPath.moveSpeed = _moveSpeed;
    }

    public virtual void StartMovement() {

    }

    public virtual void StopMovement()
    {
        countPath.StopMovement();
    }


}

public enum EntityState
{
    Idle,
    Move
}