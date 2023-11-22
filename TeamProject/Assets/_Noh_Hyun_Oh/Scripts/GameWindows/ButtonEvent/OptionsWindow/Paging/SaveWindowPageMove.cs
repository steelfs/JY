using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 저장페이지  페이지이동버튼 관련 버튼이벤트 설정
/// </summary>
public class SaveWindowPageMove : MonoBehaviour
{
    SaveWindowManager proccessClass;


    private void Start()
    {
        proccessClass =  WindowList.Instance.MainWindow; // sort 클래스는 하나이기때문에 걍찾아오자
    }

    /// <summary>
    /// 이전페이지 버튼 클릭이벤트
    /// </summary>
    public void OnPrevButton() {
        proccessClass.PageIndex--; //현재페이지 설정하고
        proccessClass.SetPageList();//페이지 리플레쉬
    }
    /// <summary>
    /// 다음페이지 버튼 클릭이벤트
    /// </summary>
    public void OnNextButton()
    {
        proccessClass.PageIndex++;
        proccessClass.SetPageList();
    } 
    /// <summary>
    /// 처음페이지 버튼 클릭이벤트
    /// </summary>
    public void OnMinPageButton() {
        proccessClass.PageIndex = 0;
        proccessClass.SetPageList();
    }
    /// <summary>
    /// 마지막페이지 버튼 클릭이벤트
    /// </summary>
    public void OnMaxPageButton()
    {
        proccessClass.PageIndex = 99999; ///페이징값 대충넣기 프로퍼티에서 처리하고있어서 막넣어도된다. 최대값으로
        proccessClass.SetPageList();


    }

}
