using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Console_Box : MonoBehaviour
{
    TextMeshProUGUI consoleText;
    TMP_InputField inputField;

    RectTransform rectTransform;
    PlayerInputAction inputActions;

    public float popUpSpeed = 3.0f;
    public float popUpDuration = 0.5f;

    bool isOpen = false;
    private void Awake()
    {
        inputField = transform.GetChild(0).GetComponent<TMP_InputField>();
        consoleText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        inputField.onEndEdit.AddListener((input) => Debug.Log(input));
        rectTransform = GetComponent<RectTransform>();
        inputActions = new();
    }
    private void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Console.performed += OnOpen_Console;
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
