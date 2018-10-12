using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    private PlayerBehaviour player;
    [Header("무기 정보")]
    public Weapon weapon;

    public GameObject rendererObject;
    private SpriteRenderer spriteRenderer;
    private Animator ani;

    [Header("속성 레벨")]
    // (min  = 0) ~ (max = 3)
    public int propLevel;

    Quaternion rotQuaternion;
    Vector3 rotVector;

    [Header("공격 애니메이션 정보")]
    public float attackAnimationCurrentTime = 0;
    public float[] attackAnimationTime;

    [Header("콤보")]
    public int combo = 0;
    public int maxCombo = 2;

    [Header("콤보 딜레이")]
    public float comboWaitTime = 0.5f;
    public float comboWaitCurrentTime;

    [Header("넉백 효과")]
    public float knockBackPower = 10f;

    [Header("카메라 쉐이크 효과")]
    public float shakeAmount = 3;
    public float shakeTime = 0.5f;
    public float shakeLerp = 1f;
    
    private string damageEffect;

    public bool isAttack = false;

    private void Awake()
    {
        if (rendererObject == null)
        {
            Debug.Log("WeaponBehaviour :: 렌더링 하는 무기 오브젝트가 존재하지 않습니다. 인스펙터를 확인해주세요.");
            return;
        }

        player = transform.root.GetComponent<PlayerBehaviour>();
        spriteRenderer = rendererObject.GetComponent<SpriteRenderer>();
        ani = rendererObject.GetComponent<Animator>();
        damageEffect = "Effect_Hit_" + transform.root.GetComponent<PropBehaviour>().prop.type.ToString();
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

        if (combo == maxCombo + 1)
            combo = 1;
        
        ani.SetInteger("Combo", combo);
        attackAnimationCurrentTime = attackAnimationTime[combo - 1];
        ani.SetTrigger("Attack");
        player.SetDirection(rotVector.ConvertToRawVector3());
        player.UpdateState(PlayerState.Attack, true);
        player.GetAnimator().SetInteger("Combo", combo);

        //TODO :: 카메라 컨트롤러 의존성 분리
        CameraController.instance.Shake(shakeAmount, shakeTime, shakeLerp);

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
            monster.SetDamageEffect(damageEffect);
            monster.KnockBack(knockBackPower, -player.GetDirectionToVector3(monster.transform.position));
            monster.Damage(1);
        }
    }



}
