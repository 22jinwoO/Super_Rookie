using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillAtk : MonoBehaviour
{
    [SerializeField]
    private UnitData unitDataCs;

    [SerializeField]
    private ISearchTarget targetDamagedCs;


    [SerializeField]
    private IUseSkill skillCs;

    private void Awake()
    {
        unitDataCs = transform.parent.GetComponent<UnitData>();
        targetDamagedCs = unitDataCs.transform.GetComponent<ISearchTarget>();
        skillCs = transform.parent.GetComponent<IUseSkill>();
        print(skillCs);
    }

    // 스킬을 사용할 때 호출되는 함수
    public void UseSkill_Atk()
    {
        // 캐릭터 타입이 힐러가 아니고, 타겟이 없을 경우 탈출
        if (!unitDataCs.characterType.Equals(CharacterType.Healer) && targetDamagedCs._targetUnit == null)
            return;

        // 이외의 경우 유닛 쿨타임 0으로 전환
        unitDataCs._current_SkillCoolTime = 0f;

        // 내가 갖고 있는 스킬 사용
        skillCs.UseSkill();
    }
}
