using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static string nextSceneName;

    [SerializeField]
    private Transform loadingImgOb;    // �ε� �̹��� ���ӿ�����Ʈ

    [SerializeField]
    private TextMeshProUGUI loadingTxt;    // �ε� �ؽ�Ʈ

    [SerializeField]
    int loadingCount = 0;

    [SerializeField]
    private TextMeshProUGUI loadingGageTxt;    // �ε� ���� �����ִ� �ؽ�Ʈ

    // �ٸ� ������ �θ� �� �ִ� �Լ�
    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        StartCoroutine(LoadingTxtCount(loadingCount));
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadingTxtCount(int loadingCnt)
    {
        switch (loadingCnt)
        {
            case 0:
                loadingTxt.text = "�ε���";
                loadingCount += 1;
                break;
            case 1:
                loadingTxt.text = "�ε���.";
                loadingCount += 1;
                break;
            case 2:
                loadingTxt.text = "�ε���..";
                loadingCount += 1;
                break;
            case 3:
                loadingTxt.text = "�ε���...";
                loadingCount += 1;
                break;
        }
        if (loadingCount == 4)
        {
            loadingCount = 0;
        }
        yield return new WaitForSeconds(0.4f);
        StartCoroutine("LoadingTxtCount", loadingCount);
    }

    #region # LoadNextScene() : �ε��� �������ִ� �Լ�
    IEnumerator LoadNextScene()
    {
        print(nextSceneName);

        // ���� ������ ��ȯ�� �ɾ���� �ٸ� �� �� �� �ְ�
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);

        // allowSceneActivation = ���� ��ȯ�� ���� ����� �� ������, �ε��� �Ǵ��� �� ��ȯ ���ϰ� ��ٸ��� (op.progress 0.9 ������)
        op.allowSceneActivation = false;

        float time = 0;
        float gage = 0;

        // �ε��� ������ ������ �ݺ�
        while (!op.isDone)
        {

            if (op.progress < 0.9f)
            {
                //�ε� ������ ���α׷��� �ٿ� ����
                gage = op.progress;
                int intGage = Mathf.FloorToInt(gage * 100f);
                loadingGageTxt.text = intGage.ToString() + " %";
            }


            else // ����ũ �ε� (1��)
            {

                time += Time.deltaTime * 0.3f;
                gage = Mathf.Lerp(0.9f, 1, time);
                int intGage = Mathf.FloorToInt(gage * 100f);
                loadingGageTxt.text = intGage.ToString() + " %";
                // �� ��ȯ ���
                if (gage >= 1)
                {
                    // �� ��ȯ ���
                    op.allowSceneActivation = true;

                }

            }
            yield return null;
        }
        #endregion

    }
}