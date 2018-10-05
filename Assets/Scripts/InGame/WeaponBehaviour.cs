using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public Weapon weapon;

    public GameObject rendererObject;

    public Prop prop;

    // (min  = 0) ~ (max = 3)
    public int propLevel;

    private Collider2D weaponCollider;

    Quaternion rotVec;

    private void Awake()
    {
        weaponCollider = GetComponent<Collider2D>();
    }


    private void FixedUpdate()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.localRotation = rotVec;
    }

    public void SetRotate(Vector3 _rot)
    {
        float degree = Mathf.Atan2(_rot.y, _rot.x) * Mathf.Rad2Deg;
        rotVec = Quaternion.Euler(0, 0, degree);
    }

    public void Attack()
    {
        weaponCollider.enabled = true;
    }


    private void OnTriggerEnter2D(Collider2D _collision)
    {

    }


}
