using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class StatusTitleBar : MonoBehaviour, IPointerDownHandler,IDragHandler,IPointerUpHandler
{
    Vector2 distance;
    RectTransform parentTransform;
    float minX = 0.0f;
    float maxX = 0.0f;
    float minY = 0.0f;
    float maxY = 0.0f;

    IPopupSortWindow popupSort;

    private void Awake()
    {
        parentTransform = transform.parent.GetComponent<RectTransform>();

        //�Ʒ� min, max ���� DragEnd�� ȭ����� ��� ��� �ɵ��� �·��� ��
        minX = parentTransform.sizeDelta.x * 0.5f;
        maxX = Screen.width - (parentTransform.sizeDelta.x * 0.5f);
        minY = parentTransform.sizeDelta.y * 0.5f;
        maxY = Screen.height - (parentTransform.sizeDelta.y * 0.5f);

        popupSort = transform.parent.GetComponent<IPopupSortWindow>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        distance = eventData.position - (Vector2)parentTransform.position; //Ŭ���ϴ¼����� eventPosition(���콺��ġ)�� �κ��丮�� �Ǻ��� �Ÿ� ����

        popupSort.PopupSorting?.Invoke(popupSort);
        //positionBeforeDrag = parentTransform.position; //Ŭ���ϴ¼����� Inventory ��ġ ����
    }
    public void OnDrag(PointerEventData eventData) //
    {
        //parentTransform.position = Vector3.zero; ������ǥ�� �ƴ� ��ũ����ǥ ���� (0, 0, 0)
        parentTransform.position = eventData.position - distance; //�̵�

    
    }


    public void OnPointerUp(PointerEventData eventData)//���ñ��� ���������� ������ ó�� Ŭ���ߴ� ������ �̵�, Drag���� �����⿡�� �ټ� �δ�ȴ�
    {
        if (parentTransform.position.x > maxX && parentTransform.position.y > maxY)
        {
            parentTransform.position = new Vector2(maxX, maxY);
        }
        else if (parentTransform.position.x > maxX && parentTransform.position.y < minY)
        {
            parentTransform.position = new Vector2(maxX, minY);
        }
        else if (parentTransform.position.x < minX && parentTransform.position.y < minY)
        {
            parentTransform.position = new Vector2(minX, minY);
        }
        else if (parentTransform.position.x < minX && parentTransform.position.y > maxY)
        {
            parentTransform.position = new Vector2(minX, maxY);
        }
        else if (parentTransform.position.x > maxX)
        {
            parentTransform.position = new Vector2(maxX, parentTransform.position.y);
        }
        else if (parentTransform.position.x < minX)
        {
            parentTransform.position = new Vector2(minX, parentTransform.position.y);
        }
        else if (parentTransform.position.y > maxY)
        {
            parentTransform.position = new Vector2(parentTransform.position.x, maxY);
        }
        else if (parentTransform.position.y < minY)
        {
            parentTransform.position = new Vector2(parentTransform.position.x, minY);
        }

    }

}
