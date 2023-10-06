using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public AnimationCurve recovryCurve;
    public float maxExpand = 100.0f;

    const float defaultExpand = 30.0f;
    float current = 0;

    RectTransform[] crossRects;
    Vector2[] direction = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    private void Awake()
    {
        crossRects = new RectTransform[transform.childCount];
        for (int i = 0; i < crossRects.Length; i++)
        {
            crossRects[i] = transform.GetChild(i) as RectTransform; 
        }
    }
    public void Expand(float amount)
    {
        current = Mathf.Min(current + amount, maxExpand);
        for(int i = 0; i < crossRects.Length; i++)
        {
            crossRects[i].anchoredPosition = (current * direction[i]);
        }
        // StopCoroutine(recoveryCoroutine);
        StopAllCoroutines();
        StartCoroutine(DelayRecovery(0.1f));
    }
    IEnumerator DelayRecovery(float wait)
    {

        yield return new WaitForSeconds(wait);

        float startExpand = current;
        float time = 0.0f;//1에서 시작해서 0으로 이동
        float duration = 0.5f;
        float div = 1 / duration;//?  1 / 0.5 = 2;

        while (time < 1)
        {
            time += Time.deltaTime * div;
            current = recovryCurve.Evaluate(time) * startExpand;

            for (int i = 0; i < crossRects.Length; i++)
            {
                crossRects[i].anchoredPosition = (current + defaultExpand) * direction[i];
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
