using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    Vector3 dir;

    UnityAction<GameObject> damageAction;

    public void SetBullet(Vector3 _dir, float _speed, UnityAction<GameObject> _damageAction)
    {
        dir = _dir;
        speed = _speed;
        damageAction += _damageAction;
    }

    private void FixedUpdate()
    {
        transform.position += dir * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Wall"))
            ObjectPoolManager.Instance.Free(this.gameObject);
        else if (_collision.CompareTag("Player"))
            damageAction(_collision.gameObject);

    }


}
