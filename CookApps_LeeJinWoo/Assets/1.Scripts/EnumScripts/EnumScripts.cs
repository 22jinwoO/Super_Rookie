
// 유닛 동작
public enum UnitAction
{
    Default = 0,
    Idle = 1,
    Move = 2,
    ReturnMove = 3,
    Tracking = 4,
    Attack = 5,
    UseSkill = 6,
    Dead = 7,
    End = Dead +1
}

// 유닛 타입
public enum CharacterType
{
    Default = 0,
    Tanker = 1,
    Meleer = 2,
    Ranged_Dealer = 3,
    Healer = 4,
    Monster = 5,
    BossMonster = 6,
    End = BossMonster + 1
}

// 증가시킬 유닛의 데이터 항목 구분
public enum PlusStas
{
    Default = 0,
    Attack = 1,
    Hp = 2,
    End = Hp + 1
}