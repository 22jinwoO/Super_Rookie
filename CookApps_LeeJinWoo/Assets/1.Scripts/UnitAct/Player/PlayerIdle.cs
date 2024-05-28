using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : MonoBehaviour, IUnitActState
{
    [SerializeField]
    private UnitData unitDataCs;

    [SerializeField]
    private IUnitController contorllerCs;

    [SerializeField]
    private ISearchTarget searchTargetCs;

    [SerializeField]
    private Animator anim;


    private readonly int hashIdle= Animator.StringToHash("isIdle");

    private void Awake()
    {
        unitDataCs = GetComponent<UnitData>();
        contorllerCs = GetComponent<IUnitController>();
        searchTargetCs = GetComponent<ISearchTarget>();
        anim = GetComponentInChildren<Animator>();
    }

    // 행동 진입 시 호출
    public void Enter()
    {

        searchTargetCs._actStateCs = this;

        anim.SetTrigger(hashIdle);
        searchTargetCs.SearchTarget();
    }

    // 대기 상태 일 때 계속 호출
    public void DoAction()
    {
        float distance = Vector2.Distance((Vector2)transform.position, (Vector2)unitDataCs.base_Pos.position);

        if (distance <= 0.3f)
        {
            searchTargetCs.SearchTarget();
        }


        else if (distance > 0.3f)
        {
            Exit();
        }
    }

    // 대기상태 탈출 시 호출
    public void Exit()
    {
        // 플레이어 유닛 현재 위치와 기본 위치(지정된 슬롯) 와의 거리 계산
        float distance = Vector2.Distance((Vector2)transform.position, (Vector2)unitDataCs.base_Pos.position);

        // 플레이어 유닛 상태 비움
        contorllerCs._unitState = null;

        // 기본 위치와의 거리가 0.3보다 멀 경우 이동 상태로 진입
        if ((distance > 0.3f))
        {
            contorllerCs.actionState = UnitAction.Move;
        }
            
        // 거리가 0.3보다 같거나 작고, 탐지한 적이 없으면 대기상태로 다시 돌입
        else if ((distance <= 0.3f) && searchTargetCs._targetUnit == null)
            contorllerCs.actionState = UnitAction.Idle;

        // 이외의 경우, 탐지한 타겟이 있다면 추적 상태로 전환
        else if (searchTargetCs._targetUnit != null)
            contorllerCs.actionState = UnitAction.Tracking;

    }
}
