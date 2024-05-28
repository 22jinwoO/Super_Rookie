using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkill : MonoBehaviour, IUseSkill
{
    [Header("스킬 공격력 비율")]
    [SerializeField]
    private float atkDMG_Rate;

    [Header("스킬 사용하는 유닛 스크립트")]
    [SerializeField]
    private UnitData unitDataCs;

    [Header("유닛의 적 탐지 스크립트")]
    [SerializeField]
    private ISearchTarget searchTargetCs;

    [Header("타겟의 유닛 데이터")]
    [SerializeField]
    private UnitData targetData;

    [Header("타겟의 데미지 스크립트")]
    [SerializeField]
    private UnitDamaged targetDamagedCs;

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

    #region UseSkill() : 원거리 딜러 스킬
    public void UseSkill()
    {
        // 이펙트 위치 조정
        _skillVFx.transform.position = searchTargetCs._targetUnit.position;

        // 복사한 이펙트 오브젝트 활성화
        _skillVFx.SetActive(true);

        // 타겟의 Data 가져오기
        targetData = searchTargetCs._targetUnit.GetComponent<UnitData>();

        // 타겟의 데미지 스크립트 가져오기
        targetDamagedCs = searchTargetCs._targetUnit.GetComponent<UnitDamaged>();

        // 타겟에게 공격한 유닛 할당
        targetDamagedCs._unit_attacked_Me = this.unitDataCs;

        // 타겟에게 피해를 부여
        targetDamagedCs.GetDamaged(AtkDmg: unitDataCs._unit_AtkDmg * atkDMG_Rate);

    }
    #endregion

    #region InitComponent() : 기초 셋팅값 할당하는 함수
    private void InitComponent()
    {
        // 스킬을 가지고 있는 유닛의 데이터
        unitDataCs = GetComponent<UnitData>();

        // 스킬을 가지고 있는 유닛의 타겟 탐지 스크립트
        searchTargetCs = GetComponent<ISearchTarget>();

    }
    #endregion

}
