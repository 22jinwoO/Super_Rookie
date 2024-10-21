using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitController
{
    public Rigidbody2D Rigid { get; set; }

    // 유닛 동작 타입
    public UnitState Action { get; set; }

    // 클래스들의 접근점(인터페이스)
    public IUnitActState UnitAct { get; set; }

    // 유닛들 동작 시 호출되는 함수
    public void SetState(UnitState state);
}
