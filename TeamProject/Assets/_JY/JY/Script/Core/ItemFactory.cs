using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� �����ϴ� ����ƽ Ŭ����
/// </summary>
static public class ItemFactory
{
    /// <summary>
    /// ������ �������� �Ϸù�ȣ
    /// </summary>
    //static int itemSerialNumber = 0;

    static float spawnPositionNoise = 0.5f;

    public static GameObject MakeItem(ItemCode code)
    {
        ItemData itemData = GameManager.Itemdata[code];             // �ڵ�� ������ ������ ��������
        GameObject itemObj = GameObject.Instantiate(itemData.modelPrefab);  // ������ �����Ϳ� �ִ� �������� �̿��� ������ ������Ʈ ����
        ItemObject item = itemObj.GetComponent<ItemObject>();
        item.ItemData = itemData;                                           // ������ ������ ������Ʈ�� ������ ������ ���

       // string[] itemName = itemData.name.Split('_');   // 00_Ruby => 00 Ruby �ΰ��� ��Ʈ������ ������ �� 
       // itemObj.name = $"{itemName[1]}_{itemSerialNumber++}";               // �̸��� �Ϸù�ȣ�� ���ļ� �̸� ����

        return itemObj;
    }

    /// <summary>
    /// �������� �ϳ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="code">������ �������� �ڵ�</param>
    /// <param name="position">������ ��ġ</param>
    /// <param name="randomNoise">��ġ�� ����� �߰����� ����(true�� ������ ����)</param>
    /// <returns></returns>
    public static GameObject MakeItem(ItemCode code, Vector3 position, bool randomNoise = false)//�������� �˸��� ��ġ�� ��ȯ�� �Ǵµ� �ڽĿ�����Ʈ�� ��ġ�� �����Ǿ��ִ�
    {
        GameObject itemObj = MakeItem(code);
        if (randomNoise)
        {
            // ����� �߰��ϴ� ��Ȳ�̸� �������� xz ����
            Vector2 noise = Random.insideUnitCircle * spawnPositionNoise;
            position.x += noise.x;
            position.z += noise.y;
        }
        itemObj.transform.position = position;  // ������ ��ġ�� �̵�

        return itemObj;
    }

    /// <summary>
    /// �������� ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="code">������ �������� �ڵ�</param>
    /// <param name="count">������ ����</param>
    /// <returns></returns>
    public static GameObject[] MakeItems(ItemCode code, uint count)
    {
        GameObject[] itemObjs = new GameObject[count];  // �迭 �����
        for (int i = 0; i < count; i++)
        {
            itemObjs[i] = MakeItem(code);   // ������ �����ؼ� �迭�� ���
        }
        return itemObjs;
    }

    /// <summary>
    /// �������� ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="code">������ �������� �ڵ�</param>
    /// <param name="count">������ ����</param>
    /// <param name="position">������ ��ġ</param>
    /// <param name="randomNoise">������ ���� ����</param>
    /// <returns></returns>
    public static GameObject[] MakeItems(ItemCode code, Vector3 position, uint count = 1 ,bool randomNoise = false)
    {
        GameObject[] itemObjs = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            itemObjs[i] = MakeItem(code, position, randomNoise);
        }
        return itemObjs;
    }
}