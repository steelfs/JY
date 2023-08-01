using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemSpliterUI : MonoBehaviour
{
    InvenSlot targetSlot;// split�� ����
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
            if( uint.TryParse(text, out uint result))  //����ȯ �����ϸ� ��ǲ�ʵ��� ���� uint�� �ٲ� ��  ItemSplitcount ������Ƽ �� �����ؼ� ����
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
            onOkClick?.Invoke(targetSlot.Index, ItemSplitCount); //ok��ư Ŭ���� ��ȣ�� ������ �ݱ�
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
        Vector2 PositionDiff = screenPos - (Vector2)transform.position;// ������Ʈ �Ǻ���ġ���� �󸶳� �������ִ���

        RectTransform rectTransform = (RectTransform)transform;
        if (!rectTransform.rect.Contains(PositionDiff)) //Ŭ���� ��ġ��  �� ������Ʈ�� �����ȿ�(rect) ���� �ʴٸ� Close;
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
