
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 팝업 창 여러개일때 클릭한 순서대로 화면 앞쪽으로 보일수있게 체크하는 이벤트 
/// </summary>
public interface IPopupSortWindow
{

    /// <summary>
    /// 인터페이스에 있는 내용은 무조건 상속받은곳에서 정의해야하나 
    /// GameObject gameObject { get;}  의 형태는 Component 에서 이미 구현이 되어있어서 
    /// 인터페이스 상속받은 클래스에서 MonoBehaviour 를 상속받아있는 상태이면 추가 구현을 안해도 에러가 안난다.
    /// 이말은 인터페이스의 함수는 다른 상속클래스 함수와도 연결할수가 있다는것이다.
    /// </summary>
    public GameObject gameObject { get;} 
    /// <summary>
    /// 팝업창 클릭시 신호를 받아오기위해 추가 
    /// </summary>
    public Action<IPopupSortWindow> PopupSorting { get; set; }

    /// <summary>
    /// 팝업창 여는 방법이 전부 틀리니 함수로빼자
    /// </summary>
    public void OpenWindow();
    /// <summary>
    /// 팝업창 닫는 방법이 전부 틀리니 함수로 따로 빼자
    /// </summary>
    public void CloseWindow();
}
