using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSlotUI : SlotUIBase
{
    Player owner;//�� �κ��丮�� ���� �÷��̾� // ����Ҷ� ���
    public Action<bool> onTempSlotOpenClose; // �ӽý����� ������ ���� �� ȣ��

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();// TempSlotUI�� Ȱ��ȭ���� �϶��� ȣ��Ǳ� ������ Update���� ����
    }
    public override void InitilizeSlot(InvenSlot slot)//�ӽ� ���� �ʱ�ȭ
    {
        onTempSlotOpenClose = null;//��������Ʈ ����
        base.InitilizeSlot(slot);

        //owner = GameManager.Inst.Player;
        owner = GameManager.Inst.InvenUI.Owner; // ���� ĳ��
        Close();//�����Ҷ� ��Ȱ��ȭ 
    }
    public void Open()
    {
        transform.position = Mouse.current.position.ReadValue();//��ġ����
        onTempSlotOpenClose?.Invoke(true);
        gameObject.SetActive(true);
    }
    public void Close()
    {
        onTempSlotOpenClose?.Invoke(false);
        gameObject.SetActive(false);
    }
    public void OnDrop(Vector2 screenPos)//Ŀ���� ��ũ����ǥ
    {

    }
}
