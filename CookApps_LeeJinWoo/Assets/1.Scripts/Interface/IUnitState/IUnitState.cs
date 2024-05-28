using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitActState
{
    // 행동 진입 시 호출
    public void Enter();

    // 행동 중 호출
    public void DoAction();

    // 행동 그만둘 때 호출
    public void Exit();
}

