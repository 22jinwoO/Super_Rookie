using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterFactory
{
    // 몬스터 프리팹
    public UnitData _monsterPrefab { get; set; }

    // 오브젝트 풀링을 위한 몬스터 유닛 풀
    public Stack<UnitData> _monsters { get; set; }

    // InitObjPool() : 오브젝트 풀링 초기값 셋팅하는 함수
    public void InitObjPool(UnitData monsterPref);


    // 몬스터 유닛 생산하는 생성자 함수
    public UnitData CreateMonsterUnit();

}
