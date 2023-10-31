using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public Action<GameObject> on_SensorTriggered;

    private void OnTriggerEnter(Collider other)
    {
        on_SensorTriggered?.Invoke(other.gameObject);
    }
}
