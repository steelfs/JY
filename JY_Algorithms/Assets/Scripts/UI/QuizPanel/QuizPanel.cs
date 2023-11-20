using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public enum QuestionType
{
    Free_Input,
    Select_Answer
}
public class QuizPanel : MonoBehaviour
{
    public int current_Player_Position { get; set; } = 0;
    public CanvasGroup canvasGroup;
    public CanvasGroup freeInput_CanvasGroup;
    public CanvasGroup selectAnswer_CanvasGroup;

    public TextMeshProUGUI[] options;
    public TextMeshProUGUI quiz_Text;
    
    TextMeshProUGUI question_Text;
    QuestionType question_Type;
    WaitForSeconds moment;
    WaitForSeconds second;
    StringBuilder npc_StringBuilder;

    TextMeshProUGUI showInput;
    TMP_InputField inputField;
    public TMP_InputField InputField => inputField;
    StringBuilder Player_StringBuilder;
    Quiz quiz = null;


    bool isCorrectAnswer = true;
    const int ChanceCount = 3;
    int remainCount = 0;
    const string Free_Input01 = "Say Something";
    const string Free_Input02 = "Say Anything You Want";
    const string Free_Input03 = "Hello There";
    const string Free_Input04 = "Waht's Up Buddy";
    string[] npc_Scripts;

    public Action on_QuizPopUp;
    public QuestionType Question_Type
    {
        get => question_Type;
        set
        {
            question_Type = value;
            switch (question_Type)
            {
                case QuestionType.Free_Input:
                    on_QuizPopUp = QuizPopup_FreeInput;
                    break;
                case QuestionType.Select_Answer:
                    on_QuizPopUp = QuizPopup_Select;
                    break;
                default:
                    break;
            }
        }
    }
    
    private void Awake()
    {
        Player_StringBuilder = new StringBuilder(50);
        Transform child = transform.GetChild(2).GetChild(0);
        inputField = child.GetChild(0).GetComponent<TMP_InputField>();
        showInput = child.GetChild(1).GetComponent<TextMeshProUGUI>();

        moment = new WaitForSeconds(0.03f);
        second = new WaitForSeconds(0.3f);
        child = transform.GetChild(2);
        question_Text = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        npc_StringBuilder = new StringBuilder(50);
        npc_Scripts = new string[4];
        npc_Scripts[0] = Free_Input01;
        npc_Scripts[1] = Free_Input02;
        npc_Scripts[2] = Free_Input03;
        npc_Scripts[3] = Free_Input04;
    }
    public void Open()
    {
        remainCount = ChanceCount;
        canvasGroup.alpha = 0.7f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        Player_StringBuilder.Clear();
        npc_StringBuilder.Clear();
    }
    void QuizPopup_FreeInput()
    {
        Open();
        OpenFreeInput();
        StartCoroutine(Print_NPC_Input(npc_Scripts[UnityEngine.Random.Range(0, npc_Scripts.Length)]));

    }
    void QuizPopup_Select()
    {
        Open();
        Open_SelectAnswer();
        StartCoroutine(Print_SelectQuiz());
    }
    IEnumerator Print_SelectQuiz()
    {
        this.quiz = GameManager.QuizData_English.Quizzes[(current_Player_Position % 10)];//
        quiz.ShuffleOptions();//정답 순서 섞기
        string question = quiz.Question;
        Player_StringBuilder.Clear();
        for (int i = 0; i < question.Length; i++)//문제 출력
        {
            Player_StringBuilder.Append(question[i]);
            quiz_Text.text = Player_StringBuilder.ToString();
            yield return moment;
        }

        for (int i = 0; i < options.Length; i++)//보기 출력
        {
            string option = quiz.Options[i];
            Player_StringBuilder.Clear();
            for (int j = 0; j < option.Length; j++)
            {
                Player_StringBuilder.Append(option[j]);
                options[i].text = $"{i + 1}) {Player_StringBuilder.ToString()}";
                yield return moment;
            }
        }
    }
    public void CheckingAnswer(int selectedNumber)
    {
        if (this.quiz.CorrectAnswer_Index != selectedNumber)
        {
            GameManager.Inst.CloseQuestionPanel();
            GameManager.Visualizer.ShowMoveRange();
        }
        else
        {
            GameManager.Visualizer.SetTerritory();
            GameManager.Inst.CloseQuestionPanel();
            GameManager.Visualizer.ShowMoveRange();
        }
    }
    void Open_SelectAnswer()
    {
        selectAnswer_CanvasGroup.alpha = 1.0f;
        selectAnswer_CanvasGroup.interactable = true;
        selectAnswer_CanvasGroup.blocksRaycasts = true;

    }
    void Close_SelectAnswer()
    {
        selectAnswer_CanvasGroup.alpha = 0;
        selectAnswer_CanvasGroup.blocksRaycasts = false;
        selectAnswer_CanvasGroup.interactable = false;
    }
    public void OpenFreeInput()
    {
        inputField.text = string.Empty;
        showInput.text = string.Empty;
        inputField.Select();
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
    public void CloseFreeInput()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
    
    IEnumerator Print_Player_Input(string input)
    {
        Player_StringBuilder.AppendLine();
        for (int i = 0; i < input.Length; i++)
        {
            Player_StringBuilder.Append(input[i]);
            showInput.text = Player_StringBuilder.ToString();
            yield return moment;
        }
    }
    IEnumerator Print_NPC_Input(string answer, string playerInput = "")
    {
        npc_StringBuilder.Clear();
        question_Text.text = string.Empty;
        if (isCorrectAnswer && playerInput != "")
        {
            answer = $"Well Done!\nYou Have Type \"{playerInput}\".\nThat Means \"{answer}\"";
        }
        //yield return second;
        for (int i = 0; i < answer.Length; i++)
        {
            npc_StringBuilder.Append(answer[i]);
            question_Text.text = npc_StringBuilder.ToString();
            yield return moment;
        }
    }
    IEnumerator ShowResult(string Player_input, string NPC_answer)
    {
        yield return Print_Player_Input(Player_input);
        yield return Print_NPC_Input(NPC_answer, Player_input);
        if (isCorrectAnswer)
        {
            yield return second;
            GameManager.Inst.CloseQuestionPanel();
            GameManager.ToolBox.IncreaseCash(Player_input.Length);
        }
    }
    public void FreeInput_Accepted()
    {
        string input = InputField.text;
        if (input != "")
        {
            if (Dictionary_DataManager.Inst.ContainsWord(input, out string value))
            {
                isCorrectAnswer = true;
                Debug.Log($"{input}__{value}!");
                StartCoroutine(ShowResult(input, value));
                inputField.text = string.Empty;

            }
            else
            {
                isCorrectAnswer = false;
                remainCount--;
                Debug.Log($"{input}은 데이터에 없습니다.");
                inputField.text = string.Empty;
                string wrongAnswer = $"There is No Word Like \"{input}\" \nYou Have{remainCount} More Chance ";
                StartCoroutine(ShowResult(input, wrongAnswer));
                inputField.Select();
            }
        }
    }
    void RecieveReward(int amount)
    {

    }
}
