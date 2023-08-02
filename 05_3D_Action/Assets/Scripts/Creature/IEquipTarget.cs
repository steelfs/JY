using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipTarget 
{
    InvenSlot this[EquipType part] { get; }//장비아이템의 부위별 슬롯확인용 인덱스  part = 확인할 파츠 null 이면 장비되어있지 않은것

    void EquipItem(EquipType part, InvenSlot slot);//param = 장비할 부위, 그 아이템이 위치한 슬롯
    void UnEquipItem(EquipType part);

    Transform GetEquipParentTransform(EquipType part);// 장비될 아이템이 자식으로 들어갈 Transform을 리턴하는 함수  
}
