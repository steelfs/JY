using System;
 using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

/// <summary>
/// 포인터 다운 핸들러는 해당오브젝트 내에서 마우스포인터의 정보를 가져오지만 오브젝트를 벗어나면 못가져온다
/// 그에비해 IDragHandler 는 화면전체의 마우스포인터 정보를 가져오기때문에 크기조절을 위해 IPointerMoveHandler 를 대신하여 사용하였다 
/// </summary>
public class PopupWindowBase : MonoBehaviour, 
    //IPointerDownHandler, //해당오브젝트안에서 이벤트(클릭) 발동시 실행함  
    IPointerUpHandler,   //해당오브젝트안에서 이벤트(클릭해제) 발동시 실행함  
    IDragHandler        //화면에서 드래그시 발동함.  
{

    /// <summary>
    /// 현재 게임화면 width 값
    /// </summary>
    float windowWidth = Screen.width;
    
    /// <summary>
    /// 현재 게임화면 height 값
    /// </summary>
    float windowHeight = Screen.height;

    /// <summary>
    /// 팝업창 크기조절 가능여부
    /// </summary>
    bool isWindowSizeChange = false;
    public bool IsWindowSizeChange {
        get => isWindowSizeChange;
        set => isWindowSizeChange = value;
    }
    /// <summary>
    /// 창크기가 변하는 속도
    /// </summary>
    [SerializeField]
    [Range(0.01f, 0.1f)]
    float sizeSpeed = 0.0f;

    [SerializeField]
    /// <summary>
    /// 팝업창 크기조절시 최소 Width 값
    /// </summary>
    protected float minWidth = 0.0f;

    [SerializeField]
    /// <summary>
    /// 팝업창 크기조절시 최소 Height 값
    /// </summary>
    protected float minHeight = 0.0f;

    [SerializeField]
    /// <summary>
    /// 팝업창 크기조절시 최대 width 값 
    /// </summary>
    protected float maxWidth = 0.0f;

    [SerializeField]
    /// <summary>
    /// 팝업창 크기조절시 최대 Height 값
    /// </summary>
    protected float maxHeight = 0.0f;

    /// <summary>
    /// 현재 팝업창의 RectTransform 컴퍼넌트
    /// </summary>
    protected RectTransform rectTransform;

    /// <summary>
    /// 팝업창내에서 클릭 여부
    /// </summary>
    protected bool isClick = false; 

    /// <summary>
    /// 팝업창 현재 클릭한위치값
    /// </summary>
    Vector2 clickPosition;
    public Vector2 ClickPosition
    {
        get => clickPosition;
        set => clickPosition = value;
    }

    /// <summary>
    /// 크기 연산값 담아둘 임시백터
    /// </summary>
    Vector2 arithmeticValue;

    /// <summary>
    /// 팝업창 닫기버튼 이벤트연결용
    /// </summary>
    protected Button closeBtn;

    /// <summary>
    /// 컨텐츠패널 위치값과 사이즈값 찾아오기
    /// </summary>
    protected RectTransform contentPanel;

    /// <summary>
    /// 탑 패널 위치와 사이즈값 가져오기
    /// </summary>
    protected RectTransform topPanel;

    protected virtual void Awake()
    {
        //Debug.Log("awake 테스트");
        rectTransform = GetComponent<RectTransform>(); //팝업창 크기조절용으로 가져온다

        try //위치 틀려서 무조건에러나는것들이 존재한다. 게임오브젝트 구조를 바꾸면 무조껀 난다.
        {
            contentPanel = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>(); //컨텐츠패널 RectTransform 가져온다
        
            topPanel = transform.GetChild(1).GetComponent<RectTransform>(); // 탑패널 RectTransform 가져온다


            int childCount = transform.GetChild(1).childCount - 1; //닫기버튼은 항상 마지막 자식으로 가있어야한다.
            closeBtn = transform.GetChild(1).GetChild(childCount).GetComponent<Button>(); //닫기버튼 가져와서 
            closeBtn.onClick.AddListener(OnCloseButtonClick); // 닫기이벤트 연결  - 버튼 오브젝트에 이러면 스크립트 추가안해도된다.
            SetContentWindowSize();//컨텐츠창 위치랑 크기 조절 , 컴파일단계에서 이미 값들이 다 설정된것만 사용하기때문에 문제없음.

        }catch (Exception ex) 
        {
            Debug.LogWarning($"{this.name} 팝업창은 크기조절이 안됩니다.{ex.Message}");
        }
    }

    

    

    /// <summary>
    /// 클릭 해제 체크
    /// </summary>
    /// <param name="eventData">이벤트 위치정보</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        isWindowSizeChange = false;
    }

    /// <summary>
    /// 클릭 체크후 이동시 발동
    /// </summary>
    /// <param name="eventData">이벤트 위치정보</param>
    public void OnDrag(PointerEventData eventData)
    {
        OnDragMove(eventData);
    }

   
    /// <summary>
    /// 클릭후 드래그시 창크기 변경로직 추가
    /// </summary>
    /// <param name="eventData">이벤트 위치정보</param>
    protected virtual void OnDragMove(PointerEventData eventData)
    {
        if (isWindowSizeChange) //창크기조절 활성화시 
        { 
            SetPopupSize(eventData); //팝업창 크기조절
        }
    }

    /// <summary>
    /// 각 팝업창마다 컨텐츠안의 내용 구성하자.
    /// </summary>
    /// <param name="contentWindowSize">사이즈 x = width , y = height</param>
    protected virtual void SetItemList(Vector2 contentWindowSize)
    {

    }

    /// <summary>
    /// 팝업창 닫기버튼 클릭시 팝업창 비활성화시킨다.
    /// 팝업창 닫힐때 처리할 기능 추가 등록하도록 하자.
    /// </summary>
    protected virtual void OnCloseButtonClick()
    {
        gameObject.SetActive(false);

    }
    /// <summary>
    /// 제대로 동작시킬려면 윈도우 리스트 (캔버스) 에 핸들러를 달아야한다. 
    /// </summary>
    /// <param name="eventData">현재 전체화면의 좌표정보를 가지고있다</param>
    //rectTransform.anchoredPosition 현재 창크기
    private void SetPopupSize(PointerEventData eventData)
    {
        //현재창의 사이즈에서 드래그시작한위치값에서 이동한거리만큼을 뺀 나머지값을 더한다.
        arithmeticValue = rectTransform.sizeDelta;
        arithmeticValue.x = arithmeticValue.x + ((    // 창의 현재 사이즈 width값 
            eventData.position.x -                  // 전체화면!!!에서 드래그하고있는 위치값 x좌표
            ClickPosition.x) *                      // 처음 드래그를 시작한 위치값 x 좌표
            sizeSpeed);        
        arithmeticValue.y = arithmeticValue.y + ((    // 창의 현재 사이즈 height값
            ClickPosition.y -                       // 처음 드래그를 시작한 위치값 y 좌표
            eventData.position.y) *                 // 전체화면!!!에서 드래그하고있는 위치값 y좌표
            sizeSpeed);

        //Height 최대 최소 사이즈 체크 
        if (arithmeticValue.y > maxHeight)
        {
            arithmeticValue.y = maxHeight;
        }
        else if (arithmeticValue.y < minHeight)
        {
            arithmeticValue.y = minHeight;
        }
        //Width 최대 최소 사이즈 체크
        if (arithmeticValue.x > maxWidth)
        {
            arithmeticValue.x = maxWidth;
        }
        else if (arithmeticValue.x < minWidth)
        {
            arithmeticValue.x = minWidth;
        }

        // 렉터 설정때문에 anchoredPosition 을 사용하였다 . 앵커기준으로부터 얼마나떨어진지 저장되는거같다.
        // 일반 position 을 사용하면 전체화면 을 기준으로 잡기때문에 앵커를 사용하였다.
        // 앵커기준은 왼쪽 위로 잡아놨다.

        //크기 늘렸을경우 윈도우에 벗어낫는지 체크 
        if (windowWidth - rectTransform.anchoredPosition.x < arithmeticValue.x) // 전체화면크기에서 창의 좌측상단의 좌표값을 가져와서 뺀값이 창크기보다 클때 
        {
            arithmeticValue.x = windowWidth - rectTransform.anchoredPosition.x;
        }

        if ((windowHeight + rectTransform.anchoredPosition.y) < arithmeticValue.y)
        {
            arithmeticValue.y = windowHeight + rectTransform.anchoredPosition.y; //y값은 항상 - 임으로 더해주면 된다.
        }


        rectTransform.sizeDelta = arithmeticValue; //처리결과 반영하기

        SetContentWindowSize();//사이즈 조절끝난뒤 컨텐츠 내용 변경

        SetItemList(arithmeticValue);


    }

    /// <summary>
    /// 팝업창 내부 컨텐츠의 사이즈조절및 위치조절 함수
    /// </summary>
    private void SetContentWindowSize() {
        //컨텐츠 창 사이즈조절 
        //arithmeticValue = rectTransform.sizeDelta; //팝업창 사이즈 가져와서

        //arithmeticValue.y -= topPanel.sizeDelta.y; //탑판넬 크기만큼 자르고

        //contentPanel.sizeDelta = arithmeticValue;  //그크기를 적용

        //컨텐츠 창 위치 조절 
        arithmeticValue = contentPanel.anchoredPosition;    //컨텐츠 앵커기준 위치값 가져오고

        arithmeticValue.y = -topPanel.sizeDelta.y;           //컨텐츠 y좌표를 설정하고 

        contentPanel.anchoredPosition = arithmeticValue;    //컨텐츠 에 적용

    }

    
}

