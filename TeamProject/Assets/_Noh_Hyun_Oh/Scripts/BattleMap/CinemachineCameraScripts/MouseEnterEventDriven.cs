using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 마우스를 스크린 맨끝에 위치시키면 카메라를 이동시키는 로직
/// 시네머신용 
/// 카메라 이동 감지 체크용 
/// </summary>
public class MouseEnterEventDriven : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// 이동할 카메라 관리 클래스
    /// </summary>
    Camera_Move moveAction;
    /// <summary>
    /// 화면 끝인지 좌표 확인용 값들
    /// </summary>
    readonly float leftCheck = Screen.width * 0.05f;
    readonly float rightCheck = Screen.width * 0.95f;
    readonly float topCheck = Screen.height * 0.95f;
    readonly float bottomCheck = Screen.height * 0.05f;

    private void Awake()
    {
        moveAction = transform.parent.GetComponent<Camera_Move>();
    }
    /// <summary>
    /// 마우스 가 화면 끝에 위치하는지 체크
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Screen_Side_Mouse_Direction screenDir = Screen_Side_Mouse_Direction.None;
        Vector2 dir = eventData.position;
        if (dir.x > rightCheck)  //오른쪽 끝
        {
            screenDir |= Screen_Side_Mouse_Direction.Right;
        }
        else if (dir.x < leftCheck) //왼쪽 끝
        {
            screenDir |= Screen_Side_Mouse_Direction.Left;
        }

        if (dir.y > topCheck) //위쪽 끝
        {
            screenDir |= Screen_Side_Mouse_Direction.Top;
        }
        else if (dir.y < bottomCheck) //아래쪽 끝 
        {
            screenDir |= Screen_Side_Mouse_Direction.Bottom;
        }
        
        moveAction.moveCamera?.Invoke(screenDir); //수정값 전달
    }

    /// <summary>
    /// 마우스 중간 일때 발생하는 이벤트
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        moveAction.moveCamera?.Invoke(Screen_Side_Mouse_Direction.None); //초기값 전달
    }

}
