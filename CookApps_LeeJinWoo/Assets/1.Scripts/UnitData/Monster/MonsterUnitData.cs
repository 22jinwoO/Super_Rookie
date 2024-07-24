using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUnitData : BaseUnitData
{
    // 스폰 위치값
    public Vector2 Default_Pos { get; set; }

    // 몬스터 팩토리
    public IMonsterFactory Factory { get; set; }

    // 드랍 골드
    [field : SerializeField]
    public int DropGoldValue { get; set; }

    // 드랍 경험치
    [field: SerializeField]
    public float DeathExpValue { get; set; }
}
