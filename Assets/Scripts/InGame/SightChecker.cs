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

    public UnityAction<GameObject> sightEnterAction, sightExitAction;

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
        if (_collision.gameObject.layer != checkLayer)
        {
            return;
        }


        if (sightEnterAction != null)
            sightEnterAction.Invoke(_collision.gameObject);

    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.gameObject.layer != checkLayer)
        {
            return;
        }

        if (sightExitAction != null)
            sightExitAction.Invoke(_collision.gameObject);

    }



}
