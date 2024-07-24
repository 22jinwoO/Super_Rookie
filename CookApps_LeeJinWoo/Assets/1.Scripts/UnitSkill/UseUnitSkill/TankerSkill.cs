using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankerSkill : MonoBehaviour, IUseSkill
{
    [Header("상태이상 적용 시간")]
    [SerializeField]
    private float applyTime;

    [Header("상태이상 적용 시간")]
    [SerializeField]
    private BaseUnitData unitData;

    [Header("유닛의 적 탐지 스크립트")]
    [SerializeField]
    private ISearchTarget searchTarget;

    [Header("타겟의 유닛 데이터")]
    [SerializeField]
    private BaseUnitData targetData;

    [Header("타겟의 데미지 스크립트")]
    [SerializeField]
    private UnitDamaged targetDamaged;

    [Header("스킬 이펙트 프리팹")]
    [SerializeField]
    private GameObject skillvfx_Pref;

    // IUseSkill 인터페이스의 이펙트 오브젝트
    public GameObject _skillVFx { get; set; }


    private void Awake()
    {
        // IUseSkill 인터페이스의 이펙트 오브젝트에 이펙트 할당
        _skillVFx = Instantiate(skillvfx_Pref);

        // 이펙트 비활성화
        _skillVFx.SetActive(false);

        // 이펙트 활성화 확인을 위해 스킬을 사용하는 오브젝트의 자식으로 설정
        _skillVFx.transform.SetParent(transform);

        // 기초 셋팅값 할당해주는 함수 호출
        InitComponent();
    }

    #region UseSkill() : 탱커 전용 스킬 - 단일 타겟 스턴 + 데미지
    public void UseSkill()
    {
        // 이펙트 위치 조정
        _skillVFx.transform.position = searchTarget.TargetUnit.position;

        // 복사한 이펙트 오브젝트 활성화
        _skillVFx.SetActive(true);

        // 타겟의 Data 가져오기
        targetData = searchTarget.TargetUnit.GetComponent<BaseUnitData>();

        // 타겟의 데미지 스크립트 가져오기
        targetDamaged = searchTarget.TargetUnit.GetComponent<UnitDamaged>();

        //targetDamaged._unit_attacked_Me = this.unitData;


        // 기존에 스턴이 적용됐을 경우의 예외 처리
        if (targetData.IsStun != null)
        {            
            // 기존 상태효과 해제
            Destroy(searchTarget.TargetUnit.GetComponent<StunStatus>());
        }

        // 새로운 상태효과(스턴) 부여
        if (targetData.IsStun == null)
        {
            // 타겟에게 스턴 상태효과 스크립트 추가
            targetData.IsStun = searchTarget.TargetUnit.gameObject.AddComponent<StunStatus>();

            // 스턴 상태효과 스크립트의 유닛 데이터에 타겟 데이터 할당
            targetData.GetComponent<StunStatus>().targetUnitData = targetData;

            // 스턴 상태효과의 이펙트 할당
            targetData.IsStun._skillVfx = _skillVFx;

            // 스턴 효과 지속시간 부여
            targetData.IsStun._statusEffecttime = applyTime;

            // 타겟에게 피해를 부여
            targetDamaged.GetDamaged(AtkDmg: unitData.AtkDmg);
        }
    }
    #endregion

    #region InitComponent() : 기초 셋팅값 할당하는 함수
    private void InitComponent()
    {
        // 스킬을 가지고 있는 유닛의 데이터
        unitData = GetComponent<BaseUnitData>();

        // 스킬을 가지고 있는 유닛의 타겟 탐지 스크립트
        searchTarget = GetComponent<ISearchTarget>();
        
        //상태이상 지속시간 할당
        applyTime = 1f;
    }
    #endregion

}
