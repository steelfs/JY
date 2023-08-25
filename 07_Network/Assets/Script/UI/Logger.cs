using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    const int maxLineCount = 20;// �ΰſ��� ǥ�� ������ �ִ� �� �� 
    int currentLine = 0;
    TextMeshProUGUI log;

    StringBuilder sb;//���ڿ��� ��ġ�� ���� Ŭ����
    private void Awake()
    {
        log = GetComponent<TextMeshProUGUI>();
        sb = new StringBuilder(maxLineCount);
    }
    private void Start()
    {
        Clear();
    }
    // string str1 = "11111"; //���� �ٲٴ� ���� ������ �߻� 
    // string str2 = "22222";
    //
    // string str3 = str1 + "\n" + str2;//�־�
    // string str4 = $"{str1}\n{str2}";// ���� // ���� ��ĥ���� sb�� ȿ���� �ö󰣴�.

    //logText = "[����]�մϴ�. {���}�Դϴ�."
    //�����̶�� ���ڴ� ���������� ���, �����ڴ� �����
    //"<#ff0000>1111</color>"
    //"<#00ff00>1111</color>"
    //string str5 = "(������){������}[�ٴٴ�]";
    //string[] result = str5.Split('(', ')');

    //sb.AppendLine
    public void Log(string logText)
    {
        sb.Append("</color>");
        for (int i = 0; i < logText.Length; i++)
        {
            if (logText[i] == '[')
            {
                sb.Append("<color=#ff0000>");
            }
            else if (logText[i] == ']')
            {
                sb.Append("</color>");
            }
            else if (logText[i] == '{')
            {
                sb.Append("<color=#ffff00>");
            }
            else if (logText[i] == '}')
            {
                sb.Append("</color>");
            }
            else
            {
                sb.Append(logText[i]);
            }
        }
        sb.AppendLine();
        
       
        log.text = sb.ToString();
    }
    void Clear()//�ΰſ� ǥ�õǴ� ���� ��� ����� 
    {
        log.text = "";
    }
}
