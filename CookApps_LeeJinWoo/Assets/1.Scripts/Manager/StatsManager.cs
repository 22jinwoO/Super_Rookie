using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    [Header("스탯을 증가시킬 유닛")]
    public PlayerUnitData _thisUnit;

    //TextMeshProUGUI plusValueText,
    #region # GetStats() : 능력치를 증가시키는 해당 플레이어 캐릭터에 값 전달 어떤 능력치를 얼마만큼 증가시키는지 값 이벤트 전달, 후 다시 뷰에 증가된 데이터 전달
    public void GetStats(PlayerUnitData playerUnit, PlusStats stat, float plusValue, TextMeshProUGUI toTalValueText, TextMeshProUGUI defaultValueText ,TextMeshProUGUI plusValueText)
    {
        _thisUnit = playerUnit;

        _thisUnit.PlusPlayerStat(stat : stat, plusValue : plusValue);

        switch (stat)
        {
            case PlusStats.Attack:
                toTalValueText.text = $"{_thisUnit.AtkDmg} ";
                defaultValueText.text = $"{_thisUnit.AtkDmg + _thisUnit.Plus_AtkStat}";
                plusValueText.text = $"{_thisUnit.Plus_AtkStat}";
                break;

            case PlusStats.Hp:
                toTalValueText.text = $"{_thisUnit.Max_Hp} ";
                defaultValueText.text = $"{_thisUnit.Unit_Hp + _thisUnit.Plus_HpStat}";
                plusValueText.text = $"{_thisUnit.Plus_HpStat}";
                break;
        }
    }
    #endregion
}
