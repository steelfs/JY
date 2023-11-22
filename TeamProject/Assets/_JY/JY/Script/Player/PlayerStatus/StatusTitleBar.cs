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

        //아래 min, max 값은 DragEnd시 화면밖을 벗어날 경우 될돌릴 좌료의 값
        minX = parentTransform.sizeDelta.x * 0.5f;
        maxX = Screen.width - (parentTransform.sizeDelta.x * 0.5f);
        minY = parentTransform.sizeDelta.y * 0.5f;
        maxY = Screen.height - (parentTransform.sizeDelta.y * 0.5f);

        popupSort = transform.parent.GetComponent<IPopupSortWindow>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        distance = eventData.position - (Vector2)parentTransform.position; //클릭하는순간의 eventPosition(마우스위치)와 인벤토리의 피봇간 거리 저장

        popupSort.PopupSorting?.Invoke(popupSort);
        //positionBeforeDrag = parentTransform.position; //클릭하는순간의 Inventory 위치 저장
    }
    public void OnDrag(PointerEventData eventData) //
    {
        //parentTransform.position = Vector3.zero; 로컬좌표가 아닌 스크린좌표 기준 (0, 0, 0)
        parentTransform.position = eventData.position - distance; //이동

    
    }


    public void OnPointerUp(PointerEventData eventData)//로컬기준 범위밖으로 나갈시 처음 클릭했던 곳으로 이동, Drag에서 돌리기에는 다소 부담된다
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
