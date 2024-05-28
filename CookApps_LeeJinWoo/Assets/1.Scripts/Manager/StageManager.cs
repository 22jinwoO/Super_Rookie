using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class StageManager : Singleton<StageManager>
{
    [Header("현재 스테이지")]
    public float cur_Stage;

    [Header("다음 스테이지로 넘어가기 위한 몬스터 처치 수")]
    public int next_deathMonsterCnt;

    [Header("현재 처치한 몬스터 수")]
    public int deathMonsterCnt;

    [Header("몬스터 스폰 매니저 스크립트")]
    public MonsterSpawnManager monsterSpawnMgrCs;

    [Header("UI 매니저 스크립트")]
    public UIManager uiMgrCs;

    [Header("타일맵")]
    [SerializeField]
    private Tilemap mapTile;

    //델리게이트 선언
    public delegate void playerAllDead();

    //이벤트 선언
    public event playerAllDead OnDeadAllPlayer;

    [Header("보스 스테이지 유무")]
    public bool isBossStage;

    [Header("플레이어 스크립트")]
    public Player playerCs;

    [Header("플레이어 스폰 매니저스크립트")]
    [SerializeField]
    private PlayerSpawnManager playerSpawnMgrCS;

    public bool gameEnd;

    private void Awake()
    {
        cur_Stage = 1;
        next_deathMonsterCnt = 5;
    }

    private void Update()
    {
        //다음 스테이지로 넘어갈 조건이 되는지 확인하는 함수 호출
        Check_CanNextStage();
    }

    #region # Check_DeadMonsters() : 다음 스테이지로 넘어갈 조건이 되는지 확인하는 함수
    public void Check_CanNextStage()
    {
        // 현재 처치한 몬스터 수가 다음 스테이지로 넘어가기 위한 몬스터의 수와 같거나 클 경우
        if (deathMonsterCnt >= next_deathMonsterCnt)
            StartCoroutine(Boss_Stage());

        if (playerSpawnMgrCS.isPlayer_All_Dead)
        {
            monsterSpawnMgrCs.canSpawn = false;
        }

        if(gameEnd)
        {
            StartCoroutine(uiMgrCs.GameFail());
            gameEnd = false;
        }
            
        
    }
    #endregion

    #region # Boss_Stage() : 보스 스테이지 시작 시 호출되는 함수
    private IEnumerator Boss_Stage()
    {
        // 현재 죽은 몬스터 수 0
        deathMonsterCnt = 0;

        // 현재 보스 스테이지임을 알림
        isBossStage = true;

        // 보스 연출함수 호출
        StartCoroutine(uiMgrCs.Boss_FadeInOut());

        yield return new WaitForSecondsRealtime(1.5f);

        // 생성되어 있던 유닛들 모두 사망하게 하는 이벤트 호출
        OnDeadAllPlayer();

        // 현재 죽은 몬스터 수 0
        deathMonsterCnt = 0;

        // 몬스터 생성주기 0
        monsterSpawnMgrCs.cur_creation_cycle = 0f;

    }
    #endregion

    #region # Boss_Dead() : 보스 사망 시 호출되는 함수
    public IEnumerator Boss_Dead()
    {
        // 현재 죽은 몬스터 수 0
        deathMonsterCnt = 0;

        // 스테이지 증가
        cur_Stage++;

        // 다음 스테이지로 넘어갈 몬스터 처치수 증가
        next_deathMonsterCnt += 10;

        // 변경된 스테이지 텍스트에 반영
        uiMgrCs.stageTxt.text = $"Stage {cur_Stage}";

        // 변경된 스테이지 텍스트에 반영
        uiMgrCs.mainStageTxt.text = $"Stage {cur_Stage}";

        // 씬 페이드아웃 함수 호출
        StartCoroutine(uiMgrCs.FadeInFadeOut(false));

        mapTile.color = new Color(Random.Range(0.18f, 1f), Random.Range(0.18f, 1f), Random.Range(0.18f, 1f));

        // 스테이지 바뀌는 연출 함수 호출
        StartCoroutine(Stage_Change());

        yield return new WaitForSecondsRealtime(1f);

        // 씬 페이드인 함수 호출
        StartCoroutine(uiMgrCs.FadeInFadeOut(true));

        yield return new WaitForSecondsRealtime(1f);

        // 보스 스테이지 끝
        isBossStage = false;
    }
    #endregion

    #region # Stage_Change() : 스테이지 전환될 때 호출되는 함수, 스테이지 텍스트 연출 담당
    public IEnumerator Stage_Change()
    {
        yield return new WaitForSecondsRealtime(1f);

        // 5번 반복
        for (int i = 0; i <= 5; i++)
        {
            // 증가되는 i 값에 0.2f를 곱해서 스테이지 텍스트 투명도 값에 할당
            uiMgrCs.stageTxt.color = new Color(1f, 1f, 1f, i * 0.2f);

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSecondsRealtime(1f);

        // 5번 반복
        for (int i = 5; i >= 0; i--)
        {
            // 증가되는 i 값에 0.2f를 곱해서 스테이지 텍스트 투명도 값에 할당
            uiMgrCs.stageTxt.color = new Color(1f, 1f, 1f, i * 0.2f);

            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion

    #region # GetPlayerGold() : 플레이어가 몬스터 처치 시 호출하여 플레이어에게 골드를 부여하는 함수
    public void GetPlayerGold(int monsterGold)
    {
        // 플레이어에 골드량 반영
        playerCs.goldValue += monsterGold;

        // 텍스트에 플레이어 골드량 반영
        uiMgrCs.playerGold_ValueTxt.text = $"{playerCs.goldValue}";
    }
    #endregion
}
