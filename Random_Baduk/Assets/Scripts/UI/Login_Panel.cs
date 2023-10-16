using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login_Panel : MonoBehaviour
{
    TMP_InputField id_InputField;
    TMP_InputField password_InputField;

    public Button login_Button;
    public Button signIn_Button;
    public SignIn_Panel signIn_Panel;

    PlayerInputAction inputActions;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inputActions = new();
        login_Button.onClick.AddListener(LogIn);
        signIn_Button.onClick.AddListener(SignIn);
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Enter.performed += OnEnter;
    }

    private void OnEnter(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        LogIn();
    }

    void SignIn()
    {
        signIn_Panel.Open();
        Hide();
    }
    void LogIn()
    {

    }
    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
