using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooled_Obj : MonoBehaviour
{
    public Action<Pooled_Obj> on_ReturnPool;

    public int poolIndex { get; set; }
    private void OnDisable()
    {
        on_ReturnPool?.Invoke(this);
    }
}
