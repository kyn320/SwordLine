using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{

    private WeaponBehaviour weaponBehaviour;

    private void Awake()
    {
        weaponBehaviour = transform.parent.GetComponent<WeaponBehaviour>();
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (!weaponBehaviour.isAttack)
            return;

        weaponBehaviour.Damage(_collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (!weaponBehaviour.isAttack)
            return;

        weaponBehaviour.Damage(_collision.gameObject);


    }



}
