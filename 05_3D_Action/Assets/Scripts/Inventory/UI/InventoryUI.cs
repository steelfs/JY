using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;// �� UI�� ������ �κ��丮
    InvenSlotUI[] slotsUI; //�� �κ��丮�� �����ִ� ��� ������ UI

    TempSlotUI tempSlotUI; // ������ �̵�, �и��� ����� �ӽý����� UI

    DetailInfo itemDetailInfo;

    public Player Owner => inven.Owner; //�� �κ��丮�� �����ڸ� Ȯ���ϱ� ���� ������Ƽ 

    public bool IsMoving { get; set; } = false;

    private void Awake()
    {
        Transform  child = transform.GetChild(0);
        slotsUI = child.GetComponentsInChildren<InvenSlotUI>(); 
        tempSlotUI = GetComponentInChildren<TempSlotUI>();
        itemDetailInfo = GetComponentInChildren<DetailInfo>();
    }

    public void InitializeInventory(Inventory playerInven)
    {
        inven = playerInven;

        //�����ʱ�ȭ
        for (uint i = 0; i < slotsUI.Length; i++)
        {
            slotsUI[i].InitilizeSlot(inven[i]);//UI �� ���� �迭�� slot�� ���ε�, ���� �� ��������Ʈ ���ε�
            slotsUI[i].onDragBegin += OnItemMoveBegin;
            slotsUI[i].onDragEnd += OnItemMoveEnd;
            slotsUI[i].onClick += OnSlotClick;
            slotsUI[i].onPointerEnter += OnItemDetailOn;
            slotsUI[i].onPointerExit += OnItemDetailOff;
            slotsUI[i].onPointerMove += OnSlotPointerMove;

            slotsUI[i].onPointerDown += itemDetailInfo.OnDetailPause;
            slotsUI[i].onPointerUp += itemDetailInfo.OnDetailAvailable;
        }

        //�ӽý��� �ʱ�ȭ
        tempSlotUI.InitilizeSlot(inven.TempSlot);
        tempSlotUI.onTempSlotOpenClose += OnDetailPause;

        itemDetailInfo.Close();
    }


    private void OnItemMoveBegin(uint index)
    {
        inven.MoveItem(index, tempSlotUI.Index);//���۽��Կ��� �ӽý������� ������ �ű��
        tempSlotUI.Open();//�ӽý��� ����
    }
    private void OnItemMoveEnd(uint index, bool succes)
    {
        inven.MoveItem(tempSlotUI.Index, index);//�ӽý��Կ��� ������������ ������ �ű��
        if (tempSlotUI.InvenSlot.isEmpty)//����ٸ� ���������� �������϶� �Ϻθ� ���� ��찡 �����Ƿ�
        {
            tempSlotUI.Close();
        }

    }
    private void OnSlotClick(uint index)
    {
        if (tempSlotUI.InvenSlot.isEmpty)
        {
            //�����ۻ��, ��� ���
        }
        else//�ӽý��Կ� �������������� Ŭ���� �Ǿ����� 
        {
            OnItemMoveEnd(index, true);// 
        }
    }
    private void OnSlotPointerMove(Vector2 screenPos)
    {
        itemDetailInfo.MovePosition(screenPos);
    }

    private void OnItemDetailOff(uint index)
    {
        itemDetailInfo.Close();
    }

    private void OnItemDetailOn(uint index)
    {
       itemDetailInfo.Open(slotsUI[index].InvenSlot.ItemData);
    }
    private void OnDetailPause(bool obj)
    {

    }
}
