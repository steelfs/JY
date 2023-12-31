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

public class InputBox_Test : MonoBehaviour
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

    //MazeType mazeType;
    //public MazeType MazeType
    //{
    //    get { return mazeType; }
    //    set 
    //    {
    //        mazeType = value;
    //        switch (mazeType)
    //        {
    //            case MazeType.None:
    //                on_MakeBoard = null;
    //                on_ClearBoard = null;
    //                on_MakeMaze = null;
    //                break;
    //            case MazeType.BackTracking:
    //                on_MakeBoard = Make_Board;
    //                on_ClearBoard = ClearBoard;
    //                on_MakeMaze = MakeMaze;
    //                break;
    //            default:
    //                break;
    //        }

    //    }
    //}
    //BackTrackingVisualizer backTrackingVisualizer;
    MazeVisualizer_Test mazeVisualizer_Test;
    MazeVisualizer mazeVisualizer;

    TMP_InputField input_X;
    TextMeshProUGUI placeHolder_X;
    TMP_InputField input_Y;
    TextMeshProUGUI placeHolder_Y;
    TextMeshProUGUI playButtonText;

    TMP_Dropdown Select_DropDown;
    Button makeBoard_Button;
    Button destroy_Button;
    Button makeMaze_Button;
    Button startPoint_Button;
    Button endPoint_Button;
    Button prev_Button;
    Button next_Button;
    Button play_Button;
    const string placeHolderText = "0 ~ 10";

    Action on_PlayMaze;
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
        destroy_Button.onClick.AddListener(ClearBoard);
        makeBoard_Button = transform.GetChild(4).GetComponent<Button>();
        makeBoard_Button.onClick.AddListener(Make_Board);
        makeMaze_Button = transform.GetChild(7).GetComponent<Button>();
        makeMaze_Button.onClick.AddListener(MakeMaze);

        Transform buttons = transform.parent.GetChild(0);
        startPoint_Button = buttons.GetChild(0).GetComponent<Button>();
        prev_Button = buttons.GetChild(1).GetComponent<Button>();
        play_Button = buttons.GetChild(2).GetComponent<Button>();
        next_Button = buttons.GetChild(3).GetComponent<Button>();
        endPoint_Button = buttons.GetChild(4).GetComponent<Button>();

        next_Button.onClick.AddListener(MoveToNext);
        prev_Button.onClick.AddListener(MoveToPrev);
        startPoint_Button.onClick.AddListener(MoveToStartPoint);
        playButtonText = play_Button.GetComponentInChildren<TextMeshProUGUI>();

        endPoint_Button.onClick.AddListener(MoveToEndPoint);
        play_Button.onClick.AddListener(() => on_PlayMaze());

        on_PlayMaze = PlayMaze;
    }
    private void Start()
    {
        mazeVisualizer_Test = GameManager.Visualizer_Test;
        mazeVisualizer = GameManager.Visualizer;
        State = InputBox_State.Standby;
        DropDownValueChange(0);// 초기상태 BackTracking으로 설정
    }   
    void PlayMaze()
    {
        mazeVisualizer_Test.PlayMaze();
        on_PlayMaze = StopMaze;
        playButtonText.text = "∥";
    }
    void StopMaze()
    {
        mazeVisualizer_Test.StopMaze();
        on_PlayMaze = PlayMaze;
        playButtonText.text = "▶";
    }

    void Make_Board()
    {
        if (!mazeVisualizer_Test.IsExistCells)
        {
            mazeVisualizer_Test.MakeBoard(GetInput_X(), GetInput_Y());
        }
        State = InputBox_State.LoadComplete;
    }
    void MakeMaze()
    {
        GameManager.Visualizer_Test.MakeMaze();
        makeMaze_Button.interactable = false;
    }
    void MoveToNext()
    {
        mazeVisualizer_Test.MoveToNext();
    }
    void MoveToPrev()
    {
        mazeVisualizer_Test.MoveToPrev();
    }
    void MoveToStartPoint()
    {
        mazeVisualizer_Test.MoveToStartPoint();
    }
    void MoveToEndPoint()
    {
        mazeVisualizer_Test.MoveToEndPoint();
    }
    void ClearBoard()
    {
        mazeVisualizer_Test.DestroyBoard();
        State = InputBox_State.Standby;
        makeMaze_Button.interactable = true;
    }
    void DropDownValueChange(int value)
    {
        if (GameManager.Visualizer_Test != null)//test모드일 때 실행
        {
            switch (value)
            {
                case 0:
                    GameManager.Visualizer_Test.MazeType = MazeType.BackTracking;
                    break;
                case 1:
                    GameManager.Visualizer_Test.MazeType = MazeType.Wilson;
                    break;
                case 2:
                    GameManager.Visualizer_Test.MazeType = MazeType.Eller;
                    break;
                case 3:
                    GameManager.Visualizer_Test.MazeType = MazeType.kruskal;
                    break;
                case 4:
                    GameManager.Visualizer_Test.MazeType = MazeType.Prim;
                    break;
                case 5:
                    GameManager.Visualizer_Test.MazeType = MazeType.Division;
                    break;
            }
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
