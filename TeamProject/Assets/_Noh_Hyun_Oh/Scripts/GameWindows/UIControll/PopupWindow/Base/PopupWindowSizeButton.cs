using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 버튼 컴퍼넌트 없이 핸들러 작동되는지 테스트겸  만든 스크립트
/// </summary>
public class PopupWindowSizeButton : MonoBehaviour, IPointerDownHandler
{
    /// <summary>
    /// 사이즈조절하는 클래스 가져오기
    /// </summary>
    [SerializeField]
    PopupWindowBase parentPopupWindow;

    private void Awake()
    {
        parentPopupWindow = transform.parent.parent.GetComponent<PopupWindowBase>(); //이건위치고정이니 고칠필요가없네..
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        parentPopupWindow.ClickPosition = eventData.position; //드래그 시작위치 전송해주기 델리쓰기 귀찮..
        parentPopupWindow.IsWindowSizeChange = true;          //이벤트 발생시 창크기 조절한다고 체크한다.
        
    }

  
}
   
