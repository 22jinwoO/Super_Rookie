using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("스폰할 위치들")]
    [SerializeField]
    private Transform[] spawnPoints;

    [Header("보스 유닛 종류")]
    [SerializeField]
    private MonsterUnitData[] bossUnits;

    [Header("보스 생성 이펙트")]
    [SerializeField]
    private GameObject bossSpawn_Vfx;

    [SerializeField]
    private StageManager stageMgrCs;

    private void Awake()
    {
        //이펙트 준비
        bossSpawn_Vfx = Instantiate(bossSpawn_Vfx);

        bossSpawn_Vfx.SetActive(false);
        bossSpawn_Vfx.transform.SetParent(transform);

        // 보스 프리팹 준비
        for (int i = 0; i < bossUnits.Length; i++)
        {
            bossUnits[i] = Instantiate(bossUnits[i]);
            bossUnits[i].gameObject.SetActive(false);
            bossUnits[i].CharacterId = CharacterID.Boss_1;
            bossUnits[i].CharacterType = CharacterType.BossMonster;
            bossUnits[i].transform.SetParent(transform);
        }

    }
    public void SpawnBoss()
    {
        // 몬스터 수를 랜덤하기 생성하기 위한 랜덤값 할당
        int rand = Random.Range(0, bossUnits.Length);

        // 몬스터 종류를 랜덤하게 생산할 수 있도록 랜덤 숫자에 해당하는 몬스터 팩토리 지정
        int randMonsterKind = Random.Range(0, bossUnits.Length);

        // 지정된 몬스터 팩토리의 몬스터 반환
        MonsterUnitData spawnMonster = bossUnits[randMonsterKind];

        // 랜덤한 위치에서 생성될 수 있도록 스폰포인트 갯수 중 랜덤 숫자 추출
        int randSpawnPoint = Random.Range(0, spawnPoints.Length);

        // 지역변수 선언
        Vector2 spawnPoint = Vector2.zero;

        // 좌표값의 마이너스 값인 부분 랜덤한 값으로 할당
        float minusValue = Random.Range(-4f, -2f);

        // 좌표값의 플러스 값인 부분 랜덤한 값으로 할당
        float plusValue = Random.Range(2f, 4f);

        switch (randSpawnPoint)
        {
            // 1 사분면
            case 0:
                spawnPoint = (Vector2)spawnPoints[randSpawnPoint].position + new Vector2(minusValue, plusValue);
                break;

            // 2 사분면
            case 1:
                spawnPoint = (Vector2)spawnPoints[randSpawnPoint].position + new Vector2(plusValue, plusValue);
                break;

            // 3 사분면
            case 2:
                spawnPoint = (Vector2)spawnPoints[randSpawnPoint].position + new Vector2(minusValue, minusValue);
                break;

            // 4 사분면
            case 3:
                spawnPoint = (Vector2)spawnPoints[randSpawnPoint].position + new Vector2(plusValue, minusValue);
                break;

        }
        bossSpawn_Vfx.transform.position = spawnPoint + new Vector2(0, -0.7f);

        // vfx활성화
        bossSpawn_Vfx.SetActive(true);

        // 기본 세팅 off
        spawnMonster.OffValue();

        // 스폰되는 몬스터 위치 랜덤한 위치로 조정
        spawnMonster.transform.position = spawnPoint;

        spawnMonster.InitValue(value: stageMgrCs.cur_Stage);

        //몬스터 스폰 연출 함수 호출
        StartCoroutine(monsterSpawnProduction(monster: spawnMonster, spawnPoint : spawnPoint));
    }

    private IEnumerator monsterSpawnProduction(MonsterUnitData monster, Vector2 spawnPoint)
    {
        // 보스 몬스터 오브젝트 활성화
        monster.gameObject.SetActive(true);

        // 연출을 위한 몬스터의 데미지 스크립트 가져오기
        UnitDamaged spawnMonsterBody = monster.GetComponent<UnitDamaged>();

        // 몬스터 데미지 스크립트의 바디 스프라이트 렌더러 수만큼 반복
        for (int i = 0; i < spawnMonsterBody.bodySprs.Length; i++)
        {
            spawnMonsterBody.bodySprs[i].color = new Color(1f, 1f, 1f, 0f);
        }

        // 5번 반복
        for (int i = 0; i <= 5; i++)
        {
            // 바디 스프라이트 렌더러 개수만큼 반복
            for (int j = 0; j < spawnMonsterBody.bodySprs.Length; j++)
            {
                // 증가되는 i 값에 0.2f를 곱해서 투명도 값에 할당
                spawnMonsterBody.bodySprs[j].color = new Color(1f, 1f, 1f, i * 0.2f);
            }

            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(0.75f);

        // 유닛 생성 시 초기값 할당하는 함수 호출 , 스테이지 단계 N 당 몬스터 공격력, 체력 N * 40%씩 강화
        monster.OnValue();
    }

}
