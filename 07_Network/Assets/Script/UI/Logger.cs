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
    public void Log(string logText)// �ΰſ� ���� �߰��ϴ� �Լ� //���� ������ ���ڸ� �̸� ���� �� ��� ���������� ���ϱ� ���� ���° �ε������� [], {} �� ���Դ��� ���� �� �װ��� ����
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
    void Clear()//�ΰſ� ǥ�õǴ� ���� ��� ����� 
    {
        log.text = "";
    }
}
