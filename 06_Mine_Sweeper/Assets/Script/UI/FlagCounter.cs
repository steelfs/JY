using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCounter : CounterBase
{
    private void Start()
    {
        GameManager.Inst.onFlagCountChange += Refresh;
        //GameManager.Inst.onGameReady += () => Refresh(GameManager.Inst.FlagCount); // ���µ��� �� 
        Refresh(GameManager.Inst.FlagCount);
        //��߰��� ���� ��������Ʈ ���� 
        //Refresh()
    }
}
