using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualStick : MonoBehaviour, IDragHandler,IPointerUpHandler, IPointerDownHandler
{
    RectTransform containerRect;// ��ü������ �簢�� // Ȯ���� ����

    RectTransform handleRect;// �ڵ�κ��� �簢�� 

    float stickRange;//�ڵ��� �����ϼ� �ִ� �ִ�Ÿ�

    public Action<Vector2> onMoveInput;//����ƽ �Է� 
    Player player;

    Vector2 clickedPos;// Ŭ�� ���������� ��ġ �����
    private void Awake()
    {
        containerRect = transform as RectTransform;
        Transform child = transform.GetChild(0);
        handleRect = child as RectTransform;
        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f; //�����̳� ���ݿ��� �ڵ� ���� ����
        player = FindObjectOfType<Player>();
    }
    public void OnDrag(PointerEventData eventData) //�ν����Ϳ��� raycast target�� Ȱ��ȭ �س��� ���� ����
    {
        // Debug.Log(eventData.position);
        //containerRect �ȿ���  eventData.position ��ġ�� containerRect �Ǻ����� �󸶳� ���������� position�� ����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);

        position = Vector2.ClampMagnitude(position, stickRange);//�ִ뿵�� ����� �ʰ� �ϱ�
      
        InputUpdate(position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handleRect.localPosition = Vector3.zero;
        InputUpdate(Vector2.zero); //������ ���� �� 0,0,0 ���� ��ȣ ������
    }

    public void OnPointerDown(PointerEventData eventData)// pointerUp�� ����Ϸ��� Down�� �־���Ѵ�.
    {

    }
    void InputUpdate(Vector2 position)//�Է¿� ���� �ڵ� �����̰� ��ȣ ������ �Լ�
    {
        handleRect.anchoredPosition = position;
        onMoveInput?.Invoke(position/stickRange);//nomalized�� ���� ���� (-1, -1)~ (1, 1) �� ��ȯ�ؼ� ����
        //��������Ʈ ����
        //�÷��̾�� �� ��������Ʈ�� ������ �Ǿ������� ��ȣ�� �°� �����δ�.
    }
}
