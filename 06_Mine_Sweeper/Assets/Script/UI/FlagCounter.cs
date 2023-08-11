using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCounter : CounterBase
{
    private void Start()
    {
        GameManager.Inst.onFlagCountChange += Refresh;
        //GameManager.Inst.onGameReady += () => Refresh(GameManager.Inst.FlagCount); // 리셋됐을 때 
        Refresh(GameManager.Inst.FlagCount);
        //깃발개수 변경 델리게이트 연결 
        //Refresh()
    }
}
