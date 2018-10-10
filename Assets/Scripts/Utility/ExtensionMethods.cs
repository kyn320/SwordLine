using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{

    public static Vector3 ConvertToRawVector3(this Vector3 _pos)
    {
        if (_pos.x < -0.5f)
            _pos.x = -1;
        else if (_pos.x > 0.5f)
            _pos.x = 1;
        else
            _pos.x = 0;

        if (_pos.y < -0.5f)
            _pos.y = -1;
        else if (_pos.y > 0.5f)
            _pos.y = 1;
        else
            _pos.y = 0;

        return _pos;
    }

    public static Vector2 ConvertToRawVector2(this Vector2 _pos)
    {
        if (_pos.x < -0.5f)
            _pos.x = -1;
        else if (_pos.x > 0.5f)
            _pos.x = 1;
        else
            _pos.x = 0;

        if (_pos.y < -0.5f)
            _pos.y = -1;
        else if (_pos.y > 0.5f)
            _pos.y = 1;
        else
            _pos.y = 0;

        return _pos;
    }

}
