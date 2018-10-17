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
    [Header("체크 할 태그")]
    public string checkTag;
    [Header("시야 내 접근 이벤트")]
    public UnityEvent sightEnter;
    [Header("시야 밖 방출 이벤트")]
    public UnityEvent sightExit;
    [Header("가장 최근 진입한 오브젝트")]
    public GameObject recentSightInGameObject;
    [Header("시야 내에 있는 오브젝트 리스트")]
    public List<GameObject> sightInGameObject;
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

        if (checkTag == null || checkTag == "" || _collision.tag == null || !_collision.CompareTag(checkTag))
        {
            return;
        }

        if (sightEnter != null)
            sightEnter.Invoke();

        if (sightEnterWithGameObject != null)
            sightEnterWithGameObject.Invoke(_collision.gameObject);

        sightInGameObject.Add(_collision.gameObject);
        recentSightInGameObject = _collision.gameObject;

    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if ((1 << _collision.gameObject.layer) != checkLayer.value)
        {
            return;
        }

        if (checkTag == null || checkTag == "" || _collision.tag == null || !_collision.CompareTag(checkTag))
        {
            return;
        }

        if (sightExit != null)
            sightExit.Invoke();

        if (sightExitWithGameObject != null)
            sightExitWithGameObject.Invoke(_collision.gameObject);

        sightInGameObject.Remove(_collision.gameObject);

    }



}
