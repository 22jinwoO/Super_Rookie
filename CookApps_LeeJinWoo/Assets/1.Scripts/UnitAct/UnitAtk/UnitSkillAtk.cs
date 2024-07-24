using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillAtk : MonoBehaviour
{
    [SerializeField]
    private BaseUnitData unitData;

    [SerializeField]
    private ISearchTarget targetDamaged;


    [SerializeField]
    private IUseSkill skillCs;

    private void Awake()
    {
        unitData = transform.parent.GetComponent<BaseUnitData>();
        targetDamaged = unitData.transform.GetComponent<ISearchTarget>();
        skillCs = transform.parent.GetComponent<IUseSkill>();
        print(skillCs);
    }

    // 스킬을 사용할 때 호출되는 함수
    public void UseSkill_Atk()
    {
        // 캐릭터 타입이 힐러가 아니고, 타겟이 없을 경우 탈출
        if (!unitData.CharacterType.Equals(CharacterType.Healer) && targetDamaged.TargetUnit == null)
            return;

        // 이외의 경우 유닛 쿨타임 0으로 전환
        unitData.Current_SkillCoolTime = 0f;

        // 내가 갖고 있는 스킬 사용
        skillCs.UseSkill();
    }
}
