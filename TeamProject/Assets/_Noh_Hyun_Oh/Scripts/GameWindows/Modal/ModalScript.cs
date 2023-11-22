using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalScript : MonoBehaviour
{
    bool invenCheck = false;
    bool quickSlotCheck = false;
    bool optionsCheck = false;
    bool battleCheck = false;

    CanvasGroup cg;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 1.0f;
    }
    private void Start()
    {
        InputSystemController.InputSystem.Common.Esc.performed += (_) => { Close(); };
    }
  
    public void Open() 
    {
        battleCheck = InputSystemController.InputSystem.BattleMap_Player.enabled;
        InputSystemController.InputSystem.BattleMap_Player.Disable();
        invenCheck = InputSystemController.InputSystem.UI_Inven.enabled;
        InputSystemController.InputSystem.UI_Inven.Disable();
        quickSlotCheck = InputSystemController.InputSystem.QuickSlot.enabled;
        InputSystemController.InputSystem.QuickSlot.Disable();
        optionsCheck = InputSystemController.InputSystem.Options.enabled;
        InputSystemController.InputSystem.Options.Disable();
        gameObject.SetActive(true);
    }
    public void Close() 
    {
        if(battleCheck)
            InputSystemController.InputSystem.BattleMap_Player.Enable();
        if (invenCheck)
            InputSystemController.InputSystem.UI_Inven.Enable();
        if (quickSlotCheck)
            InputSystemController.InputSystem.QuickSlot.Enable();
        if (optionsCheck)
            InputSystemController.InputSystem.Options.Enable();
        gameObject.SetActive(false);
    }
}
