using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [Header("�ػ� �����ؾ� �ϴ� ī�޶��")]
    [SerializeField]
    private Camera[] cameras = new Camera[2];

    private void Awake()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            // �ػ� �����ؾ��ϴ� ī�޶��� rect��
            Rect rect = cameras[i].rect;

            // �ػ� ���� ���� - 16 (16:9)
            float scaleHeight = ((float)Screen.width / Screen.height) / ((float)16 / 9);

            // �ػ� ���� ���� - 9
            float scaleWidth = 1f / scaleHeight;

            //�ػ� ���� ���̰� 1���� ���� ��
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
