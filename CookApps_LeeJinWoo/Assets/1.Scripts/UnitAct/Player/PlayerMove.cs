using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour, IUnitActState
{
    [SerializeField]
    private UnitData unitDataCs;


    [SerializeField]
    private IUnitController contorllerCs;

    [SerializeField]
    private ISearchTarget searchTargetCs;

    [SerializeField]
    private Animator anim;


    private readonly int hashWalk = Animator.StringToHash("isWalk");

    private void Awake()
    {
        unitDataCs = GetComponent<UnitData>();
        contorllerCs = GetComponent<IUnitController>();
        searchTargetCs = GetComponent<ISearchTarget>();
        anim = GetComponentInChildren<Animator>();
    }
    public void Enter()
    {
        contorllerCs._rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        searchTargetCs._actStateCs = this;
    }

    public void DoAction()
    {
        // 목표지점의 x값이 나의 x 값보다 클 경우 오브젝트 방향 전환
        if (unitDataCs.base_Pos.position.x > transform.position.x)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            transform.localScale = Vector3.one;

        // 베이스캠프까지의 거리 구하기
        float distance = Vector2.Distance(unitDataCs.base_Pos.position, transform.position);

        //베이스캠프까지의 거리가 0.3보다 클 경우 행동 탈출
        if (distance <= 0.3f)
            Exit();

        anim.SetBool(hashWalk, true);
        // 타겟을 향한 이동방향 구하기
        Vector2 dir = (Vector2)unitDataCs.base_Pos.position - (Vector2)transform.position;

        Vector2 nextVec = dir.normalized * unitDataCs._unit_Speed * Time.deltaTime;

        contorllerCs._rigid.MovePosition(contorllerCs._rigid.position + nextVec);

        // 타겟 탐지함수 호출
        searchTargetCs.SearchTarget();
    }

    public void Exit()
    {
        float distance = Vector2.Distance(unitDataCs.base_Pos.position, transform.position);


        // 타겟이 탐지됐을 경우 추적상태로 전환
        if (searchTargetCs._targetUnit != null)
        {
            anim.SetBool(hashWalk, false);
            contorllerCs._unitState = null;
            contorllerCs.actionState = UnitAction.Tracking;

        }

        // 자신의 베이스 캠프와의 거리가 0.3보다 작아졌을 경우 대기상태로 전환
        else if (distance <= 0.3f)
        {
            anim.SetBool(hashWalk, false);
            contorllerCs._unitState = null;
            contorllerCs.actionState = UnitAction.Idle;

        }

        // 이외의 경우 계속 이동
        else
            return;

    }
}
