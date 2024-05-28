using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitController
{
    public Rigidbody2D _rigid { get; set; }

    // 유닛 동작 타입
    public UnitAction actionState { get; set; }

    // 클래스들의 접근점(인터페이스)
    public IUnitActState _unitState { get; set; }

    // 유닛들 동작 시 호출되는 함수
    public void setActionType(UnitAction state);
}
