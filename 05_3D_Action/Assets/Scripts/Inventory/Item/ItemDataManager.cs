using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode
{
    Ruby = 0,
    Emerald,
    Sapphire,
    CopperCoin,
    SilverCoin,
    GoldCoin,
    FishSteak,
    Drink
}
public enum ItemSortBy// itemdata �ȿ��ִ� �� �� �� ����� �������� ������ ���ΰ� 
{
    Code,
    Name,
    Price
}
//��� �Ƥ��� ������ ���� ������ ���� �ִ� ������ Ŭ����. ���Ӹ޴����� ���� ���� ����
public class ItemDataManager : MonoBehaviour
{
    public ItemData[] itemDatas = null;
    public ItemData this[ItemCode code] => itemDatas[(int)code]; // ������ ������ ������ ���� �ε���  enum���� ������ �ε�����ȣ�� �ڵ����� �־� ã���ش�. ex) ItemDataManager[Itemcode.Ruby]
    public ItemData this[int index] => itemDatas[index];
    public int length => itemDatas.Length;
}
