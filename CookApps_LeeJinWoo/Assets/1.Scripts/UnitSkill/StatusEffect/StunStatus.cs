using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunStatus : MonoBehaviour, IStatusEffect
{
    [Header("타겟 유닛 데이터")]
    public UnitData _targetUnitData;

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
        //unitData = GetComponent<UnitData>();
        //unitData.isStun = this;
        //Debug.Log(unitData.isStun);
        //unitData._canAct = false;




        _targetUnitData = GetComponent<UnitData>();
    }

    private void Update()
    {
        // 상태이상 효과 적용하는 함수 호출
        OnStatusEffect(_statusEffecttime);
    }

    public void OnStatusEffect(float StatusEffecttime)
    {
        // 상태이상 효과가 적용된 상태에서 재적용될 경우 컴포넌트 파괴
        if (_targetUnitData.isStun == null)
        {
            Destroy(this);
        }

        // 상태이상 효과가 적용중일 경우
        else if (_targetUnitData.isStun != null && duringTime <= StatusEffecttime)
        {
            _targetUnitData._canAct = false;
            duringTime += Time.deltaTime;
        }

        // 상태이상 효과가 적용시간까지 정상 적용된 경우
        else
        {
            // 스턴 상태효과 해제
            _targetUnitData.isStun = null;

            // 행동 제어 해제
            _targetUnitData._canAct = true;
        }

        anim.SetTrigger(hashIdle);
        anim.SetBool(hashAttack, false);
        anim.SetBool(hashUseSkill, false);

        controllerCs._unitState = null;
        controllerCs.actionState = UnitAction.Idle;
    }

    private void OnDestroy()
    {
        // 상태이상 효과가 적용된 상태에서 재적용될 경우 
        if (_targetUnitData.isStun != null)
        {
            // 행동 제어 안풀리도록 설정
            _targetUnitData._canAct = false;
            _targetUnitData.isStun = null;
        }
    }
}
