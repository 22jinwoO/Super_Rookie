using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform titleTxtParent;

    [SerializeField]
    private Button startBtn;    
    
    [SerializeField]
    private Image startBtnImg;   
    
    private void Awake()
    {
        StartCoroutine(StartEvent());

        // 시작버튼에 배틀씬 호출해주는 함수 연결
        startBtn.onClick.AddListener(() => LoadingManager.LoadScene("1.BattleScene"));
    }


    #region # StartEvent() : 시작 이벤트 구현해주는 함수
    IEnumerator StartEvent()
    {
        // 타이틀 텍스트 떨어지는 연출
        while (titleTxtParent.anchoredPosition.y > 0f)
        {
            titleTxtParent.anchoredPosition += new Vector2(0, -60f) * Time.deltaTime * 3f;
            yield return null;
        }

        yield return null;

        //시작 버튼 활성화
        startBtn.gameObject.SetActive(true);

        // 시작 버튼 연출함수 호출
        StartCoroutine(Btn_Production());
    }
    #endregion

    #region # Btn_Production() : 시작 버튼 연출해주는 함수
    private IEnumerator Btn_Production()
    {
        if (startBtnImg.color.a > 0f)
        {
            for(int i = 5; i >= 0; i--)
            {
                startBtnImg.color = new Color(1f, 1f, 1f, i*0.2f);
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        else if(startBtnImg.color.a <= 0f)
        {
            for (int i = 0; i <= 5; i++)
            {
                startBtnImg.color = new Color(1f, 1f, 1f, i *0.2f);
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        yield return new WaitForSecondsRealtime(0.1f);

        // 재귀함수
        StartCoroutine(Btn_Production());
    }
    #endregion
}
