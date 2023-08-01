using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMana 
{
    float MP { get; set; } // 확인 및 설정용 프로퍼티 
    float MaxMP { get; }
    Action<float> onManaChange { get; set; } //파라미터는 비율 

    bool IsAlive { get; }//생존확인용 

    void RegenerateMana(float totalRegen, float duration);//체력을 지속적으로 지속시켜주는 함수 초당 totalRegen/duration 만큼 회복 
}
