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

        // ���۹�ư�� ��Ʋ�� ȣ�����ִ� �Լ� ����
        startBtn.onClick.AddListener(() => LoadingManager.LoadScene("1.BattleScene"));
    }


    #region # StartEvent() : ���� �̺�Ʈ �������ִ� �Լ�
    IEnumerator StartEvent()
    {
        // Ÿ��Ʋ �ؽ�Ʈ �������� ����
        while (titleTxtParent.anchoredPosition.y > 0f)
        {
            titleTxtParent.anchoredPosition += new Vector2(0, -60f) * Time.deltaTime * 3f;
            yield return null;
        }

        yield return null;

        //���� ��ư Ȱ��ȭ
        startBtn.gameObject.SetActive(true);

        // ���� ��ư �����Լ� ȣ��
        StartCoroutine(Btn_Production());
    }
    #endregion

    #region # Btn_Production() : ���� ��ư �������ִ� �Լ�
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

        // ����Լ�
        StartCoroutine(Btn_Production());
    }
    #endregion
}
