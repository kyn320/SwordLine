using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    Vector3 dir;

    public void SetBullet(Vector3 _dir, float _speed)
    {
        dir = _dir;
        speed = _speed;
    }

    private void FixedUpdate()
    {
        transform.position += dir * Time.deltaTime * speed;
    }


}
