using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : Singleton<ExpManager>
{
    [Header("플레이어 캐릭터들")]
    [SerializeField]
    private PlayerUnitData[] p_unitDatas;

    [Header("플레이어 캐릭터들 경험치 스크립트들")]
    [SerializeField]
    private PlayerExp[] p_unit_Exps;

    #region # Check_Exp() 몬스터가 죽을 때, 호출하여 플레이어 캐릭터들에게 경험치를 부여하는 함수
    public void Check_Exp(CharacterType characterType, float expValue)
    {
        for(int i = 0; i < p_unitDatas.Length; i++)
        {
            // 플레이어 캐릭터가 살아있을 경우에만 경험치 부여
            if(p_unitDatas[i].gameObject.activeSelf)
            {
                // 자신을 처치한 유닛과 캐릭터 타입이 같을 경우 expValue 원본 값 추가경험치 획득
                if (p_unitDatas[i].CharacterType.Equals(characterType))
                    p_unit_Exps[i].CalculateExpValue = expValue;

                // 자신을 처치한 유닛과 다른 플레이어 유닛일 경우 경험치 획득량 1/2
                else
                    p_unit_Exps[i].CalculateExpValue = (expValue * 0.5f);
            }
        }
    }
    #endregion
}
