using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUseSkill : MonoBehaviour, IUnitActState
{
    [SerializeField]
    private UnitData unitDataCs;

    [SerializeField]
    private IUnitController contorllerCs;

    [Header("유닛의 적 탐지 스크립트")]
    [SerializeField]
    private ISearchTarget searchTargetCs;

    [SerializeField]
    private IUseSkill skillCs;

    [SerializeField]
    private float delayTime;

    [SerializeField]
    private Animator anim;


    private readonly int hashUseSkill = Animator.StringToHash("isSkillAttack");

    private void Awake()
    {
        skillCs = GetComponent<IUseSkill>();
        unitDataCs = GetComponent<UnitData>();
        searchTargetCs = GetComponent<ISearchTarget>();
        contorllerCs = GetComponent<IUnitController>();
        anim = GetComponentInChildren<Animator>();
    }


    public void Enter()
    {
        delayTime = 0f;
    }


    public void DoAction()
    {
        contorllerCs._rigid.constraints = RigidbodyConstraints2D.FreezeAll;

        // 유닛이 스킬 사용 가능 상태이면, 스킬 사용 애니메이션 실행
        if (unitDataCs._useSkill)
            anim.SetBool(hashUseSkill, true);

        // 유닛이 스킬 사용이 불가한 경우
        else if (!unitDataCs._useSkill)
        {
            anim.SetBool(hashUseSkill, false);

            // 0.2초 딜레이 후
            if (delayTime <= 0.2f)
            {
                delayTime += Time.deltaTime;
            }

            // 상태 전환
            else
                Exit();
        }
    }


    // 상태 전환 시 추적 상태로 변경
    public void Exit()
    {
        contorllerCs._rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        contorllerCs._unitState = null;
        contorllerCs.actionState = UnitAction.Tracking;
    }
}
