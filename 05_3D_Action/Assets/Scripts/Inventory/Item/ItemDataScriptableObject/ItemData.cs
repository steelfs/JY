using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 한 종류의 데이터를 가질 스크립터블 오브젝트
[CreateAssetMenu(fileName = "Empty Item", menuName = "Scriptable Object/ItemData", order = 1)] // 데이터파일의 양식을 설정할 수 있게 해주는 클레스 
public class ItemData : ScriptableObject
{
    [Header("아이템 기본 데이터")]
    public ItemCode code;
    public string itemName = "아이템";
    public GameObject modelPrefab; // 씬에 랜더링 될 프리팹
    public Sprite itemIcon; // 인벤토리 UI에서 보여질 아이콘
    public uint price = 0; // 아이템 가치
    public uint maxStackCount = 1;  //누적가능한 최대 개수  굳이 bool 타입의 IsStackable을 만들필요는 없을 것같다.
    public string itemDescription = "설명";


}
