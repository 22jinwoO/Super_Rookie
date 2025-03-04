﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour, IUnitController
{
    [Header("유닛 데이터 스크립트")]
    [SerializeField]
    private MonsterUnitData unitData;

    [field : SerializeField]
    public UnitState Action { get; set; }
    [field: SerializeField]
    public IUnitActState UnitAct { get; set; }

    public Rigidbody2D Rigid { get; set; }

    public ISearchTarget searchTarget;

    [Header("유닛 애니메이터")]
    [SerializeField]
    private Animator anim;


    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashUseSkill = Animator.StringToHash("isSkillAttack");

    [Header("몬스터 대기 상태 스크립트")]
    [SerializeField]
    private IUnitActState monsterIdle;

    [Header("몬스터 움직임 스크립트")]
    [SerializeField]
    private IUnitActState monsterMove;

    [Header("몬스터 패트롤 스크립트")]
    [SerializeField]
    private IUnitActState monsterReturnMove;

    [Header("타겟 추적 스크립트")]
    [SerializeField]
    private IUnitActState unitTracking;

    [Header("유닛 공격 스크립트")]
    [SerializeField]
    private IUnitActState unitAtk;

    private void Awake()
    {
        unitData = GetComponent<MonsterUnitData>();
        searchTarget = GetComponent<ISearchTarget>();
        Rigid = GetComponent<Rigidbody2D>();

        monsterIdle = GetComponent<MonsterIdle>();
        monsterMove = GetComponent<MonsterMove>();
        monsterReturnMove = GetComponent<MonsterReturnMove>();
        unitTracking = GetComponent<UnitTracking>();
        unitAtk = GetComponent<UnitAtk>();


        anim = GetComponentInChildren<Animator>();
        Action = UnitState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        Rigid.velocity = Vector2.zero;

        // 타겟이 죽었는지 체크하는 함수 호출
        Check_TargetDead();

        // 유닛이 행동 가능할 때만 동작하는 함수 호출
        if (unitData.CanAct) SetState(Action);
    }

    #region # setActionType(UnitAction state)
    public void SetState(UnitState state)
    {

        // 현재 상태 저장
        Action = state;

        // 다양한 상태 중에 어떤 것을 가져와야 할 지 모르므로
        // 인터페이스를 대표로 해서 가져온다.

        // 자유 모드 상태 중 유닛 행동 상태에 따른 FSM
        switch (Action)
        {
            // 유닛 대기 상태
            case UnitState.Idle:

                if (UnitAct == null)
                {
                    UnitAct = monsterIdle;
                    UnitAct.Enter();
                }
                break;

             // 몬스터 이동
            case UnitState.Move:

                if (UnitAct == null && UnitAct != monsterMove)
                {
                    UnitAct = monsterMove;
                    UnitAct.Enter();
                }
                UnitAct.DoAction();
                break;

            // 베이스 지점으로 돌아옴
            case UnitState.ReturnMove:

                if (UnitAct == null)
                {
                    UnitAct = monsterReturnMove;
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
        }
    }
    #endregion

    #region # Check_TargetDead()
    private void Check_TargetDead()
    {
        if (searchTarget.TargetUnit != null && searchTarget.TargetColider.enabled == false)
        {
            searchTarget.TargetUnit = null;

            //Component c = (Component)GetComponent<IUnitActState>();

            //Destroy(c);
            anim.SetBool(hashAttack, false);
            anim.SetBool(hashUseSkill, false);
            Action = UnitState.Idle;
            UnitAct = null;
        }
    }
    #endregion
}
