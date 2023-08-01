using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float HP { get; set; } // 확인 및 설정용 프로퍼티 
    float MaxHP { get; }
    Action<float> onHealthChange { get; set; } //파라미터는 비율 

    void Die();// 사망처리 
    Action onDie { get; set; }// 사망시 호출 
    bool IsAlive { get; }//생존확인용 

    void HealthRegenerate(float totalRegen, float duration);//체력을 지속적으로 지속시켜주는 함수 초당 totalRegen/duration 만큼 회복 
    void RecoveryHealthByTick_(float tickRegen, float tickTime, uint totalTickCount);
}
