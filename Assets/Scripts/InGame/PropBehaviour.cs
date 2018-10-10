using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PropBehaviour : MonoBehaviour
{

    public Prop prop;

    private UnityAction<Color> propChangeAction;

    private void Awake()
    {
        PropRenderer[] renderers = GetComponentsInChildren<PropRenderer>();
        for (int i = 0; i < renderers.Length; ++i)
        {
            propChangeAction += renderers[i].SetPropColor;
        }
    }

    private void Start()
    {
        ChangeProp(prop);
    }

    public void ChangeProp(Prop _prop)
    {
        if (_prop == null)
            return;

        prop = _prop;

        if (propChangeAction != null)
            propChangeAction.Invoke(prop.color);

    }



}
