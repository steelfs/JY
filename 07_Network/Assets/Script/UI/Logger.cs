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

    const int maxLineCount = 20;// �ΰſ��� ǥ�� ������ �ִ� �� �� 
    int currentLine = 0;
    TextMeshProUGUI log;

    StringBuilder sb;//���ڿ��� ��ġ�� ���� Ŭ����
    Queue<string> logLines;

    private void Awake()
    {
        log = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        sb = new StringBuilder(maxLineCount + 5);
        logLines = new Queue<string>(maxLineCount + 5);//string Ÿ���� 32��Ʈ
        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.onSubmit.AddListener((input) => // onSubmit �̺�Ʈ�� �Է��� �Ϸ�Ǿ��� �� ����, ������� ���� focus�� �ű� ���� �ߵ����� ����. EndEdit�� focus�� �Űܵ� ����ȴ�.
        {
            if (GameManager.Inst.Player != null)
            {
                NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();//�������̸� ä������
            }
            else
            {
                Log(input); //�ƴϸ� �α׸� ���
            }
            GameManager.Inst.Player.SendChat(input);
            inputField.text = string.Empty;//�Է�â ����
            inputField.ActivateInputField();//��Ŀ�� �ٽ� Ȱ��ȭ(������ Ȱ��ȭ)
           // inputField.Select();//Ȱ��ȭ �Ǿ������� ��Ȱ��ȭ , ��Ȱ��ȭ�Ǿ����� ���� Ȱ��ȭ 
        });
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
        logText = Emphasize(logText, '[', ']', errorColor);
        logText = Emphasize(logText, '{', '}', warningColor);

        logLines.Enqueue(logText);
        if (logLines.Count > maxLineCount)//�� ���� �ʰ��� ��ó�� �Է� ����
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

            for (int  i = 0; i < split.Length; i++) // ��ū ������ ó��
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
        //��Ȯ�� ��ȣ�� ���� 
        //1. ������ ������ �Ѵ�.
        //�����ؼ� ���� ���� �ȵȴ�.
        bool result = true;

        int count = 0;
        for (int i = 0; i < source.Length; i++)
        {
            if (source[i] == open || source[i] == close)
            {
                count++;
                if (count % 2 == 1)//ī��Ʈ�� Ȧ����
                {
                    if (source[i] != open)//Ȧ���̸鼭 open�� �ƴϸ� false
                    {
                        result = false;
                        break;
                    }
                }
                else
                {
                    if (source[i] != close)//¦���̸鼭 close�� �ƴ� ��
                    {
                        result = false;
                        break;
                    }
                }
            }
        }
        if (count == 0 || count % 2 != 0)//��ȣ�� �ϳ��� ���ų� ���� �ȸ����� false;
        {
            result = false;
        }
        return  result;
    }

    void Clear()//�ΰſ� ǥ�õǴ� ���� ��� ����� 
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
