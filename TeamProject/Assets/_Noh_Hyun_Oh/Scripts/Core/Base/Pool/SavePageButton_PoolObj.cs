using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Ǯ���� �����ɶ� �ڵ����� �����ͱ��� �ʱ�ȭ�ϵ��� ���ƴ�.
/// </summary>
public class SavePageButton_PoolObj : Base_PoolObj 
{
    /// <summary>
    /// ȭ�鿡 ������ ����¡��ȣ 
    /// </summary>
    int pageIndex = -1;
    public int PageIndex { 
        get => pageIndex;
        set { 
            pageIndex = value;
            realIndex = value - 1;
            text.text = $"{pageIndex}";
        }
    }
    /// <summary>
    /// ��������ư ���������� -1������ �ʿ��ؼ� ��� �����λ���.
    /// </summary>
    int realIndex = -1; 
    
    /// <summary>
    /// ó���� Ŭ���� ��������
    /// </summary>
    SaveWindowManager proccessClass;

    /// <summary>
    /// ȭ�鿡 ������ �ؽ�Ʈ��ġ ��������
    /// </summary>
    TextMeshProUGUI text;

    /// <summary>
    /// ������Ʈã��
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    }
    protected override void OnEnable()
    {
        //��ġ�� �ʱ�ȭ �Ƚ�Ű������ �������̵�
    }
    private void Start()
    {
        proccessClass = WindowList.Instance.MainWindow;
    }
    /// <summary>
    /// ������ ��ư Ŭ�� �̺�Ʈ
    /// </summary>
    public void OnPageDownButton()
    {
        if (realIndex > -1)
        {
            proccessClass.SetPageList(realIndex); 
        }
    }
}
