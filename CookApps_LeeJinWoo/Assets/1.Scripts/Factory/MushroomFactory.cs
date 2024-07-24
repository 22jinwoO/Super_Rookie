using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomFactory : MonoBehaviour, IMonsterFactory
{
    [field: SerializeField]
    public MonsterUnitData MonsterPrefab { get; set; }

    // 오브젝트 풀링을 위한 몬스터 유닛 풀
    public Stack<MonsterUnitData> Monsters { get; set; }

    private void Awake()
    {
        // 스택 초기화
        Monsters = new Stack<MonsterUnitData>();

        MonsterPrefab.CharacterId = CharacterNumber.Mushoroom;
        MonsterPrefab.CharacterType = CharacterType.Monster;

        // 오브젝트 풀링 셋팅하는 함수
        InitObjPool(MonsterPrefab);
    }

    #region # InitObjPool : 오브젝트 풀링 구현해주는 함수
    public void InitObjPool(MonsterUnitData monsterPref)
    {
        {
            MonsterUnitData monsterUnit = null;


            // 오브젝트 풀링을 위해 프리팹 미리 생성
            for (int i = 0; i < 10; i++)
            {
                // 생산할 유닛 프리팹 복사
                monsterUnit = Instantiate(monsterPref);

                // 오브젝트 비활성화
                monsterUnit.gameObject.SetActive(false);

                // 생성된 유닛의 팩토리를 이 팩토리로 지정
                monsterUnit.Factory = this;

                // 인스펙터 창에서 깔끔하게 보이기 위해 팩토리 자식으로 설정
                monsterUnit.transform.SetParent(transform);

                // 오브젝트 풀링 스택의 요소로 추가
                Monsters.Push(monsterUnit);
            }

        }
    }
    #endregion

    // 생산될 유닛을 결정해주는 구상 생산자
    public MonsterUnitData CreateMonsterUnit()
    {
        MonsterUnitData MonsterUnit = null;

        // 스택 요소가 0보다 클 경우
        if (Monsters.Count > 0)
        {
            MonsterUnit = Monsters.Pop();
        }

        // 스택 요소가 0 보다 작을 경우 요소 추가
        else
        {
            MonsterUnit = Instantiate(MonsterPrefab);
            MonsterUnit.Factory = this;
        }

        // 생성된 몬스터 반환
        return MonsterUnit;
    }

}
