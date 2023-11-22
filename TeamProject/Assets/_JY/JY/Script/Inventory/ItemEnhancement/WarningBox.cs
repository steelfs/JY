using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WarningBox : MonoBehaviour, IPopupSortWindow, IPointerClickHandler
{
    Item_Enhancer item_Enhancer;
    Item_Mixer item_Mixer;

    CanvasGroup canvasGroup;
    Button confirmButton;
    Button cancelButton;

    public Action onWarningBoxClose;

    public Action<IPopupSortWindow> PopupSorting { get ; set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        confirmButton = transform.GetChild(3).GetComponent<Button>();
        cancelButton = transform.GetChild(2).GetComponent<Button>();

        cancelButton.onClick.AddListener(Close);

    }
    void Start()
    {

        item_Enhancer = FindObjectOfType<Item_Enhancer>();
        item_Enhancer.onConfirmButtonClick += Open;
        item_Enhancer.onWaitforResult += Close;
        confirmButton.onClick.AddListener(WaitForResult);
     


        item_Mixer = FindObjectOfType<Item_Mixer>();
        item_Mixer.onConfirmButtonClick += Open;
        item_Mixer.onWaitforResult += Close;
    }
    void WaitForResult()
    {
        if (item_Enhancer.EnhancerUI.AfterSlot.ItemData != null)
        {
            item_Enhancer.EnhancerState = EnhancerState.WaitforResult;
        }
        else if (item_Mixer.MixerUI.Result_Slot.ItemData != null)
        {
            item_Mixer.MixerState = ItemMixerState.WaitforResult;
        }
    }
    void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        PopupSorting?.Invoke(this);
    }
    void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        switch (item_Mixer.MixerState)//강화중이면 신호를 보내지 마라(신호가 보내지면 UI의 모든 interactable이 true가 됨)
        {
            case ItemMixerState.Success:
            case ItemMixerState.Fail:
                return;
            default:
                break;
        }
        onWarningBoxClose?.Invoke();//
    }

    public void OpenWindow()
    {
        Open();
    }

    public void CloseWindow()
    {
        Close();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupSorting?.Invoke(this);
    }
}
