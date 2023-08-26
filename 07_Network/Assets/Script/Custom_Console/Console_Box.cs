using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Console_Box : MonoBehaviour
{
    TextMeshProUGUI consoleText;
    TMP_InputField inputField;

    RectTransform rectTransform;
    PlayerInputAction inputActions;
    StringBuilder sb;

    public float popUpSpeed = 3.0f;
    public float popUpDuration = 0.5f;
    const int maxLine = 5;

    List<string> consoleList = new List<string>();

    bool isOpen = false;
    private void Awake()
    {
        inputField = transform.GetChild(0).GetComponent<TMP_InputField>();
        consoleText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        inputField.onEndEdit.AddListener((input) => OnEndEdit(input));
        rectTransform = GetComponent<RectTransform>();
        inputActions = new();
        sb = new StringBuilder(maxLine);
    }
    private void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Console.performed += OnOpen_Console;
        inputActions.Test.Console_Clear.performed += OnConsole_Clear;
    }

    private void OnConsole_Clear(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        consoleList.Clear();
        sb.Clear();
        consoleText.text = "";
    }

    void OnEndEdit(string input)
    {
        sb.Append("</color>");
        if (Include(input))
        {
            ChangeFontColor(input);
        }
        else
        {
            sb.AppendLine(input);
            consoleList.Add(input);
        }

  
        if (consoleList.Count > 5)
        {
            consoleList.RemoveAt(0);
            sb.Clear();
            for (int i = 0; i < consoleList.Count; i++)//갯수가 넘치면 가장 먼저 입력한 Line을지우고 업데이트  완료시점
            {
                sb.AppendLine(consoleList[i]);
            }
        }
        consoleText.text = sb.ToString();
    }
    void ChangeFontColor(string input)
    {
        char[] chars = input.ToCharArray();
        string value = "";
        for (int i = 0; i < chars.Length; i++)
        {
            switch (chars[i])
            {
                case '[':
                    sb.Append("<color=#ff0000>");
                    value += "<color=#ff0000>";
                    break;
                case '{':
                    sb.Append("<color=#ffff00>");
                    value += "<color=#ffff00>";
                    break;
                case ']':
                case '}':
                    sb.Append("</color>");
                    value += "</color>";
                    break;
                default:
                    sb.Append(chars[i]);
                    value += chars[i];
                    break;
            }
        }
        sb.AppendLine();
        consoleList.Add(value);
    }
    bool Include(string input)
    {
        foreach(char c in input)
        {
            switch(c)
            {
                case '[':
                    return true;
                case ']':
                    return true;
                case '{':
                    return true;
                case '}':
                    return true;
                default:
                    break;
            }
        }
        return false; 
    }
    private void OnOpen_Console(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        if (!isOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
      
    }

    void Open()
    {
        isOpen = true;
        StartCoroutine(Popup(popUpSpeed, popUpDuration));
    }
    void Close()
    {
        isOpen = false;
        StartCoroutine(Close(popUpSpeed, popUpDuration));
    }
    IEnumerator Popup(float speed, float duration)
    {
        Vector2 pos = new Vector2(0, -200);
        float increaseValue = speed / duration;
        while (rectTransform.anchoredPosition.y < 0)
        {
            pos.y += increaseValue;
            pos.y = Mathf.Clamp(pos.y, -200, 0);
            rectTransform.anchoredPosition = pos;
            yield return null;
        }
    }
    IEnumerator Close(float speed, float duration)
    {
        Vector2 pos = Vector2.zero;
        float decreaseValue = speed / duration;
        while (rectTransform.anchoredPosition.y > -200)
        {
            pos.y -= decreaseValue;
            rectTransform.anchoredPosition = pos;
            yield return null;
        }
    }
}
