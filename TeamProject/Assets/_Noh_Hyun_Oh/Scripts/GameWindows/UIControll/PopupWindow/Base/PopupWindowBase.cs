using System;
 using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

/// <summary>
/// ������ �ٿ� �ڵ鷯�� �ش������Ʈ ������ ���콺�������� ������ ���������� ������Ʈ�� ����� �������´�
/// �׿����� IDragHandler �� ȭ����ü�� ���콺������ ������ �������⶧���� ũ�������� ���� IPointerMoveHandler �� ����Ͽ� ����Ͽ��� 
/// </summary>
public class PopupWindowBase : MonoBehaviour, 
    //IPointerDownHandler, //�ش������Ʈ�ȿ��� �̺�Ʈ(Ŭ��) �ߵ��� ������  
    IPointerUpHandler,   //�ش������Ʈ�ȿ��� �̺�Ʈ(Ŭ������) �ߵ��� ������  
    IDragHandler        //ȭ�鿡�� �巡�׽� �ߵ���.  
{

    /// <summary>
    /// ���� ����ȭ�� width ��
    /// </summary>
    float windowWidth = Screen.width;
    
    /// <summary>
    /// ���� ����ȭ�� height ��
    /// </summary>
    float windowHeight = Screen.height;

    /// <summary>
    /// �˾�â ũ������ ���ɿ���
    /// </summary>
    bool isWindowSizeChange = false;
    public bool IsWindowSizeChange {
        get => isWindowSizeChange;
        set => isWindowSizeChange = value;
    }
    /// <summary>
    /// âũ�Ⱑ ���ϴ� �ӵ�
    /// </summary>
    [SerializeField]
    [Range(0.01f, 0.1f)]
    float sizeSpeed = 0.0f;

    [SerializeField]
    /// <summary>
    /// �˾�â ũ�������� �ּ� Width ��
    /// </summary>
    protected float minWidth = 0.0f;

    [SerializeField]
    /// <summary>
    /// �˾�â ũ�������� �ּ� Height ��
    /// </summary>
    protected float minHeight = 0.0f;

    [SerializeField]
    /// <summary>
    /// �˾�â ũ�������� �ִ� width �� 
    /// </summary>
    protected float maxWidth = 0.0f;

    [SerializeField]
    /// <summary>
    /// �˾�â ũ�������� �ִ� Height ��
    /// </summary>
    protected float maxHeight = 0.0f;

    /// <summary>
    /// ���� �˾�â�� RectTransform ���۳�Ʈ
    /// </summary>
    protected RectTransform rectTransform;

    /// <summary>
    /// �˾�â������ Ŭ�� ����
    /// </summary>
    protected bool isClick = false; 

    /// <summary>
    /// �˾�â ���� Ŭ������ġ��
    /// </summary>
    Vector2 clickPosition;
    public Vector2 ClickPosition
    {
        get => clickPosition;
        set => clickPosition = value;
    }

    /// <summary>
    /// ũ�� ���갪 ��Ƶ� �ӽù���
    /// </summary>
    Vector2 arithmeticValue;

    /// <summary>
    /// �˾�â �ݱ��ư �̺�Ʈ�����
    /// </summary>
    protected Button closeBtn;

    /// <summary>
    /// �������г� ��ġ���� ����� ã�ƿ���
    /// </summary>
    protected RectTransform contentPanel;

    /// <summary>
    /// ž �г� ��ġ�� ����� ��������
    /// </summary>
    protected RectTransform topPanel;

    protected virtual void Awake()
    {
        //Debug.Log("awake �׽�Ʈ");
        rectTransform = GetComponent<RectTransform>(); //�˾�â ũ������������ �����´�

        try //��ġ Ʋ���� �����ǿ������°͵��� �����Ѵ�. ���ӿ�����Ʈ ������ �ٲٸ� ������ ����.
        {
            contentPanel = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>(); //�������г� RectTransform �����´�
        
            topPanel = transform.GetChild(1).GetComponent<RectTransform>(); // ž�г� RectTransform �����´�


            int childCount = transform.GetChild(1).childCount - 1; //�ݱ��ư�� �׻� ������ �ڽ����� ���־���Ѵ�.
            closeBtn = transform.GetChild(1).GetChild(childCount).GetComponent<Button>(); //�ݱ��ư �����ͼ� 
            closeBtn.onClick.AddListener(OnCloseButtonClick); // �ݱ��̺�Ʈ ����  - ��ư ������Ʈ�� �̷��� ��ũ��Ʈ �߰����ص��ȴ�.
            SetContentWindowSize();//������â ��ġ�� ũ�� ���� , �����ϴܰ迡�� �̹� ������ �� �����Ȱ͸� ����ϱ⶧���� ��������.

        }catch (Exception ex) 
        {
            Debug.LogWarning($"{this.name} �˾�â�� ũ�������� �ȵ˴ϴ�.{ex.Message}");
        }
    }

    

    

    /// <summary>
    /// Ŭ�� ���� üũ
    /// </summary>
    /// <param name="eventData">�̺�Ʈ ��ġ����</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        isWindowSizeChange = false;
    }

    /// <summary>
    /// Ŭ�� üũ�� �̵��� �ߵ�
    /// </summary>
    /// <param name="eventData">�̺�Ʈ ��ġ����</param>
    public void OnDrag(PointerEventData eventData)
    {
        OnDragMove(eventData);
    }

   
    /// <summary>
    /// Ŭ���� �巡�׽� âũ�� ������� �߰�
    /// </summary>
    /// <param name="eventData">�̺�Ʈ ��ġ����</param>
    protected virtual void OnDragMove(PointerEventData eventData)
    {
        if (isWindowSizeChange) //âũ������ Ȱ��ȭ�� 
        { 
            SetPopupSize(eventData); //�˾�â ũ������
        }
    }

    /// <summary>
    /// �� �˾�â���� ���������� ���� ��������.
    /// </summary>
    /// <param name="contentWindowSize">������ x = width , y = height</param>
    protected virtual void SetItemList(Vector2 contentWindowSize)
    {

    }

    /// <summary>
    /// �˾�â �ݱ��ư Ŭ���� �˾�â ��Ȱ��ȭ��Ų��.
    /// �˾�â ������ ó���� ��� �߰� ����ϵ��� ����.
    /// </summary>
    protected virtual void OnCloseButtonClick()
    {
        gameObject.SetActive(false);

    }
    /// <summary>
    /// ����� ���۽�ų���� ������ ����Ʈ (ĵ����) �� �ڵ鷯�� �޾ƾ��Ѵ�. 
    /// </summary>
    /// <param name="eventData">���� ��üȭ���� ��ǥ������ �������ִ�</param>
    //rectTransform.anchoredPosition ���� âũ��
    private void SetPopupSize(PointerEventData eventData)
    {
        //����â�� ������� �巡�׽�������ġ������ �̵��ѰŸ���ŭ�� �� ���������� ���Ѵ�.
        arithmeticValue = rectTransform.sizeDelta;
        arithmeticValue.x = arithmeticValue.x + ((    // â�� ���� ������ width�� 
            eventData.position.x -                  // ��üȭ��!!!���� �巡���ϰ��ִ� ��ġ�� x��ǥ
            ClickPosition.x) *                      // ó�� �巡�׸� ������ ��ġ�� x ��ǥ
            sizeSpeed);        
        arithmeticValue.y = arithmeticValue.y + ((    // â�� ���� ������ height��
            ClickPosition.y -                       // ó�� �巡�׸� ������ ��ġ�� y ��ǥ
            eventData.position.y) *                 // ��üȭ��!!!���� �巡���ϰ��ִ� ��ġ�� y��ǥ
            sizeSpeed);

        //Height �ִ� �ּ� ������ üũ 
        if (arithmeticValue.y > maxHeight)
        {
            arithmeticValue.y = maxHeight;
        }
        else if (arithmeticValue.y < minHeight)
        {
            arithmeticValue.y = minHeight;
        }
        //Width �ִ� �ּ� ������ üũ
        if (arithmeticValue.x > maxWidth)
        {
            arithmeticValue.x = maxWidth;
        }
        else if (arithmeticValue.x < minWidth)
        {
            arithmeticValue.x = minWidth;
        }

        // ���� ���������� anchoredPosition �� ����Ͽ��� . ��Ŀ�������κ��� �󸶳��������� ����Ǵ°Ű���.
        // �Ϲ� position �� ����ϸ� ��üȭ�� �� �������� ��⶧���� ��Ŀ�� ����Ͽ���.
        // ��Ŀ������ ���� ���� ��Ƴ���.

        //ũ�� �÷������ �����쿡 ������� üũ 
        if (windowWidth - rectTransform.anchoredPosition.x < arithmeticValue.x) // ��üȭ��ũ�⿡�� â�� ��������� ��ǥ���� �����ͼ� ������ âũ�⺸�� Ŭ�� 
        {
            arithmeticValue.x = windowWidth - rectTransform.anchoredPosition.x;
        }

        if ((windowHeight + rectTransform.anchoredPosition.y) < arithmeticValue.y)
        {
            arithmeticValue.y = windowHeight + rectTransform.anchoredPosition.y; //y���� �׻� - ������ �����ָ� �ȴ�.
        }


        rectTransform.sizeDelta = arithmeticValue; //ó����� �ݿ��ϱ�

        SetContentWindowSize();//������ ���������� ������ ���� ����

        SetItemList(arithmeticValue);


    }

    /// <summary>
    /// �˾�â ���� �������� ������������ ��ġ���� �Լ�
    /// </summary>
    private void SetContentWindowSize() {
        //������ â ���������� 
        //arithmeticValue = rectTransform.sizeDelta; //�˾�â ������ �����ͼ�

        //arithmeticValue.y -= topPanel.sizeDelta.y; //ž�ǳ� ũ�⸸ŭ �ڸ���

        //contentPanel.sizeDelta = arithmeticValue;  //��ũ�⸦ ����

        //������ â ��ġ ���� 
        arithmeticValue = contentPanel.anchoredPosition;    //������ ��Ŀ���� ��ġ�� ��������

        arithmeticValue.y = -topPanel.sizeDelta.y;           //������ y��ǥ�� �����ϰ� 

        contentPanel.anchoredPosition = arithmeticValue;    //������ �� ����

    }

    
}

