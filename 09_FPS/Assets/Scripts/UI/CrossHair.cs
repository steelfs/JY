using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public AnimationCurve recovryCurve;
    /// <summary>
    /// 최대확장크기
    /// </summary>
    public float maxExpand = 100.0f;

    /// <summary>
    /// 기본, 최소확장크기
    /// </summary>
    const float defaultExpand = 30.0f;
    float current = 0;

    /// <summary>
    /// 조준선들의 RectTransform
    /// </summary>
    RectTransform[] crossRects;
    readonly Vector2[] direction = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    private void Awake()
    {
        crossRects = new RectTransform[transform.childCount];
        for (int i = 0; i < crossRects.Length; i++)
        {
            crossRects[i] = transform.GetChild(i) as RectTransform; 
        }
    }

    /// <summary>
    /// 조준선 확장 함수
    /// </summary>
    /// <param name="amount">확장되는 정도</param>
    public void Expand(float amount)
    {
        current = Mathf.Min(current + amount, maxExpand);//확장은 최대치 까지만
        for(int i = 0; i < crossRects.Length; i++)
        {
            crossRects[i].anchoredPosition = (current * direction[i]);
        }
        // StopCoroutine(recoveryCoroutine);
        StopAllCoroutines();
        StartCoroutine(DelayRecovery(0.1f));
    }

    /// <summary>
    /// 디폴트 위치로 되돌리는 코루틴
    /// </summary>
    /// <param name="wait">처음 기다리는 시간</param>
    /// <returns></returns>
    IEnumerator DelayRecovery(float wait)
    {
        yield return new WaitForSeconds(wait);

        float startExpand = current;//확장된 상태를 최대치로 저장
        float curveProcess = 0.0f;//커브 진행상황. 1에서 시작해서 0으로 이동
        float duration = 0.5f; //조준선이 디폴트값까지 이동하는데 걸리는 시간
        float div = 1 / duration;//?  1 / 0.5 = 2;

        while (curveProcess < 1)//duration초 동안 반복
        {
            curveProcess += Time.deltaTime * div;
            current = recovryCurve.Evaluate(curveProcess) * startExpand;//curve를 이용해 current계산

            for (int i = 0; i < crossRects.Length; i++)
            {
                crossRects[i].anchoredPosition = (current + defaultExpand) * direction[i];//계산 결과대로 점점 축소시키기
            }
            yield return null;
        }
        for (int i = 0; i < crossRects.Length; i++)
        {
            crossRects[i].anchoredPosition = defaultExpand * direction[i];
        }
        current = 0;
    }
}
