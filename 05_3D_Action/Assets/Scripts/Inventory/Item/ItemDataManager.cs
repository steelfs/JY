using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode
{
    Ruby = 0,
    Emerald,
    Saphaire
}
//모든 아ㅣ템 종류에 대한 정보를 갖고 있는 관리자 클래스. 게임메니저를 통해 접근 가능
public class ItemDataManager : MonoBehaviour
{
    public ItemData[] itemDatas = null;
    public ItemData this[ItemCode code] => itemDatas[(int)code]; // 아이템 종류별 접근을 위한 인덱서  enum값을 넣으면 인덱스번호를 자동으로 넣어 찾아준다. ex) ItemDataManager[Itemcode.Ruby]
    public int length => itemDatas.Length;
}
