using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [Header("카메라 타겟 대상")]
    public Transform target;

    [Header("위치 조정")]
    public Vector3 margin;
    [Header("이동 러프 속도")]
    public float lerpSpeed = 10f;

    private CameraShake shaker;

    private void Awake()
    {
        instance = this;
        shaker = GetComponentInChildren<CameraShake>();
    }

    private void FixedUpdate()
    {
        if (target != null)
            transform.position = Vector3.Lerp(transform.position, target.position + margin, Time.deltaTime * lerpSpeed);

    }

    public void Shake(float _amount, float _time, float _lerpTime)
    {
        shaker.ShakeCam(_amount, _time, _lerpTime);
    }

}
