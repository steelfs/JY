using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mixer_Slot_Result : Mixer_Slot_Base
{
    //Mixer_Slot_Left left_Slot;
    //Mixer_Slot_Middle middle_Slot;

    //ItemData leftData = null;
    //ItemData middleData = null;
    protected override void Awake()
    {
        base.Awake();
        //left_Slot = transform.parent.GetChild(1).GetComponent<Mixer_Slot_Left>();
        //middle_Slot = transform.parent.GetChild(2).GetComponent<Mixer_Slot_Middle>();
    }
    protected override void Start()
    {
        base.Start();
        //item_Mixer.onSetItem
    }




}
