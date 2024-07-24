using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleerSkill : MonoBehaviour, IUseSkill
{
    [Header("스킬 공격력 비율")]
    [SerializeField]
    private float atkDMG_Rate;

    [Header("스킬 사용하는 유닛 스크립트")]
    [SerializeField]
    private BaseUnitData unitData;

    [Header("유닛의 적 탐지 스크립트")]
    [SerializeField]
    private ISearchTarget searchTargetCs;

    [Header("타겟의 유닛 데이터")]
    [SerializeField]
    private BaseUnitData targetData;

    [Header("타겟의 데미지 스크립트")]
    [SerializeField]
    private UnitDamaged targetDamagedCs;

    [Header("스킬 이펙트 프리팹")]
    [SerializeField]
    private GameObject skillvfx_Pref;

    // IUseSkill 인터페이스의 이펙트 오브젝트
    public GameObject _skillVFx { get; set; }

    [Header("타겟 탐지 타입")]
    [SerializeField]
    private LayerMask layerMask;

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

    #region UseSkill() : 근거리 딜러 전용 스킬
    public void UseSkill()
    {
        // 오버랩 스피어 생성
        Collider2D[] _cols = Physics2D.OverlapCircleAll((Vector2)transform.position, unitData.UnitSightRange, layerMask);

        // 탐지된 적이 없다면 함수 탈출
        if (_cols.Length <= 0)
        {
            unitData.Current_SkillCoolTime = unitData.UnitSkillCoolTime;
            return;
        }

        // 이펙트 위치 조정
        _skillVFx.transform.position = transform.position;

        for (int i = 0; i < _cols.Length; i++)
        {
            UnitDamaged targetUnitDamaged = _cols[i].GetComponent<UnitDamaged>();
            targetUnitDamaged.GetDamaged(AtkDmg: unitData.AtkDmg * atkDMG_Rate);
            // 타겟에게 공격한 유닛 할당
            //targetUnitDamaged._unit_attacked_Me = this.unitData;
        }

        // 복사한 이펙트 오브젝트 활성화
        _skillVFx.SetActive(true);

    }
    #endregion

    #region InitComponent() : 기초 셋팅값 할당하는 함수
    private void InitComponent()
    {
        // 스킬을 가지고 있는 유닛의 데이터
        unitData = GetComponent<BaseUnitData>();

        // 스킬을 가지고 있는 유닛의 타겟 탐지 스크립트
        searchTargetCs = GetComponent<ISearchTarget>();

    }
    #endregion

}
