using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SightChecker : MonoBehaviour
{
    private CircleCollider2D circleCollider;

    [Header("시야 반지름")]
    public float sightRange = 1f;
    [Header("체크 할 레이어")]
    public LayerMask checkLayer;
    [Header("시야 내 접근 이벤트")]
    public UnityEvent sightEnter;
    [Header("시야 밖 방출 이벤트")]
    public UnityEvent sightExit;

    public UnityAction<GameObject> sightEnterWithGameObject, sightExitWithGameObject;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();

        if (!circleCollider.isTrigger)
            circleCollider.isTrigger = true;

    }

    public virtual void SetSight(float _sight)
    {
        sightRange = _sight;
        circleCollider.radius = sightRange;
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if ((1 << _collision.gameObject.layer) != checkLayer.value)
        {
            return;
        }

        if (sightEnter != null)
            sightEnter.Invoke();

        if (sightEnterWithGameObject != null)
            sightEnterWithGameObject.Invoke(_collision.gameObject);

    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if ((1 << _collision.gameObject.layer) != checkLayer.value)
        {
            return;
        }

        if (sightExit != null)
            sightExit.Invoke();

        if (sightExitWithGameObject != null)
            sightExitWithGameObject.Invoke(_collision.gameObject);

    }



}
