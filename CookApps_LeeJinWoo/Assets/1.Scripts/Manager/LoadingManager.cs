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
    private Transform loadingImgOb;    // 로딩 이미지 게임오브젝트

    [SerializeField]
    private TextMeshProUGUI loadingTxt;    // 로딩 텍스트

    [SerializeField]
    int loadingCount = 0;

    [SerializeField]
    private TextMeshProUGUI loadingGageTxt;    // 로딩 숫자 보여주는 텍스트

    // 다른 씬에서 부를 수 있는 함수
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
                loadingTxt.text = "로딩중";
                loadingCount += 1;
                break;
            case 1:
                loadingTxt.text = "로딩중.";
                loadingCount += 1;
                break;
            case 2:
                loadingTxt.text = "로딩중..";
                loadingCount += 1;
                break;
            case 3:
                loadingTxt.text = "로딩중...";
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

    #region # LoadNextScene() : 로딩씬 연출해주는 함수
    IEnumerator LoadNextScene()
    {
        print(nextSceneName);

        // 다음 씬으로 전환을 걸어놓고 다른 일 할 수 있게
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);

        // allowSceneActivation = 씬이 전환될 것을 허용해 줄 것인지, 로딩이 되더라도 씬 전환 못하게 기다리기 (op.progress 0.9 까지만)
        op.allowSceneActivation = false;

        float time = 0;
        float gage = 0;

        // 로딩이 끝나기 전까지 반복
        while (!op.isDone)
        {

            if (op.progress < 0.9f)
            {
                //로딩 과정을 프로그레스 바에 적용
                gage = op.progress;
                int intGage = Mathf.FloorToInt(gage * 100f);
                loadingGageTxt.text = intGage.ToString() + " %";
            }


            else // 페이크 로딩 (1초)
            {

                time += Time.deltaTime * 0.3f;
                gage = Mathf.Lerp(0.9f, 1, time);
                int intGage = Mathf.FloorToInt(gage * 100f);
                loadingGageTxt.text = intGage.ToString() + " %";
                // 씬 전환 허용
                if (gage >= 1)
                {
                    // 씬 전환 허용
                    op.allowSceneActivation = true;

                }

            }
            yield return null;
        }
        #endregion

    }
}