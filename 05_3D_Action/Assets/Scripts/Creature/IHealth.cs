using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float HP { get; set; } // Ȯ�� �� ������ ������Ƽ 
    float MaxHP { get; }
    Action<float> onHealthChange { get; set; } //�Ķ���ʹ� ���� 

    void Die();// ���ó�� 
    Action onDie { get; set; }// ����� ȣ�� 
    bool IsAlive { get; }//����Ȯ�ο� 

    void HealthRegenerate(float totalRegen, float duration);//ü���� ���������� ���ӽ����ִ� �Լ� �ʴ� totalRegen/duration ��ŭ ȸ�� 
    void RecoveryHealthByTick_(float tickRegen, float tickTime, uint totalTickCount);
}
