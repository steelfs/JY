using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public AnimationCurve recovryCurve;
    /// <summary>
    /// �ִ�Ȯ��ũ��
    /// </summary>
    public float maxExpand = 100.0f;

    /// <summary>
    /// �⺻, �ּ�Ȯ��ũ��
    /// </summary>
    const float defaultExpand = 30.0f;
    float current = 0;

    /// <summary>
    /// ���ؼ����� RectTransform
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
    /// ���ؼ� Ȯ�� �Լ�
    /// </summary>
    /// <param name="amount">Ȯ��Ǵ� ����</param>
    public void Expand(float amount)
    {
        current = Mathf.Min(current + amount, maxExpand);//Ȯ���� �ִ�ġ ������
        for(int i = 0; i < crossRects.Length; i++)
        {
            crossRects[i].anchoredPosition = (current * direction[i]);
        }
        // StopCoroutine(recoveryCoroutine);
        StopAllCoroutines();
        StartCoroutine(DelayRecovery(0.1f));
    }

    /// <summary>
    /// ����Ʈ ��ġ�� �ǵ����� �ڷ�ƾ
    /// </summary>
    /// <param name="wait">ó�� ��ٸ��� �ð�</param>
    /// <returns></returns>
    IEnumerator DelayRecovery(float wait)
    {
        yield return new WaitForSeconds(wait);

        float startExpand = current;//Ȯ��� ���¸� �ִ�ġ�� ����
        float curveProcess = 0.0f;//Ŀ�� �����Ȳ. 1���� �����ؼ� 0���� �̵�
        float duration = 0.5f; //���ؼ��� ����Ʈ������ �̵��ϴµ� �ɸ��� �ð�
        float div = 1 / duration;//?  1 / 0.5 = 2;

        while (curveProcess < 1)//duration�� ���� �ݺ�
        {
            curveProcess += Time.deltaTime * div;
            current = recovryCurve.Evaluate(curveProcess) * startExpand;//curve�� �̿��� current���

            for (int i = 0; i < crossRects.Length; i++)
            {
                crossRects[i].anchoredPosition = (current + defaultExpand) * direction[i];//��� ������ ���� ��ҽ�Ű��
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
