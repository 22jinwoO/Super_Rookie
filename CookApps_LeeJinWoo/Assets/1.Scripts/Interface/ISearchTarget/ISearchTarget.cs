using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISearchTarget
{
    // 유닛이 타겟으로 할 대상
    public Transform _targetUnit { get; set; }

    public CapsuleCollider2D _targetColider { get; set; }

    // 유닛의 행동 상태
    public IUnitActState _actStateCs { get; set; }

    // 타겟을 탐지하는 함수 선언
    public void SearchTarget();
}
