using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantModalPopup : MonoBehaviour
{

    ItemData itemData;
    Slot itemSlot;
    uint itemCount;


    int inputValueCountCheck = 0;

    int selectedValue = 0;
    int SelectedValue 
    {
        get => selectedValue;
        set 
        {
            if (value != selectedValue) 
            { 
                selectedValue = value;

            }
        }
    }


    TextMeshProUGUI warningText;
    
    TMP_InputField valueInput;
    
    Slider valueSlider;
    
    Button successBtn;
    
    Button cancelBtn;

    ModalScript parentModal;

    Merchant_Manager manager;
    private void Awake()
    {
        manager = FindObjectOfType<Merchant_Manager>(); 
        
        parentModal = transform.parent.GetComponent<ModalScript>();

        warningText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

        valueInput = transform.GetChild(1).GetComponent<TMP_InputField>();
        valueInput.onValueChanged.AddListener((value) => 
        {
           
            if (int.TryParse(value,out int parsingValue)) 
            {
                if (parsingValue > inputValueCountCheck)
                {
                    parsingValue = inputValueCountCheck;
                } else if (parsingValue < 0) 
                {
                    parsingValue = 0 ;
                }
                valueInput.text = $"{parsingValue}";
                SelectedValue = parsingValue;
                valueSlider.value = parsingValue;
            }
        });
        

        valueSlider = transform.GetChild(2).GetComponent<Slider>();
        valueSlider.onValueChanged.AddListener((value) =>
        {
            SelectedValue = (int)value;
            valueInput.text = $"{selectedValue}";
        });
        successBtn = transform.GetChild(3).GetComponent<Button>();
        successBtn.onClick.AddListener(() => {
            manager.ItemClick(itemData,(uint)selectedValue, itemSlot);

            parentModal.Close();
        });

        cancelBtn = transform.GetChild(4).GetComponent<Button>();
        cancelBtn.onClick.AddListener(() => { 
            parentModal.Close();
        });
        OnClose();
    }

    public void OnClose() 
    {
        itemData =null;
        itemCount = 0;
        selectedValue = 0;
        itemSlot = null;
        inputValueCountCheck = 0;
        parentModal.Close();
    }

    public void OnPopup(ItemData itemData, uint itemCount , Slot itemSlot = null) 
    {
        this.itemData = itemData;
        this.itemCount = itemCount;
        selectedValue = (int)itemCount;
        this.itemSlot = itemSlot;
        inputValueCountCheck = (int)itemCount;
        valueSlider.maxValue = itemCount;
        if (manager.Selected == Merchant_Selected.Buy) 
        {
            warningText.text = "구매 하시겠습니까?";
        }
        else 
        {
            warningText.text = "판매 하시겠습니까?";
        }
        valueInput.text = $"{selectedValue:0}";

        parentModal.Open();
    }
}
