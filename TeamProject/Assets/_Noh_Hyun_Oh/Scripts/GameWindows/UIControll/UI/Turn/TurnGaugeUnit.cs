using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
/// <summary>
///  �� �������� ���� ���ֿ�����Ʈ
///  ���� ������Ʈ�� �߰��Ǽ� Ȱ��ȭ�Ǹ� �ڵ����� ��ġ�� ��´� 
/// </summary>
public class TurnGaugeUnit : Base_PoolObj
{
    /// <summary>
    /// �������� �����̴� �ð� �ø����� ������
    /// </summary>
    [SerializeField]
    private float gaugeSpeed = 1.5f;

    /// <summary>
    /// ������ ������ġ �������� �β�
    /// </summary>
    [SerializeField]
    [Range(0.0f, 0.01f)]
    private float boldBar = 0.01f;

    /// <summary>
    /// ���൵ ó���� �� 0.0f ~ 1.0f;
    /// �̰��� �������� ������ ��ġ�� �����Ѵ�.
    /// </summary>
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float progressValue = 0.0f;
    public float ProgressValue
    {
        get => progressValue;
        set
        {
            progressValue = value;
            if (value > 1.0f) //�ִ밪�� 1.0f�̴�.
            {
                progressValue = 1.0f;
            }
        }
    }

    /// <summary>
    /// ���൵���� ���� �������̹��� ���� �߰�
    /// </summary>
    Image icon;

    /// <summary>
    /// �������� �����̴� ���  ���� �߰�
    /// </summary>
    Animator iconAnim;

    /// <summary>
    /// Position �̵��̾ƴ϶� ��Ŀ���� �����Ұ��̶� �ʿ�
    /// </summary>
    RectTransform moveRect;

    /// <summary>
    /// �����Ȳ ������ ����
    /// anchor�� ���� ����
    /// </summary>
    Vector2 minValue;
    Vector2 maxValue;

    /// <summary>
    /// ������ȭ���� right left ���� ���� ���� 
    /// </summary>
    Vector2 rectLeft;
    Vector2 rectRight;

    /// <summary>
    /// ���̺�ȯ�̵Ǹ� ������ �����̴� ������ ������� �ڸ�ƾ
    /// ���̵��Ŀ��� ���߱����� �����λ���.
    /// </summary>
    IEnumerator ProgressBar;

    protected override void Awake()
    {
        base.Awake();
        icon = GetComponent<Image>();
        moveRect = GetComponent<RectTransform>();
        iconAnim = GetComponent<Animator>();
        ProgressBar = ProgressMove();
    }

    protected override void OnEnable()
    {
        StartCoroutine(ProgressBar);
    }
    protected override void OnDisable()
    {
        StopCoroutine(ProgressBar);
        base.OnDisable();
    }

  

    /// <summary>
    /// progressValue ���� ����Ǹ� ������ �����̴� ������ �ֱ����� �߰� 
    /// ��ü â����� ����ǵ� ������������ �����̴´����� �ֱ����� ��Ŀ�������� ����ߴ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ProgressMove() 
    {
        if (moveRect != null)
        {

            minValue = moveRect.anchorMin;
            maxValue = moveRect.anchorMax;
           
            rectLeft = new Vector2(0, moveRect.offsetMin.y); //�����ͻ��� left �� ����  �̰� ����� �Էµ�
            rectRight = new Vector2(0, moveRect.offsetMax.y); //�����ͻ��� right �� ���� -�� �ԷµǱ⶧���� ����Է��Ϸ��� -�߰�
            while (true) //��ġ�� ��� 
            {
                
                minValue.x = Mathf.Lerp(minValue.x, progressValue ,Time.deltaTime * gaugeSpeed);
                //min max ���� ������ ����ٿ��� ����Ⱑ �Ⱥ������� ������ ���̸��ξ���.
                maxValue.x = Mathf.Lerp(maxValue.x , progressValue + boldBar,Time.deltaTime* gaugeSpeed);

                moveRect.anchoredPosition3D = Vector3.zero;
                //��Ŀ �����ǰ� ����
                moveRect.anchorMin = minValue;
                moveRect.anchorMax = maxValue;

                //��Ŀ �����ǰ��� �����ϸ� right  left ���� �ڵ����ιٲ�µ� ����� ���̰��Ϸ��� 
                //�ʱⰪ���� �������Ѵ� �׷��� 0������ ����
                moveRect.offsetMin = rectLeft;
                moveRect.offsetMax = rectRight;
                yield return null;
            }
            
        }
        
    }

    /// <summary>
    /// �ʱ�ȭ 
    /// </summary>
    public void ResetData() 
    {
        ProgressValue = 0.0f;//�� �ʱ�ȭ�ϰ�
        if (TurnManager.Instance.IsViewGauge) //�������� Ȱ��ȭ�������� 
        {
            gameObject.SetActive(false); //ť�� ������
        }
        else //�ȵ�������
        {
            //Debug.Log($"���� : {onDisable}");
            onDisable?.Invoke(); //ť�� ���������� ������ �Լ����� 
        }
    }

}
