using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemFactory //�������� �����ϴ� static Ŭ����
{
    static int itemSerialNumber = 0;
    static float SpawnPosNoiseValue = 1.0f;

    public static GameObject MakeItem(ItemCode code)
    {
        ItemData itemData = GameManager.Inst.ItemData[code]; // �ڵ�� ������ ������ ������
        GameObject itemObj = GameObject.Instantiate(itemData.modelPrefab);
        ItemObject item = itemObj.GetComponent<ItemObject>();
        item.ItemData = itemData;

        string[] itemNames = itemData.name.Split('_'); // �ش� ���ڿ��� _�� �ִٸ� _�� �������� ����  00_Ruby => 00, Ruby  ������
        itemObj.name = $"{itemNames[1]}_{itemSerialNumber++}";                                               

        return itemObj;
    }
    public static GameObject MakeItem(ItemCode code, Vector3 position, bool randomNoise = false)
    {
        GameObject itemobj = MakeItem(code);
        if (randomNoise)
        {
            Vector2 noise = Random.insideUnitCircle * SpawnPosNoiseValue;
            position.x += noise.x;
            position.z += noise.y;
        }
        itemobj.transform.position = position;
        return itemobj;
    }
    public static GameObject[] MakeItems(ItemCode code, uint count)
    {
        GameObject[] itemObjs = new GameObject[count];
        for (int i= 0; i < count; i++)
        {
            itemObjs[i] = MakeItem(code);
        }
        return itemObjs;
    }
    public static GameObject[] MakeItems(ItemCode code, uint count, Vector3 position, bool randomNoise = false)
    {
        GameObject[] itemObjs = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            itemObjs[i] = MakeItem(code, position, randomNoise);
        }
        return itemObjs;
    }
}