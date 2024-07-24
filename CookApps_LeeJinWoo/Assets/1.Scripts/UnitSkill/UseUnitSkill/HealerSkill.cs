using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerSkill : MonoBehaviour, IUseSkill
{
    [Header("스킬 공격력 비율")]
    [SerializeField]
    private float atkDMG_Rate;

    [Header("스킬 사용하는 유닛 스크립트")]
    [SerializeField]
    private PlayerUnitData unitData;

    [Header("스킬 이펙트 프리팹")]
    [SerializeField]
    private GameObject skillvfx_Pref;

    // IUseSkill 인터페이스의 이펙트 오브젝트
    public GameObject _skillVFx { get; set; }


    // IUseSkill 인터페이스의 이펙트 오브젝트
    public Transform targetTr;


    [Header("타겟 탐지 타입")]
    [SerializeField]
    private LayerMask layerMask;

    private void Awake()
    {
        // IUseSkill 인터페이스의 이펙트 오브젝트에 이펙트 할당
        _skillVFx = Instantiate(skillvfx_Pref);

        // 이펙트 비활성화
        _skillVFx.SetActive(false);

        // 이펙트 활성화 확인을 위해 스킬을 사용하는 오브젝트의 자식으로 설정
        _skillVFx.transform.SetParent(transform);

        // 기초 셋팅값 할당해주는 함수 호출
        InitComponent();
    }

    private void Update()
    {
        if (_skillVFx.activeSelf)
            _skillVFx.transform.position = targetTr.position;
    }

    #region UseSkill() : 힐러 전용 스킬, 아군 치유
    public void UseSkill()
    {
        // 오버랩 스피어 생성
        Collider2D[] _cols = Physics2D.OverlapCircleAll((Vector2)transform.position, unitData.UnitSightRange, layerMask);

        // 탐지된 적이 없다면 함수 탈출
        if (_cols.Length <= 0)
        {
            unitData.Current_SkillCoolTime = unitData.UnitSkillCoolTime;
            return;
        }

        // 이펙트 위치 조정
        _skillVFx.transform.position = transform.position;

        int low = 0;
        int high = _cols.Length - 1;

        //퀵정렬
        QuickSort(_cols, low, high);

        targetTr = _cols[0].transform;

        bool cant_Heal = _cols[0].GetComponent<PlayerUnitData>().Unit_Hp >= _cols[0].GetComponent<PlayerUnitData>().Max_Hp;

        // 힐 불가능할 경우 스킬 쿨타임 초기화 하고 함수 탈출 -> 쿨타임 다시 반환할 시 애니메이션 무한으로 실행되기 때문
        if (cant_Heal)
        {
            unitData.Current_SkillCoolTime = 0f;
            return;
        }

        PlayerUnitData healTarget = _cols[0].GetComponent<PlayerUnitData>();

        // 최대 Hp 량 넘지 않게 힐량 구하기
        float healValue = Mathf.Clamp(healTarget.Unit_Hp += unitData.AtkDmg * atkDMG_Rate, healTarget.Max_Hp, healTarget.Max_Hp);

        // 타겟에게 힐량 전달
        healTarget.Unit_Hp = healValue;

        // 복사한 이펙트 오브젝트 활성화
        _skillVFx.SetActive(true);

    }
    #endregion

    #region # QuickSort() 함수 : 퀵정렬 기능 해주는 함수
    public void QuickSort(Collider2D[] arr, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(arr, low, high);
            QuickSort(arr, low, pivotIndex - 1);
            QuickSort(arr, pivotIndex + 1, high);
        }

    }
    #endregion

    #region # Partition() 함수 : 배열값 정렬해주는 함수
    private int Partition(Collider2D[] arr, int low, int high)
    {
        float pivot = arr[high].GetComponent<PlayerUnitData>().Unit_Hp;
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (arr[j].GetComponent<PlayerUnitData>().Unit_Hp <= pivot)
            {
                i++;
                Swap(arr, i, j);
            }
        }
        Swap(arr, i + 1, high);
        return i + 1;
    }
    #endregion

    #region # Swap() 함수 : 오버랩 스피어 배열 값 위치 변경해주는 함수
    private void Swap(Collider2D[] arr, int a, int b)
    {
        Collider2D temp = arr[a];
        arr[a] = arr[b];
        arr[b] = temp;
    }
    #endregion


    #region InitComponent() : 기초 셋팅값 할당하는 함수
    private void InitComponent()
    {
        // 스킬을 가지고 있는 유닛의 데이터
        unitData = GetComponent<PlayerUnitData>();

        // 스킬을 가지고 있는 유닛의 타겟 탐지 스크립트
        //searchTarget = GetComponent<ISearchTarget>();

    }
    #endregion

}
