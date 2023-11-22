using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableOnDisableTest : MonoBehaviour
{

    private void Awake()
    {
        Debug.Log($"{name}_Awake");
    }

    private void OnEnable()
    {
        Debug.Log($"{name}_OnEnable");
        
    }
    private void OnDisable()
    {
        
        Debug.Log($"{name}_OnDisable");
    }
}
