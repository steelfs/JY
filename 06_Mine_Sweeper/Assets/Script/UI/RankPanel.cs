using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TabType//상태제어용 enum
{
    None,
    Time,
    Action,
    Open,
    Close
}
public class RankPanel : MonoBehaviour
{
    enum TabType_// 실제 오브젝트 프로퍼티 참조용
    {
        Action,
        Time
    }

    Tab[] tabs;
    Tab this[TabType_ type] => tabs[(int) type];


    TabType type = TabType.None;
    public TabType Type
    {
        get => type;
        set
        {
            type = value;
            switch (type)
            {
                case TabType.Time:
                    SwitchTab(TabType.Time);
                    break;
                case TabType.Action:
                    SwitchTab(TabType.Action);
                    break;
                case TabType.Close:
                    SwitchTab(TabType.Close);
                    break;
                case TabType.Open:
                    SwitchTab(TabType.Open);
                    break;
                default:
                    break;
            }
        }
    }
    private void Awake()
    {
        tabs = GetComponentsInChildren<Tab>();
        foreach (Tab tab in tabs)
        {
            tab.onSwitchTab += SwitchTab;
        }
    }
    private void Start()
    {
        GameManager.Inst.onGameClear += () => Type = TabType.Open;
        GameManager.Inst.onGameReady += () => Type = TabType.Close;
        Type = TabType.Close;
    }
    private void SwitchTab(TabType type)
    {
        switch (type)
        {
            case TabType.Time:
                this[TabType_.Time].Open();
                this[TabType_.Action].Close();
                break;
            case TabType.Action:
                this[TabType_.Time].Close();
                this[TabType_.Action].Open();
                break;
            case TabType.Close:
                this[TabType_.Time].gameObject.SetActive(false);
                this[TabType_.Action].gameObject.SetActive(false);
                break;
            case TabType.Open:
                this[TabType_.Time].gameObject.SetActive(true);
                this[TabType_.Time].Open();
                this[TabType_.Action].gameObject.SetActive(true);
                this[TabType_.Action].Close();
                break;
            default:
                break;
        }
    }
}
