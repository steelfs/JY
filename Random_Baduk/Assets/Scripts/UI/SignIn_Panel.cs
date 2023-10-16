using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignIn_Panel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public Login_Panel loginPanel;

    public Button register_Button;
    public Button back_Button;
    public TMP_InputField id_InputField;
    public TMP_InputField password_InputField;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        register_Button.onClick.AddListener(Register);
        back_Button.onClick.AddListener(Close);
    }
    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        loginPanel.Show();
    }
    public void Register()
    {
        string id = id_InputField.text;
        string password = password_InputField.text;
    }
}
