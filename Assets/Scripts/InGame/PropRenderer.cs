using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPropColor(Color _color)
    {
        spriteRenderer.color = _color;
    }



}
