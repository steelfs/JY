using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;// �� UI�� ������ �κ��丮
    InvenSlotUI[] slotsUI; //�� �κ��丮�� �����ִ� ��� ������ UI

    TempSlotUI tempSlotUI; // ������ �̵�, �и��� ����� �ӽý����� UI

    public Player Owner => inven.Owner; //�� �κ��丮�� �����ڸ� Ȯ���ϱ� ���� ������Ƽ 

    private void Awake()
    {
        Transform  child = transform.GetChild(0);
        slotsUI = child.GetComponentsInChildren<InvenSlotUI>(); 
        tempSlotUI = GetComponentInChildren<TempSlotUI>();
    }

    void InitializeInventory(Inventory playerInven)
    {
        inven = playerInven;

        //�����ʱ�ȭ
        //�ӽý��� �ʱ�ȭ
    }
}
