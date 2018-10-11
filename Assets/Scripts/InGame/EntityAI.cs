using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Astar2DPathFinding.Mika;

public class EntityAI : MonoBehaviour
{
    [Header("시야 인식 범위")]
    public float sightRange = 1f;

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



    protected virtual void Update()
    {

    }

   

}

public enum EntityState
{
    Idle,
    Move
}