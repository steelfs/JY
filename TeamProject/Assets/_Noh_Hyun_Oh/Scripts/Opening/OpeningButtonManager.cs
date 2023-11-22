using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI 버튼 이벤트를 연결해줄 컴포넌트
/// </summary>
public class OpeningButtonManager : MonoBehaviour
{
    public Action<TextMeshProUGUI> speedUpButton;
    public Action<TextMeshProUGUI> skipButton;

    private void Awake()
    {
        Transform child =  transform.GetChild(0);
        Button speedUpBt = child.GetComponent<Button>();
        TextMeshProUGUI speedText = child.GetChild(0).GetComponent<TextMeshProUGUI>();
        speedUpBt.onClick.AddListener(() => {
            speedUpButton?.Invoke(speedText);
        });
        child = transform.GetChild(1);
        TextMeshProUGUI skipText = child.GetChild(0).GetComponent<TextMeshProUGUI>();
        Button skipBt = child.GetComponent<Button>();
        skipBt.onClick.AddListener(() => {
            skipButton?.Invoke(skipText);
        });
    }
}
