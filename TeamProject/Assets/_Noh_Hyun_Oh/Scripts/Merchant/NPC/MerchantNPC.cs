using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���� ��л���
/// </summary>
public enum Merchant_State
{
    Nomal = 0,          // �������
    High,               // ��������� ���� �ǸŹ�ǰ�� ������ �϶��ϰ� �����Ҷ� ��ΰ� �����Ѵ�
    Low                 // ��г��ܶ� ���� �ǸŹ�ǰ�� ������ ����ϰ� �����Ҷ� ���ΰ� �����Ѵ�
}

public class MerchantNPC : NpcBase_Gyu
{
    /// <summary>
    /// ���� ���Ǿ� ���� ��л���
    /// </summary>
    Merchant_State merchant_State;
    public Merchant_State Merchant_State => merchant_State;

    private void Start()
    {
        Merchant_State temp = (Merchant_State)Random.Range(0, 3);//��л��� �������� 
        //Debug.Log(temp);
    }

}
