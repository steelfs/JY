using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMana 
{
    float MP { get; set; } // Ȯ�� �� ������ ������Ƽ 
    float MaxMP { get; }
    Action<float> onManaChange { get; set; } //�Ķ���ʹ� ���� 

    bool IsAlive { get; }//����Ȯ�ο� 

    void RegenerateMana(float totalRegen, float duration);//ü���� ���������� ���ӽ����ִ� �Լ� �ʴ� totalRegen/duration ��ŭ ȸ�� 
}
