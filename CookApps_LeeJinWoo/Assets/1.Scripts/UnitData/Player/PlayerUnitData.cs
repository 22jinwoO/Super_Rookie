using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitData : BaseUnitData
{
    // 캐릭터 레벨
    public int Unit_Level { get; set; }

    // 스킬 사거리
    public int Unit_SkillRange { get; set; }

    // 스킬 쿨타임
    public float Unit_SkillCoolTime { get; set; }

    // 레벨업 시 추가 공격력
    public float Level_Plus_AtkStat { get; set; }

    // 레벨업 시 추가 체력
    public float Level_Plus_HpStat { get; set; }

    // 스탯창의 추가 공격력
    public float Plus_AtkStat { get; set; }

    // 스탯창의 추가 체력
    public float Plus_HpStat { get; set; }

    #region # InitStats() : 유닛의 기본 데이터 값 할당해주는 함수
    public override void InitStats(CharacterNumber characterId = CharacterNumber.Default)
    {
        // 유닛의 최대 체력
        Max_Hp = float.Parse(data_Dialog[(int)characterId]["MaxHp"].ToString());

        // 유닛의 이동속도
        UnitSpeed = float.Parse(data_Dialog[(int)characterId]["UnitSpeed"].ToString());

        // 유닛의 공격력
        DefaultAtkDmg = float.Parse(data_Dialog[(int)characterId]["Atk_Dmg"].ToString());

        // 유닛의 기본 공격 사거리
        AtkRange = float.Parse(data_Dialog[(int)characterId]["Atk_Range"].ToString());

        // 유닛의 기본 공격 쿨타임
        UnitAtkCoolTime = float.Parse(data_Dialog[(int)characterId]["Atk_CoolTime"].ToString());

        // 유닛의 스킬 공격 사거리
        UnitSkillRange = int.Parse(data_Dialog[(int)characterId]["Skill_Range"].ToString());

        // 유닛의 스킬 공격 쿨타임
        UnitSkillCoolTime = float.Parse(data_Dialog[(int)characterId]["Skill_CoolTime"].ToString());

        // 유닛의 적 탐지 사거리
        UnitSightRange = float.Parse(data_Dialog[(int)characterId]["Sight_Range"].ToString());

        // 유닛의 현재 체력 = 유닛의 최대 체력
        Max_Hp = Max_Hp + Level_Plus_HpStat + Plus_HpStat;
        Unit_Hp = Max_Hp;
        AtkDmg = DefaultAtkDmg + Level_Plus_AtkStat + Plus_AtkStat;
    }
#endregion

    public void PlusPlayerStat(PlusStats stat, float plusValue)
    {
        // 스탯 증가 로직
        switch (stat)
        {
            case PlusStats.Attack:
                Plus_AtkStat += plusValue;
                AtkDmg = DefaultAtkDmg + Plus_AtkStat + Level_Plus_AtkStat;
                break;

            case PlusStats.Hp:
                Plus_HpStat += plusValue;
                Max_Hp = Max_Hp + Plus_HpStat + Level_Plus_HpStat;
                break;
        }
    }

}

