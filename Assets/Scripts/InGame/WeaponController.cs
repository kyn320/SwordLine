using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private WeaponBehaviour weaponBehaviour;

    public Transform target;

    public float attackDamage;
    public float attackSpeed;

    public float heavyAttackTime = 1f;
    public float heavyAttackCurrentTime;

    private void Awake()
    {
        weaponBehaviour = GetComponent<WeaponBehaviour>();
    }

    public void SetWeapon(float _attackDamage, float _attackSpeed)
    {
        attackDamage = _attackDamage;
        attackSpeed = _attackSpeed;
    }

    private void Update()
    {
        if (weaponBehaviour.isAttack)
            return;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = target.position.z - Camera.main.transform.position.z;
        Vector3 rot = Camera.main.ScreenToWorldPoint(mousePosition) - target.position;
        weaponBehaviour.SetRotate(rot.normalized);

        if (Input.GetMouseButtonDown(0))
        {
            weaponBehaviour.Attack();
        }

    }

}
