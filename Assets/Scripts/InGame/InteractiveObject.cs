using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour
{
    public UnityEvent interactiveEvent;

    public void Interactive()
    {
        if (interactiveEvent != null)
            interactiveEvent.Invoke();
    }

    public void PrintObjectName()
    {
        Debug.Log("Interactive Action : :" + gameObject.name);
    }

}
