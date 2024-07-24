using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour, IUnitController
{
    [Header("유닛 데이터 스크립트")]
    [SerializeField]
    private BaseUnitData unitData;

    public UnitAction Action { get; set; }
    public IUnitActState UnitState { get; set; }
    public Rigidbody2D Rigid { get; set; }

    public ISearchTarget searchTargetCs;

    public UnitAction actionState2;

    [Header("유닛 애니메이터")]
    [SerializeField]
    private Animator anim;

    private readonly int hashWalk = Animator.StringToHash("isWalk");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashIdle = Animator.StringToHash("isIdle");
    private readonly int hashUseSkill = Animator.StringToHash("isSkillAttack");

    [Header("몬스터 대기 상태 스크립트")]
    [SerializeField]
    private IUnitActState monsterIdle;

    [Header("몬스터 움직임 스크립트")]
    [SerializeField]
    private IUnitActState monsterMove;

    [Header("타겟 추적 스크립트")]
    [SerializeField]
    private IUnitActState unitTracking;

    [Header("유닛 공격 스크립트")]
    [SerializeField]
    private IUnitActState unitAtk;

    [Header("유닛 스킬 사용 스크립트")]
    [SerializeField]
    private IUnitActState unitUseSkill;


    private void Awake()
    {
        unitData = GetComponent<BaseUnitData>();
        searchTargetCs = GetComponent<ISearchTarget>();
        Rigid = GetComponent<Rigidbody2D>();

        monsterIdle = GetComponent<MonsterIdle>();
        monsterMove = GetComponent<MonsterMove>();
        unitTracking = GetComponent<UnitTracking>();
        unitAtk = GetComponent<UnitAtk>();
        unitUseSkill = GetComponent<UnitUseSkill>();

        anim = GetComponentInChildren<Animator>();
        Action = UnitAction.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        Rigid.velocity = Vector2.zero;

        Check_TargetDead();

        actionState2 = Action;

        if (unitData.CanAct)
            SetState(Action);
    }

    #region # SetState(UnitAction state)
    public void SetState(UnitAction state)
    {
        // 현재 상태 저장
        Action = state;

        // 다양한 상태 중에 어떤 것을 가져와야 할 지 모르므로
        // 인터페이스를 대표로 해서 가져온다.

        // 자유 모드 상태 중 유닛 행동 상태에 따른 FSM
        switch (Action)
        {
            // 유닛 대기 상태
            case UnitAction.Idle:

                if (UnitState == null)
                {
                    UnitState = monsterIdle;
                    print(UnitState.ToString());
                    UnitState.Enter();
                }

                break;

            // 유닛 추적 상태
            case UnitAction.Tracking:
                if (UnitState == null)
                {
                    UnitState = unitTracking;

                    UnitState.Enter();
                }

                UnitState.DoAction();
                break;

            // 유닛 공격 상태
            case UnitAction.Attack:
                if (UnitState == null)
                {
                    UnitState = unitAtk;

                    UnitState.Enter();
                }

                UnitState.DoAction();
                break;

            // 유닛 스킬 사용
            case UnitAction.UseSkill:
                if (UnitState == null)
                {
                    UnitState = unitUseSkill;

                    UnitState.Enter();
                }

                UnitState.DoAction();
                break;
        }
    }
    #endregion

    #region # Check_TargetDead()
    private void Check_TargetDead()
    {
        if (searchTargetCs.TargetUnit != null && searchTargetCs.TargetColider.enabled == false)
        {
            searchTargetCs.TargetUnit = null;

            anim.SetBool(hashAttack, false);
            anim.SetBool(hashUseSkill, false);
            Action = UnitAction.Idle;
            UnitState = null;
        }
    }
    #endregion

}
