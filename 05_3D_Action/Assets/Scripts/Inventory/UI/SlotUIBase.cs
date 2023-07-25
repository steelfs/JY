using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUIBase : MonoBehaviour
{

    InvenSlot invenSlot;// �� UI�� ǥ���� ����
    public InvenSlot InvenSlot => invenSlot; // Ȯ�ο� ������Ƽ

    public uint Index => invenSlot.Index; // ������ ���° ��������

    Image itemIcon;//������ ������ ǥ�ÿ� �̹���
    TextMeshProUGUI itemCount;

    protected virtual void Awake()//��ӹ��� Ŭ�������� ����� �߰��� �� �ֱ⶧���� virtual
    {
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemCount = child.GetComponent<TextMeshProUGUI>();
    }

    public virtual void InitilizeSlot(InvenSlot slot)
    {
        invenSlot = slot; //Invenslot�� ���ε�
        invenSlot.onSlotItemChange = Refresh; // ��������Ʈ ����
        Refresh();//������ ���̴� ��� ����
    }

    private void Refresh()
    {
        if (InvenSlot.isEmpty)// InvenSlot�� �������� ����Ǿ� ��������Ʈ�� ȣ���ϸ� (IsEmpty�� ��� )
        {
            itemIcon.color = Color.clear;
            itemCount.text = string.Empty;
            itemIcon.sprite = null;
        }
        else
        {
            itemIcon.sprite = invenSlot.ItemData.itemIcon;
            itemIcon.color = Color.white;
            itemCount.text = invenSlot.ItemCount.ToString();
        }
        OnRefresh();// �߰������� ������ �ؾ��� ���� ���� ��� �����Լ��� �߰��� ����
    }

    protected virtual void OnRefresh()
    {

    }
}
