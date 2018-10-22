using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour
{
    [Header("상호 작용 시 이벤트 목록")]
    public UnityEvent interactiveEvent;

    public UnityAction<GameObject> interactiveEventToGameObject;

    public void Interactive()
    {
        if (interactiveEvent != null)
            interactiveEvent.Invoke();
    }

    public void Interactive(GameObject _object)
    {

        if (interactiveEvent != null)
            interactiveEvent.Invoke();

        if (interactiveEventToGameObject != null)
            interactiveEventToGameObject.Invoke(_object);
    }



}
