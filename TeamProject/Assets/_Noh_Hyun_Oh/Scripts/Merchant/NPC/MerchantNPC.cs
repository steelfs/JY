using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상인의 현재 기분상태
/// </summary>
public enum Merchant_State
{
    Nomal = 0,          // 보통상태
    High,               // 기분좋을때 상태 판매물품의 가격이 하락하고 구입할때 비싸게 구입한다
    Low                 // 기분나쁠때 상태 판매물품의 가격이 상승하고 구입할때 더싸게 구입한다
}

public class MerchantNPC : NpcBase_Gyu
{
    /// <summary>
    /// 상인 엔피씨 현재 기분상태
    /// </summary>
    Merchant_State merchant_State;
    public Merchant_State Merchant_State => merchant_State;

    private void Start()
    {
        Merchant_State temp = (Merchant_State)Random.Range(0, 3);//기분상태 랜덤으로 
        //Debug.Log(temp);
    }

}
