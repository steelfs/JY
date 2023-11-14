using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public enum QuestionType
{
    Free_Input,
    Select_Answer
}
public class QuestionPanel : MonoBehaviour
{

    CanvasGroup canvasGroup;
    
    TextMeshProUGUI question_Text;
    QuestionType question_Type;
    Select_Answer select_Answer;
    WaitForSeconds moment;
    WaitForSeconds second;
    StringBuilder npc_StringBuilder;

    CanvasGroup freeInput_CanvasGroup;
    TextMeshProUGUI showInput;
    TMP_InputField inputField;
    public TMP_InputField InputField => inputField;
    StringBuilder Player_StringBuilder;

    bool isCorrectAnswer = true;
    const int ChanceCount = 3;
    int remainCount = 0;
    const string Free_Input01 = "Say Something";
    const string Free_Input02 = "Say Anything You Want";
    const string Free_Input03 = "Hello There";
    const string Free_Input04 = "Waht's Up Buddy";
    string[] npc_Scripts;
    public QuestionType Question_Type
    {
        get => question_Type;
        set
        {
            question_Type = value;
            switch (question_Type)
            {
                case QuestionType.Free_Input:
                    FreeInputOpen();
                    StartCoroutine(Print_NPC_Input(npc_Scripts[Random.Range(0, npc_Scripts.Length)]));
                    break;
                case QuestionType.Select_Answer:
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
        freeInput_CanvasGroup = child.GetComponent<CanvasGroup>();
        inputField = child.GetChild(0).GetComponent<TMP_InputField>();
        showInput = child.GetChild(1).GetComponent<TextMeshProUGUI>();

        moment = new WaitForSeconds(0.02f);
        second = new WaitForSeconds(0.3f);
        canvasGroup = GetComponent<CanvasGroup>();
        child = transform.GetChild(2);
        select_Answer = child.GetChild(1).GetComponent<Select_Answer>();
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
    }
   

    public void FreeInputOpen()
    {
        inputField.text = string.Empty;
        showInput.text = string.Empty;
        inputField.Select();
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
    public void FreeInputClose()
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

        yield return second;
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
        RecieveReward(Player_input.Length);
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
