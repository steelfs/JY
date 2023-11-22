using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ��ư ���۳�Ʈ ���� �ڵ鷯 �۵��Ǵ��� �׽�Ʈ��  ���� ��ũ��Ʈ
/// </summary>
public class PopupWindowSizeButton : MonoBehaviour, IPointerDownHandler
{
    /// <summary>
    /// �����������ϴ� Ŭ���� ��������
    /// </summary>
    [SerializeField]
    PopupWindowBase parentPopupWindow;

    private void Awake()
    {
        parentPopupWindow = transform.parent.parent.GetComponent<PopupWindowBase>(); //�̰���ġ�����̴� ��ĥ�ʿ䰡����..
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        parentPopupWindow.ClickPosition = eventData.position; //�巡�� ������ġ �������ֱ� �������� ����..
        parentPopupWindow.IsWindowSizeChange = true;          //�̺�Ʈ �߻��� âũ�� �����Ѵٰ� üũ�Ѵ�.
        
    }

  
}
   
