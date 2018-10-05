using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRenderer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public void SetPropColor(Color _color)
    {
        spriteRenderer.color = _color;
    }



}
