using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuest_Item 
{
    Action<int, ItemCode> on_QuestItem_CountChange { get; set; }
}
