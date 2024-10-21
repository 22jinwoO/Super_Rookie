using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseUnitData
{
    public CharacterID CharacterId { get; set; }
    public CharacterType CharacterType { get; set; }
    public float Max_Hp { get; set; }
    public float Unit_Hp { get; set; }
    public float UnitSpeed { get; set; }
    
    public float AtkDmg { get; set; }
    public float AtkRange { get; set; }
    public float UnitAtkCoolTime { get; set; }
    public int UnitSkillRange { get; set; }
    public float UnitSkillCoolTime { get; set; }
    public float UnitSightRange { get; set; }
    public float Current_AtkCoolTime { get; set; }
    public float Current_SkillCoolTime { get; set; }
    public bool CanAct { get; set; }
    public bool CanAtk { get; set; }
    public bool UseSkill { get; set; }
    public Rigidbody2D Rigid { get; set; }
    public CapsuleCollider2D CapsuleCollider { get; set; }
    public Animator Anim { get; set; }
    public ISearchTarget SearchTarget { get; set; }
    public IStatusEffect IsStun { get; set; }
    public IUnitController Controller { get; set; }
    public Transform BasePos { get; set; }
    public Vector2 DefaultPos { get; set; }
    public UnitDamaged UnitDamaged { get; set; }

    public void CheckAttackCoolTime();
    public void CheckSkillCoolTime();
    public void InitComponent();
    public void InitStats(CharacterID characterId = CharacterID.Default);
    public void InitValue(float value = 1);
    public void OffValue();
    public void UnitDead();
}
