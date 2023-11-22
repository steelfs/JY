using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OptionsListButton : MonoBehaviour
{
    public void SaveAction() 
    {
        WindowList.Instance.IOPopupWindow.SaveProccessOpenPopupAction(EnumList.SaveLoadButtonList.SAVE);
    }

    public void LoadAction()
    {
        WindowList.Instance.IOPopupWindow.SaveProccessOpenPopupAction(EnumList.SaveLoadButtonList.LOAD);

    }

    public void CopyAction() 
    {
        if (WindowList.Instance.IOPopupWindow.NewIndex > -1 &&  //선택 값이 있거나
            SaveLoadManager.Instance?.SaveDataList[WindowList.Instance.IOPopupWindow.NewIndex] != null) //선택한값의 데이터가 있을때
        {
            WindowList.Instance.IOPopupWindow.CopyCheck = true; //카피 플래그 온

            Debug.Log("복사될 위치를 클릭하세요");
        }
        else {

            Debug.Log("복사할 파일을 클릭하세요");

        }
    }

    public void DeleteAction()
    {
        WindowList.Instance.IOPopupWindow.SaveProccessOpenPopupAction(EnumList.SaveLoadButtonList.DELETE);
       
    }

    public void OptionsAction()
    {
        JsonGameData testData = new();//테스트 데이터 생성
        testData = new TestSaveData<string>().SetSaveData();//이것도값추가
        SaveLoadManager.Instance.GameSaveData = testData; //저장데이터에 넣기
    }

    public void TitleAction()
    {
        //Debug.Log("옵션창에서 타이틀로 이동");
        if (TurnManager.Instance.TurnIndex > 0) //배틀맵이면
        {
            SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestReset();  //초기화 하기
        }
        LoadingScene.SceneLoading(EnumList.SceneName.TITLE);
    }

    

}
