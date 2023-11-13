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
    Free_Input free_Input;
    Select_Answer select_Answer;
    WaitForSeconds moment;
    WaitForSeconds second;
    StringBuilder sb;
    const string Free_Input01 = "Say Something";
    const string Free_Input02 = "Say Anything You Want";
    const string Free_Input03 = "Hello There";
    const string Free_Input04 = "Waht's Up Buddy";
    string[] free_Input_Arr;
    public QuestionType Question_Type
    {
        get => question_Type;
        set
        {
            question_Type = value;
            switch (question_Type)
            {
                case QuestionType.Free_Input:
                    free_Input.Open();
                    StartCoroutine(PrintText_For_Free_Input());
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
        moment = new WaitForSeconds(0.07f);
        second = new WaitForSeconds(1);
        canvasGroup = GetComponent<CanvasGroup>();
        Transform child = transform.GetChild(2);
        free_Input = child.GetChild(0).GetComponent<Free_Input>();
        select_Answer = child.GetChild(1).GetComponent<Select_Answer>();
        question_Text = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        sb = new StringBuilder(50);
        free_Input_Arr = new string[4];
        free_Input_Arr[0] = Free_Input01;
        free_Input_Arr[1] = Free_Input02;
        free_Input_Arr[2] = Free_Input03;
        free_Input_Arr[3] = Free_Input04;
    }
    public void Open()
    {
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
    IEnumerator PrintText_For_Free_Input()
    {
        sb.Clear();
        question_Text.text = string.Empty;
        yield return second;
        string chosen = free_Input_Arr[Random.Range(0, free_Input_Arr.Length)];
        for (int i = 0; i < chosen.Length; i++)
        {
            sb.Append(chosen[i]);
            question_Text.text = sb.ToString();
            yield return moment;
        }
    }
}
