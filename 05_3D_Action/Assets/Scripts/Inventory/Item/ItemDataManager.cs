using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode
{
    Ruby = 0,
    Emerald,
    Saphaire
}
//��� �Ƥ��� ������ ���� ������ ���� �ִ� ������ Ŭ����. ���Ӹ޴����� ���� ���� ����
public class ItemDataManager : MonoBehaviour
{
    public ItemData[] itemDatas = null;
    public ItemData this[ItemCode code] => itemDatas[(int)code]; // ������ ������ ������ ���� �ε���  enum���� ������ �ε�����ȣ�� �ڵ����� �־� ã���ش�. ex) ItemDataManager[Itemcode.Ruby]
    public int length => itemDatas.Length;
}
