using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitDamaged : MonoBehaviour
{
    [Header("유닛 데이터 스크립트")]
    [SerializeField]
    private BaseUnitData unitData;

    [Header("유닛의 컨트롤러")]
    [SerializeField]
    private IUnitController controllerCs;

    [Header("유닛 바디 스프라이트들")]
    public SpriteRenderer[] bodySprs;

    [Header("유닛 애니메이터")]
    [SerializeField]
    private Animator anim;

    [Header("유닛 콜라이더")]
    [SerializeField]
    private CapsuleCollider2D myColider;

    [Header("나를 공격한 유닛")]
    [SerializeField]
    public BaseUnitData _unit_attacked_Me;

    [Header("체력바")]
    [SerializeField]
    public Image hpBar;


    //[Header("플레이어 캐릭터 스폰 매니저")]
    //[SerializeField]
    //private PlayerSpawnManager p_SpawnManager;

    // 애니메이터 값 해싱
    private readonly int hashDead = Animator.StringToHash("isDead");
    private readonly int hashHurt = Animator.StringToHash("isHurt");

    private bool isPlayerUnit;

    private void Awake()
    {
        // 컴포넌트 및 기본값 세팅해주는 함수 호출
        InitComponent();
        isPlayerUnit = unitData.CharacterType.Equals(CharacterType.Tanker) || unitData.CharacterType.Equals(CharacterType.Meleer) || unitData.CharacterType.Equals(CharacterType.Ranged_Dealer) || unitData.CharacterType.Equals(CharacterType.Healer);
    }

    private void Update()
    {
        if(isPlayerUnit && hpBar != null)
        {
            // 캐릭터 방향 전환시 체력바도 같이 방향 전환
            hpBar.transform.localScale = transform.localScale;

            // 체력바에 유닛 hp 반영
            hpBar.fillAmount = unitData.Unit_Hp / unitData.Max_Hp;

            // Green Color
            if (hpBar.fillAmount > 0.7f)
                hpBar.color = new Color(0.2f, 0.8f, 0f, 1f); 

            // Orange Color
            else if (hpBar.fillAmount > 0.4f)
               hpBar.color = new Color(1f, 0.46f, 0f, 1f);

            // Red Color
            else
                hpBar.color = new Color(1f, 0.09f, 0f, 1f);

        }

        else if(!isPlayerUnit && hpBar != null)
        {
            // 캐릭터 방향 전환시 체력바도 같이 방향 전환
            hpBar.transform.localScale = transform.localScale;

            // 체력바에 유닛 hp 반영
            hpBar.fillAmount = unitData.Unit_Hp / unitData.Max_Hp;
        }
    }

    #region # InitComponent() : 컴포넌트 및 기본값 세팅해주는 함수
    private void InitComponent()
    {
        // 애니메이터 가져오기
        anim = GetComponentInChildren<Animator>();

        // 유닛 컨트롤러 가져오기
        controllerCs = GetComponentInChildren<IUnitController>();

        // 콜라이더 가져오기
        myColider = GetComponent<CapsuleCollider2D>();

        // 유닛 데이터 가져오기
        unitData = GetComponent<BaseUnitData>();
        
        // 바디 스프라이트 렌더러 가져오기
        bodySprs = GetComponentsInChildren<SpriteRenderer>();

    }
    #endregion

    #region # GetDamaged(float AtkDmg) : 공격하는 유닛이 타겟에 데미지를 줄 때 호출하는 함수
    public void GetDamaged(float AtkDmg)
    {
        // 공격한 유닛의 공격력만큼 Hp 감소
        unitData.Unit_Hp -= AtkDmg;

        // 유닛의 사망 유무 판단하는 함수 호출
        DeadCheck();
    }
    #endregion

    #region # Get_DamagedBody() : 피격 시 오브젝트 깜빡이는 연출을 담당하는 함수
    private IEnumerator Get_DamagedBody()
    {

        // 3 번 동작하기 위한 반복문
        for (int i = 0; i < 3; i++)
        {
            // 바디 스프라이트 렌더러 개수만큼 반복
            for (int j = 0; j < bodySprs.Length; j++)
            {
                // 스프라이트 렌더러 컬러 변경
                bodySprs[j].color = new Color(0.6f, 0.6f, 0.6f);
            }

            yield return new WaitForSeconds(0.1f);

            // 바디 스프라이트 렌더러 개수만큼 반복
            for (int j = 0; j < bodySprs.Length; j++)
            {
                // 스프라이트 렌더러 컬러 기존 값으로 전환
                bodySprs[j].color = new Color(1f, 1f, 1f);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion

    #region # DeadBody() : 유닛이 죽었을 때 호출되는 함수
    private IEnumerator DeadBody()
    {
        // 사망했을 경우 현재 위치에 움직임 멈춤
        controllerCs.Rigid.MovePosition(transform.position);

        // 콜라이더 비활성화
        myColider.enabled = false;

        // 죽음 애니메이션 호출
        anim.SetTrigger(hashDead);

        // 유닛이 몬스터일 경우
        if (unitData.CharacterType.Equals(CharacterType.Monster))
        {
            // 보스 생성되기 전 호출되는 이벤트 해제
            StageManager.Instance.OnDeadAllPlayer -= unitData.UnitDead;

            // 몬스터를 공격한 플레이어 유닛이 있을 경우
            // => 보스 스테이지 시작 전 이벤트가 호출되었을 때, 플레이어 유닛이 처치한 몬스터가 아니여도, 스테이지 몬스터 처치 수가 증가되는 것을 방지하기 위함
            if (_unit_attacked_Me != null)
            {
                // 스테이지의 몬스터 처치 수 증가
                StageManager.Instance.deathMonsterCnt++;


                MonsterUnitData monsterData = unitData as MonsterUnitData;

                // 처치한 보스 몬스터의 골드량 플레이어에게 전달
                StageManager.Instance.GetPlayerGold(monsterData.DropGoldValue);


                // ExpManager에 몬스터를 처치한 유닛, 몬스터 처치시 부여되는 경험치 량 전달
                ExpManager.Instance.Check_Exp(characterType: _unit_attacked_Me.CharacterType, expValue: monsterData.DeathExpValue);

                // 오브젝트 풀링 스택으로 푸쉬
                monsterData.Factory.Monsters.Push(monsterData);
            }
        }

        // 유닛이 보스 몬스터 일 경우
        if (unitData.CharacterType.Equals(CharacterType.BossMonster))
        {
            print("보스 몬스터 실행");
            // 몬스터를 공격한 플레이어 유닛이 있을 경우
            // => 보스 스테이지 시작 전 이벤트가 호출되었을 때, 플레이어 유닛이 처치한 몬스터가 아니여도, 스테이지 몬스터 처치 수가 증가되는 것을 방지하기 위함
            if (_unit_attacked_Me != null)
            {
                // 보스 생성되기 전 호출되는 이벤트 해제
                StageManager.Instance.OnDeadAllPlayer -= unitData.UnitDead;

                MonsterUnitData monsterData = unitData as MonsterUnitData;

                // 처치한 보스 몬스터의 골드량 플레이어에게 전달
                StageManager.Instance.GetPlayerGold(monsterData.DropGoldValue);


                // 보스 사망 알리는 함수 호출
                StageManager.Instance.StartCoroutine(StageManager.Instance.Boss_Dead());
                

                // ExpManager에 몬스터를 처치한 유닛, 몬스터 처치시 부여되는 경험치 량 전달
                ExpManager.Instance.Check_Exp(characterType: _unit_attacked_Me.CharacterType, expValue: monsterData.DeathExpValue);
            }
        }



        // 죽음 연출을 담당하는 부분

        // 바디 스프라이트 렌더러 개수만큼 반복
        for (int i = 0; i < bodySprs.Length; i++)
        {
            // 바디 스프라이트 렌더러 컬러 기본값으로 전환
            bodySprs[i].color = new Color(1f, 1f, 1f, 1f);
        }

        // 5번 반복
        for (int i = 5; i >= 0; i--)
        {
            // 바디 스프라이트 렌더러 개수만큼 반복
            for (int j = 0; j < bodySprs.Length; j++)
            {              
                // 감소되는 i 값에 0.2f를 곱해서 투명도 값에 할당
                bodySprs[j].color = new Color(1f, 1f, 1f, i*0.2f);
            }

            yield return new WaitForSeconds(0.15f);
        }

        yield return null;

        // 오브젝트 비활성화
        gameObject.SetActive(false);
    }
    #endregion

    #region # DeadCheck() : 피격당한 유닛의 사망 유무를 판단하는 함수
    public void DeadCheck()
    {
        // 유닛의 hp가 0과 같거나 작을 경우
        if (unitData.Unit_Hp <= 0f)
        {
            // 오브젝트가 활성화되어 있는 상태일 때만 비동기 함수(죽음 연출 함수) 호출
            if(gameObject.activeSelf)
                StartCoroutine(DeadBody());
        }
            
        // 유닛의 hp가 0보다 클 경우 피격 연춣 함수 호출
        else
            StartCoroutine(Get_DamagedBody());

    }
    #endregion
}
