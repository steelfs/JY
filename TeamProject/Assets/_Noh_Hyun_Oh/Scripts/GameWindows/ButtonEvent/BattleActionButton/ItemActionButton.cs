using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionButton : BattleActionButtonBase
{
    Inventory inven;
    private void Start()
    {
        inven = WindowList.Instance.InvenWindow;
    }
    protected override void OnClick()
    {
        inven.Open_Inventory();
    }

}
