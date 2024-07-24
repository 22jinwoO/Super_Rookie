using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnitData : MonoBehaviour, IBaseUnitData
{
    [field: SerializeField]
    public CharacterNumber CharacterId { get; set; }

    [field: SerializeField]
    public CharacterType CharacterType { get; set; }

    [field: SerializeField]
    public float Max_Hp { get; set; }

    [field: SerializeField]
    public float Unit_Hp { get; set; }

    [field: SerializeField]
    public float UnitSpeed { get; set; }

    [field: SerializeField]
    public float DefaultAtkDmg { get; set; }

    [field: SerializeField]
    public float AtkDmg { get; set; }

    [field: SerializeField]
    public float AtkRange { get; set; }

    [field: SerializeField]
    public float UnitAtkCoolTime { get; set; }

    [field: SerializeField]
    public int UnitSkillRange { get; set; }

    [field: SerializeField]
    public float UnitSkillCoolTime { get; set; }

    [field: SerializeField]
    public float UnitSightRange { get; set; }

    [field: SerializeField]
    public float Current_AtkCoolTime { get; set; }

    [field: SerializeField]
    public float Current_SkillCoolTime { get; set; }

    [field: SerializeField]
    public bool CanAct { get; set; }

    [field: SerializeField]
    public bool CanAtk { get; set; }

    [field: SerializeField]
    public bool UseSkill { get; set; }
    public Rigidbody2D Rigid { get; set; }
    public ISearchTarget SearchTarget { get; set; }
    public IStatusEffect IsStun { get; set; }
    public IUnitController Controller { get; set; }

    [field : SerializeField]
    public Transform BasePos { get; set; }
    public Vector2 DefaultPos { get; set; }
    public CapsuleCollider2D CapsuleCollider { get; set; }
    public Animator Anim { get; set; }
    public UnitDamaged UnitDamaged { get; set; }

    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashUseSkill = Animator.StringToHash("isSkillAttack");

    public List<Dictionary<string, object>> data_Dialog;

    private void Awake()
    {
        // UnitData CSV 파일 불러오기
        data_Dialog = CSVReader.Read("UnitData");

        InitComponent();
    }

    void Update()
    {
        CheckAttackCoolTime();
        CheckSkillCoolTime();
    }

    public void InitComponent()
    {
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        SearchTarget = GetComponent<ISearchTarget>();
        Controller = GetComponent<IUnitController>();
        Rigid = GetComponent<Rigidbody2D>();
        UnitDamaged = GetComponent<UnitDamaged>();
        Anim = GetComponentInChildren<Animator>();
    }

    public void CheckAttackCoolTime()
    {
        // 기본 공격이 가능한지 확인
        CanAtk = Current_AtkCoolTime >= UnitAtkCoolTime ? true : false;

        //현재 기본 공격 쿨타임이 유닛의 기본 공격속도 보다 낮다면 쿨타임 돌려주기
        if (!CanAtk) Current_AtkCoolTime += Time.deltaTime;
    }

    public void CheckSkillCoolTime()
    {
        // 스킬 공격이 가능한지 확인
        UseSkill = Current_SkillCoolTime >= UnitSkillCoolTime ? true : false;

        //현재 스킬 공격 쿨타임이 유닛의 스킬 공격 쿨타임 보다 낮다면 쿨타임 돌려주기
        if (!UseSkill) Current_SkillCoolTime += Time.deltaTime;
    }

    #region # InitStats() : 유닛의 기본 데이터 값 할당해주는 함수
    public virtual void InitStats(CharacterNumber characterId = CharacterNumber.Default)
    {
        // 유닛의 최대 체력
        Max_Hp = float.Parse(data_Dialog[(int)characterId]["MaxHp"].ToString());

        // 유닛의 이동속도
        UnitSpeed = float.Parse(data_Dialog[(int)characterId]["UnitSpeed"].ToString());

        // 유닛의 공격력
        AtkDmg = float.Parse(data_Dialog[(int)characterId]["Atk_Dmg"].ToString());

        // 유닛의 기본 공격 사거리
        AtkRange = float.Parse(data_Dialog[(int)characterId]["Atk_Range"].ToString());

        // 유닛의 기본 공격 쿨타임
        UnitAtkCoolTime = float.Parse(data_Dialog[(int)characterId]["Atk_CoolTime"].ToString());

        // 유닛의 스킬 공격 사거리
        UnitSkillRange = int.Parse(data_Dialog[(int)characterId]["Skill_Range"].ToString());

        // 유닛의 스킬 공격 쿨타임
        UnitSkillCoolTime = float.Parse(data_Dialog[(int)characterId]["Skill_CoolTime"].ToString());

        // 유닛의 적 탐지 사거리
        UnitSightRange = float.Parse(data_Dialog[(int)characterId]["Sight_Range"].ToString());

        // 유닛의 현재 체력 = 유닛의 최대 체력
        Unit_Hp = Max_Hp;
    }
    #endregion

    public void InitValue(float value = 1)
    {
        InitStats(characterId: CharacterId);

        // 오브젝트 타입이 몬스터 일 경우 - 스테이지 단계 N 당 몬스터 공격력, 체력 N * 10%씩 강화
        if (CharacterType.Equals(CharacterType.Monster))
        {
            Max_Hp *= (1 + value * 0.1f);
            AtkDmg *= (1 + value * 0.1f);
        }

        // 오브젝트 타입이 보스몬스터 일 경우 - 스테이지 단계 N 당 몬스터 공격력, 체력 N * 40%씩 강화
        if (CharacterType.Equals(CharacterType.BossMonster))
        {
            Max_Hp *= (1 + value * 0.4f);
            AtkDmg *= (1 + value * 0.4f);
        }

        OffValue();
    }

    public void OnValue()
    {
        Unit_Hp = Max_Hp;

        // 타겟 없음으로 변경
        SearchTarget.TargetUnit = null;

        // 리지드바디 회전 고정
        Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 유닛의 투명도 1로 변경
        for (int i = 0; i < UnitDamaged.bodySprs.Length; i++)
            UnitDamaged.bodySprs[i].color = new Color(1f, 1f, 1f, 1f);

        // 상태이상 효과 해제
        IsStun = null;

        // 콜라이더 활성화
        CapsuleCollider.enabled = true;

        // 공격 애니메이션 취소
        Anim.SetBool(hashAttack, false);

        // 스킬 공격 애니메이션 취소
        Anim.SetBool(hashUseSkill, false);

        // 컨트롤러 동작 상태 Idle로 변경
        Controller.Action = UnitAction.Idle;

        // 유닛의 동작 시 기준이 되는 스크립트 null로 변경
        Controller.UnitState = null;

        // 이 유닛이 몬스터라면 StageManager의 이벤트 연결
        if (gameObject.CompareTag("Monster")) StageManager.Instance.OnDeadAllPlayer += UnitDead;

        // 유닛 행동 가능하도록 변경
        CanAct = true;

    }

    public void OffValue()
    {
        Controller.Action = UnitAction.Idle;

        Anim.SetBool("isWalk", false);

        Anim.SetBool(hashAttack, false);

        Anim.SetBool(hashUseSkill, false);

        Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        SearchTarget.TargetUnit = null;

        CapsuleCollider.enabled = false;

        Controller.UnitState = null;

        CanAct = false;
    }

    public void UnitDead()
    {
        CapsuleCollider.enabled = false;
        Unit_Hp = 0f;
        CanAct = false;

        UnitDamaged.DeadCheck();
    }
}
