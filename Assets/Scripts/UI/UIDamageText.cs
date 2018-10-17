using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDamageText : MonoBehaviour
{
    public Text damageText;

    public void SetText(string _text)
    {
        damageText.text = _text;
    }

    public void SetText(string _text, Color _color)
    {
        damageText.text = _text;
        damageText.color = _color;
    }

}
