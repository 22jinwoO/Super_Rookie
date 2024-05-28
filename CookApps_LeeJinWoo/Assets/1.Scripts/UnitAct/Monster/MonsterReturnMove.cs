using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterReturnMove : MonoBehaviour, IUnitActState
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

    [SerializeField]
    private Vector2 arrivePoint;

    [SerializeField]
    private float delayTime;

    [SerializeField]
    private float arrivalTime;

    private void Awake()
    {
        unitDataCs = GetComponent<UnitData>();
        contorllerCs = GetComponent<IUnitController>();
        searchTargetCs = GetComponent<ISearchTarget>();
        anim = GetComponentInChildren<Animator>();
    }
    public void Enter()
    {

        searchTargetCs._actStateCs = this;
        delayTime = 0f;
        arrivePoint = unitDataCs.default_Pos;
    }

    public void DoAction()
    {
        

        // 목표지점과 현재 나의 위치의 거리값 구하기
        float distance = Vector2.Distance(arrivePoint, (Vector2)transform.position);

        if (distance > 0.3f)
        {
            anim.SetBool(hashWalk, true);

            // 이동하는 방향에 맞게 오브젝트 방향 전환하기
            if (arrivePoint.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }


            else if (arrivePoint.x <= transform.position.x)
            {
                transform.localScale = Vector3.one;
            }

            // 타겟을 향한 이동방향 구하기
            Vector2 dir = arrivePoint - (Vector2)transform.position;

            Vector2 nextVec = dir.normalized * unitDataCs._unit_Speed * Time.deltaTime;

            contorllerCs._rigid.MovePosition(contorllerCs._rigid.position + nextVec);

            // 딜레이 시간 더해주기
            delayTime += Time.deltaTime;

            // 딜레이 시간이 13초보다 커졌을 경우 - 오브젝트끼리 충돌하여 제자리에 멈춰있는 것으로 판정
            if (delayTime > 13f)
                Exit();


        }

        if (distance <= 0.3f)
        {

            anim.SetBool(hashWalk, false);
            arrivalTime += Time.deltaTime;
            contorllerCs._rigid.MovePosition(transform.position);
            if (arrivalTime > 3f)
                Exit();
        }

        //if (distance <= 0.3f)
        //    Exit();






        // 방향 벡터 정규화
        //dir.Normalize();

        //// 리지드바디로 이동방향으로 이동
        //contorllerCs._rigid.velocity = dir * Time.deltaTime * 250f;

        searchTargetCs.SearchTarget();
    }

    public void Exit()
    {
        float distance = Vector2.Distance((Vector2)transform.position, arrivePoint);


        if (searchTargetCs._targetUnit != null)
        {
            contorllerCs._unitState = null;
            contorllerCs.actionState = UnitAction.Tracking;
            delayTime = 0f;
            arrivalTime = 0f;
        }

        if (arrivalTime > 3f && distance <= 0.3f)
        {
            contorllerCs._unitState = null;
            contorllerCs.actionState = UnitAction.Idle;
            arrivalTime = 0f;
        }

        if (delayTime > 13f)
        {
            contorllerCs._unitState = null;
            contorllerCs.actionState = UnitAction.Idle;
            delayTime = 0f;
        }

        return;
    }

}
