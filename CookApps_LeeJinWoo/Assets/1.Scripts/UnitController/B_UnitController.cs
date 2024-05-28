using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_UnitController : MonoBehaviour, IUnitController
{
    [Header("유닛 데이터 스크립트")]
    [SerializeField]
    private UnitData unitDataCs;

    public UnitAction actionState { get; set; }
    public IUnitActState _unitState { get; set; }
    public Rigidbody2D _rigid { get; set; }

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
    private IUnitActState monsterIdleCs;

    [Header("몬스터 움직임 스크립트")]
    [SerializeField]
    private IUnitActState monsterMoveCs;

    [Header("타겟 추적 스크립트")]
    [SerializeField]
    private IUnitActState unitTrackingCs;

    [Header("유닛 공격 스크립트")]
    [SerializeField]
    private IUnitActState unitAtkCs;

    [Header("유닛 스킬 사용 스크립트")]
    [SerializeField]
    private IUnitActState unitUseSkillCs;


    private void Awake()
    {
        unitDataCs = GetComponent<UnitData>();
        searchTargetCs = GetComponent<ISearchTarget>();
        _rigid = GetComponent<Rigidbody2D>();

        monsterIdleCs = GetComponent<MonsterIdle>();
        monsterMoveCs = GetComponent<MonsterMove>();
        unitTrackingCs = GetComponent<UnitTracking>();
        unitAtkCs = GetComponent<UnitAtk>();
        unitUseSkillCs = GetComponent<UnitUseSkill>();

        anim = GetComponentInChildren<Animator>();
        actionState = UnitAction.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        _rigid.velocity = Vector2.zero;

        Check_TargetDead();

        actionState2 = actionState;

        if (unitDataCs._canAct)
            setActionType(actionState);
    }

    #region # setActionType(UnitAction state)
    public void setActionType(UnitAction state)
    {
        // 현재 상태 저장
        actionState = state;

        // 다양한 상태 중에 어떤 것을 가져와야 할 지 모르므로
        // 인터페이스를 대표로 해서 가져온다.

        // 자유 모드 상태 중 유닛 행동 상태에 따른 FSM
        switch (actionState)
        {
            // 유닛 대기 상태
            case UnitAction.Idle:

                if (_unitState == null)
                {
                    _unitState = monsterIdleCs;
                    print(_unitState.ToString());
                    _unitState.Enter();
                }

                break;

            // 유닛 추적 상태
            case UnitAction.Tracking:
                if (_unitState == null)
                {
                    _unitState = unitTrackingCs;

                    _unitState.Enter();
                }

                _unitState.DoAction();
                break;

            // 유닛 공격 상태
            case UnitAction.Attack:
                if (_unitState == null)
                {
                    _unitState = unitAtkCs;

                    _unitState.Enter();
                }

                _unitState.DoAction();
                break;

            // 유닛 스킬 사용
            case UnitAction.UseSkill:
                if (_unitState == null)
                {
                    _unitState = unitUseSkillCs;

                    _unitState.Enter();
                }

                _unitState.DoAction();
                break;
        }
    }
    #endregion

    #region # Check_TargetDead()
    private void Check_TargetDead()
    {
        if (searchTargetCs._targetUnit != null && searchTargetCs._targetColider.enabled == false)
        {
            searchTargetCs._targetUnit = null;

            //Component c = (Component)GetComponent<IUnitActState>();

            //Destroy(c);
            anim.SetBool(hashAttack, false);
            anim.SetBool(hashUseSkill, false);
            actionState = UnitAction.Idle;
            _unitState = null;
        }
    }
    #endregion

}
