using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemSpliterUI : MonoBehaviour
{
    InvenSlot targetSlot;// split할 슬롯
    PlayerInputAction inputActions;

    const uint minItemCount = 1;
    uint itemSplitCount = minItemCount;
    uint ItemSplitCount
    {
        get => itemSplitCount;
        set
        {
            itemSplitCount = Math.Clamp(value, minItemCount, targetSlot.ItemCount - 1);
            inputField.text = itemSplitCount.ToString();

            slider.value = itemSplitCount;
        }
    }

    public Action<uint, uint> onOkClick;
    public Action onCancel;

    TMP_InputField inputField;
    Image itemIcon;
    Slider slider;


    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        itemIcon = transform.GetChild(0).GetComponent<Image>();
        slider = GetComponentInChildren<Slider>();

        inputActions = new PlayerInputAction();

        inputField.onValueChanged.AddListener((text) =>
        {
            //ItemSplitCount = Math.Max(minItemCount, uint.Parse(text));
            if( uint.TryParse(text, out uint result))  //형변환 성공하면 인풋필드의 값을 uint로 바꾼 후  ItemSplitcount 프로퍼티 값 대입해서 수정
            {
                ItemSplitCount = result;
            }
            else
            {
                ItemSplitCount = minItemCount;
            }
        });

        slider.onValueChanged.AddListener((ratio) =>
        {
            ItemSplitCount = (uint)ratio;
        });

        Button plus = transform.GetChild(2).GetComponent<Button>();
        plus.onClick.AddListener(() =>
        {
            ItemSplitCount++;
        });

        Button minus = transform.GetChild(3).GetComponent<Button>();
        minus.onClick.AddListener(() =>
        {
            ItemSplitCount--;
        });

        Button okButton = transform.GetChild(5).GetComponent<Button>();
        okButton.onClick.AddListener(() =>
        {
            onOkClick?.Invoke(targetSlot.Index, ItemSplitCount); //ok버튼 클릭시 신호반 보내고 닫기
            Close();
        });

        Button cancel = transform.GetChild(6).GetComponent<Button>();
        cancel.onClick.AddListener(() =>
        {
            onCancel?.Invoke();
            Close();
        });
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += OnClick;
    }
    private void OnDisable()
    {
        inputActions.UI.Click.performed -= OnClick;
        inputActions.UI.Disable();
    }
    private void OnClick(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 PositionDiff = screenPos - (Vector2)transform.position;// 오브젝트 피봇위치에서 얼마나 떨어져있는지

        RectTransform rectTransform = (RectTransform)transform;
        if (!rectTransform.rect.Contains(PositionDiff)) //클릭한 위치가  이 오브젝트의 범위안에(rect) 있지 않다면 Close;
        {
            Close();
        }

    }

    public void Open(InvenSlot target)
    {
        if (!target.isEmpty && target.ItemCount > minItemCount)
        {
            targetSlot = target;
            itemIcon.sprite = targetSlot.ItemData.itemIcon;
            slider.minValue = minItemCount;
            slider.maxValue = targetSlot.ItemCount - 1;
            ItemSplitCount = minItemCount;
            gameObject.SetActive(true);
        }
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
