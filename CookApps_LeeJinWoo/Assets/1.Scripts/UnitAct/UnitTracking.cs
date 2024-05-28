using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTracking : MonoBehaviour, IUnitActState
{
    [SerializeField]
    private UnitData unitDataCs;

    [SerializeField]
    private IUnitController contorllerCs;

    [SerializeField]
    private ISearchTarget searchTargetCs;

    //[SerializeField]
    //private SpriteRenderer spriteRender;
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
    }

    public void DoAction()
    {
        // 타겟이 없으면 탈출
        if (searchTargetCs._targetUnit == null)
        {
            Exit();
            return;
        }

        // 리지드바디 각도 고정
        contorllerCs._rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 방향전환
        if (searchTargetCs._targetUnit.position.x > transform.position.x)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            transform.localScale = Vector3.one;

        // 타겟을 향한 이동방향 구하기
        Vector2 dir = (Vector2)searchTargetCs._targetUnit.position - (Vector2)transform.position;

        Vector2 nextVec = dir.normalized * unitDataCs._unit_Speed * Time.deltaTime;

        // 타겟과 유닛과의 거리 구하기
        float distance = Vector2.Distance((Vector2)searchTargetCs._targetUnit.position, (Vector2)transform.position);

        // 플레이어 유닛이고, 타겟과의 거리가 스킬 사거리보다 클 경우 타겟을 향해 이동
        if ((!gameObject.CompareTag("Monster") && distance > unitDataCs.unit_SkillRange))
        {
            anim.SetBool(hashWalk, true);
            contorllerCs._rigid.MovePosition(contorllerCs._rigid.position + nextVec);
        }

        // 유닛이 플레이어 캐릭터이고 이동중 타겟과의 거리가 스킬 사거리와 같거나 작아질 경우 멈추고 스킬 사용하기
        else if (!gameObject.CompareTag("Monster") && distance <= unitDataCs.unit_SkillRange && unitDataCs._useSkill)
        {
            anim.SetBool(hashWalk, false);

            contorllerCs._rigid.MovePosition(transform.position);

            Exit();
        }

        //타겟과의 거리가 공격사거리보다 클 경우 걷기 애니메이션 재실행
        else if (distance > unitDataCs._unit_AtkRange)
        {
            contorllerCs._rigid.MovePosition(contorllerCs._rigid.position + nextVec);
            anim.SetBool(hashWalk, true);
        }


        // 타겟과의 거리가 공격사거리와 같거나 작아질 경우 멈추고 기본 공격하기
        else if (distance <= unitDataCs._unit_AtkRange)
        {
            anim.SetBool(hashWalk, false);
            contorllerCs._rigid.MovePosition(transform.position);

            if (unitDataCs._canAtk)
                Exit();
        }

        // 사거리 밖으로 타겟이 나가면 Idle 상태로 전환
        if (distance > unitDataCs._unit_sightRange)
        {
            unitDataCs._isSearch = false;
            searchTargetCs._targetUnit = null;
            Exit();
            return;
        }
    }

    // 상태 탈출
    public void Exit()
    {
        // 타겟이 스킬 공격 범위 안에 있는 상태일 경우
        if (!gameObject.CompareTag("Monster") && searchTargetCs._targetUnit != null && unitDataCs._useSkill)
        {
            contorllerCs._unitState = null;
            contorllerCs.actionState = UnitAction.UseSkill;
        }

        // 타겟이 공격범위 안에 있는 상태일 경우
        else if (searchTargetCs._targetUnit != null && unitDataCs._canAtk)
        {
            contorllerCs._unitState = null;
            contorllerCs.actionState = UnitAction.Attack;
        }

        // 타겟이 시야 범위 밖으로 나가서 타겟이 null 상태가 됐을 경우
        else if (searchTargetCs._targetUnit == null)
        {
            contorllerCs._unitState = null;
            contorllerCs.actionState = UnitAction.Idle;
        }
    }
}
