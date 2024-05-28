using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    [Header("스탯을 증가시킬 유닛")]
    public UnitData _thisUnit;

    //TextMeshProUGUI plusValueText,
    #region # GetStats() : 능력치를 증가시키는 해당 플레이어 캐릭터에 값 전달 어떤 능력치를 얼마만큼 증가시키는지 값 이벤트 전달, 후 다시 뷰에 증가된 데이터 전달
    public void GetStats(UnitData playerUnit, PlusStas stat, float plusValue, TextMeshProUGUI toTalValueText, TextMeshProUGUI defaultValueText ,TextMeshProUGUI plusValueText)
    {
        _thisUnit = playerUnit;

        _thisUnit.Plus_PlayerStat(stat : stat, plusValue : plusValue);

        switch (stat)
        {
            case PlusStas.Attack:
                toTalValueText.text = $"{_thisUnit._unit_AtkDmg} ";
                defaultValueText.text = $"{_thisUnit.default_AtkDmg + _thisUnit._level_Plus_AtkStat}";
                plusValueText.text = $"{_thisUnit._plus_AtkStat}";
                break;

            case PlusStas.Hp:
                toTalValueText.text = $"{_thisUnit._max_Hp} ";
                defaultValueText.text = $"{_thisUnit.default_HpValue + _thisUnit._level_Plus_HpStat}";
                plusValueText.text = $"{_thisUnit._plus_HpStat}";
                break;
        }
    }
    #endregion
}
