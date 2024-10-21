using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : MonoBehaviour
{
    [Header("====== 캐릭터 스테이터스 ======\n\n기본 체력 값")]
    public float default_HpValue;

    [Header("기본 일반 공격력")]
    public float default_AtkDmg;

    [Header("기본 일반 공격 사거리")]
    public float default_AtkRange;

    [Header("기본 일반 공격 쿨타임")]
    [SerializeField]
    private float default_AtkCoolTime;

    [Header("기본 스킬 사거리")]
    [SerializeField]
    public int default_SkillRange;

    [Header("기본스킬 쿨타임")]
    public float default_SkillCoolTime;

    [Header("기본 추적 사거리")]
    public float default__sightRange;

    [Header("==============\n\n유닛 타입")]
    public CharacterType characterType;

    [Header("캐릭터 레벨")]
    public int _unit_Level;

    [Header("캐릭터 최대 HP")]
    public float _max_Hp;

    [Header("캐릭터 HP")]
    public float _unit_Hp;

    [Header("유닛 이동속도")]
    public float _unit_Speed;

    [Header("일반 공격력")]
    public float _unit_AtkDmg;

    [Header("일반 공격 사거리")]
    public float _unit_AtkRange;

    [Header("일반 공격 쿨타임")]
    [SerializeField]
    private float unit_AtkCoolTime;

    [Header("스킬 사거리")]
    [SerializeField]
    public int unit_SkillRange;

    [Header("스킬 쿨타임")]
    public float unit_SkillCoolTime;

    [Header("적 추적 사거리")]
    public float _unit_sightRange;

    [Header("현재 일반 공격 쿨타임")]
    public float _current_AtkCoolTime;

    [Header("현재 스킬 쿨타임")]
    public float _current_SkillCoolTime;

    [Header("드랍 골드")]
    public int _dropGoldValue;

    [Header("죽었을 경우 경험치 전댤값")]
    public float _deathExpValue;

    [Header("레벨업 시 추가 공격력")]
    public float _level_Plus_AtkStat;

    [Header("레벨업 시  추가 체력")]
    public float _level_Plus_HpStat;

    [Header("스탯창의 추가 공격력")]
    public float _plus_AtkStat;

    [Header("스탯창의 추가 체력")]
    public float _plus_HpStat;


    // ===========================================


    [Header("적탐지 유무")]
    public bool _isSearch;

    [Header("현재 행동 가능 유무")]
    public bool _canAct;

    [Header("공격 가능 유무")]
    public bool _canAtk;

    [Header("스킬 사용 유무")]
    public bool _useSkill;

    [Header("상태이상 상태 : 기절")]
    public IStatusEffect isStun;

    [Header("베이스 포지션")]
    public Transform base_Pos;

    [Header("원래 위치")]
    public Vector2 default_Pos;

    [Header("해당 유닛의 팩토리")]
    public IMonsterFactory _factory;

    // ===================

    private CapsuleCollider2D capsuleCollider;

    private ISearchTarget searchTargetCs;

    private IUnitController controllerCs;

    private UnitDamaged unitDamagedCs;

    [SerializeField]
    private Rigidbody2D rigid;

    private Animator anim;

    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashUseSkill = Animator.StringToHash("isSkillAttack");

    

    private void Awake()
    {
        List<Dictionary<string, object>> data_Dialog = CSVReader.Read("UnitData");
        print(data_Dialog.Count);
        for (int i = 0; i < data_Dialog.Count; i++)
        {
            print(data_Dialog[i]["MaxHp"].ToString());
            print(data_Dialog[i]["UnitSpeed"].ToString());
            print(data_Dialog[i]["Atk_Dmg"].ToString());
            print(data_Dialog[i]["Atk_Range"].ToString());
            print(data_Dialog[i]["Atk_CoolTime"].ToString());
            print(data_Dialog[i]["Skill_Range"].ToString());
            print(data_Dialog[i]["Skill_CoolTime"].ToString());
            print(data_Dialog[i]["Sight_Range"].ToString());
        }

        _max_Hp = default_HpValue;

        _unit_Hp = _max_Hp;

        _unit_AtkDmg = default_AtkDmg;
        _unit_AtkRange = default_AtkRange;

        unit_AtkCoolTime = default_AtkCoolTime;
        unit_SkillRange = default_SkillRange;
        unit_SkillCoolTime = default_SkillCoolTime;

        _unit_sightRange = default__sightRange;

        _current_AtkCoolTime = 0f;
        _current_SkillCoolTime = 0f;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        searchTargetCs = GetComponent<ISearchTarget>();
        controllerCs = GetComponent<IUnitController>();
        rigid = GetComponent<Rigidbody2D>();
        unitDamagedCs = GetComponent<UnitDamaged>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Unit_Attack_Skill_CoolTime();
    }

    #region # Unit_Attack_Skill_CoolTime() : 유닛 기본공격, 스킬공격 쿨타임 돌려주는 함수
    public void Unit_Attack_Skill_CoolTime()
    {
        // 기본 공격이 가능한지 확인
        _canAtk =  _current_AtkCoolTime >= unit_AtkCoolTime ? true : false;

        // 스킬 공격이 가능한지 확인
        _useSkill = _current_SkillCoolTime >= unit_SkillCoolTime ? true : false;


        //현재 기본 공격 쿨타임이 유닛의 기본 공격속도 보다 낮다면 쿨타임 돌려주기
        if (!_canAtk)
            _current_AtkCoolTime += Time.deltaTime;


        //현재 스킬 공격 쿨타임이 유닛의 스킬 공격 쿨타임 보다 낮다면 쿨타임 돌려주기
        if (!_useSkill)
            _current_SkillCoolTime += Time.deltaTime;


    }
    #endregion

    public void OffValue()
    {
        _unit_Hp = _max_Hp;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        searchTargetCs.TargetUnit = null;

        capsuleCollider.enabled = false;

        anim.SetBool(hashAttack, false);
        anim.SetBool(hashUseSkill, false);
        controllerCs.Action = UnitState.Idle;
        controllerCs.UnitAct = null;

        _canAct = false;
    }

    #region
    public void InitValue(float value = 1)
    {
        // 오브젝트 타입이 몬스터 일 경우 - 스테이지 단계 N 당 몬스터 공격력, 체력 N * 10%씩 강화
        if (characterType.Equals(CharacterType.Monster) || characterType.Equals(CharacterType.BossMonster))
        {
            _max_Hp = default_HpValue * (1 + value * 0.1f);
            _unit_AtkDmg = default_AtkDmg * (1 + value * 0.1f);
        }

        // 오브젝트 타입이 보스몬스터 일 경우 - 스테이지 단계 N 당 몬스터 공격력, 체력 N * 40%씩 강화
        if (characterType.Equals(CharacterType.BossMonster))
        {
            _max_Hp = default_HpValue * (1 + value * 0.4f);
            _unit_AtkDmg = default_AtkDmg * (1 + value * 0.4f);
        }


        // 현재 Hp = 최대 Hp
        _unit_Hp = _max_Hp;

        // 타겟 없음으로 변경
        searchTargetCs.TargetUnit = null;

        // 리지드바디 회전 고정
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 유닛의 투명도 1로 변경
        for (int i = 0; i < unitDamagedCs.bodySprs.Length; i++)
            unitDamagedCs.bodySprs[i].color = new Color(1f, 1f, 1f, 1f);

        // 상태이상 효과 해제
        isStun = null;

        // 콜라이더 활성화
        capsuleCollider.enabled = true;

        // 공격 애니메이션 취소
        anim.SetBool(hashAttack, false);

        // 스킬 공격 애니메이션 취소
        anim.SetBool(hashUseSkill, false);

        // 컨트롤러 동작 상태 Idle로 변경
        controllerCs.Action = UnitState.Idle;

        // 유닛의 동작 시 기준이 되는 스크립트 null로 변경
        controllerCs.UnitAct = null;

        // 이 유닛이 몬스터라면 StageManager의 이벤트 연결
        if(gameObject.CompareTag("Monster"))
            StageManager.Instance.OnDeadAllMonster += UnitDead;

        // 유닛 행동 가능하도록 변경
        _canAct = true;
    }
    #endregion

    #region
    public void UnitDead()
    {
        capsuleCollider.enabled = false;
        _unit_Hp = 0f;
        _canAtk = false;

        unitDamagedCs.DeadCheck();
    }
    #endregion

    #region
    public void Plus_PlayerStat(PlusStats stat, float plusValue)
    {
        switch (stat)
        {
            case PlusStats.Attack:
                _plus_AtkStat += plusValue;
                _unit_AtkDmg = default_AtkDmg + _level_Plus_AtkStat + _plus_AtkStat;
                break;

            case PlusStats.Hp:
                _plus_HpStat += plusValue;
                _max_Hp = default_HpValue + _level_Plus_HpStat + _plus_HpStat;
                break;
        }
    }
    #endregion

}
