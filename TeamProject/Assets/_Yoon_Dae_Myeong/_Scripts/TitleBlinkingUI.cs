using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine . UI;
using TMPro;

public class TitleBlinkingUI : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public float minBlinkInterval = 0.025f; // 최소 깜빡이는 간격
    public float maxBlinkInterval = 0.9f; // 최대 깜빡이는 간격
    public float minVisibleTime = 4.0f;   // 최소 켜져 있는 시간
    public float probabilityThreshold = 0.5f; // 안깜빡 확률

    private bool isVisible = true;

    void Start ( )
    {
        StartCoroutine ( BlinkingCoroutine ( ) );
    }

    IEnumerator BlinkingCoroutine ( )
    {
        while (true)
        {
            if (isVisible)
            {
                textMeshProUGUI . enabled = true;
                yield return new WaitForSeconds ( Random . Range ( minBlinkInterval , maxBlinkInterval ) );
                isVisible = false;

                if (Random . value < probabilityThreshold)
                {
                    float additionalTime = Random . Range ( 0.0f , 2.0f - minVisibleTime );
                    yield return new WaitForSeconds ( minVisibleTime + additionalTime );
                }
            }
            else
            {
                textMeshProUGUI . enabled = false;
                yield return new WaitForSeconds ( Random . Range ( minBlinkInterval , maxBlinkInterval ) );
                isVisible = true;
            }
        }
    }
}