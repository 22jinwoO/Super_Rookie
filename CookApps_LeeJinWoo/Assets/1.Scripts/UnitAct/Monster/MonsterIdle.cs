using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdle : MonoBehaviour, IUnitActState
{
    [SerializeField]
    private MonsterUnitData unitData;

    [SerializeField]
    private IUnitController contorllerCs;

    [SerializeField]
    private ISearchTarget searchTargetCs;

    [SerializeField]
    private Animator anim;


    private readonly int hashIdle = Animator.StringToHash("isIdle");

    private void Awake()
    {
        unitData = GetComponent<MonsterUnitData>();



        contorllerCs = GetComponent<IUnitController>();
        searchTargetCs = GetComponent<ISearchTarget>();
        anim = GetComponentInChildren<Animator>();
    }

    // 대기 상태 돌입
    public void Enter()
    {
        unitData.Default_Pos = (Vector2)transform.position;
        searchTargetCs.ActState = this;
        anim.SetTrigger(hashIdle);
        searchTargetCs.SearchTarget();
    }


    public void DoAction()
    {

    }

    // 대기 상태 탈출
    public void Exit()
    {

        contorllerCs.UnitState = null;

        // 타겟이 없다면 이동상태로 전환
        if (searchTargetCs.TargetUnit == null)
            contorllerCs.Action = UnitAction.Move;

        // 탐지된 타겟이 있다면 추적상태로 돌입
        if (searchTargetCs.TargetUnit != null)
            contorllerCs.Action = UnitAction.Tracking;

    }

}
