using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputBox : MonoBehaviour
{
    BackTrackingVisualizer backTrackingVisualizer;

    TMP_InputField input_X;
    TextMeshProUGUI placeHolder_X;
    TMP_InputField input_Y;
    TextMeshProUGUI placeHolder_Y;

    TMP_Dropdown Select_DropDown;
    Button generateButton;
    Button destroyButton;
    const string placeHolderText = "0~100";

    Action on_ActiveGenerate;
    Action on_ClearBoard;
    private void Awake()
    {
        input_X = transform.GetChild(2).GetComponent<TMP_InputField>();
        input_Y = transform.GetChild(3).GetComponent<TMP_InputField>();
        placeHolder_X = input_X.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        placeHolder_Y = input_Y.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        input_X.onValueChanged.AddListener(InputX_ValueChanged);
        input_Y.onValueChanged.AddListener(InputY_ValueChanged);

        Select_DropDown = transform.GetChild(6).GetComponent<TMP_Dropdown>();
        Select_DropDown.onValueChanged.AddListener(DropDownValueChange);

        destroyButton = transform.GetChild(5).GetComponent<Button>();
        destroyButton.onClick.AddListener(() => on_ClearBoard());

        generateButton = transform.GetChild(4).GetComponent<Button>();
        generateButton.onClick.AddListener(() => on_ActiveGenerate());
    }
    private void Start()
    {
        backTrackingVisualizer = GameManager.BackTrackingVisualizer;
        DropDownValueChange(0);
    }
   
    void MakeRecursive_BackTracking()
    {
        backTrackingVisualizer.MakeBoard(GetInput_X(), GetInput_Y());
    }
    void ClearBoard_MakeRecursive_BackTracking()
    {
        backTrackingVisualizer.DestroyBoard();
    }
    void DropDownValueChange(int value)
    {
        switch (value)
        {
            case 0:
                on_ActiveGenerate = MakeRecursive_BackTracking;
                on_ClearBoard = ClearBoard_MakeRecursive_BackTracking;
                break;
            case 1:
                on_ActiveGenerate = null;
                on_ClearBoard = null; 
                break;
            case 2:
                on_ActiveGenerate = null;
                on_ClearBoard = null;
                break;
        }
    }
    int GetInput_X()
    {
        int result = int.Parse(input_X.text);
        return result;
    }
    int GetInput_Y()
    {
        int result = int.Parse(input_Y.text);
        return result;
    }
    void InputX_ValueChanged(string input)
    {
        if (input_X.text != "")
        {
            placeHolder_X.text = string.Empty;
        }
        else
        {
            placeHolder_X.text = placeHolderText;
        }
        int value;
        if(int.TryParse(input, out value))
        {
            if (value > 100)
            {
                input_X.text = "100";
            }
            else if (value < 0)
            {
                input_X.text = "0";
            }
        }
    }
    void InputY_ValueChanged(string input)
    {
        if (input_Y.text != "")
        {
            placeHolder_Y.text = string.Empty;
        }
        else
        {
            placeHolder_Y.text = placeHolderText;
        }
        int value;
        if (int.TryParse(input, out value))
        {
            if (value > 100)
            {
                input_Y.text = "100";
            }
            else if (value < 0)
            {
                input_Y.text = "0";
            }
        }
 
    }


}
