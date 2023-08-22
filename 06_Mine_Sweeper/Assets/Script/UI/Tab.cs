using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tab : MonoBehaviour
{
    TabSubPanel tabSubPanel;
    Image image;
    public TabType Type;

    Button button;
    public Action<TabType> onSwitchTab;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => onSwitchTab?.Invoke(Type));
        tabSubPanel = GetComponentInChildren<TabSubPanel>();
        image = GetComponent<Image>();
    }
    public void Open()
    {
        image.color = Color.white;
        tabSubPanel.gameObject.SetActive(true);
    }
    public void Close()
    {
        image.color = new Color(1, 1, 1, 0.3f);
        tabSubPanel.gameObject.SetActive(false);
    }

}
