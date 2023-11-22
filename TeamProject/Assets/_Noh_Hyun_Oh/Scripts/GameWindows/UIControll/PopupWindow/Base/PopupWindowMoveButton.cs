using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 팝업창 이동과 화면밖으로 벗어나지 않게 처리하는 부분
/// </summary>
public class PopupWindowMoveButton : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    /// <summary>
    /// 화면 넓이 가져오기
    /// </summary>
    float windowWidth = Screen.width;

    /// <summary>
    /// 화면 높이 가져오기
    /// </summary>
    float windowHeight = Screen.height;

    /// <summary>
    /// 이동할 윈도우창 위치의 렉트트랜스폼을 가져온다
    /// </summary>
    private RectTransform parentWindow;

    /// <summary>
    /// 드래그시작위치를담을 백터
    /// </summary>
    private Vector2 startPosition;

    /// <summary>
    /// 드래그시작시 이벤트위치값을 담을 백터
    /// </summary>
    private Vector2 movePosition;

    /// <summary>
    /// 창이동시 임시로 위치담아둘 변수
    /// </summary>
    private Vector2 moveOffSet;
    private void Awake()
    {
        parentWindow = transform.parent.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 드래그시작할때 한번만발동 
    /// </summary>
    /// <param name="eventData">이벤트위치값정보</param>
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)//인터페이스두개를사용하기때문에 명시적으로 작성
    {
        startPosition = parentWindow.anchoredPosition;//드래그시작할때 위치값저장
        movePosition = eventData.position; //드래그시작할때의 이동처리할 포지션값 저장
    }

    /// <summary>
    /// 드래그 진행중 일때 화면밖으로 벗어났는지 체크하고 안벗어났으면 이동시킨다
    /// </summary>
    /// <param name="eventData">이벤트위치정보값</param>
    void IDragHandler.OnDrag(PointerEventData eventData) //인터페이스두개를사용하기때문에 명시적으로 작성
    {

        //이동한 드래그만큼 값을 더해준다.
        //시작지점 에서 이동한거리(이동한값에서 처음값을뺀값)을 더한다.
        CheckOutOfWindow(eventData);  //창밖으로 벗어나지 않았는지 체크 하면서 이동시킨다
            
        
    }

    /// <summary>
    /// 현재화면에서 벗어나지 않았는지 체크하고 벗어낫으면 더이상안벗어나게 계산한다.
    /// </summary>
    /// <returns>화면에서 벗어났으면 false</returns>
    void CheckOutOfWindow(PointerEventData eventData)
    {
        //창이동이 이상하면 전체 스크린 사이즈를 확인해보자
        moveOffSet = startPosition + (eventData.position - movePosition); //이동한 값

        // 추가 -  나중에 마우스가 이동한방향으로 조금씩이동시키는 로직으로 변경하는게 좋을거같다.


        //왼쪽 체크
        if (moveOffSet.x < 0) { 
            moveOffSet.x = 0;
        }
         //위 체크
        if (moveOffSet.y > 0) { 
            moveOffSet.y = 0;
        }
        //오른쪽 체크
        float x = moveOffSet.x + parentWindow.rect.width; //오른쪽으로벗어나는것을체크하기위해 창크기랑좌표랑 합친다
        if (x > windowWidth) { 
            moveOffSet.x = windowWidth - parentWindow.rect.width; //윈도우 크기의 좌표값에서 창크기를 빼서 적용
        }

        //아래체크
        float y = moveOffSet.y - parentWindow.rect.height; // 아래방향이기때문에 더하는게아니라 빼준다. moveOffSet.y는 항상 음수이다.
        if (-y > windowHeight) {
            moveOffSet.y =  parentWindow.rect.height - windowHeight; // -좌표이기때문에 창크기에서 윈도우창가장밑의 좌표값을 뺀다.
        }
        parentWindow.anchoredPosition = moveOffSet;  //벗어나지 않았으면 이동시킨다.
    }
}