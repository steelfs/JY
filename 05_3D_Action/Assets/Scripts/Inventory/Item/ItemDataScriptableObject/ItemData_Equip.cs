using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Equip : ItemData, IEquipable
{
    [Header("��� ������ ������")]
    public GameObject equipPrefab;//���� ��� ���� �� �÷��̾�𵨿� ���� ���� ������// ��ӵǴ� �����۰��� �ٸ� �������̴�.

    public virtual EquipType EquipParts => EquipType.Weapon;//�������� ���� ��ġ�� �˷��ִ� ������Ƽ

    public void EquipItem(GameObject target, InvenSlot slot)//target = ����� ���, ������ �������� ��ġ�� ����
    {

        if (target != null)
        {
            IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();// ����� IEquipTarget�� ��� �޾Ҵٸ�
            if (equipTarget != null)
            {
                equipTarget.EquipItem(EquipParts, slot); //slot�� ����ִ� �������� �����ض�
            }
        }
    }

    public void UnEquipItem(GameObject target, InvenSlot slot)
    {
        if (target != null)
        {
            IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
            if (equipTarget != null)
            {
                equipTarget.UnEquipItem(EquipParts);
            }
        }
    }
    public void ToggleEquip(GameObject target, InvenSlot slot)
    {
        if (target != null)
        {
            IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
            if (equipTarget != null)
            {
                InvenSlot oldSlot = equipTarget[EquipParts];
                if (oldSlot == null) //���� ��� ���� ���� ��Ȳ�̸� 
                {
                    EquipItem(target, slot); //����ض�
                }
                else //��� �Ǿ������� 
                {
                    UnEquipItem(target, oldSlot); //�켱 �������
                    if (oldSlot != slot)// ���� ���õ� ������ ������ �ٸ��ٸ�
                    {
                        EquipItem(target, slot);// �ٸ� �������� ������ ���̹Ƿ� ����Ŭ���� ������ �������� �����ض�
                    }
                }
            }
        }
    }


 
}
