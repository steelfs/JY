using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_PlayerUnit : Base_Pool_Multiple<BattleMapPlayerBase>
{
    protected override void EndInitialize()
    {
        //start 에서 초기화하기위해 활성화 시킨다.
        Base_PoolObj temp;
        foreach (var item in pool)  // 풀에있는거 전부다 적용하기위해 전부돌린다.
        {
            temp = readyQueue.Dequeue();    //start 에서 다시 비활성화 시킬것이기때문에 디큐로 뽑아낸다.
            temp.gameObject.SetActive(true);  //활성화 시켜서 start 로직이 실행되도록 한다.
        }
    }
}
