using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour, IUnitActState
{
    [SerializeField]
    private MonsterUnitData unitData;


    [SerializeField]
    private IUnitController contorller;

    [SerializeField]
    private ISearchTarget searchTarget;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Vector2 arrivePoint;

    [SerializeField]
    private float delayTime;    
    
    [SerializeField]
    private float arrivalTime;

    private readonly int hashWalk = Animator.StringToHash("isWalk");

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        unitData = GetComponent<MonsterUnitData>();
        contorller = GetComponent<IUnitController>();
        searchTarget = GetComponent<ISearchTarget>();

    }
    public void Enter()
    {
        
        delayTime = 0f;
        arrivalTime = 0f;

        contorller.Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        searchTarget.ActState = this;

        // 목표지점 구하기 
        arrivePoint = (Vector2)transform.position + new Vector2(Random.Range(-4f,5f), Random.Range(-4f, 5f));

    }

    public void DoAction()
    {
        // 목표지점과 현재 나의 위치의 거리값 구하기
        float distance = Vector2.Distance(arrivePoint, (Vector2)transform.position);

        if(distance > 0.3f)
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
                //print($"{arrivePoint.x} <= {transform.position.x}");
            }

            // 타겟을 향한 이동방향 구하기
            Vector2 dir = arrivePoint - (Vector2)transform.position;

            Vector2 nextVec = dir.normalized * unitData.UnitSpeed * Time.deltaTime;

            contorller.Rigid.MovePosition(contorller.Rigid.position + nextVec);

            // 딜레이 시간 더해주기
            delayTime += Time.deltaTime;

            // 딜레이 시간이 13초보다 커졌을 경우 - 오브젝트끼리 충돌하여 제자리에 멈춰있는 것으로 판정
            if (delayTime > 13f)
                Exit();
        }

        // 목표지점에 도착했을 때
        if (distance <= 0.3f)
        {
            anim.SetBool(hashWalk, false);

            // 도착 시간 더해주기
            arrivalTime += Time.deltaTime;

            // 현재 제자리 사수
            contorller.Rigid.MovePosition(transform.position);

            // 도착 시간이 3초보다 커졌을 경우 Exit()함수 호출
            if (arrivalTime > 3f)
                Exit();
        }

        searchTarget.SearchTarget();
    }

    public void Exit()
    {
        //anim.SetBool(hashWalk, false);

        float distance = Vector2.Distance((Vector2)transform.position, arrivePoint);

        //contorller.UnitState = null;

        // 타겟을 찾았을 경우
        if (searchTarget.TargetUnit != null)
        {
            contorller.UnitState = null;
            contorller.Action = UnitAction.Tracking;
            arrivalTime = 0f;
            delayTime = 0f;
        }

        // 도착시간이 3초 이상 경과하였고, 목표지점에 도착했을 경우
        if (arrivalTime > 3f && distance <= 0.3f)
        {
            contorller.UnitState = null;
            contorller.Action = UnitAction.ReturnMove;
            arrivalTime = 0f;

        }

        // 딜레이 시간이 3초보다 커졌을 경우 Exit()함수 호출
        if (delayTime > 13f)
        {
            contorller.UnitState = null;
            contorller.Action = UnitAction.Idle;
            delayTime = 0f;
        }

        else
            return; 
    }
}
