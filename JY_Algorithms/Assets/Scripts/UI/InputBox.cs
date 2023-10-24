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
    Button makeMaze_Button;
    const string placeHolderText = "0~100";

    Action on_MakeBoard;
    Action on_ClearBoard;
    Action on_MakeMaze;
    private void Awake()
    {
        input_X = transform.GetChild(2).GetComponent<TMP_InputField>();
        input_Y = transform.GetChild(3).GetComponent<TMP_InputField>();
        placeHolder_X = input_X.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        placeHolder_Y = input_Y.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        input_X.onValueChanged.AddListener(InputX_ValueChanged);
        input_Y.onValueChanged.AddListener(InputY_ValueChanged);

        Select_DropDown = transform.GetChild(6).GetComponent<TMP_Dropdown>();
        Select_DropDown.onValueChanged.AddListener(DropDownValueChange);

        destroyButton = transform.GetChild(5).GetComponent<Button>();
        destroyButton.onClick.AddListener(() => on_ClearBoard());

        generateButton = transform.GetChild(4).GetComponent<Button>();
        generateButton.onClick.AddListener(() => on_MakeBoard());

        makeMaze_Button = transform.GetChild(7).GetComponent<Button>();
        makeMaze_Button.onClick.AddListener(() => on_MakeMaze());
    }
    private void Start()
    {
        backTrackingVisualizer = GameManager.BackTrackingVisualizer;
        DropDownValueChange(0);
    }
   
    void Make_BackTracking_Board()
    {
        backTrackingVisualizer.MakeBoard(GetInput_X(), GetInput_Y());
    }
    void MakeMaze_BackTracking()
    {
        backTrackingVisualizer.backTracking.MakeMaze();
    }
    void ClearBoard_Recursive_BackTracking()
    {
        backTrackingVisualizer.DestroyBoard();
    }
    void DropDownValueChange(int value)
    {
        switch (value)
        {
            case 0:
                on_MakeBoard = Make_BackTracking_Board;
                on_ClearBoard = ClearBoard_Recursive_BackTracking;
                on_MakeMaze = MakeMaze_BackTracking;
                break;
            case 1:
                on_MakeBoard = null;
                on_ClearBoard = null;
                on_MakeMaze = null;
                break;
            case 2:
                on_MakeBoard = null;
                on_ClearBoard = null;
                on_MakeMaze = null;
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
