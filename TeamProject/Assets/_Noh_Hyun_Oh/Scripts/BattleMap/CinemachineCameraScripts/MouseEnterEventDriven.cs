using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ���콺�� ��ũ�� �ǳ��� ��ġ��Ű�� ī�޶� �̵���Ű�� ����
/// �ó׸ӽſ� 
/// ī�޶� �̵� ���� üũ�� 
/// </summary>
public class MouseEnterEventDriven : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// �̵��� ī�޶� ���� Ŭ����
    /// </summary>
    Camera_Move moveAction;
    /// <summary>
    /// ȭ�� ������ ��ǥ Ȯ�ο� ����
    /// </summary>
    readonly float leftCheck = Screen.width * 0.05f;
    readonly float rightCheck = Screen.width * 0.95f;
    readonly float topCheck = Screen.height * 0.95f;
    readonly float bottomCheck = Screen.height * 0.05f;

    private void Awake()
    {
        moveAction = transform.parent.GetComponent<Camera_Move>();
    }
    /// <summary>
    /// ���콺 �� ȭ�� ���� ��ġ�ϴ��� üũ
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Screen_Side_Mouse_Direction screenDir = Screen_Side_Mouse_Direction.None;
        Vector2 dir = eventData.position;
        if (dir.x > rightCheck)  //������ ��
        {
            screenDir |= Screen_Side_Mouse_Direction.Right;
        }
        else if (dir.x < leftCheck) //���� ��
        {
            screenDir |= Screen_Side_Mouse_Direction.Left;
        }

        if (dir.y > topCheck) //���� ��
        {
            screenDir |= Screen_Side_Mouse_Direction.Top;
        }
        else if (dir.y < bottomCheck) //�Ʒ��� �� 
        {
            screenDir |= Screen_Side_Mouse_Direction.Bottom;
        }
        
        moveAction.moveCamera?.Invoke(screenDir); //������ ����
    }

    /// <summary>
    /// ���콺 �߰� �϶� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        moveAction.moveCamera?.Invoke(Screen_Side_Mouse_Direction.None); //�ʱⰪ ����
    }

}
