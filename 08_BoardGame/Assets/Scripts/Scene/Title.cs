using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    PlayerInputActions inputActions;
    private void Awake()
    {
        inputActions = new();

    }
    private void OnEnable()
    {
        inputActions.Title.Enable();
        inputActions.Title.Anything.performed += OnAnything;
    }
    private void OnDisable()
    {
        inputActions.Title.Anything.performed -= OnAnything;
        inputActions.Title.Disable();
    }
    private void OnAnything(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        SceneManager.LoadScene("ShipdeploymentScene");
    }
}
