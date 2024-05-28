using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_UnitController : MonoBehaviour, IUnitController
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

    [Header("유닛 대기 상태 스크립트")]
    [SerializeField]
    private IUnitActState playerIdleCs;

    [Header("유닛 움직임 스크립트")]
    [SerializeField]
    private IUnitActState playerMoveCs;

    [Header("타겟 추적 스크립트")]
    [SerializeField]
    private IUnitActState unitTrackingCs;

    [Header("유닛 공격 스크립트")]
    [SerializeField]
    private IUnitActState unitAtkCs;

    [Header("유닛 스킬 사용 스크립트")]
    [SerializeField]
    private IUnitActState unitUseSkillCs;

    private readonly int hashWalk = Animator.StringToHash("isWalk");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashIdle = Animator.StringToHash("isIdle");
    private readonly int hashUseSkill = Animator.StringToHash("isSkillAttack");

    private void Awake()
    {
        unitDataCs = GetComponent<UnitData>();
        searchTargetCs = GetComponent<ISearchTarget>();
        _rigid = GetComponent<Rigidbody2D>();

        playerIdleCs = GetComponent<PlayerIdle>();
        playerMoveCs = GetComponent<PlayerMove>();
        unitTrackingCs = GetComponent<UnitTracking>();
        unitAtkCs = GetComponent<UnitAtk>();
        unitUseSkillCs = GetComponent<UnitUseSkill>();

        anim = GetComponentInChildren<Animator>();

        // 대기상태로 시작하도록 지정
        actionState = UnitAction.Idle;
    }

    void Update()
    {
        _rigid.velocity = Vector2.zero;

        Check_TargetDead();

        actionState2 = actionState;

        // 유닛이 힐러이고 스킬 가능한 상태일 경우
        bool isHealer = unitDataCs._canAct && unitDataCs.characterType.Equals(CharacterType.Healer) && unitDataCs._useSkill;

        // 어떤 상태이든 상관없이 바로 치유 스킬을 사용하도록 상태 전환
        if (isHealer)
        {
            anim.SetBool(hashWalk, false);
            anim.SetBool(hashAttack, false);
            //anim.SetTrigger(hashIdle);

            searchTargetCs._targetUnit = null;
            _unitState = null;
            actionState = UnitAction.UseSkill;

            setActionType(actionState);
        }

        // 힐러 이외의 다른 유닛일 경우 행동 가능 상태일 때만 동작
        else if (unitDataCs._canAct)
            setActionType(actionState);
    }

    #region # setActionType(UnitAction state)
    public void setActionType(UnitAction state)
    {
        // 현재 상태 저장
        actionState = state;

        // 다양한 상태 중에 어떤 것을 가져와야 할 지 모르므로
        // 인터페이스를 대표로 해서 가져온다.

        // 유닛 행동 상태에 따른 FSM
        switch (actionState)
        {
            // 유닛 대기 상태
            case UnitAction.Idle:
                
                if (_unitState == null && _unitState != playerIdleCs)
                {
                    _unitState = playerIdleCs;

                    _unitState.Enter();
                }
                if (_unitState != null)
                    _unitState.DoAction();

                break;

            // 유닛 이동 상태
            case UnitAction.Move:

                if (_unitState == null)
                {
                    _unitState = playerMoveCs;

                    _unitState.Enter();
                }

                _unitState.DoAction();
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

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, unitDataCs._unit_sightRange);
    //}
}
