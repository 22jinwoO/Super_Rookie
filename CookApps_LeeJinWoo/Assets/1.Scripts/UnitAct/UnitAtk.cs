using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAtk : MonoBehaviour, IUnitActState
{

    [SerializeField]
    private UnitData unitDataCs;    
    
    [SerializeField]
    private IUnitController contorllerCs;

    [SerializeField]
    private ISearchTarget searchTargetCs;

    [SerializeField]
    private UnitDamaged targetDamagedCs;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private float delayTime;

    private readonly int hashAttack = Animator.StringToHash("isAttack");


    private void Awake()
    {
        unitDataCs = GetComponent<UnitData>();
        contorllerCs = GetComponent<IUnitController>();
        searchTargetCs = GetComponent<ISearchTarget>();

        anim = GetComponentInChildren<Animator>();
    }
    public void Enter()
    {
        delayTime = 0f;
        targetDamagedCs = searchTargetCs._targetUnit.GetComponent<UnitDamaged>();
        //searchTargetCs._actStateCs = this;
    }

    public void DoAction()
    {
        contorllerCs._rigid.constraints = RigidbodyConstraints2D.FreezeAll;

        // 기본 공격이 가능할 경우, 기본공격 애니메이션 실행
        if (unitDataCs._canAtk)
            anim.SetBool(hashAttack, true);

        // 기본 공격이 불가능한 상태
        else if (!unitDataCs._canAtk)
        {
            // 기본 공격 애니메이션 취소
            anim.SetBool(hashAttack, false);

            // 일정 딜레이
            if (delayTime <= 0.2f)
            {
                delayTime += Time.deltaTime;
            }

            // 행동 탈출
            else
                Exit();
        }

    }

    public void Exit()
    {
        contorllerCs._rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 추적상태로 전환
        contorllerCs._unitState = null;

        contorllerCs.actionState = UnitAction.Tracking;
    }

}
