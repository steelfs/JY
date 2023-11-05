using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooled_Obj : MonoBehaviour
{
   //public Action<Pooled_Obj> on_ReturnPool;

    public int poolIndex { get; set; }

    public void ReturnToPool()
    {
        GameManager.Pools.ReturnPool(this);
        gameObject.SetActive(false);
    }
    //Disable에서 델리게이트 신호를 보내는 것은 불안정한 코드같아 보인다.
    //따로 함수를 만들어서 ReturnPool함수를 실행시키고 그다음 따로 Disable을 시켜주는게 맞는것 같다.
}
