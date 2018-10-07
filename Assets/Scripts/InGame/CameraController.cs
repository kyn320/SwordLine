using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 margin;
    public float lerpTime = 10f;

    private CameraShake shaker;

    private void Awake()
    {
        shaker = GetComponent<CameraShake>();
    }

    private void LateUpdate()
    {
        if (target != null)
            transform.position = Vector3.Lerp(transform.position, target.position + margin, Time.deltaTime * lerpTime);

    }

    public void Shake() {
        shaker.ShakeCam();
    }

}
