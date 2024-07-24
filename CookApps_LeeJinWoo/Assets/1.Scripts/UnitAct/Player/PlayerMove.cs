using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour, IUnitActState
{
    [SerializeField]
    private BaseUnitData unitData;


    [SerializeField]
    private IUnitController contorllerCs;

    [SerializeField]
    private ISearchTarget searchTargetCs;

    [SerializeField]
    private Animator anim;


    private readonly int hashWalk = Animator.StringToHash("isWalk");

    private void Awake()
    {
        unitData = GetComponent<BaseUnitData>();
        contorllerCs = GetComponent<IUnitController>();
        searchTargetCs = GetComponent<ISearchTarget>();
        anim = GetComponentInChildren<Animator>();
    }
    public void Enter()
    {
        contorllerCs.Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        searchTargetCs.ActState = this;
    }

    public void DoAction()
    {
        // 목표지점의 x값이 나의 x 값보다 클 경우 오브젝트 방향 전환
        if (unitData.BasePos.position.x > transform.position.x)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            transform.localScale = Vector3.one;

        // 베이스캠프까지의 거리 구하기
        float distance = Vector2.Distance(unitData.BasePos.position, transform.position);

        //베이스캠프까지의 거리가 0.3보다 클 경우 행동 탈출
        if (distance <= 0.3f)
            Exit();

        anim.SetBool(hashWalk, true);
        // 타겟을 향한 이동방향 구하기
        Vector2 dir = (Vector2)unitData.BasePos.position - (Vector2)transform.position;

        Vector2 nextVec = dir.normalized * unitData.UnitSpeed * Time.deltaTime;

        contorllerCs.Rigid.MovePosition(contorllerCs.Rigid.position + nextVec);

        // 타겟 탐지함수 호출
        searchTargetCs.SearchTarget();
    }

    public void Exit()
    {
        float distance = Vector2.Distance(unitData.BasePos.position, transform.position);


        // 타겟이 탐지됐을 경우 추적상태로 전환
        if (searchTargetCs.TargetUnit != null)
        {
            anim.SetBool(hashWalk, false);
            contorllerCs.UnitState = null;
            contorllerCs.Action = UnitAction.Tracking;

        }

        // 자신의 베이스 캠프와의 거리가 0.3보다 작아졌을 경우 대기상태로 전환
        else if (distance <= 0.3f)
        {
            anim.SetBool(hashWalk, false);
            contorllerCs.UnitState = null;
            contorllerCs.Action = UnitAction.Idle;

        }

        // 이외의 경우 계속 이동
        else
            return;

    }
}
