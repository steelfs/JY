using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitBehaviour : MonoBehaviour
{
	[SerializeField]
	private Transform bulletProjectilePrefab;

	InputKeyMouse inputSpace;

private void Awake()
	{
		inputSpace = new InputKeyMouse();
	}
	private void OnEnable()
	{
		inputSpace.Player.Enable();
		inputSpace.Player.Jump.performed += OnSpace;
	}

	private void OnDisable()
	{
		inputSpace.Player.Jump.performed -= OnSpace;
		inputSpace.Player.Disable();
	}

	private void OnSpace(InputAction.CallbackContext context)
	{
		Instantiate(bulletProjectilePrefab.gameObject);
	}





}
