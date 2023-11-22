using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SlotUI_Base : MonoBehaviour
{
    Slot slotComp;
    Image itemIcon;

    TextMeshProUGUI itemCountText;
    QuickSlot bindingSlot;
    public QuickSlot BindingSlot 
    {
        get => bindingSlot;
        set
        {
            if (value == null)//������ ���ε�����(QuickSlot)�� null�� ���� �� ��  
            {
                bindingSlot.ItemCount = 0;
                bindingSlot.ItemData = null;//���ε��� ������ ItemData���� null �� ���� �Ѵ�.
            }
            bindingSlot = value;
        }
    }
    public Action<QuickSlot, ItemData> onItemCountChange;
    public Action<ItemData> onItemDataChange;
    public bool IsEmpty => ItemData == null;//SlotManager����  �� �������� Ȯ���Ҷ� ���� ������Ƽ// �ʱ� 
    private ItemData itemData = null;
    public ItemData ItemData//SlotManager��  GetItem �Լ��� ����ɶ� Item�� ������ �޾ƿ������� ������Ƽ
    {
        get => itemData;
        set
        {
            if (itemData != value)
            {
                itemData = value;
                onItemDataChange?.Invoke(itemData);
                IQuest_Item questItem = itemData as IQuest_Item;
                if (questItem != null)
                {
                    questItem.on_QuestItem_CountChange?.Invoke((int)itemCount, itemData.code);
                }
            }
        }
    }
    uint itemCount;
    public uint ItemCount
    {
        get => itemCount;
        set
        {
            if (itemCount != value)
            {
                itemCount = value;
                onItemDataChange?.Invoke(itemData);
                if (BindingSlot != null)
                {
                    onItemCountChange?.Invoke(BindingSlot, itemData);
                }
            }
            if (itemData != null) 
            {
                ItemData.ItemCountBinding((int)itemCount);
            }
        }
    }
    protected virtual void Awake()
    {
        // ��ӹ��� Ŭ�������� �߰����� �ʱ�ȭ�� �ʿ��ϱ� ������ �����Լ��� ����
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemCountText = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// ���� �ʱ�ȭ�� �Լ�
    /// </summary>
    /// <param name="slot">�� UI�� ������ ����</param>
    public virtual void InitializeSlot(Slot slot)
    {
        this.slotComp = slot;                       // ���� ����
        onItemDataChange = Refresh;   // ���Կ� ��ȭ�� ���� �� ����� �Լ� ���
        ItemData = null;
        Refresh(itemData);                              // �ʱ� ��� ����
    }

    /// <summary>
    /// ������ ���̴� ����� �����ϴ� �Լ�
    /// </summary>
    protected virtual void Refresh(ItemData itemData)
    {
        if (IsEmpty)
        {
            // ���������
            itemIcon.color = Color.clear;   // ������ �Ⱥ��̰� ����ȭ
            itemIcon.sprite = null;         // �����ܿ��� �̹��� ����
            itemCountText.text = string.Empty;  // ������ �Ⱥ��̰� ���� ����
        }
        else
        {
            // �������� ���������
            itemIcon.sprite = ItemData.itemIcon;      // �����ܿ� �̹��� ����
            itemIcon.color = Color.white;                       // �������� ���̵��� ���� ����
            itemCountText.text = ItemCount.ToString(); 
            if (itemCount < 2)
            {
                itemCountText.text = string.Empty;
            }
        }

        OnRefresh();        // ��ӹ��� Ŭ�������� ������ �����ϰ� ���� �ڵ� ����
    }

    /// <summary>
    /// ��ӹ��� Ŭ�������� ���������� �����ϰ� ���� �ڵ带 ��Ƴ��� �Լ�
    /// </summary>
    protected virtual void OnRefresh()
    {
    }
}