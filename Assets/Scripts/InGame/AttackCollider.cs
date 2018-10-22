using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackCollider : MonoBehaviour
{
    public string checkTag;

    public UnityAction<GameObject> damageAction;
    [SerializeField]
    private bool isAttack;

    public void SetAttack(bool _isAttack)
    {
        isAttack = _isAttack;
    }

    protected virtual void OnCollisionEnter2D(Collision2D _collision)
    {
        if (!isAttack || !_collision.gameObject.CompareTag(checkTag))
            return;

        if (damageAction != null)
            damageAction.Invoke(_collision.gameObject);
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D _collision)
    {
        if (!isAttack || !_collision.gameObject.CompareTag(checkTag))
            return;
        
        if (damageAction != null)
            damageAction.Invoke(_collision.gameObject);
        
    }

}
