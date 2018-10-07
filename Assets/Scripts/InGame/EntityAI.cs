using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Astar2DPathFinding.Mika;

public class EntityAI : MonoBehaviour
{
    private CountPath countPath;

    protected virtual void Awake()
    {
        countPath = GetComponent<CountPath>();
    }

}

public enum EntityState
{
    Idle,
    Move
}