using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawnManager : MonoBehaviour
{
    [Header("플레이어 생성 주기")]
    [SerializeField]
    private float respawn_P_Cycle;

    [Header("플레이어 유닛들")]
    [SerializeField]
    private PlayerUnitData[] playerUnits;

    [Header("플레이어 현재 주기")]
    [SerializeField]
    private float[] current_Cycle;

    [Header("플레이어 현재 주기 보여주는 마스크 이미지")]
    [SerializeField]
    private Image[] unitsImgs;

    [Header("모든 플레이어 죽었는지 확인하는 변수")]
    [SerializeField]
    public bool isPlayer_All_Dead;

    [Header("스테이지 매니저 스크립트")]
    [SerializeField]
    private StageManager stageManagerCs;

    [Header("플레이어 유닛 스폰 포인트들")]
    [SerializeField]
    private Transform[] spawnPoints;

    private void Start()
    {
        for(int i = 0; i < playerUnits.Length; i++)
        {
            playerUnits[i].CharacterId = CharacterID.Tanker + i;
            playerUnits[i].CharacterType = CharacterType.Tanker + i;

            playerUnits[i].BasePos = spawnPoints[i];

            playerUnits[i].InitStats(playerUnits[i].CharacterId);

            playerUnits[i].OnValue();

        }
    }

    private void Update()
    {
        // 플레이어가 다 죽지 않았을 때만 리스폰 함수 호출
        if(!isPlayer_All_Dead)
        {
            //플레이어가 모두 죽었는지 확인하는 변수
            playerDeadCheck();

            for (int i = 0; i < playerUnits.Length; i++)
            {
                if (!playerUnits[i].gameObject.activeSelf)
                    RespawnPlayer(i);
            }
        }
    }

    // 사망한 플레이어 유닛 생성 주기 이후 재생성해주는 함수
    public void RespawnPlayer(int playerUnit_Num)
    {
        if (respawn_P_Cycle > current_Cycle[playerUnit_Num])
        {
            current_Cycle[playerUnit_Num] += Time.deltaTime;

            unitsImgs[playerUnit_Num].fillAmount = current_Cycle[playerUnit_Num] / respawn_P_Cycle;
        }

        else if (respawn_P_Cycle <= current_Cycle[playerUnit_Num])
            SetInit(playerUnit_Num);
    }
    
    //플레이어가 모두 죽었는지 확인하는 변수
    private void playerDeadCheck()
    {
        // 플레이어 캐릭터들이 전부 죽으면 게임 끝
        isPlayer_All_Dead = !playerUnits[0].gameObject.activeSelf && !playerUnits[1].gameObject.activeSelf && !playerUnits[2].gameObject.activeSelf && !playerUnits[3].gameObject.activeSelf;

        // 게임 끝인 것을 전달
        if (isPlayer_All_Dead)
            stageManagerCs.gameEnd = true;
    }

    // 재생성될 플레이어 유닛 기초값 할당해주는 함수
    private void SetInit(int playerUnit_Num)
    {
        playerUnits[playerUnit_Num].InitValue();

        playerUnits[playerUnit_Num].transform.position = playerUnits[playerUnit_Num].BasePos.position;

        playerUnits[playerUnit_Num].gameObject.SetActive(true);

        playerUnits[playerUnit_Num].OnValue();

        current_Cycle[playerUnit_Num] = 0f;

    }


}
