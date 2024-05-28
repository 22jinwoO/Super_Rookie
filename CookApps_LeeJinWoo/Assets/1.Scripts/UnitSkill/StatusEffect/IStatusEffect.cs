using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEffect
{
    // 상태이상 효과 이펙트
    public GameObject _skillVfx { get; set; }

    // 상태 이상 지속 시간
    public float _statusEffecttime { get; set; }

    // 상태이상 적용하는 함수 정의

    public void OnStatusEffect(float StatusEffecttime);
}
