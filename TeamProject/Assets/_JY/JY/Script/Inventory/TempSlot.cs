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
    /// �� �κ��丮�� ���� �÷��̾�(������ ��� ������ �ʿ�)
    /// </summary>
    Player_ owner;

    /// <summary>
    /// �ӽ� ������ ������ ���� �� ����Ǵ� �Լ�
    /// </summary>
    public Action<bool> onTempSlotOpenClose;

    private void Update()
    {
        // �ӽ� ������ ��κ� ���� ���� �Ŷ� �δ��� ����
        transform.position = Mouse.current.position.ReadValue();    // �ӽ� ������ ���콺 ��ġ�� ���� ������
    }
    public override void InitializeSlot(Slot slot)
    {
        base.InitializeSlot(slot);
        owner = GameManager.Player_;
    }
    /// <summary>
    /// �ӽ� ���� �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    /// <param name="slot">�� �ӽ� ���԰� ����� �κ� ����</param>
    private void Start()
    {
        Close();
        Refresh(ItemData);
        Index = 9898989;
    }

    /// <summary>
    /// �ӽ� ������ ���� �Լ�
    /// </summary>
    public void Open()
    {
        transform.position = Mouse.current.position.ReadValue();    // ��ġ�� ���콺 ��ġ�� ����
        onTempSlotOpenClose?.Invoke(true);                          // ���ȴٰ� ��ȣ ������
        gameObject.SetActive(true);                                 // Ȱ��ȭ ��Ű��(���̰� �����)
    }

    /// <summary>
    /// �ӽ� ������ �ݴ� �Լ�
    /// </summary>
    public void Close()
    {
        onTempSlotOpenClose?.Invoke(false);     // �����ٰ� ��ȣ ������
        gameObject.SetActive(false);            // ��Ȱ��ȭ ��Ű��(�Ⱥ��̰� �����)
    }

    /// <summary>
    /// �ٴڿ� �������� ����ϴ� �Լ�
    /// </summary>
    /// <param name="screenPos">���콺 Ŀ���� ��ũ�� ��ǥ</param>
    public void OnDrop(Vector2 screenPos) //LayerMask - Enhancer,Befor, After ���� �����ϸ� 
    {
        if (!IsEmpty)    // �ӽ� ���Կ� �������� ���� ���� ó��
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);  // ��ũ�� ��ǥ�� �̿��� ray ���
            if (Physics.Raycast(ray, out RaycastHit ground, 1000.0f, LayerMask.GetMask("Ground"))) // ���̿� �ٴ��� ����� �˻�
            {
                Vector3 dropPos = ground.point;    // �ٴڿ� ���̰� ������� ���� ��ġ�� ����
                
                Vector3 dropDir = dropPos - owner.transform.position;   // ���� ��ġ���� ���̰� ���� ���������� ���� ���� ���
                if (dropDir.sqrMagnitude > owner.pickupRange * owner.pickupRange)    // ���� ������ ũ�Ⱑ ItemPickupRange�� �Ѵ��� üũ
                {
                    // �Ѿ����� ItemPickupRange�� ����� ���� �����ڸ� �������� dropPos ����
                    dropPos = owner.transform.position + dropDir.normalized * owner.pickupRange;
                }

                ItemFactory.MakeItems(ItemData.code, dropPos, ItemCount, true);
                ClearSlotItem();//�ӽý��� ����
                Close();//�ݱ�
            }
            else if (Physics.Raycast(ray, out RaycastHit enhancer_Before, 1000.0f, LayerMask.GetMask("Enhancer_Before")))
            {
                Debug.Log("��� ����");
            }
        }

    }

}