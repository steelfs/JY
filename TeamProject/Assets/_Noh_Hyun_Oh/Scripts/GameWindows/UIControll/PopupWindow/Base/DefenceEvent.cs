using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 클릭이벤트를 막기위해 전체화면으로 덮어버리고 단축키 이벤트를 끄고 esc이벤트만 추가한다.
/// </summary>
public class DefenceEvent: MonoBehaviour
{
    
    SaveLoadPopupButton targetWindow;

    private void Awake()
    {
        int index = transform.parent.childCount - 1;
        targetWindow = transform.parent.GetChild(index).GetChild(5).GetComponent<SaveLoadPopupButton>();// esc눌렀을때 처리할 종료 process 실행클래스 
    }
    private void Start()
    {
        InputSystemController.Instance.On_Common_Esc += Close;  //모달 관련 설정은 따로 생각을 좀더해봐야될듯
    }
    //private void OnEnable()
    //{
    //    WindowList.Instance.InputKeyEvent.Disable();// 단축키 비활성화
    //    inputSystem.Enable();
    //    inputSystem.KeyBoard.System.performed += Close;
    //}


    //private void OnDisable()
    //{

    //    inputSystem.KeyBoard.System.performed -= Close;
    //    inputSystem.Disable();

    //    WindowList.Instance.InputKeyEvent.Enable(); //단축키 활성화
    //}
    /// <summary>
    /// esc 눌렀을때 팝업창닫기로직실행
    /// </summary>
    private void Close()
    {
        targetWindow.CancelButton();//닫기창누르는것과 같다 
    }
}
