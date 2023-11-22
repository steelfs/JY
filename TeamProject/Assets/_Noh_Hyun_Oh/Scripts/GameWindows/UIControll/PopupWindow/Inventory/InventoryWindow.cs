using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventoryWindow : PopupWindowBase , IPopupSortWindow ,IPointerDownHandler
{
    /// <summary>
    /// 아이템정렬 시 창크기 최소값 
    /// </summary>
    //float minSizeWidth = 200.0f;
    
    /// <summary>
    /// 아이템정렬 시 창크기 최소값 
    /// </summary>
    //float minSizeHeight = 200.0f;

    GameObject contentsObj;

    GameObject contentObj;


    public Action<IPopupSortWindow> PopupSorting { get; set; }

    /// <summary>
    /// 화면 정렬용 이벤트함수 추가 
    /// </summary>
    /// <param name="eventData">사용안함</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        PopupSorting(this);
    }
    protected override void Awake()
    {
        base.Awake();
        contentsObj = transform.GetChild(0).GetChild(0).gameObject; //컨텐츠 위치찾기
        contentObj = contentsObj.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;

        //화면 최소 최대 사이즈 셋팅
        minHeight = 600.0f;
        minWidth = 400.0f;
        maxHeight = 1000.0f;
        maxWidth = 800.0f;

        SetItemList(new Vector2(minWidth, minHeight));
    }
  
    protected override void SetItemList(Vector2 contentWindowSize)
    {

        int childCount = contentObj.transform.childCount; //아이템셋팅할것이있는지 카운트
        if (childCount == 0) return;
        Vector2 v2 = contentObj.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        int a = (int)(rectTransform.sizeDelta.x / v2.x);
        v2.x *= a;
        v2.y *= ((childCount / a )+1);

        contentObj.GetComponent<RectTransform>().sizeDelta = v2;
       //자동그리기는 자동레이아웃으로 처리하고 컴퍼넌트 창크기만 조절하자 로직짜려면 골치아프다 렉트 오브젝트갯수만큼 가져와서 처리해야되니.

    }


    public void OpenWindow()
    {
        this.gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        //윈도우 닫혔을때 전부닫혀야하지않을까?
        this.gameObject.SetActive(false);
    }
}
