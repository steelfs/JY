using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualStick : MonoBehaviour, IDragHandler,IPointerUpHandler, IPointerDownHandler
{
    RectTransform containerRect;// 전체영역의 사각형 // 확인할 영영

    RectTransform handleRect;// 핸들부분의 사각형 

    float stickRange;//핸들이 움직일수 있는 최대거리

    public Action<Vector2> onMoveInput;//가상스틱 입력 
    Player player;

    Vector2 clickedPos;// 클릭 시작했을때 위치 저장용
    private void Awake()
    {
        containerRect = transform as RectTransform;
        Transform child = transform.GetChild(0);
        handleRect = child as RectTransform;
        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f; //컨테이너 절반에서 핸들 절반 빼기
        player = FindObjectOfType<Player>();
    }
    public void OnDrag(PointerEventData eventData) //인스펙터에서 raycast target을 활성화 해놔야 감지 가능
    {
        // Debug.Log(eventData.position);
        //containerRect 안에서  eventData.position 위치가 containerRect 피봇에서 얼마나 움직였는지 position에 저장
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);

        position = Vector2.ClampMagnitude(position, stickRange);//최대영역 벗어나지 않게 하기
      
        InputUpdate(position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handleRect.localPosition = Vector3.zero;
        InputUpdate(Vector2.zero); //포인터 땠을 때 0,0,0 으로 신호 보내기
    }

    public void OnPointerDown(PointerEventData eventData)// pointerUp을 사용하려면 Down이 있어야한다.
    {

    }
    void InputUpdate(Vector2 position)//입력에 따라 핸들 움직이고 신호 보내는 함수
    {
        handleRect.anchoredPosition = position;
        onMoveInput?.Invoke(position/stickRange);//nomalized와 같은 역할 (-1, -1)~ (1, 1) 로 변환해서 보냄
        //델리게이트 실행
        //플레이어는 이 델리게이트에 연결이 되어있으면 신호에 맞게 움직인다.
    }
}
