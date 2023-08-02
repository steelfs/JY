using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    EquipType EquipParts { get; } //�� �������� ������ ����

    void EquipItem(GameObject target, InvenSlot slot);//target = ���� ���, slot = targetSlot
    void UnEquipItem(GameObject target, InvenSlot slot);//������ ����

    void ToggleEquip(GameObject target, InvenSlot slot);//��Ȳ��¥�� �������� ����, �����ϴ� �Լ� 
}
