using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : MonoBehaviour
{

    public int hp;
    public MonsterState state;

    public float moveSpeed = 1f;

    public bool isSuperPower = false;

    private MonsterAI ai;
    private Rigidbody2D ri;

    private void Awake()
    {
        ri = GetComponent<Rigidbody2D>();
        ai = GetComponent<MonsterAI>();
    }

    private void Start()
    {
        ai.SetMoveSpeed(moveSpeed);
    }

    public void UpdateState(MonsterState _state, bool _isOn = true)
    {
        switch (_state)
        {
            case MonsterState.Idle:
                state = MonsterState.Idle;
                //TODO :: LED 초록색으로 변경
                break;
            case MonsterState.Trace:
                state = MonsterState.Trace;
                //TODO :: LED 빨간색으로 변경

                break;
            case MonsterState.Attack:
                if (_isOn)
                {
                    ri.velocity = Vector2.zero;
                    ai.StopMovement();
                    state = MonsterState.Attack;
                }
                else
                {
                    state = MonsterState.Idle;
                }
                break;
            case MonsterState.Hacking:
                state = MonsterState.Hacking;
                //TODO :: 스턴 구현
                ai.StopMovement();

                //TODO :: 스킬 사용 불가능
                //TODO :: LED 보라색으로 변경
                //TODO :: 해킹 이펙트 출력
                break;
            case MonsterState.Reset:
                state = MonsterState.Reset;
                //TODO ::  체력을 100% 회복
                break;
            case MonsterState.End:
                state = MonsterState.End;
                //TODO :: 왜 필요한건지 아직 모르겠음..?
                break;
            case MonsterState.Return:
                state = MonsterState.Return;
                //TODO :: 최초 스폰 장소로 귀환
                //TODO :: 도착 할때까지 무적 상태, 이동속도 2배
                isSuperPower = true;
                ai.SetMoveSpeed(moveSpeed * 2f);
                break;
            case MonsterState.Death:
                state = MonsterState.Death;
                //TODO :: 사망 애니메이션 출력
                //TODO :: 3초 동안 불투명도 (Alpha) n 만큼 값 하락
                //TODO :: 불투명도가 0이 되면 삭제
                break;
        }

    }

    #region HP 제어
    public void Heal(int _value)
    {
        hp += _value;
    }

    public void Damage(int _value)
    {
        if (isSuperPower)
            return;

        hp -= _value;

        if (hp < 1)
            Death();
    }

    public void Death()
    {
        state = MonsterState.Death;
        //TODO :: 사망 판정 구현

    }
    #endregion

    #region 넉백효과
    public void KnockBack(float _power, Vector3 _dir)
    {

        if (knockBack != null)
        {
            StopCoroutine(knockBack);
        }

        knockBack = StartCoroutine(KnockBackEffect(_power, _dir));

    }

    Coroutine knockBack = null;

    IEnumerator KnockBackEffect(float _power, Vector3 _dir)
    {
        ri.velocity = Vector2.zero;
        ri.drag = _power * 0.7f;
        ri.AddForce(_power * _dir, ForceMode2D.Impulse);

        while (ri.velocity.sqrMagnitude > 0.1f)
        {
            yield return null;
        }

        ri.drag = 0f;
        ri.velocity = Vector2.zero;
        knockBack = null;

    }

    #endregion
}

public enum MonsterState
{
    /// <summary>
    /// 비전투 상태
    /// </summary>
    Idle,
    /// <summary>
    /// 인식 범위에 들어와 추적하는 상태
    /// </summary>
    Trace,
    /// <summary>
    /// 공격 범위에 들어와 공격하는 상태
    /// </summary>
    Attack,
    /// <summary>
    /// 해킹 당한 상태
    /// </summary>
    Hacking,
    /// <summary>
    /// 추적거리를 넘어선 상태
    /// </summary>
    Reset,
    /// <summary>
    /// 전투 종료 상태
    /// </summary>
    End,
    /// <summary>
    /// 초기 위치로 이동하는 상태
    /// </summary>
    Return,
    /// <summary>
    /// 사망 상태
    /// </summary>
    Death
}