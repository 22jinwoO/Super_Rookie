using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAtk : MonoBehaviour, IUnitActState
{

    [SerializeField]
    private BaseUnitData unitData;    
    
    [SerializeField]
    private IUnitController contorller;

    [SerializeField]
    private ISearchTarget searchTarget;

    [SerializeField]
    private UnitDamaged targetDamaged;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private float delayTime;

    private readonly int hashAttack = Animator.StringToHash("isAttack");


    private void Awake()
    {
        unitData = GetComponent<BaseUnitData>();
        contorller = GetComponent<IUnitController>();
        searchTarget = GetComponent<ISearchTarget>();

        anim = GetComponentInChildren<Animator>();
    }
    public void Enter()
    {
        delayTime = 0f;
        targetDamaged = searchTarget.TargetUnit.GetComponent<UnitDamaged>();
    }

    public void DoAction()
    {
        contorller.Rigid.constraints = RigidbodyConstraints2D.FreezeAll;

        // 기본 공격이 가능할 경우, 기본공격 애니메이션 실행
        if (unitData.CanAtk) anim.SetBool(hashAttack, true);

        // 기본 공격이 불가능한 상태
        else if (!unitData.CanAtk)
        {
            // 기본 공격 애니메이션 취소
            anim.SetBool(hashAttack, false);

            // 일정 딜레이
            if (delayTime <= 0.2f)
            {
                delayTime += Time.deltaTime;
            }

            // 행동 탈출
            else Exit();
        }

    }

    public void Exit()
    {
        contorller.Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 추적상태로 전환
        contorller.UnitState = null;

        contorller.Action = UnitAction.Tracking;
    }

}
