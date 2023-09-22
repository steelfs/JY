using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipDeploymentPanel : MonoBehaviour
{
    Deployment_Toggle[] toggles;
    private void Awake()
    {
        toggles = new Deployment_Toggle[transform.childCount];
        toggles = GetComponentsInChildren<Deployment_Toggle>();

        foreach (Deployment_Toggle toggle in toggles)
        {
            //toggle.onPress += OnTogglePress;
            toggle.onStateChange += OnToggleStateChange;
        }
    }

    private void OnToggleStateChange(Deployment_Toggle changed)
    {
        foreach (var toggle in toggles)
        {
            if (toggle != changed)
            {
                toggle.SetRelease();
            }
        }
    }

    private void OnTogglePress(Deployment_Toggle pressed)
    {
        foreach(var toggle in toggles)
        {
            if (toggle != pressed)
            {
                toggle.SetRelease();
            }
        }
    }
}
//토글버튼은 한번에 하나만 선택 가능
// 함선이 배치되면 해당 배와 연결된 버튼은 알파값 -
