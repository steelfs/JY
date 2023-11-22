using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.UI.GridLayoutGroup;

public class TempSlot : Slot, IPointerClickHandler,IPointerDownHandler
{
    /// <summary>
    /// 이 인벤토리를 가진 플레이어(아이템 드랍 때문에 필요)
    /// </summary>
    Player_ owner;

    /// <summary>
    /// 임시 슬롯이 열리고 닫힐 때 실행되는 함수
    /// </summary>
    public Action<bool> onTempSlotOpenClose;

    private void Update()
    {
        // 임시 슬롯은 대부분 꺼져 있을 거라 부담이 적음
        transform.position = Mouse.current.position.ReadValue();    // 임시 슬롯은 마우스 위치를 따라 움직임
    }
    public override void InitializeSlot(Slot slot)
    {
        base.InitializeSlot(slot);
        owner = GameManager.Player_;
    }
    /// <summary>
    /// 임시 슬롯 초기화하는 함수
    /// </summary>
    /// <param name="slot">이 임시 슬롯과 연결된 인벤 슬롯</param>
    private void Start()
    {
        Close();
        Refresh(ItemData);
        Index = 9898989;
    }

    /// <summary>
    /// 임시 슬롯을 여는 함수
    /// </summary>
    public void Open()
    {
        transform.position = Mouse.current.position.ReadValue();    // 위치를 마우스 위치로 조정
        onTempSlotOpenClose?.Invoke(true);                          // 열렸다고 신호 보내고
        gameObject.SetActive(true);                                 // 활성화 시키기(보이게 만들기)
    }

    /// <summary>
    /// 임시 슬롯을 닫는 함수
    /// </summary>
    public void Close()
    {
        onTempSlotOpenClose?.Invoke(false);     // 닫혔다고 신호 보내고
        gameObject.SetActive(false);            // 비활성화 시키기(안보이게 만들기)
    }

    /// <summary>
    /// 바닥에 아이템을 드랍하는 함수
    /// </summary>
    /// <param name="screenPos">마우스 커서의 스크린 좌표</param>
    public void OnDrop(Vector2 screenPos) //LayerMask - Enhancer,Befor, After 감지 가능하면 
    {
        if (!IsEmpty)    // 임시 슬롯에 아이템이 있을 떄만 처리
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);  // 스크린 좌표를 이용해 ray 계산
            if (Physics.Raycast(ray, out RaycastHit ground, 1000.0f, LayerMask.GetMask("Ground"))) // 레이와 바닥이 닿는지 검사
            {
                Vector3 dropPos = ground.point;    // 바닥에 레이가 닿았으면 닿은 위치를 저장
                
                Vector3 dropDir = dropPos - owner.transform.position;   // 오너 위치에서 레이가 닿은 지점까지의 방향 벡터 계산
                if (dropDir.sqrMagnitude > owner.pickupRange * owner.pickupRange)    // 방향 벡터의 크기가 ItemPickupRange를 넘는지 체크
                {
                    // 넘었으면 ItemPickupRange가 만드는 원의 가장자리 저점으로 dropPos 변경
                    dropPos = owner.transform.position + dropDir.normalized * owner.pickupRange;
                }

                ItemFactory.MakeItems(ItemData.code, dropPos, ItemCount, true);
                ClearSlotItem();//임시슬롯 비우기
                Close();//닫기
            }
            else if (Physics.Raycast(ray, out RaycastHit enhancer_Before, 1000.0f, LayerMask.GetMask("Enhancer_Before")))
            {
                Debug.Log("드롭 감지");
            }
        }

    }

}