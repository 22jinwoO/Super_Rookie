using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    [Header("스폰할 위치들")]
    [SerializeField]
    private Transform[] spawnPoints;

    [Header("현재 몬스터 생성 주기")]
    public float cur_creation_cycle;

    [Header("몬스터 생성 주기")]
    [SerializeField]
    private float creation_cycle;

    [Header("몬스터의 동시 생성 가능한 최대 수")]
    [SerializeField]
    private int maxMonster_SpawnCnt;

    [Header("몬스터 팩토리들")]
    [field : SerializeField]
    private IMonsterFactory[] monsterFactories;


    [Header("몬스터 소환 이펙트")]
    [SerializeField]
    private GameObject monsterSpawnVfx;

    [Header("몬스터 소환 이펙트 오브젝트 풀링 스택")]
    [SerializeField]
    private Stack<GameObject> m_SpawnVfxs;

    [Header("스테이지 매니저 스크립트")]
    [SerializeField]
    private StageManager stageMgrCs;

    [Header("몬스터 스폰 가능한지 확인하는 변수")]
    [SerializeField]
    public bool canSpawn;


    private void Awake()
    {
        canSpawn = true;

        // 몬스터 생성주기 변수 초기화
        cur_creation_cycle = 0f;

        // 몬스터 스폰 이벡트 오브젝트 풀링 초기화
        Init_Vfx_ObjectPool();

        // 몬스터 팩토리들 가져오기
        monsterFactories = transform.GetComponentsInChildren<IMonsterFactory>();
    }

    void Update()
    {
        // 보스 스테이지 일 경우 생성주기 0으로 전환
        if (stageMgrCs.isBossStage && canSpawn)
        {
            cur_creation_cycle = 0f;
        }
            

        // 몬스터 생성주기이고 보스 스테이지가 아닐 경우, 몬스터 생산자 함수 호출
        if ( cur_creation_cycle >= creation_cycle && !stageMgrCs.isBossStage && canSpawn)
            CreateMonster(maxMonster_SpawnCnt);

        // 몬스터 생성주기가 안됐을 경우 시간 더해주기
        else if(cur_creation_cycle < creation_cycle && !stageMgrCs.isBossStage && canSpawn)
            cur_creation_cycle += Time.deltaTime;
    }

    #region # Init_Vfx_ObjectPool() : 몬스터 스폰 이펙트 오브젝트 풀링 초기화해주는 함수
    private void Init_Vfx_ObjectPool()
    {
        // 오브젝트 풀링을 위한 스택 초기화
        m_SpawnVfxs = new Stack<GameObject>();

        // 10번 반복
        for(int i = 0; i < 10; i++)
        {
            // 프리팹 복사
            GameObject vfxPrefab = Instantiate(monsterSpawnVfx);

            // 스택에 프리팹 푸쉬
            m_SpawnVfxs.Push(vfxPrefab);

            // 인스펙터 창 깔끔하게 보이기 위해 부모 지정
            vfxPrefab.transform.SetParent(transform);

            // 프리팹 비활성화
            vfxPrefab.SetActive(false);
        }
    }
    #endregion

    #region # Active_Vfx : 몬스터 생성 이펙트 반환해주는 함수
    private GameObject Active_Vfx(Vector2 spawnPoint)
    {

        // 프리팹 복사
        GameObject vfxPrefab = null;

        // 스택에 요소가 없다면
        if (m_SpawnVfxs.Count <= 0)
        {
            // 프리팹 복사
            vfxPrefab = Instantiate(monsterSpawnVfx);

            // 스택에 프리팹 푸쉬
            m_SpawnVfxs.Push(vfxPrefab);
        }

        // 스택에서 요소 꺼내기
        vfxPrefab = m_SpawnVfxs.Pop();

        // vfx 위치 지정
        vfxPrefab.transform.position = spawnPoint + new Vector2(0, -0.5f);

        // vfx 오브젝트 반환
        return vfxPrefab;
    }
    #endregion


    #region # CreateMonster() 함수 :  몬스터 생산자 함수
    private void CreateMonster(int monsterCnt)
    {
        // 몬스터 수를 랜덤하기 생성하기 위한 랜덤값 할당
        int rand = Random.Range(1, maxMonster_SpawnCnt);

        // 생성할 몬스터 수만큼 반복
        for(int i = 0; i < rand; i ++)
        {
            // 몬스터 종류를 랜덤하게 생산할 수 있도록 랜덤 숫자에 해당하는 몬스터 팩토리 지정
            int randMonsterKind = Random.Range(0, monsterFactories.Length);

            // 지정된 몬스터 팩토리의 몬스터 반환
            MonsterUnitData spawnMonster = monsterFactories[randMonsterKind].CreateMonsterUnit();

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

            // 오브젝트 풀링에서 vfx 추출
            GameObject vfxPrefab = Active_Vfx(spawnPoint);

            // vfx활성화
            vfxPrefab.SetActive(true);

            // 기본 세팅 off
            spawnMonster.OffValue();

            // 스폰되는 몬스터 위치 랜덤한 위치로 조정
            spawnMonster.transform.position = spawnPoint;

            //몬스터 스폰 연출 함수 호출
            StartCoroutine(monsterSpawnProduction(monster: spawnMonster, spawnPoint : spawnPoint, Vfx : vfxPrefab));
        }

        // 몬스터 생성주기 0으로 초기화
        cur_creation_cycle = 0f;


    }
    #endregion

    private IEnumerator monsterSpawnProduction(MonsterUnitData monster, Vector2 spawnPoint, GameObject Vfx)
    {
        // 유닛 생성 시 초기값 할당하는 함수 호출 , 스테이지 단계 N 당 몬스터 공격력, 체력 N * 10%씩 강화
        monster.InitValue(value: stageMgrCs.cur_Stage);

        // 몬스터 오브젝트 활성화
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


        // 연출 이후 스택에 Vfx 다시 푸쉬
        m_SpawnVfxs.Push(Vfx);

        yield return new WaitForSeconds(0.25f);

        monster.OnValue();

    }
}
