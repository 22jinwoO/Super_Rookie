using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinFactory : MonoBehaviour, IMonsterFactory
{
    public UnitData _monsterPrefab { get; set; }
    public Stack<UnitData> _monsters { get; set; }

    [SerializeField]
    private UnitData thisMonsterPrefab;

    private void Awake()
    {

        // 인터페이스 변수에 값 할당
        _monsterPrefab = thisMonsterPrefab;

        // 스택 초기화
        _monsters = new Stack<UnitData>();

        // 오브젝트 풀링 셋팅하는 함수
        InitObjPool(_monsterPrefab);
    }

    #region # InitObjPool : 오브젝트 풀링 구현해주는 함수
    public void InitObjPool(UnitData monsterPref)
    {
        {
            UnitData monsterUnit = null;


            // 오브젝트 풀링을 위해 프리팹 미리 생성
            for (int i = 0; i < 10; i++)
            {
                // 생산할 유닛 프리팹 복사
                monsterUnit = Instantiate(monsterPref);

                // 오브젝트 비활성화
                monsterUnit.gameObject.SetActive(false);

                // 생성된 유닛의 팩토리를 이 팩토리로 지정
                monsterUnit._factory = this;

                // 인스펙터 창에서 깔끔하게 보이기 위해 팩토리 자식으로 설정
                monsterUnit.transform.SetParent(transform);

                // 오브젝트 풀링 스택의 요소로 추가
                _monsters.Push(monsterUnit);
            }

        }
    }
    #endregion

    // 생산될 유닛을 결정해주는 구상 생산자
    public UnitData CreateMonsterUnit()
    {
        UnitData MonsterUnit = null;
        
        // 스택 요소가 0보다 클 경우
        if (_monsters.Count > 0)
        {
            MonsterUnit = _monsters.Pop();
        }

        // 스택 요소가 0 보다 작을 경우 요소 추가
        else
        {
            MonsterUnit = Instantiate(_monsterPrefab);
            MonsterUnit._factory = this;
        }

        // 생성된 몬스터 반환
        return MonsterUnit;
    }

}
