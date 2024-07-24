using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunStatus : MonoBehaviour, IStatusEffect
{
    [Header("타겟 유닛 데이터")]
    public BaseUnitData targetUnitData;

    [SerializeField]
    private float duringTime;    
    
    public float _statusEffecttime { get; set; }
    public GameObject _skillVfx { get; set; }

    [Header("유닛 애니메이터")]
    [SerializeField]
    private Animator anim;

    private IUnitController controllerCs;

    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashUseSkill = Animator.StringToHash("isSkillAttack");
    private readonly int hashIdle = Animator.StringToHash("isIdle");

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        controllerCs = GetComponentInChildren<IUnitController>();

        // 현재 스턴 적용중인 시간
        duringTime = 0f;

        targetUnitData = GetComponent<BaseUnitData>();
    }

    private void Update()
    {
        // 상태이상 효과 적용하는 함수 호출
        OnStatusEffect(_statusEffecttime);
    }

    public void OnStatusEffect(float StatusEffecttime)
    {
        // 상태이상 효과가 적용된 상태에서 재적용될 경우 컴포넌트 파괴
        if (targetUnitData.IsStun == null)
        {
            Destroy(this);
        }

        // 상태이상 효과가 적용중일 경우
        else if (targetUnitData.IsStun != null && duringTime <= StatusEffecttime)
        {
            targetUnitData.CanAct = false;
            duringTime += Time.deltaTime;
        }

        // 상태이상 효과가 적용시간까지 정상 적용된 경우
        else
        {
            // 스턴 상태효과 해제
            targetUnitData.IsStun = null;

            // 행동 제어 해제
            targetUnitData.CanAct = true;
        }

        anim.SetTrigger(hashIdle);
        anim.SetBool(hashAttack, false);
        anim.SetBool(hashUseSkill, false);

        controllerCs.UnitState = null;
        controllerCs.Action = UnitAction.Idle;
    }

    private void OnDestroy()
    {
        // 상태이상 효과가 적용된 상태에서 재적용될 경우 
        if (targetUnitData.IsStun != null)
        {
            // 행동 제어 안풀리도록 설정
            targetUnitData.CanAct = false;
            targetUnitData.IsStun = null;
        }
    }
}
