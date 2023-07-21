using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �� ������ �����͸� ���� ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "Empty Item", menuName = "Scriptable Object/ItemData", order = 1)] // ������������ ����� ������ �� �ְ� ���ִ� Ŭ���� 
public class ItemData : ScriptableObject
{
    [Header("������ �⺻ ������")]
    public ItemCode code;
    public string itemName = "������";
    public GameObject modelPrefab; // ���� ������ �� ������
    public Sprite itemIcon; // �κ��丮 UI���� ������ ������
    public uint price = 0; // ������ ��ġ
    public uint maxStackCount = 1;  //���������� �ִ� ����  ���� bool Ÿ���� IsStackable�� �����ʿ�� ���� �Ͱ���.
    public string itemDescription = "����";


}
