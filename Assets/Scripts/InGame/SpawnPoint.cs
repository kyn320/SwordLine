using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private Vector3 boxSize = Vector3.one;

    [Header("스폰 포인트 렌더링 체크")]
    public bool viewGizmos = true;

    public void OnDrawGizmos()
    {
        if (!viewGizmos)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, boxSize);
    }


}
