using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enhancer_Slot_Before : Enhancer_Slot_Base,IPointerClickHandler
{
  
    public void OnPointerClick(PointerEventData eventData)
    {
        item_Enhancer.EnhancerState = EnhancerState.ClearItem;
    }


}
