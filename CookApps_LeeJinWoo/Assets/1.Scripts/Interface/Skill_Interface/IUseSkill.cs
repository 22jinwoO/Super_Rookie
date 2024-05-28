using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseSkill
{
    // 스킬 이펙트 게임오브젝트
    public GameObject _skillVFx { get; set; }

    // 스킬 사용 시 호출되는 함수
    public void UseSkill();
}
