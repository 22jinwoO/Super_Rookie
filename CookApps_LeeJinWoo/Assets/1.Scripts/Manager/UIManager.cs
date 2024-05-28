using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("플레이어 스크립트")]
    [SerializeField]
    private Player playerCs;


    [Header("페이드인 페이드 아웃 이미지")]
    [SerializeField]
    private Image fadeInFadeOutImg;

    #region 보스 연출 관련 변수들
    [Header("보스 페이드인 페이드 아웃 이미지")]
    [SerializeField]
    private Image boss_fadeInFadeOutImg;

    [Header("보스 스테이지 연출")]
    [SerializeField]
    private GameObject boss_Production_Obj;

    [Header("보스 이미지")]
    [SerializeField]
    private Image boss_Img;

    [Header("보스 이름 텍스트")]
    [SerializeField]
    private TextMeshProUGUI boss_NameTxt;

    [Header("보스 매니저 스크립트")]
    [SerializeField]
    private BossManager bossMgrCs;
    #endregion


    [Header("유닛 상점 활성화 상태인지 비활성화 상태인지 확인하는 변수")]
    [SerializeField]
    public bool isUnitSlotUp;


    [Header("유닛 정보 팝업창 활성화 버튼")]
    [SerializeField]
    private Button usePopUpBtn;

    [Header("유닛 정보 팝업창 트랜스폼 변수")]
    [SerializeField]
    private RectTransform unitSlotParent;

    [Header("현재 스테이지 텍스트")]
    public TextMeshProUGUI stageTxt;

    [Header("현재 스테이지 메인 텍스트")]
    public TextMeshProUGUI mainStageTxt;

    [Header("플레이어 골드 보유량 텍스트")]
    public TextMeshProUGUI playerGold_ValueTxt;
    
    // ==========================================

    [Header("유닛 정보창 게임오브젝트")]
    [SerializeField]
    private GameObject unitStatBorad_Obj;


    [Header("Hp 증가 시 지불하는 금액")]
    [SerializeField]
    private int hp_payValue;

    [Header("Atk 증가 시 지불하는 금액")]
    [SerializeField]
    private int atk_payValue;


    [Header("해당 슬롯의 플레이어 유닛들")]
    [SerializeField]
    private UnitData[] click_slotUnits;

    [Header("유닛 슬롯 버튼들")]
    [SerializeField]
    private Button[] unitSlotBtns;

    [Header("클릭한 슬롯의 플레이어 유닛")]
    [SerializeField]
    private UnitData click_slotUnit;

    [Header("클릭한 슬롯의 플레이어 유닛의 경험치 텍스트")]
    [SerializeField]
    private PlayerExp click_slotUnit_Exp;


    [Header("능력치 추가 버튼들")]
    [SerializeField]
    private Button[] plusStasBtn;

    [Header("스탯 매니저 스크립트")]
    [SerializeField]
    private StatsManager statsManagerCs;

    [Header("토탈 능력치 텍스트들")]
    public TextMeshProUGUI[] totalValueTxts;

    [Header("기본 능력치 텍스트들")]
    public TextMeshProUGUI[] defaultValueTxts;

    [Header("스탯창 추가 능력치 텍스트들")]
    public TextMeshProUGUI[] plusStatsTxts;

    [Header("유닛들 얼굴 이미지")]
    public GameObject[] unitFaceImgs;


    [Header("유닛 데이터 보드 제목 텍스트")]
    public TextMeshProUGUI unitDataTitleTxt;

    [Header("유닛 데이터 보드 레벨 텍스트")]
    public TextMeshProUGUI levelTxt;


    [Header("유닛 데이터 보드 Hp 텍스트")]
    public TextMeshProUGUI unitDataBoard_Hp_Txt;

    [Header("유닛 데이터 보드 Exp 텍스트")]
    public TextMeshProUGUI unitDataBoard_Exp_Txt;

    [Header("클릭한 유닛의 hp Bar 이미지")]
    public Image hpBarImg;

    [Header("클릭한 유닛의 exp Bar 이미지")]
    public Image expBarImg;

    [Header("유닛 데이터 보드 닫는 버튼")]
    public Button unitDataBoard_CloseBtn;

    [Header("설정 버튼")]
    [SerializeField]
    private Button settingBtn;

    [Header("설정창 오브젝트")]
    [SerializeField]
    private GameObject setting_Obj;

    [Header("시작화면으로 이동 버튼")]
    [SerializeField]
    private Button HomegBtn;

    [Header("다시하기 버튼")]
    [SerializeField]
    private Button RetryBtn;

    [Header("설정창 닫기 버튼")]
    [SerializeField]
    private Button setting_CloseBtn;


    private void Awake()
    {
        hp_payValue = 150;

        atk_payValue = 300;

        //팝업 슬롯 닫혀있음
        isUnitSlotUp = true;

        // 팝업 버튼에 함수 연결
        usePopUpBtn.onClick.AddListener(() => StartCoroutine(UseUnitPopUp()));

        plusStasBtn[0].onClick.AddListener(Plus_HpStatBtn);

        plusStasBtn[1].onClick.AddListener(Plus_AtkStatBtn);


        Set_ClickSlotBtns();

        // 유닛 데이터보드 활성화 창 닫는 함수 연결
        unitDataBoard_CloseBtn.onClick.AddListener(() => unitStatBorad_Obj.SetActive(false));

        // 설정버튼 누를 시 설정창 오브젝트 활성화하는 함수 연결
        settingBtn.onClick.AddListener(() => setting_Obj.SetActive(true));

        // 시작화면으로 이동 버튼 누를 시 시작화면 씬 불러오는 함수 연결
        HomegBtn.onClick.AddListener(() => LoadingManager.LoadScene("0.TitleScene"));

        // 다시하기 버튼 누를 시 해당 씬 다시 불러오는 함수 연결
        RetryBtn.onClick.AddListener(() => LoadingManager.LoadScene(SceneManager.GetActiveScene().name));

        // 설정창 닫기버튼 누를 시 설정창 오브젝트 비활성화하는 함수 연결
        setting_CloseBtn.onClick.AddListener(() => setting_Obj.SetActive(false));




        // 페이드인 연출 시작
        StartCoroutine(FadeInFadeOut(true));
    }

    private void Update()
    {
        // 유닛데이터 정보창이 활성화 되어 있을 경우
        if(unitStatBorad_Obj.activeSelf)
        {
            // Ui에서 - 값 보이지 않게 변경
            if (click_slotUnit._unit_Hp <= 0)
                click_slotUnit._unit_Hp = 0;

            // 클릭한 유닛의 hp 반영
            unitDataBoard_Hp_Txt.text = $"{click_slotUnit._unit_Hp} / {click_slotUnit._max_Hp} ";

            // hp 바에 클릭한 유닛의 hp 반영
            hpBarImg.fillAmount = click_slotUnit._unit_Hp / click_slotUnit._max_Hp;

            // 클릭한 유닛의 Exp반영
            unitDataBoard_Exp_Txt.text = $"{click_slotUnit_Exp.cur_ExpValue} / {click_slotUnit_Exp.max_ExpValue}";

            // Exp 바에 클릭한 유닛의 Exp 반영
            expBarImg.fillAmount = click_slotUnit_Exp.cur_ExpValue / click_slotUnit_Exp.max_ExpValue;

            // 레벨 반영
            levelTxt.text = $"Lv {click_slotUnit._unit_Level}";

            // 클릭한 유닛의 Hp 보여주기
            totalValueTxts[0].text = $"{click_slotUnit._max_Hp}";
            defaultValueTxts[0].text = $"{click_slotUnit.default_HpValue + click_slotUnit._level_Plus_HpStat}";
            plusStatsTxts[0].text = $"{click_slotUnit._plus_HpStat}";

            // 클릭한 유닛의 ATk_Dmg 보여주기
            totalValueTxts[1].text = $"{click_slotUnit._unit_AtkDmg}";
            defaultValueTxts[1].text = $"{click_slotUnit.default_AtkDmg + click_slotUnit._level_Plus_AtkStat}";
            plusStatsTxts[1].text = $"{click_slotUnit._plus_AtkStat}";

        }

        // 정보창 활성화되어 있지 않을 경우 팝업창 활성화 버튼 활성화
        else
            usePopUpBtn.interactable = true;

    }

    #region # FadeInFadeOut() : 화면 페이드인 페이드 아웃 해주는 함수
    public IEnumerator FadeInFadeOut(bool FadeInOutCheck)  // 화면 페이드인 페이드 아웃 해주는 함수
    {
        fadeInFadeOutImg.color = Color.black;

        fadeInFadeOutImg.gameObject.SetActive(true);

        // 페이드 인 상황일 때
        if (FadeInOutCheck) 
        {
            float time = 1;
            while (time >= 0f)
            {
                fadeInFadeOutImg.color = new Color(0f, 0f, 0f, time);
                time -= Time.deltaTime;
                yield return null;
            }
            
        }

        else  // 페이드 아웃 상황일 때
        {
            float time = 0;
            while (time <= 1f)
            {
                print("페이드아웃");
                fadeInFadeOutImg.color = new Color(0f, 0f, 0f, time);
                time += Time.deltaTime;
                yield return null;
            }

        }

        fadeInFadeOutImg.gameObject.SetActive(false);
    }
    #endregion

    #region # GameFail() : 플레이어 유닛들이 모두 죽었을 경우 호출되는 함수
    public IEnumerator GameFail()  // 화면 페이드인 페이드 아웃 해주는 함수
    {
        print("게임 끝");

        fadeInFadeOutImg.color = Color.black;

        fadeInFadeOutImg.gameObject.SetActive(true);


        // 페이드 아웃 연출
        float time = 0;
        while (time <= 1f)
        {
            fadeInFadeOutImg.color = new Color(0f, 0f, 0f, time);
            time += Time.deltaTime;
            yield return null;
        }

        // 텍스트 컬러 변경
        stageTxt.color = Color.white;

        stageTxt.text = "플레이어들이 전부 사망하였습니다...";

        // 해당 시간동안 이미지 및 텍스트 투명도 변경
        for (int i = 0; i <= 5; i++)
        {
            stageTxt.color = new Color(stageTxt.color.r, stageTxt.color.g, stageTxt.color.b, i * 0.2f);

            yield return new WaitForSecondsRealtime(0.1f);
        }




        yield return new WaitForSecondsRealtime(4f);


        // 로딩씬을 통해 현재 씬 다시 불러오기
        LoadingManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion

    #region # Boss_FadeInOut() : 보스 전용 페이드인 페이드아웃 연출 담당하는 스크립트
    public IEnumerator Boss_FadeInOut()
    {
        // 보스전용 페이드인, 아웃 이미지 컬러 지정
        boss_fadeInFadeOutImg.color = Color.black;

        // 보스전용 페이드인, 아웃 이미지 활성화
        boss_fadeInFadeOutImg.gameObject.SetActive(true);

        // 시간 할당
        float time = 0;

        // 시간값동안 페이드 아웃 연출
        while (time <= 1f)
        {
            boss_fadeInFadeOutImg.color = new Color(0f, 0f, 0f, time);
            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);

        // 보스전용 페이드인, 아웃 이미지 컬러 지정
        fadeInFadeOutImg.color = Color.white;

        // 보스전용 페이드인, 아웃 이미지 활성화
        fadeInFadeOutImg.gameObject.SetActive(true);

        // 시간 할당
        time = 0;

        // 시간값동안 페이드 아웃 연출
        while (time <= 1f)
        {
            fadeInFadeOutImg.color = new Color(1f, 1f, 1f, time);
            time += Time.deltaTime * 3f;
            yield return null;
        }

        // 시간 할당
        time = 1;

        // 시간값동안 페이드 인 연출
        while (time >= 0f)
        {
            fadeInFadeOutImg.color = new Color(1f, 1f, 1f, time);
            time -= Time.deltaTime * 3f;
            yield return null;
        }

        // 보스전용 페이드인, 아웃 이미지 비활성화
        fadeInFadeOutImg.gameObject.SetActive(false);

        yield return null;

        // 보스 연출 함수 호출
        StartCoroutine(Boss_Production());
    }
    #endregion

    #region # Boss_Production() : 보스 연출 담당하는 함수
    public IEnumerator Boss_Production()
    {
        yield return null;

        // 보스 전용 연출 이미지 활성화
        boss_Production_Obj.SetActive(true);

        // 보스전용 페이드인, 아웃 이미지 비활성화
        boss_fadeInFadeOutImg.gameObject.SetActive(false);
    
        // 보스이미지 컬러 지정
        boss_Img.color = new Color(boss_Img.color.r, boss_Img.color.g, boss_Img.color.b, 0f);

        // 보스 이름 텍스트 컬러 지정
        boss_NameTxt.color = new Color(boss_NameTxt.color.r, boss_NameTxt.color.g, boss_NameTxt.color.b, 0f);

        yield return null;

        // 해당 시간동안 이미지 및 텍스트 투명도 변경
        for (int i = 0; i <= 5; i++)
        {
            boss_Img.color = new Color(boss_Img.color.r, boss_Img.color.g, boss_Img.color.b, i * 0.2f);

            boss_NameTxt.color = new Color(boss_NameTxt.color.r, boss_NameTxt.color.g, boss_NameTxt.color.b, i * 0.2f);

            yield return new WaitForSecondsRealtime(0.1f);
        }

        yield return new WaitForSecondsRealtime(1.5f);


        // 보스 전용 연출 이미지 비활성화
        boss_Production_Obj.SetActive(false);

        // 씬 페이드 인 함수 호출
        StartCoroutine(FadeInFadeOut(true));

        yield return new WaitForSecondsRealtime(1.5f);

        // 보스 매니저에서 보스 스폰하는 함수 호출
        bossMgrCs.SpawnBoss();
    }
    #endregion

    #region # UseUnitPopUp() 함수 : 플레이어 유닛 슬롯 팝업창 활성화 하는 함수
    public IEnumerator UseUnitPopUp()
    {
        // 유닛데이터 정보창이 활성화 되어 있을 경우
        if (unitStatBorad_Obj.activeSelf)
            usePopUpBtn.interactable = false;

        yield return null;

        // 팝업창이 내려가있다면 렉트트랜스폼 y 값 증가
        if (isUnitSlotUp)
        {
            while (unitSlotParent.anchoredPosition.y < 120f)
            {
                usePopUpBtn.interactable = false;
                unitSlotParent.anchoredPosition += new Vector2(0, +10f);
                yield return null;
            }
            usePopUpBtn.interactable = true;
            isUnitSlotUp = false;
            yield break;
        }

        // 팝업창이 올라가있다면 렉트트랜스폼 y 값 감소
        if (!isUnitSlotUp)
        {
            while (unitSlotParent.anchoredPosition.y > -130f)
            {
                usePopUpBtn.interactable = false;
                unitSlotParent.anchoredPosition += new Vector2(0, -10f);
                yield return null;
            }

            isUnitSlotUp = true;
            yield break;

        }

    }
    #endregion

    #region # Plus_HpStatBtn() : 클릭한 유닛의 Hp 스탯을 증가시켜주는 함수
    private void Plus_HpStatBtn()
    {
        // 지불 금액만큼 플레이어가 돈을 가졌는지 확인
        bool canPay = playerCs.goldValue >= hp_payValue;

        if (canPay)
        {
            // 플레이어 보유 골드에서 지불한 값 빼주기
            playerCs.goldValue -= hp_payValue;

            // 플레이어 골드 보유량 텍스트에 골드량 반영
            playerGold_ValueTxt.text = $"{playerCs.goldValue}";

            // 스탯 매니저에 클릭한 유닛 전달
            statsManagerCs._thisUnit = click_slotUnit;

            // 스탯매니저의 함수 호출
            statsManagerCs.GetStats(playerUnit: click_slotUnit, stat: PlusStas.Hp, plusValue: 40f, toTalValueText: totalValueTxts[0], defaultValueText: defaultValueTxts[0], plusValueText: plusStatsTxts[0]);
        }

    }
    #endregion

    #region # Plus_AtkStatBtn() : 클릭한 유닛의 ATk 스탯을 증가시켜주는 함수
    private void Plus_AtkStatBtn()
    {
        // 지불 금액만큼 플레이어가 돈을 가졌는지 확인
        bool canPay = playerCs.goldValue >= atk_payValue;

        if (canPay)
        {
            // 플레이어 보유 골드에서 지불한 값 빼주기
            playerCs.goldValue -= hp_payValue;

            // 플레이어 골드 보유량 텍스트에 골드량 반영
            playerGold_ValueTxt.text = $"{playerCs.goldValue}";

            // 스탯 매니저에 클릭한 유닛 전달
            statsManagerCs._thisUnit = click_slotUnit;

            // 스탯매니저의 함수 호출
            statsManagerCs.GetStats(playerUnit: click_slotUnit, stat: PlusStas.Attack, plusValue: 1f, toTalValueText: totalValueTxts[1], defaultValueText: defaultValueTxts[1], plusValueText: plusStatsTxts[1]);
        }
        
    }
    #endregion

    #region # Set_ClickSlotBtns() : 클릭한 슬롯에 해당 캐릭터 연결시켜주는 함수
    private void Set_ClickSlotBtns()
    {
        // 유닛 슬롯마다 클릭시 호출되는 유닛 연결

        unitSlotBtns[0].onClick.AddListener(() => Link_SlotUnit(click_slotUnits[0]));
        unitSlotBtns[1].onClick.AddListener(() => Link_SlotUnit(click_slotUnits[1]));
        unitSlotBtns[2].onClick.AddListener(() => Link_SlotUnit(click_slotUnits[2]));
        unitSlotBtns[3].onClick.AddListener(() => Link_SlotUnit(click_slotUnits[3]));
    }
    #endregion

    #region # Link_SlotUnit : 유닛 팝업창 활성화 후, 클릭한 슬롯의 유닛 데이터 보여주는 함수
    private void Link_SlotUnit(UnitData playerUnit)
    {
        unitStatBorad_Obj.SetActive(true);

        // 활성화되어 있던 이미지들 비활성화
        for(int i = 0; i < unitFaceImgs.Length; i ++)
            unitFaceImgs[i].SetActive(false);

        // 스탯창 활성화 후 클릭한 슬롯의 유닛 반영
        click_slotUnit = playerUnit;

        // 클릭한 유닛의 경험치 슬롯 가져오기
        click_slotUnit_Exp = click_slotUnit.GetComponent<PlayerExp>();

        // 캐릭터 타입 확인
        CharacterType characterType = playerUnit.characterType;

        // 클릭한 유닛의 Hp 보여주기
        totalValueTxts[0].text = $"{click_slotUnit._max_Hp}";
        defaultValueTxts[0].text = $"{click_slotUnit.default_HpValue + click_slotUnit._level_Plus_HpStat}";
        plusStatsTxts[0].text = $"{click_slotUnit._plus_HpStat}";

        // 클릭한 유닛의 ATk_Dmg 보여주기
        totalValueTxts[1].text = $"{click_slotUnit._unit_AtkDmg}";
        defaultValueTxts[1].text = $"{click_slotUnit.default_AtkDmg + click_slotUnit._level_Plus_AtkStat}";
        plusStatsTxts[1].text = $"{click_slotUnit._plus_AtkStat}";


        // 캐릭터 이미지 활성화
        switch (characterType)
        {
            case CharacterType.Tanker:
                unitDataTitleTxt.text = "탱커";
                unitFaceImgs[0].gameObject.SetActive(true);
                break;

            case CharacterType.Meleer:
                unitDataTitleTxt.text = "근거리 딜러";
                unitFaceImgs[1].gameObject.SetActive(true);
                break;

            case CharacterType.Ranged_Dealer:
                unitDataTitleTxt.text = "원거리 딜러";
                unitFaceImgs[2].gameObject.SetActive(true);
                break;

            case CharacterType.Healer:
                unitDataTitleTxt.text = "힐러";
                unitFaceImgs[3].gameObject.SetActive(true);
                break;

        }
        usePopUpBtn.interactable = false;

        isUnitSlotUp = false;

        StartCoroutine(UseUnitPopUp());
    }
    #endregion


}
