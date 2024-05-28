using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [Header("해상도 대응해야 하는 카메라들")]
    [SerializeField]
    private Camera[] cameras = new Camera[2];

    private void Awake()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            // 해상도 대응해야하는 카메라의 rect값
            Rect rect = cameras[i].rect;

            // 해상도 가로 길이 - 16 (16:9)
            float scaleHeight = ((float)Screen.width / Screen.height) / ((float)16 / 9);

            // 해상도 세로 길이 - 9
            float scaleWidth = 1f / scaleHeight;

            //해상도 가로 길이가 1보다 작을 때
            if (scaleHeight < 1)
            {
                rect.height = scaleHeight;
                rect.y = (1f - scaleHeight) / 2f;
            }

            else
            {
                rect.width = scaleWidth;
                rect.x = (1f - scaleWidth) / 2f;
            }


            cameras[i].rect = rect;

        }
    }
}
