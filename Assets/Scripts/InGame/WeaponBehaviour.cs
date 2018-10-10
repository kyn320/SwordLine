using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    private PlayerBehaviour player;

    public Weapon weapon;

    public GameObject rendererObject;
    private SpriteRenderer spriteRenderer;
    private Animator ani;

    // (min  = 0) ~ (max = 3)
    public int propLevel;

    private Collider2D weaponCollider;

    Quaternion rotQuaternion;
    Vector3 rotVector;

    public float attackAnimationCurrentTime = 0;
    public float[] attackAnimationTime;

    public int combo = 0;
    public int maxCombo = 2;

    public float comboWaitTime = 0.5f;
    public float comboWaitCurrentTime;

    public float power = 10f;

    public bool isAttack = false;

    private void Awake()
    {
        if (rendererObject == null)
        {
            Debug.Log("WeaponBehaviour :: 렌더링 하는 무기 오브젝트가 존재하지 않습니다. 인스펙터를 확인해주세요.");
            return;
        }

        player = transform.root.GetComponent<PlayerBehaviour>();
        weaponCollider = rendererObject.GetComponent<Collider2D>();
        spriteRenderer = rendererObject.GetComponent<SpriteRenderer>();
        ani = rendererObject.GetComponent<Animator>();
    }
        
    private void FixedUpdate()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.localRotation = rotQuaternion;
    }

    public void SetRotate(Vector3 _rot)
    {
        rotVector = _rot;
        float degree = Mathf.Atan2(_rot.y, _rot.x) * Mathf.Rad2Deg;
        rotQuaternion = Quaternion.Euler(0, 0, degree);
    }

    public void Attack()
    {
        if (attackAnimationCurrentTime > 0)
            return;

        isAttack = true;

        ++combo;

        if (combo == maxCombo +1)
            combo = 1;

        weaponCollider.enabled = true;
        ani.SetInteger("Combo", combo);
        attackAnimationCurrentTime = attackAnimationTime[combo - 1];
        ani.SetTrigger("Attack");
        player.SetDirection(rotVector.ConvertToRawVector3());
        player.UpdateState(PlayerState.Attack, true);
        player.GetAnimator().SetInteger("Combo", combo);
        CameraController.instance.Shake(3, 0.3f, 1f);
        if (attackWaitTimer != null)
        {
            StopCoroutine(attackWaitTimer);
        }
        attackWaitTimer = StartCoroutine(AttackWaitTimer());
    }

    Coroutine attackWaitTimer = null;

    IEnumerator AttackWaitTimer()
    {
        while (attackAnimationCurrentTime > 0)
        {
            attackAnimationCurrentTime -= Time.deltaTime;
            yield return null;
        }

        if (attackComboWaitTimer != null)
        {
            StopCoroutine(attackComboWaitTimer);
        }
        attackComboWaitTimer = StartCoroutine(AttackComboWaitTimer());

        player.UpdateState(PlayerState.Attack, false);
        player.GetAnimator().SetInteger("Combo", 0);
        attackAnimationCurrentTime = 0f;
        weaponCollider.enabled = false;
        isAttack = false;

    }

    Coroutine attackComboWaitTimer = null;

    IEnumerator AttackComboWaitTimer()
    {
        comboWaitCurrentTime = comboWaitTime;

        while (comboWaitCurrentTime > 0)
        {
            comboWaitCurrentTime -= Time.deltaTime;
            yield return null;
        }

        combo = 0;
        comboWaitCurrentTime = 0f;

    }


    public void Damage(GameObject _monster)
    {
        if (_monster.CompareTag("Monster"))
        {
            MonsterBehaviour monster = _monster.GetComponent<MonsterBehaviour>();
            monster.Damage(1);
            monster.KnockBack(power, -player.GetDirectionToVector3(monster.transform.position));
        }
    }



}
