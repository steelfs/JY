using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlockKey : DoorKey
{
    public Action onConsume;
    protected override void ResetTarget(DoorBase target)
    {
        if (this.target != null)
        {
            DoorAutoLock doorAutoLock = target as DoorAutoLock;
            if (doorAutoLock != null)
            {
                onConsume = doorAutoLock.UnLock; // ��������Ʈ = ��
            }
            else
            {
                onConsume = null;
            }
        }
    }
    protected override void OnConsume()
    {
        onConsume?.Invoke();
        Destroy(gameObject);
    }
}
