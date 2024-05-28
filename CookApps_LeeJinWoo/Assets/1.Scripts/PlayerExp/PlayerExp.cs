using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerExp : MonoBehaviour
{
    [Header("플레이어 유닛 레벨")]
    [SerializeField]
    private int level;

    [Header("총 경험치 량")]
    public float max_ExpValue;

    [Header("현재 경험치 량")]
    public float cur_ExpValue;

    [Header("레벨업 이펙트")]
    [SerializeField]
    private GameObject levelUp_Vfx;

    [Header("유닛 데이터 스크립트")]
    public UnitData unitDataCs;

    [Header("경험치 바 이미지")]
    [SerializeField]
    private Image ExpBarImg;

    [Header("현재 레벨 텍스트")]
    [SerializeField]
    private TextMeshProUGUI levelTxt;


    private void Awake()
    {
        level = 1;
        levelUp_Vfx = Instantiate(levelUp_Vfx);
        levelUp_Vfx.SetActive(false);
        levelUp_Vfx.transform.SetParent(transform);
        unitDataCs = GetComponent<UnitData>();
        max_ExpValue = 60f;
    }

    #region # GetExpValue() : 몬스터 죽을 때, 호출되는 함수
    public void GetExpValue(float expValue)
    {
        // 현재 경험치량에 expValue 값만큼 증가
        cur_ExpValue += expValue;

        // 경험치 바에 현재 경험치 량 반영
        ExpBarImg.fillAmount = cur_ExpValue / max_ExpValue;

        // 레벨업 가능한지 체크하는 함수 호출
        Check_LevelUp();
    }
    #endregion

    #region # Check_LevelUp() : 레벨업 가능한지 체크하는 함수
    private void Check_LevelUp()
    {
        if (cur_ExpValue >= max_ExpValue)
            LevelUp();
    }
    #endregion

    #region # LevelUp() : 플레이어 유닛 레벨업 시 호출되는 함수
    private void LevelUp()
    {
        //게임 오브젝트가 활성화 되어 있을 때만 코루틴 실행
        if(gameObject.activeSelf)
            StartCoroutine(Show_Vfx());

        // 현재경험치 = 현재경험치 - 최대 경험치량
        cur_ExpValue -= max_ExpValue;

        // 최대경험치 40 증가
        max_ExpValue += 40f;

        // 레벨 증가
        level++;

        // 유닛 데이터에 레벨 반영
        unitDataCs._unit_Level = level;

        // UI Exp바에 경험치 반영
        ExpBarImg.fillAmount = cur_ExpValue / max_ExpValue;

        // Ui 레벨 텍스트에 레벨 반영
        levelTxt.text = $"Lv {level}";

        // 레벨업으로 인한 캐릭터 능력치 증가
        LevelUp_PlusValue();
    }
    #endregion

    #region # Show_Vfx() : 레벨업 이펙트 효과 생성해주는 함수
    private IEnumerator Show_Vfx()
    {
        // 이펙트 위치 지정
        levelUp_Vfx.transform.position = transform.position + new Vector3(0f,-0.75f);

        levelUp_Vfx.SetActive(true);

        yield return new WaitForSeconds(2f);

        levelUp_Vfx.SetActive(false);
    }
    #endregion

    #region LevelUp_PlusValue() : 플레이어 유닛 레벨업 시 능력치 증가시켜주는 함수
    private void LevelUp_PlusValue()
    {
        // 레벨업 추가 공격력 1 증가
        unitDataCs._level_Plus_AtkStat += 1f;

        // 유닛의 현재 공격력 = 유닛 기본 공격력 + 레벨업 추가 공격력 + 스탯창 추가 공격력
        unitDataCs._unit_AtkDmg = unitDataCs.default_AtkDmg + unitDataCs._level_Plus_AtkStat + unitDataCs._plus_AtkStat;

        // 레벨업 추가 체력 40 증가
        unitDataCs._level_Plus_HpStat += 40f;

        // 최대 체력 = 기본 체력 + 레벨업 추가 체력
        unitDataCs._max_Hp = unitDataCs.default_HpValue + unitDataCs._level_Plus_HpStat + unitDataCs._plus_HpStat;

        // 현재 체력에 최대 체력 반영
        unitDataCs._unit_Hp = unitDataCs._max_Hp;


    }
    #endregion


}
