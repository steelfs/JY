using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSlotUI : SlotUIBase
{
    Player owner;//이 인벤토리를 가진 플레이어 // 드랍할때 사용
    public Action<bool> onTempSlotOpenClose; // 임시슬롯이 열리고 닫힐 때 호출

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();// TempSlotUI가 활성화상태 일때만 호출되기 때문에 Update에서 돌림
    }
    public override void InitilizeSlot(InvenSlot slot)//임시 슬롯 초기화
    {
        onTempSlotOpenClose = null;//델리게이트 비우기
        base.InitilizeSlot(slot);

        //owner = GameManager.Inst.Player;
        owner = GameManager.Inst.InvenUI.Owner; // 오너 캐싱
        Close();//시작할땐 비활성화 
    }
    public void Open()
    {
        transform.position = Mouse.current.position.ReadValue();//위치조정
        onTempSlotOpenClose?.Invoke(true);
        gameObject.SetActive(true);
    }
    public void Close()
    {
        onTempSlotOpenClose?.Invoke(false);
        gameObject.SetActive(false);
    }
    public void OnDrop(Vector2 screenPos)//커서의 스크린좌표
    {

    }
}
