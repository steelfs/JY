using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Logger : MonoBehaviour
{
    TMP_InputField inputField;
    public Color warningColor;
    public Color errorColor;

    const int maxLineCount = 20;// 로거에서 표시 가능한 최대 줄 수 
    int currentLine = 0;
    TextMeshProUGUI log;

    StringBuilder sb;//문자열을 합치기 위한 클래스
    Queue<string> logLines;

    private void Awake()
    {
        log = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        sb = new StringBuilder(maxLineCount + 5);
        logLines = new Queue<string>(maxLineCount + 5);//string 타입의 32비트
        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.onSubmit.AddListener((input) => // onSubmit 이벤트에 입력이 완료되었을 때 실행, 비어있을 때나 focus를 옮길 때는 발동하지 않음. EndEdit은 focus를 옮겨도 실행된다.
        {
            if (GameManager.Inst.Player != null)
            {
                NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();//접속중이면 채팅으로
            }
            else
            {
                Log(input); //아니면 로그만 찍기
            }
            GameManager.Inst.Player.SendChat(input);
            inputField.text = string.Empty;//입력창 비우고
            inputField.ActivateInputField();//포커스 다시 활성화(무조건 활성화)
           // inputField.Select();//활성화 되어있으면 비활성화 , 비활성화되어있을 때는 활성화 
        });
    }
    private void Start()
    {
        Clear();
    }
    // string str1 = "11111"; //값을 바꾸는 순간 가비지 발생 
    // string str2 = "22222";
    //
    // string str3 = str1 + "\n" + str2;//최악
    // string str4 = $"{str1}\n{str2}";// 차악 // 많이 합칠수록 sb의 효율이 올라간다.

    //logText = "[위험]합니다. {경고}입니다."
    //위험이라는 글자는 빨간색으로 출력, 경고글자는 노란색
    //"<#ff0000>1111</color>"
    //"<#00ff00>1111</color>"
    //string str5 = "(가가가){나나나}[다다다]";
    //string[] result = str5.Split('(', ')');


    //sb.AppendLine
    public void Log(string logText)
    {
        logText = Emphasize(logText, '[', ']', errorColor);
        logText = Emphasize(logText, '{', '}', warningColor);

        logLines.Enqueue(logText);
        if (logLines.Count > maxLineCount)//줄 갯수 초과시 맨처음 입력 제거
        {
            logLines.Dequeue();
        }
        sb.Clear();
        foreach(string line in logLines)
        {
            sb.AppendLine(line);
        }
        log.text = sb.ToString();
    }

    string Emphasize(string source, char open, char close, Color color)
    {
        string result = source;
        if (IsPair(source, open, close))
        {
            string[] split = result.Split(open, close);

            string colorText = ColorUtility.ToHtmlStringRGB(color);


            result = string.Empty;

            for (int  i = 0; i < split.Length; i++) // 토큰 단위로 처리
            {
                result += split[i];
                if (i != split.Length - 1)
                {
                    if (i % 2 == 0)
                    {
                        result += $"<#{colorText}>";
                    }
                    else
                    {
                        result += "</color>";
                    }
                }
            }
        }
        return result;
    }

    bool IsPair(string source, char open, char close)
    {
        //정확한 괄호의 조건 
        //1. 열리면 닫혀야 한다.
        //연속해서 여는 것은 안된다.
        bool result = true;

        int count = 0;
        for (int i = 0; i < source.Length; i++)
        {
            if (source[i] == open || source[i] == close)
            {
                count++;
                if (count % 2 == 1)//카운트가 홀수면
                {
                    if (source[i] != open)//홀수이면서 open이 아니면 false
                    {
                        result = false;
                        break;
                    }
                }
                else
                {
                    if (source[i] != close)//짝수이면서 close가 아닐 때
                    {
                        result = false;
                        break;
                    }
                }
            }
        }
        if (count == 0 || count % 2 != 0)//괄호가 하나도 없거나 쌍이 안맞으면 false;
        {
            result = false;
        }
        return  result;
    }

    void Clear()//로거에 표시되는 글자 모두 지우기 
    {
        log.text = "";
    }
    public void TestClear()
    {
        Clear();
    }
    public bool TestIsPair(string source, char open, char close)
    {
        return IsPair(source, open, close);
    }
}
