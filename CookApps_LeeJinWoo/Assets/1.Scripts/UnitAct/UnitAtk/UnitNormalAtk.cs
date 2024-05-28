using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitNormalAtk : MonoBehaviour
{
    [SerializeField]
    private UnitData unitDataCs;

    [Header("공격 이펙트")]
    [SerializeField]
    private GameObject atkVfx;

    [SerializeField]
    private ISearchTarget targetDamagedCs;

    private void Awake()
    {
        // 공격 이펙트 프리팹 준비
        atkVfx = Instantiate(atkVfx);
        atkVfx.SetActive(false);
        atkVfx.transform.SetParent(transform);

        // 데이터 가져오기
        unitDataCs = transform.parent.GetComponent<UnitData>();
        targetDamagedCs = unitDataCs.transform.GetComponent<ISearchTarget>();
    }
    public void NormalAttack()
    {
        // 타겟이 없으면 탈출
        if (targetDamagedCs._targetUnit == null)
            return;

        // 이펙트 위치 지정
        atkVfx.transform.position = targetDamagedCs._targetUnit.transform.position;
        
        // 이펙트 활성화
        atkVfx.SetActive(true);

        // 타겟에게 데미지 전달
        targetDamagedCs._targetUnit.GetComponent<UnitDamaged>().GetDamaged(AtkDmg: unitDataCs._unit_AtkDmg);

        // 타겟을 공격한 유닛으로 자신을 지정
        targetDamagedCs._targetUnit.GetComponent<UnitDamaged>()._unit_attacked_Me = this.unitDataCs;
        unitDataCs._current_AtkCoolTime = 0f;
    }
}
