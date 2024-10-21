using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using System;

public class PlayerController : MonoBehaviour, IUnitController
{
    [Header("유닛 데이터 스크립트")]
    [SerializeField]
    private PlayerUnitData unitData;

    // 유닛의 현재 상태
    public UnitState Action { get; set; }

    // 유닛의 현재 행동
    public IUnitActState UnitAct { get; set; }

    public Rigidbody2D Rigid { get; set; }

    public ISearchTarget searchTarget;

    [Header("유닛 애니메이터")]
    [SerializeField]
    private Animator anim;

    [Header("유닛 대기 상태 스크립트")]
    [SerializeField]
    private IUnitActState playerIdle;

    [Header("유닛 움직임 스크립트")]
    [SerializeField]
    private IUnitActState playerMove;

    [Header("타겟 추적 스크립트")]
    [SerializeField]
    private IUnitActState unitTracking;

    [Header("유닛 공격 스크립트")]
    [SerializeField]
    private IUnitActState unitAtk;

    [Header("유닛 스킬 사용 스크립트")]
    [SerializeField]
    private IUnitActState unitUseSkill;

    private readonly int hashWalk = Animator.StringToHash("isWalk");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashIdle = Animator.StringToHash("isIdle");
    private readonly int hashUseSkill = Animator.StringToHash("isSkillAttack");

    private void Awake()
    {
        InitComponent();

        // 대기상태로 시작하도록 지정
        Action = UnitState.Idle;
    }

    void Update()
    {
        Rigid.velocity = Vector2.zero;

        Check_TargetDead();

        // 유닛이 힐러이고 스킬 가능한 상태일 경우
        bool isHealer = unitData.CanAct && unitData.CharacterType.Equals(CharacterType.Healer) && unitData.UseSkill;

        // 어떤 상태이든 상관없이 바로 치유 스킬을 사용하도록 상태 전환
        if (isHealer)
        {
            anim.SetBool(hashWalk, false);
            anim.SetBool(hashAttack, false);
            //anim.SetTrigger(hashIdle);

            searchTarget.TargetUnit = null;
            UnitAct = null;
            Action = global::UnitState.UseSkill;

            SetState(Action);
        }

        // 힐러 이외의 다른 유닛일 경우 행동 가능 상태일 때만 동작
        else if (unitData.CanAct)
            SetState(Action);
    }
    private void InitComponent()
    {
        unitData = GetComponent<PlayerUnitData>();
        searchTarget = GetComponent<ISearchTarget>();
        Rigid = GetComponent<Rigidbody2D>();

        playerIdle = GetComponent<PlayerIdle>();
        playerMove = GetComponent<PlayerMove>();
        unitTracking = GetComponent<UnitTracking>();
        unitAtk = GetComponent<UnitAtk>();
        unitUseSkill = GetComponent<UnitUseSkill>();

        anim = GetComponentInChildren<Animator>();

    }

    #region # SetState(UnitAction state)
    public void SetState(UnitState state)
    {
        // 현재 상태 저장
        Action = state;

        // 다양한 상태 중에 어떤 것을 가져와야 할 지 모르므로
        // 인터페이스를 대표로 해서 가져온다.

        // 유닛 행동 상태에 따른 FSM
        switch (Action)
        {
            // 유닛 대기 상태
            case UnitState.Idle:
                
                if (UnitAct == null && UnitAct != playerIdle)
                {
                    UnitAct = playerIdle;

                    UnitAct.Enter();
                }
                if (UnitAct != null)
                    UnitAct.DoAction();

                break;

            // 유닛 이동 상태
            case UnitState.Move:

                if (UnitAct == null)
                {
                    UnitAct = playerMove;

                    UnitAct.Enter();
                }

                UnitAct.DoAction();
                break;

            // 유닛 추적 상태
            case UnitState.Tracking:
                if (UnitAct == null)
                {
                    UnitAct = unitTracking;

                    UnitAct.Enter();
                }

                UnitAct.DoAction();
                break;

            // 유닛 공격 상태
            case UnitState.Attack:
                if (UnitAct == null)
                {
                    UnitAct = unitAtk;

                    UnitAct.Enter();
                }

                UnitAct.DoAction();
                break;

            // 유닛 스킬 사용
            case UnitState.UseSkill:
                if (UnitAct == null)
                {
                    UnitAct = unitUseSkill;

                    UnitAct.Enter();
                }

                UnitAct.DoAction();
                break;
        }
    }
    #endregion

    #region # Check_TargetDead()
    private void Check_TargetDead()
    {
        if (searchTarget.TargetUnit != null && searchTarget.TargetColider.enabled == false)
        {
            searchTarget.TargetUnit = null;

            anim.SetBool(hashAttack, false);
            anim.SetBool(hashUseSkill, false);
            Action = global::UnitState.Idle;
            UnitAct = null;
        }
    }
    #endregion

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, unitData._unit_sightRange);
    //}
}
