using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponBehaviour : MonoBehaviour
{
    private PlayerBehaviour player;
    [Header("무기 정보")]
    public Weapon weapon;
    private AttackCollider attackCollider;

    public GameObject rendererObject;
    private SpriteRenderer spriteRenderer;
    private Animator ani;

    [Header("속성 레벨 (min  = 0) ~ (max = 3)")]
    public int propLevel;
    public PropAttack propAttack;

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

    private UnityAction<float> damageOperator;

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
        attackCollider = GetComponentInChildren<AttackCollider>();
    }

    private void Start()
    {
        SetProp();
        attackCollider.damageAction += Damage;
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

        attackCollider.SetAttack(true);

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
        attackCollider.SetAttack(false);

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

    public void SetProp()
    {
        PropType type = player.propBehaviour.prop.type;

        switch (type)
        {
            case PropType.OverDrive:
                propAttack = gameObject.AddComponent<PropOverDrive>();
                break;
            case PropType.Hacking:
                propAttack = gameObject.AddComponent<PropHacking>();
                break;
            case PropType.Hologram:
                propAttack = gameObject.AddComponent<PropHologram>();
                break;
            default:
                Debug.LogError("WeaponBehaviour :: 존재하지 않는 속성 타입입니다. 무기 정보를 확인해 주세요.");
                return;
        }

        propAttack.prop = player.propBehaviour.prop;

    }


    public void Damage(GameObject _monsterObject)
    {
        if (_monsterObject.CompareTag("Monster"))
        {
            MonsterBehaviour monster = _monsterObject.GetComponent<MonsterBehaviour>();
            monster.KnockBack(knockBackPower, -player.GetDirectionToVector3(monster.transform.position));
            monster.Damage(OperateDamage());
            if (propAttack != null)
            {
                propAttack.Attack(monster);
            }
        }
    }

    public int OperateDamage()
    {
        float totalDamage = 0;

        //TODO ::  속성을 제외한 모든 공격력 연산을 구현

        //무기 고유 공격력 대입
        totalDamage = weapon.damage;

        //속성 특성에 따른 공격력 연산
        if (propAttack != null)
        {
            totalDamage = propAttack.OperateDamage(totalDamage);
        }

        return (int)totalDamage;
    }

}
