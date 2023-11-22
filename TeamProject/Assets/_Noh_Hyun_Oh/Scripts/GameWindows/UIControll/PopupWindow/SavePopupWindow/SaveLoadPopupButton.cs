using StructList;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;


public class SaveLoadPopupButton : MonoBehaviour
{
    /// <summary>
    /// �˾�â ��ġã��
    /// </summary>
    GameObject parentPopupWindow;

    SaveWindowManager proccessClass;

    /// <summary>
    /// � �˾����� Ÿ�Լ���
    /// </summary>
    EnumList.SaveLoadButtonList type;
    /// <summary>
    /// ���̺����� ���ý� ���ð�
    /// </summary>
    int selectIndex = -1;
    /// <summary>
    /// ī�ǿ����ɰ� 
    /// </summary>
    int oldSelectIndex = -1;

    SlotManager slotManager;

    private void Awake()
    {
        slotManager = FindObjectOfType<SlotManager>(true);
        parentPopupWindow = WindowList.Instance.IOPopupWindow.transform.GetChild(WindowList.Instance.IOPopupWindow.transform.childCount - 1).gameObject; //�˾�â ��ġ ã��
        proccessClass = WindowList.Instance.MainWindow;//�ʱ�ȭ�� �ʿ��� �Լ��ҷ��������� ���  
    }
    private void OnEnable()//�˾�âȰ��ȭ�� 
    {
        type = WindowList.Instance.IOPopupWindow.ButtonType; //�˾�â�� Ÿ���� �����Ѵ�.

        selectIndex = WindowList.Instance.IOPopupWindow.NewIndex; //���̺굥���� Ŭ���Ѱ��� �ε������� �Ѱܹ޴´�

        oldSelectIndex = WindowList.Instance.IOPopupWindow.OldIndex;// ī���� ������ �ε������� �Ѱܹ޴´�.
    }
    private void OnDisable()//��Ȱ��ȭ�� �⺻���ð� �ʱ�ȭ
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
        parentPopupWindow.gameObject.SetActive(false); //�˾�â�� �ݴ´� 
    }
    public void SaveAcceptCheck() 
    {
        SaveLoadManager.Instance.ParsingProcess.SaveParsing();
        //����� ���� 
        if (SaveLoadManager.Instance.Json_Save(selectIndex)) { 
            CancelButton(); //â�ݱ�
        }
    }
    public void LoadAcceptCheck() 
    {
        if (SaveLoadManager.Instance.Json_Load(selectIndex)) { 
            CancelButton(); //â�ݱ�
        }
    }
    public void DeleteAcceptCheck()
    {
        if (SaveLoadManager.Instance.Json_FileDelete(selectIndex)) {
            CancelButton(); //â�ݱ�
        
        }
    }
    public void CopyAcceptCheck() 
    {
        if (oldSelectIndex > -1) //ī�ǿ��δ� �õ��ε����� �����Ѵ�.
        {
            if (SaveLoadManager.Instance.Json_FileCopy(oldSelectIndex, selectIndex)) {
                Debug.Log($"{oldSelectIndex}��°�� {selectIndex}��°�� ī�Ǽ���");
            }
        }
        else 
        {
            Debug.Log($"oldSelectIndex = {oldSelectIndex} �������ȵ�");    
        } 
        CancelButton(); //â�ݱ�
    }

    
}

