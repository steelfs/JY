using EnumList;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용되는 전제는 이스크립트가 포함된 오브젝트밑에 팝업이 전부 들어가있고 그것들만 처리되게 한다.
/// 만약 다른 상위오브젝트의 자식으로 들어간것들은 제어안됨.
///  팝업창 정렬 관련 관리 클래스 
///  단축키 
/// </summary>
/// <typeparam name="T">팝업창 공통 된내용</typeparam>
public class PopupSortManager : MonoBehaviour  
{

    LinkedList<IPopupSortWindow> popupLList;
    public LinkedList<IPopupSortWindow> PopupLList => popupLList;

    
    IPopupSortWindow[]  popupArray; //창의 갯수는 정해져있음으로 배열로 선언

    public void Awake() 
    {
        popupArray = GetComponentsInChildren<IPopupSortWindow>(true);
        /*
         팝업창 관련해서 여기서 불러와서 데이터 list 에 담는다.
         */
        foreach (var popupWindow in popupArray)
        {
            popupWindow.PopupSorting = SetWindowSort; //각팝업 이벤트의 클릭이벤트연결
        }

        popupLList = new LinkedList<IPopupSortWindow>(popupArray);
    }

    /// <summary>
    /// IPopupSortWindow를 상속받아서  PopupSorting을 정의한 컴포넌트들의 클릭이벤트에 실행되도록 정의 하면 사용 가능하다.
    /// 팝업창마다 클릭시 이벤트 발동하는내용 클릭한팝업창을 맨앞으로 끌고온다.
    /// </summary>
    private void SetWindowSort(IPopupSortWindow target) 
    {
        popupLList.Remove(target);
        popupLList.AddFirst(target);
        PopupObjectChange(); //바뀐노드순서로 화면정렬
    }

    /// <summary>
    /// 팝업 창호출 시 호출할 함수 
    /// </summary>
    /// <param name="target">창이 오픈될때의 팝업창종류</param>
    public void PopupOpen(IPopupSortWindow target)
    {
        //팝업창을 빼서 맨뒤로 넣고 오브젝트정렬시킨다 그러면 맨앞에보인다.
        target.OpenWindow();
        popupLList.Remove(target);
        popupLList.AddFirst(target);
        PopupObjectChange();
    }

    /// <summary>
    /// 정한 팝업창을 닫는다
    /// </summary>
    /// <param name="target">닫힐 팝업창</param>
    public void PopupClose(IPopupSortWindow target)
    {
        popupLList.Remove(target);
        target.CloseWindow();
    }


    /// <summary>
    /// 팝업 관련 데이터 추가및 리플래쉬
    /// </summary>
    /// <param name="target">창이 오픈될때의 팝업창종류</param>
    public void PopupSortDataAppend(IPopupSortWindow target)
    {
        //팝업창을 빼서 맨뒤로 넣고 오브젝트정렬시킨다 그러면 맨앞에보인다.
        popupLList.Remove(target);
        popupLList.AddFirst(target);
        PopupObjectChange();
    }

    /// <summary>
    /// 팝업 관련 데이터 삭제 
    /// </summary>
    /// <param name="target">닫힐 팝업창</param>
    public void PopupSortDataRemove(IPopupSortWindow target)
    {
        popupLList.Remove(target);
    }




    /// <summary>
    /// 맨앞의 팝업창을 닫는다 
    /// </summary>
    public void PopupClose() 
    {
        if (popupLList.Count > 0) // 열린 팝업창이 있는경우 
        {
            popupLList.First.Value.CloseWindow(); //감추고 
            popupLList.RemoveFirst(); //리스트에서 제거 
        }
    }

    /// <summary>
    /// 열린 팝업창 전부 닫기용으로 작성
    /// </summary>
    public void CloseAllWindow()
    {
        foreach (IPopupSortWindow go in popupArray) //관리할팝업등록된 갯수만큼 전부 닫는다 
        {
            go.CloseWindow();
            popupLList.Remove(go);

        }
    }
    

    /// <summary>
    /// 열린 팝업창중에 정렬하기 
    /// </summary>
    private void PopupObjectChange() 
    {

        if (popupLList != null) { 
        
            foreach (var t in popupLList)
            {
                t.gameObject.transform.SetAsFirstSibling();
            }
        }
    
    }
}
