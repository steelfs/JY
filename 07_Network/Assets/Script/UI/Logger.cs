using System;
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
            if (input[0] == '/')
            {
                //개발용
                ConsoleCommand(input);
            }
            else
            {
                if (GameManager.Inst.Player != null)
                {
                    //일반 채팅 처리
                    NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();//접속중이면 채팅으로
                }
                else
                {
                    //접속해있지 않은 상황
                    Log(input); //아니면 로그만 찍기
                }
            }
           // GameManager.Inst.Player.SendChat(input);
            inputField.text = string.Empty;//입력창 비우고
            inputField.ActivateInputField();//포커스 다시 활성화(무조건 활성화)
           // inputField.Select();//활성화 되어있으면 비활성화 , 비활성화되어있을 때는 활성화 
        });
    }
    private void Start()
    {
        Clear();
    }
    void ConsoleCommand(string commandLine)
    {
        int space = commandLine.IndexOf(' ');//첫번째 스페이스의 인덱스 
        string commandToken = commandLine.Substring(0, space);//0번째부터 space 인덱스까지 
        string parameterToken = commandLine.Substring(space + 1);// space 이후 인덱스부터
        commandToken = commandToken.ToLower();

        switch (commandToken)
        {
            case "/setname":
                GameManager.Inst.UserName = parameterToken;
                Debug.Log("이름 변경");
                break;
            case "/setcolor":
                string[] splitNumbers = parameterToken.Split(',', ' ');

                float[] colorValue = new float[4] { 0, 0, 0, 0 };
                int count = 0;
                foreach (string number in splitNumbers)
                {
                    if (number.Length == 0)// 비었으면 스킵
                        continue;

                    if (count > 3)
                    {
                        break;
                    }
                    if (!float.TryParse(number, out colorValue[count]))//number 를 float 으로 변환 colorValue의 count 번째 인덱스에 복사됨
                    {
                        colorValue[count] = 0;
                    }
                    count++;
                }
                for (int i = 0; i < colorValue.Length; i++)
                {
                    colorValue[i] = Mathf.Clamp01(colorValue[i]);
                }
             
                Color color = new Color(colorValue[0], colorValue[1], colorValue[2], colorValue[3]);
                if (GameManager.Inst.PlayerDeco != null)
                {
                    GameManager.Inst.PlayerDeco.SetColor(color);
                }
                break;
            default:
                break;
        }
    }


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
