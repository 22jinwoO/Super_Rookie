using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public UnitState actionState;

    // Concrete클래스들의 접근점(인터페이스)
    public IUnitActState _unitState;

    [Header("리지드 바디")]
    public Rigidbody2D _rigid;

}
