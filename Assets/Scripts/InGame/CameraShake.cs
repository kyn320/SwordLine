using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public float shakeAmount;
    public float shakeLerpTime;

    float shakePercent;
    float shakeDuration;


    public void ShakeCam(float _amount, float _time, float _lerpTime)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeAmount = _amount;
        shakeDuration = _time;
        shakeLerpTime = _lerpTime;

        shakeCoroutine = StartCoroutine(Shake());
    }

    Coroutine shakeCoroutine = null;

    IEnumerator Shake()
    {
        while (shakeDuration > 0f)
        {
            Vector3 amountPositionVec = Random.insideUnitSphere * shakeAmount;
            Vector3 amountRotationVec = Random.insideUnitSphere * shakeAmount;
            amountPositionVec.z = 0;
            amountRotationVec.x = amountRotationVec.y = 0;

            shakePercent = shakeAmount * shakePercent;
            shakeDuration -= Time.deltaTime;

            transform.localPosition = Vector3.Lerp(transform.localPosition, amountPositionVec, Time.deltaTime * shakeLerpTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(amountRotationVec), Time.deltaTime * shakeLerpTime);

            yield return null;
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        shakeCoroutine = null;
    }
}
