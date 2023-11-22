using System.Collections;
using UnityEngine;
using UnityEngine . UI;

public class ChangeAlphaAfterDelay : MonoBehaviour
{
    public float transitionDuration = 2.0f; // 2초 동안 변화
    private Image panelImage;

    EndScene endProccess;

    void Start ( )
    {
        endProccess = FindObjectOfType<EndScene>(true);
        panelImage = GetComponent<Image> ( );
        Invoke ( "StartColorChange" , 4.0f ); // 게임 시작 후 5초 뒤에 실행
    }

    void StartColorChange ( )
    {
        endProccess.AppendInputAnykey();
        //StartCoroutine ( FadeToBlack ( ) );
    }

    IEnumerator FadeToBlack ( )
    {
        float startTime = Time . time;
        Color startColor = Color . clear; // 투명
        Color targetColor = Color . black; // 검정색

        while (Time . time < startTime + transitionDuration)
        {
            float t = ( Time . time - startTime ) / transitionDuration;
            panelImage . color = Color . Lerp ( startColor , targetColor , t );
            yield return null;
        }

        panelImage . color = targetColor; // 확실하게 검정색으로 설정
    }
}