using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;


public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource cinemachineImpulseSource;

	InputKeyMouse inputClick;

	private void Awake()
	{
		inputClick = new InputKeyMouse();
		cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
	}
	private void OnEnable()
	{
		inputClick.Mouse.Enable();
		//inputClick.Mouse.MouseClick.performed += onClick;
		inputClick.Mouse.MouseClickRight.performed += onClickRight;
		//inputClick.Test.Test3.performed += onUnitDie;
	}

	private void OnDisable()
	{
		//inputClick.Test.Test3.performed -= onUnitDie;
		inputClick.Mouse.MouseClickRight.performed -= onClickRight;
		//inputClick.Mouse.MouseClick.performed -= onClick;
		inputClick.Mouse.Disable();
	}

	protected void onClickRight(InputAction.CallbackContext callback)
	{
		Shake();
	}

	public void Shake(float intensity = 1f)
	{
		//Debug.Log("xptmxm");
		cinemachineImpulseSource.GenerateImpulse(intensity);
	}
	
}
