using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move; //�̵��Է�
		public Vector2 look;//�þ�ȸ�� ��Ȳ
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;//�Ƴ��α� ��ƽ�� �ִ��� ���� 

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;		//Ŀ���� ����� ����� ������ ����(���� �Ǹ� Ŀ���� �Ⱥ��δ�.)
		public bool cursorInputForLook = true;


        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (cursorInputForLook)
            {
                LookInput(context.ReadValue<Vector2>());
				//Debug.Log(context.ReadValue<Vector2>());
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
			JumpInput(context.performed);
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
			//SprintInput(context.ReadValueAsButton());
            SprintInput(context.ReadValue<float>() > 0.1f);
        }

		public void OnFire(InputAction.CallbackContext context)
		{
			Debug.Log("Fire");
			RomanToInt("III");
		}
		public void OnZoom(InputAction.CallbackContext context)
		{
            Debug.Log("Zoom");
        }

        public int RomanToInt(string s)
        {
            Dictionary<char, int> roman = new Dictionary<char, int>()
			{
				{'I', 1}, {'V', 5}, {'X', 10}, {'L', 50}, {'C', 100}, {'D', 500}, {'M', 1000}
			};

            int result = 0;
            int now = 0;
            int next = 0;

            char[] chars = s.ToCharArray();

            for (int i = 0; i < chars.Length - 1; i++)
            {
                now = roman[chars[i]];
                next = roman[chars[i + 1]];

                if (now < next)
                {
                    result -= now;
                }
                else
                {
                    result += now;
                }
            }
            result += chars[chars.Length - 1];

            return result;
        }

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED

        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		/// <summary>
		/// �������α׷��� ��Ŀ���� �Ű����� ����Ǵ� �Լ�.
		/// true = ��Ŀ��, false = ������.
		/// </summary>
		/// <param name="hasFocus"></param>
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		/// <summary>
		/// Ŀ���� �� ���¸� �����ϴ� �Լ� 
		/// </summary>
		/// <param name="newState">true = ��, false = �� ����</param>
		private void SetCursorState(bool newState)
		{
			//���̵Ǹ� Ŀ���� �Ⱥ���
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}

  
}