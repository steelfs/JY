using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipTarget 
{
    InvenSlot this[EquipType part] { get; }//���������� ������ ����Ȯ�ο� �ε���  part = Ȯ���� ���� null �̸� ���Ǿ����� ������

    void EquipItem(EquipType part, InvenSlot slot);//param = ����� ����, �� �������� ��ġ�� ����
    void UnEquipItem(EquipType part);

    Transform GetEquipParentTransform(EquipType part);// ���� �������� �ڽ����� �� Transform�� �����ϴ� �Լ�  
}
