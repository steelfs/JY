using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum InputBox_State
{
    Standby,
    Loading,
    LoadComplete
}
public enum MazeType
{
    None,
    BackTracking,
}
public class InputBox : MonoBehaviour
{
    InputBox_State state;
    public InputBox_State State
    {
        get { return state; }
        set
        {
            state = value;
            switch (state)
            {
                case InputBox_State.Standby:
                    makeBoard_Button.interactable = true;
                    makeMaze_Button.interactable = false;
                    destroy_Button.interactable = false;
                    startPoint_Button.interactable = false;
                    prev_Button.interactable = false;
                    play_Button.interactable = false;
                    next_Button.interactable = false;
                    endPoint_Button.interactable = false;
                    break;
                case InputBox_State.Loading:
                    makeBoard_Button.interactable = true;
                    makeMaze_Button.interactable = false;
                    destroy_Button.interactable = false;
                    startPoint_Button.interactable = false;
                    prev_Button.interactable = false;
                    play_Button.interactable = false;
                    next_Button.interactable = false;
                    endPoint_Button.interactable = false;
                    break;
                case InputBox_State.LoadComplete:
                    makeBoard_Button.interactable = false;
                    makeMaze_Button.interactable = true;
                    destroy_Button.interactable = true;
                    startPoint_Button.interactable = true;
                    prev_Button.interactable = true;
                    play_Button.interactable = true;
                    next_Button.interactable = true;
                    endPoint_Button.interactable = true;
                    break;
                default:
                    break;
            }
        }
    }

    MazeType mazeType;
    public MazeType MazeType
    {
        get { return mazeType; }
        set 
        {
            mazeType = value;
            switch (mazeType)
            {
                case MazeType.None:
                    on_MakeBoard = null;
                    on_ClearBoard = null;
                    on_MakeMaze = null;
                    break;
                case MazeType.BackTracking:
                    on_MakeBoard = Make_BackTracking_Board;
                    on_ClearBoard = ClearBoard;
                    on_MakeMaze = MakeMaze;
                    break;
                default:
                    break;
            }

        }
    }
    //BackTrackingVisualizer backTrackingVisualizer;
    MazeVisualizer mazeVisualizer;
    TMP_InputField input_X;
    TextMeshProUGUI placeHolder_X;
    TMP_InputField input_Y;
    TextMeshProUGUI placeHolder_Y;

    TMP_Dropdown Select_DropDown;
    Button makeBoard_Button;
    Button destroy_Button;
    Button makeMaze_Button;
    Button startPoint_Button;
    Button endPoint_Button;
    Button prev_Button;
    Button next_Button;
    Button play_Button;
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
        destroy_Button = transform.GetChild(5).GetComponent<Button>();
        destroy_Button.onClick.AddListener(() => on_ClearBoard());
        makeBoard_Button = transform.GetChild(4).GetComponent<Button>();
        makeBoard_Button.onClick.AddListener(() => on_MakeBoard());
        makeMaze_Button = transform.GetChild(7).GetComponent<Button>();
        makeMaze_Button.onClick.AddListener(() => on_MakeMaze());

        Transform buttons = transform.parent.GetChild(0);
        startPoint_Button = buttons.GetChild(0).GetComponent<Button>();
        prev_Button = buttons.GetChild(1).GetComponent<Button>();
        play_Button = buttons.GetChild(2).GetComponent<Button>();
        next_Button = buttons.GetChild(3).GetComponent<Button>();
        endPoint_Button = buttons.GetChild(4).GetComponent<Button>();

        next_Button.onClick.AddListener(MoveToNext);

    }
    private void Start()
    {
        State = InputBox_State.Standby;
        DropDownValueChange(0);// 초기상태 BackTracking으로 설정
    }   
   
    void Make_BackTracking_Board()
    {
        if (!mazeVisualizer.IsExistCells)
        {
            mazeVisualizer.MakeBoard(GetInput_X(), GetInput_Y());
        }
        State = InputBox_State.LoadComplete;
    }
    void MakeMaze()
    {
        BackTrackingVisualizer backTrackingVisualizer = mazeVisualizer as BackTrackingVisualizer;
        backTrackingVisualizer.backTracking.MakeMaze();

        makeMaze_Button.interactable = false;
    }
    void MoveToNext()
    {
        mazeVisualizer.MoveToNext();
    }
    void ClearBoard()
    {
        mazeVisualizer.DestroyBoard();
        State = InputBox_State.Standby;
        makeMaze_Button.interactable = true;
    }
    void DropDownValueChange(int value)
    {
        switch (value)
        {
            case 0:
                MazeType = MazeType.BackTracking;
                mazeVisualizer = GameManager.BackTrackingVisualizer;
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
