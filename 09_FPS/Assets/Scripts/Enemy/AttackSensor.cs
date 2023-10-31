using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : Sensor
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            on_SensorTriggered?.Invoke(other.gameObject);
        }
    }
}
