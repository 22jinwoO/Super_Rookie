using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour, IUnitActState
{
    [SerializeField]
    private UnitData unitDataCs;


    [SerializeField]
    private IUnitController contorllerCs;

    [SerializeField]
    private ISearchTarget searchTargetCs;

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
        unitDataCs = GetComponent<UnitData>();
        contorllerCs = GetComponent<IUnitController>();
        searchTargetCs = GetComponent<ISearchTarget>();

    }
    public void Enter()
    {
        
        delayTime = 0f;
        arrivalTime = 0f;

        contorllerCs._rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        searchTargetCs._actStateCs = this;

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

            Vector2 nextVec = dir.normalized * unitDataCs._unit_Speed * Time.deltaTime;

            contorllerCs._rigid.MovePosition(contorllerCs._rigid.position + nextVec);

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
            contorllerCs._rigid.MovePosition(transform.position);

            // 도착 시간이 3초보다 커졌을 경우 Exit()함수 호출
            if (arrivalTime > 3f)
                Exit();
        }

        searchTargetCs.SearchTarget();
    }

    public void Exit()
    {
        //anim.SetBool(hashWalk, false);

        float distance = Vector2.Distance((Vector2)transform.position, arrivePoint);

        //contorllerCs._unitState = null;

        // 타겟을 찾았을 경우
        if (searchTargetCs._targetUnit != null)
        {
            contorllerCs._unitState = null;
            contorllerCs.actionState = UnitAction.Tracking;
            arrivalTime = 0f;
            delayTime = 0f;
        }

        // 도착시간이 3초 이상 경과하였고, 목표지점에 도착했을 경우
        if (arrivalTime > 3f && distance <= 0.3f)
        {
            contorllerCs._unitState = null;
            contorllerCs.actionState = UnitAction.ReturnMove;
            arrivalTime = 0f;

        }

        // 딜레이 시간이 3초보다 커졌을 경우 Exit()함수 호출
        if (delayTime > 13f)
        {
            contorllerCs._unitState = null;
            contorllerCs.actionState = UnitAction.Idle;
            delayTime = 0f;
        }

        else
            return; 
    }
}
