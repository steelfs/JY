using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    const int maxLineCount = 20;// 로거에서 표시 가능한 최대 줄 수 
    int currentLine = 0;
    TextMeshProUGUI log;

    StringBuilder sb;//문자열을 합치기 위한 클래스
    private void Awake()
    {
        log = GetComponent<TextMeshProUGUI>();
        sb = new StringBuilder(maxLineCount);
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
    public void Log(string logText)// 로거에 문장 추가하는 함수 //색깔 변경할 문자를 미리 저장 후 어디서 삽입할지를 구하기 위해 몇번째 인덱스에서 [], {} 가 나왔는지 저장 후 그곳에 삽입
    {
        string[] red = logText.Split('[', ']');
        string[] yellow = logText.Split('{', '}');

        char[] chars = new char[logText.Length];
        for (int i = 0; i < logText.Length; i++)
        {
            if (logText[i] == '[')
            {
                for (int j = i; j < logText.Length; j++)
                {
                    if (logText[j] == ']')
                    {

                    }
                }
            }
            else if (logText[i] == '{')
            {

            }
            else
            {
                chars[i] = logText[i];
            }
        }

     
        sb.Append($"<#ff0000>{yellow[1]}</color>");
        
       
        log.text = sb.ToString();
    }
    void Clear()//로거에 표시되는 글자 모두 지우기 
    {
        log.text = "";
    }
}
