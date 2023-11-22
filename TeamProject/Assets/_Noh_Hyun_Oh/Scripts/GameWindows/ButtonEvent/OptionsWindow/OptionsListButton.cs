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
        if (WindowList.Instance.IOPopupWindow.NewIndex > -1 &&  //���� ���� �ְų�
            SaveLoadManager.Instance?.SaveDataList[WindowList.Instance.IOPopupWindow.NewIndex] != null) //�����Ѱ��� �����Ͱ� ������
        {
            WindowList.Instance.IOPopupWindow.CopyCheck = true; //ī�� �÷��� ��

            Debug.Log("����� ��ġ�� Ŭ���ϼ���");
        }
        else {

            Debug.Log("������ ������ Ŭ���ϼ���");

        }
    }

    public void DeleteAction()
    {
        WindowList.Instance.IOPopupWindow.SaveProccessOpenPopupAction(EnumList.SaveLoadButtonList.DELETE);
       
    }

    public void OptionsAction()
    {
        JsonGameData testData = new();//�׽�Ʈ ������ ����
        testData = new TestSaveData<string>().SetSaveData();//�̰͵����߰�
        SaveLoadManager.Instance.GameSaveData = testData; //���嵥���Ϳ� �ֱ�
    }

    public void TitleAction()
    {
        //Debug.Log("�ɼ�â���� Ÿ��Ʋ�� �̵�");
        if (TurnManager.Instance.TurnIndex > 0) //��Ʋ���̸�
        {
            SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestReset();  //�ʱ�ȭ �ϱ�
        }
        LoadingScene.SceneLoading(EnumList.SceneName.TITLE);
    }

    

}
