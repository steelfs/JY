using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//UI 와 분리되어있었다면 delegate로 신호를 보내야하지만  지금은 그냥 Set함수에서 바로 함수를 호출하도록한다
public class Slot : SlotUI_Base, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerDownHandler
{
    RectTransform itemDescriptionTransform;
    TextMeshProUGUI itemDescription_Text;
    Animator anim;


    int popUpHash = Animator.StringToHash("PopUp");

    public Action<ItemData, uint> onDragBegin;
    public Action<ItemData, uint, bool> onDragEnd;
    public Action<ItemData, uint> onClick;
    public Action<ItemData, uint> onPointerEnter;
    public Action<uint> onPointerExit;
    public Action<Vector2> onPointerMove;
    public Action<Slot> onSet_Just_ChangeSlot;
    public bool IsMoving { get; set; } = false; //이동중 description 팝업을 방지하기 위한 변수 
    bool isEquipped = false;

    public bool IsEquipped
    {
        get => isEquipped;
        private set
        {
            if (isEquipped != value)
            {
                isEquipped = value;
                
            }
        }
    }


    public uint Index { get; set; }
 
    public void AssignSlotItem(ItemData data, uint count = 1)
    {
        if (data != null)
        {
            ItemData = data;
            ItemCount = count;
            IsEquipped = false;
            //Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템이 {ItemCount}개 설정");
        }
        else
        {
            ClearSlotItem();    // data가 null이면 해당 슬롯은 초기화
        }
    }
    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)
    {
        bool result;
        int over;

        uint newCount = ItemCount + increaseCount;
        over = (int)newCount - (int)ItemData.maxStackCount;

        if (over > 0)
        {
            // 넘쳤다.
            ItemCount = ItemData.maxStackCount;
            overCount = (uint)over;
            result = false;
            //Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템이 최대치까지 증가. 현재 {ItemCount}개. {over}개 넘침");
        }
        else
        {
            // 안넘쳤다.
            ItemCount = newCount;
            overCount = 0;
            result = true;
            //Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템이 {increaseCount}개 증가. 현재 {ItemCount}개.");
        }

        return result;
    }
    public void DecreaseSlotItem(uint decreaseCount = 1)
    {
        int newCount = (int)ItemCount - (int)decreaseCount;
        if (newCount < 1)
        {
            // 슬롯이 완전히 비는 경우
            ClearSlotItem();
        }
        else
        {
            // 슬롯에 아이템이 남아있는 경우
            ItemCount = (uint)newCount;
            //Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템이 {decreaseCount}개 감소. 현재 {ItemCount}개.");
        }
    }
    public void ClearSlotItem()
    {
        ItemCount = 0;
        onItemCountChange?.Invoke(BindingSlot, ItemData);
        ItemData = null;
        IsEquipped = false;
        //Debug.Log($"인벤토리 {slotIndex}번 슬롯을 비웁니다.");
    }
  
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(ItemData, Index);
    }
    public void OnPointerClick(PointerEventData eventData)// itemdata가 null 이 되는 문제
    {
        //Debug.Log("슬롯클릭");
        onClick?.Invoke(ItemData, Index);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke(Index);
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(eventData.position);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        onDragBegin?.Invoke(ItemData, Index);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그가 끝나는지점 확인
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;    // 마우스 있는 위치에 게임 오브젝트가 있는지 확인
        if (obj != null)
        {
            // 마우스 위치에 어떤 오브젝트가 있다.
            Slot endSlot = obj.GetComponent<Slot>();  // 마우스 위치에 있는 오브젝트가 슬롯UI인지 확인
            //Mixer_Slot_Left leftSlot = obj.GetComponent<Mixer_Slot_Left>();
            //Mixer_Slot_Middle middleSlot = obj.GetComponent <Mixer_Slot_Middle>();
            if (endSlot != null)
            {
                // 슬롯UI다.
                Debug.Log($"드래그 종료 : {endSlot.Index}번 슬롯");
                onDragEnd?.Invoke(GameManager.SlotManager.TempSlot.ItemData, endSlot.Index, true); // 끝난지점에 있는 슬롯의 인덱스와 정상적으로 끝났다고 알람 보내기
            }
            //else if (leftSlot != null || middleSlot != null)
            //{
            //    //카운트를 줄여야하는데 이미 null이라서 줄일수가 없음
            //}
            else
            {
                // 슬롯UI가 아니다.
                Debug.Log($"슬롯이 아닙니다.");           // (드래그 실패했을 때는 원래 위치로 되돌리는 것이 정상)
                onDragEnd?.Invoke(GameManager.SlotManager.TempSlot.ItemData, Index, false);        // 원래 드래그가 시작한 인덱스와 비정상적으로 끝났다고 알람 보내기
            }
        }
        else
        {
            // 마우스 위치에 아무런 오브젝트도 없다.
            Debug.Log("아무런 오브젝트도 없습니다.");
        }
    }



    public void OnDrag(PointerEventData eventData)
    {
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onSet_Just_ChangeSlot?.Invoke(this);
    }
}
