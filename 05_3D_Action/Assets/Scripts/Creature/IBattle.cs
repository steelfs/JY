using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle
{
    Transform transform { get; }//�� ������Ʈ�� Ʈ������(���� �ʿ����)
    float AttackPower { get; }
    float DefencePower { get; }

    void Attack(IBattle target);//���� ������ ��� 
    void defence(float damage);// ���� ���� ������ 
}