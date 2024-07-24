using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISearchTarget
{
    // 적 탐지 확인
    public bool IsSearch { get; set; }

    // 유닛이 타겟으로 할 대상
    public Transform TargetUnit { get; set; }

    public CapsuleCollider2D TargetColider { get; set; }

    // 유닛의 행동 상태
    public IUnitActState ActState { get; set; }

    // 타겟을 탐지하는 함수 선언
    public void SearchTarget();
}
