using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : BattleActionButtonBase
{
    protected override void OnMouseEnter()
    {
        uiController.ViewButtons();
    }
    protected override void OnMouseExit()
    {
        uiController.ResetButtons();
    }
}
