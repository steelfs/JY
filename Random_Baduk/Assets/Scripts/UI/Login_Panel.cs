using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login_Panel : MonoBehaviour
{
    TMP_InputField id_InputField;
    TMP_InputField password_InputField;

    Button login_Button;
    Button signIn_Button;

    PlayerInputAction inputActions;

    private void Awake()
    {
        inputActions = new();
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

    void LogIn()
    {

    }
    void SignIn()
    {

    }
}
