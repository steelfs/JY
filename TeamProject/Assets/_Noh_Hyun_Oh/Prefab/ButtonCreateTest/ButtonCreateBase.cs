using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static BattleActionUI;

public static class ButtonCreateBase<T> where T : Enum
{

    public static float width = 100.0f;
    public static float height = 50.0f;
    public static float fontSize = 24.0f;

    /// <summary>
    /// 버튼 게임오브젝트 만들고 버튼에필요한 이미지 컴포넌트 추가해서 색만 입힌다.
    /// 앵커및 피벗 위치잡고 창크기조절후 maskable 설정및 버튼 클릭이벤트 발동하기위한 raycastTarget 설정 
    /// 그리고 마지막에 인자로받은 액션을 리스너에 연결한다.
    /// </summary>
    /// <param name="objectName">버튼이무엇인지 표시해줄 이름</param>
    /// <param name="parentPosition">버튼이 포함될 부모트랜스폼</param>
    /// <param name="index">버튼이 여러개일때 아래로 위치를잡아줄 변수 하나면 입력안해도된다.</param>
    /// <returns></returns>
    private static void CreateButtonObject(string objectName, Transform parentPosition, Action buttonListener, int index = 1)
    {
        GameObject tempObj = new GameObject(objectName); // 게임 오브젝트만들고 
        tempObj.transform.SetParent(parentPosition);     // 트랜스폼 매개변수의 자식으로 집어넣고
        tempObj.AddComponent<RectTransform>();           // RectTransform 으로 바꿔주고 
        tempObj.AddComponent<Button>();                  // Button 추가 

        Image tempImg = tempObj.AddComponent<Image>();   // Button 에 사용할 Image 생성하고 

        //부모설정 
        GameObject childObj = new GameObject(objectName); // TextMeshPro 넣어줄 오브젝트생성 

        //텍스트 설정
        TextMeshProUGUI tempText = childObj.AddComponent<TextMeshProUGUI>(); //TextMeshPro 넣어주고
        childObj.transform.SetParent(tempObj.transform); // Button 을 부모로 설정
        tempText.text = objectName;                      // EnumList 의 값을 설정하거나 기본값을 설정하고
        tempText.fontSize = fontSize;                    // 폰트사이즈조절 
        tempText.alignment = TextAlignmentOptions.Center;// 정렬방법은 정가운데


        RectTransform temprt = tempObj.GetComponent<RectTransform>(); //버튼 RectTransform 을 찾아와서 
        //버튼 크기및 위치잡기
        temprt.anchorMin =  Vector2.up;                                 //앵커 0,1 으로 왼쪽위로 셋팅
        temprt.anchorMax =  Vector2.up;                                 //앵커 0,1 으로 왼쪽위로 셋팅
        temprt.pivot =      Vector2.up;                                 //피벗 0,1 으로 왼쪽위로 셋팅
        temprt.anchoredPosition = new Vector2(0, -(height * index));    //위에 정렬된것이있으면 계산해서 그밑으로 집어넣고
        temprt.sizeDelta = new Vector2(width, height);                  //사이즈를 다시맞춘다.
        //기타설정
        tempImg.maskable = true;                                        //마스크를적용시켜서 버튼이 부모위치에서 벗어나면 감추도록셋팅
        tempImg.raycastTarget = true;                                   //버튼의 클릭이벤트를 위해 RayCast 를 받을수있게설정
        tempImg.color = Color.black;                                   //색 

        tempObj.GetComponent<Button>().onClick.AddListener(() => 
                        { 
                            buttonListener?.Invoke(); 
                        });


    }

    /// <summary>
    /// 아래로 나열되는 버튼을 자동생성해서 인자로 날라온 함수를 연결해준다.
    /// </summary>
    /// <param name="buttonPosition">버튼이 포함될 부모 오브젝트위치</param>
    /// <param name="_">버튼이여러개일때 Enum 을 아무값이나 넘긴다 타입만필요함</param>
    /// <param name="function">버튼에 연결될 델리게이트 Enum 순서랑 맞춰야한다</param>
    static public void SettingNoneScriptButton(Transform buttonPosition, T _, Action[] function)
    {
        Array battleButtonEnumList = Enum.GetValues(typeof(T)); // 버튼마다 스크립트만들기 귀찮아서 코딩으로 처리하기위한 enum 리스트.

        RectTransform rt = buttonPosition.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, height * battleButtonEnumList.Length);
        for (int i = 0; i < battleButtonEnumList.Length; i++) //이넘 크기만큼만 돌린다
        {
            CreateButtonObject(battleButtonEnumList.GetValue(i).ToString(), buttonPosition, function[i], i);//분명히 함수연결된다.
        }
    }

    /// <summary>
    /// 아래로 나열되는 버튼을 자동생성해서 인자로 날라온 함수를 연결해준다.
    /// </summary>
    /// <param name="buttonPosition">버튼이 포함될 부모 오브젝트위치</param>
    /// <param name="function">버튼에 연결될 델리게이트 </param>
    /// <param name="objectName">버튼에 표시될 이름</param>
    static public void SettingNoneScriptButton(Transform buttonPosition, Action function , string objectName = "Empty")
    {
        CreateButtonObject(objectName, buttonPosition, function);
    }
}
