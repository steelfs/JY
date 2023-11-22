using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleActionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler ,IEndDragHandler
{
    /// <summary>
    /// 클릭해서 드래그로 이동시 메뉴가 안보여야되서 추가해준변수
    /// 드래그상황을 입력한다.
    /// </summary>
    bool isMove = false;
    /// <summary>
    /// 델리게이트나 버튼 순서나 이넘을 기반으로 맞춰줘야함 귀찮네.
    /// 이넘 추가하려면 버튼도 느려야함. 그리고 Action델리게이트도 기능추가해줘야함
    /// </summary>
    public enum BattleButton
    {
        Move = 0,
        Attack,
        Skill,
        Item,
        Options,
        TurnEnd,
        
    }
    Animator hideAnim;
    int str_OnViewButton;

    /// <summary>
    /// BattleButton 의 순번에맞게 리스트의 기능추가가 필요하다.
    /// </summary>
    public Action<Transform>[] list;
    private void Awake()
    {
        str_OnViewButton = Animator.StringToHash("OnViewButton");
        hideAnim = transform.GetComponent<Animator>();
        Transform buttonPanel = transform.GetChild(0);
        //ButtonCreateBase<BattleButton>.SettingNoneScriptButton(buttonPanel, BattleButton.Skill,null);
        //SettingNoneScriptButton();
    }
  
    /// <summary>
    /// 버튼에 스크립트를 넣지않으려는 발악
    /// 
    /// 이스크립트 자식으로 버튼을 생성하고 이넘추가 그리고 밑에 스위치문에 리스너 연결 하자.
    /// </summary>
    //private void SettingNoneScriptButton() {
    //    //버튼 다끌고와서
    //    //Button[] ActionButtons = transform.GetComponentsInChildren<Button>(true);//자식 버튼끌고와서
    //    //이넘을 배열로 받아오고
    //    BattleButton[] battleButtonEnumList = (BattleButton[])Enum.GetValues(typeof(BattleButton)); // 버튼마다 스크립트만들기 귀찮아서 이리처리했음.
    //    //이건필요없고 액션 연결해주려면 사용 
    //    list = new Action<Transform>[battleButtonEnumList.Length];//        
        
    //    Transform buttonPanel = transform.GetChild(0);
    //    RectTransform rt = buttonPanel.GetComponent<RectTransform>();
    //    rt.sizeDelta = new Vector2(width,height*battleButtonEnumList.Length);
    //    for (int i = 0; i < battleButtonEnumList.Length; i++) //이넘 크기만큼만 돌린다
    //    {

    //        //람다식으로 넘길 변수선언
    //        //int index = i; //포문안에서 람다식을 썻을때 i를 람다식안에 넘겨주면 마지막인덱스값을 받는다
    //        //람다식은 포문돌때 값을전달받는게아니라 변수의 주소를 참조만하고 있다가 포문이 끝난뒤 참조된 주소를 찾아가 값을 셋팅한다 .
    //        //그래서 임시변수하나에 i값을 담고 넘겨주니 잘넘어간다 
    //        //이유는 아마 포문안에서 선언했기때문에 포문한번돌때마다 index 라는 변수는 매번 스택메모리에 새로생길것이다 
    //        //index 변수로 선언하고 람다식에서 가져다썻기때문에 람다식안에서는 포문돌아서 사라지기 전의 index 주소값을 참조하고있을것이다.
    //        //그래서 다시포문돌때는 새로운 index 변수를 생성하고 새로운 주소에다가 만들기때문에 람다식에서 index변수명은같지만 주소가 다른것들이 
    //        //여러개 생기고 각각의 인덱스값을 참조하여 제대로된 값을 셋팅할수가 있다.
    //        //가비지 컬렉션은 일단 람다식이 기존index(포문돌면서 변수명이없어진index 주소값) 를 참조하고있기때문에 청소대상에서 제외되니 문제가안된다.
    //        //만약 index 변수를 포문밖에서 선언했다면 이또한 제대로된값이 전달되지않을것이다 
    //        //배열 인덱스는 인자로 넘길때 값타입으로 전환이되기때문에 델리게이트같은 특수한 객체는 넘기기힘들다.
    //        //ref 를 사용해도되지만 내부적으로 사용할때 제약이걸린다.
    //        //ex ) 이벤트리스너에 추가하기위한 람다식안의 내용으로 집어넣으려면 ref out in 같은 예약어는 사용이불가능하다.
    //        GameObject button = null;
    //        //각자 달아주기위해서 이넘으로 체크.
    //        switch (battleButtonEnumList[i])
    //        {
    //            case BattleButton.Move:
    //            case BattleButton.Attack:
    //            case BattleButton.Skill:
    //            case BattleButton.Item:
    //            case BattleButton.Options:  
    //            case BattleButton.TurnEnd:
    //            case BattleButton.nextGame:
    //            case BattleButton.newGame:
    //                button = CreateButtonObject(battleButtonEnumList[i].ToString(),buttonPanel,i);
    //                break;
    //            default:
    //                break;
    //        }
    //        int index = i;
    //        if (button != null) 
    //        {
    //            //포문돌면서 전부 리스너 달아주기 
    //            button.GetComponent<Button>().
    //                onClick.
    //                AddListener(() => {
    //                    list[index]?.Invoke(transform); 
    //                });  
    //        }
    //    }
    //}
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isMove)
        {
            hideAnim.SetBool(str_OnViewButton, true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hideAnim.SetBool(str_OnViewButton, false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isMove = true;
        transform.position = eventData.position;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isMove = false;
    }
}
