using StructList;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;


public class SaveLoadPopupButton : MonoBehaviour
{
    /// <summary>
    /// 팝업창 위치찾기
    /// </summary>
    GameObject parentPopupWindow;

    SaveWindowManager proccessClass;

    /// <summary>
    /// 어떤 팝업인지 타입설정
    /// </summary>
    EnumList.SaveLoadButtonList type;
    /// <summary>
    /// 세이브파일 선택시 선택값
    /// </summary>
    int selectIndex = -1;
    /// <summary>
    /// 카피에사용될값 
    /// </summary>
    int oldSelectIndex = -1;

    SlotManager slotManager;

    private void Awake()
    {
        slotManager = FindObjectOfType<SlotManager>(true);
        parentPopupWindow = WindowList.Instance.IOPopupWindow.transform.GetChild(WindowList.Instance.IOPopupWindow.transform.childCount - 1).gameObject; //팝업창 위치 찾기
        proccessClass = WindowList.Instance.MainWindow;//초기화때 필요한 함수불러오기위해 사용  
    }
    private void OnEnable()//팝업창활성화시 
    {
        type = WindowList.Instance.IOPopupWindow.ButtonType; //팝업창의 타입을 셋팅한다.

        selectIndex = WindowList.Instance.IOPopupWindow.NewIndex; //세이브데이터 클릭한것의 인덱스값을 넘겨받는다

        oldSelectIndex = WindowList.Instance.IOPopupWindow.OldIndex;// 카피할 데이터 인덱스값을 넘겨받는다.
    }
    private void OnDisable()//비활성화시 기본셋팅값 초기화
    {
        selectIndex = -1;
        oldSelectIndex = -1;
        type = EnumList.SaveLoadButtonList.NONE;
        proccessClass.ResetSaveFocusing();
    }

    public void CancelButton()
    {
        switch (type) { 
            case EnumList.SaveLoadButtonList.SAVE:
                parentPopupWindow.transform.GetChild(1).gameObject.SetActive(false);
                break;
            case EnumList.SaveLoadButtonList.LOAD:
                parentPopupWindow.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case EnumList.SaveLoadButtonList.COPY:
                parentPopupWindow.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case EnumList.SaveLoadButtonList.DELETE:
                parentPopupWindow.transform.GetChild(4).gameObject.SetActive(false);
                break;
            case EnumList.SaveLoadButtonList.NONE:
                break;
        }
        parentPopupWindow.gameObject.SetActive(false); //팝업창을 닫는다 
    }
    public void SaveAcceptCheck() 
    {
        SaveLoadManager.Instance.ParsingProcess.SaveParsing();
        //저장될 시점 
        if (SaveLoadManager.Instance.Json_Save(selectIndex)) { 
            CancelButton(); //창닫기
        }
    }
    public void LoadAcceptCheck() 
    {
        if (SaveLoadManager.Instance.Json_Load(selectIndex)) { 
            CancelButton(); //창닫기
        }
    }
    public void DeleteAcceptCheck()
    {
        if (SaveLoadManager.Instance.Json_FileDelete(selectIndex)) {
            CancelButton(); //창닫기
        
        }
    }
    public void CopyAcceptCheck() 
    {
        if (oldSelectIndex > -1) //카피여부는 올드인덱스로 결정한다.
        {
            if (SaveLoadManager.Instance.Json_FileCopy(oldSelectIndex, selectIndex)) {
                Debug.Log($"{oldSelectIndex}번째를 {selectIndex}번째로 카피성공");
            }
        }
        else 
        {
            Debug.Log($"oldSelectIndex = {oldSelectIndex} 값설정안됨");    
        } 
        CancelButton(); //창닫기
    }

    
}

