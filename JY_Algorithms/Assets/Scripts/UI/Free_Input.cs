using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Free_Input : MonoBehaviour
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI showResult;
    //TextMeshProUGUI inputText;
    TMP_InputField inputField;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inputField = transform.GetChild(0).GetComponent<TMP_InputField>();
        showResult = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
      //  inputText = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    
    public void Open()
    {
        inputField.text = string.Empty;
        showResult.text = string.Empty;
        inputField.Select();
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
