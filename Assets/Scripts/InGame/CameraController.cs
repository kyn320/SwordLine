using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform target;

    public Vector3 margin;
    public float lerpTime = 10f;

    private CameraShake shaker;

    private void Awake()
    {
        instance = this;
        shaker = GetComponentInChildren<CameraShake>();
    }

    private void FixedUpdate()
    {
        if (target != null)
            transform.position = Vector3.Lerp(transform.position, target.position + margin, Time.deltaTime * lerpTime);

    }

    public void Shake(float _amount, float _time, float _lerpTime)
    {
        shaker.ShakeCam(_amount, _time, _lerpTime);
    }

}
