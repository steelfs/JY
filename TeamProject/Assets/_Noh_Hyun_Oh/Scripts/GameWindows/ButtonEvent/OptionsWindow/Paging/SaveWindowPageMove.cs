using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����������  �������̵���ư ���� ��ư�̺�Ʈ ����
/// </summary>
public class SaveWindowPageMove : MonoBehaviour
{
    SaveWindowManager proccessClass;


    private void Start()
    {
        proccessClass =  WindowList.Instance.MainWindow; // sort Ŭ������ �ϳ��̱⶧���� ��ã�ƿ���
    }

    /// <summary>
    /// ���������� ��ư Ŭ���̺�Ʈ
    /// </summary>
    public void OnPrevButton() {
        proccessClass.PageIndex--; //���������� �����ϰ�
        proccessClass.SetPageList();//������ ���÷���
    }
    /// <summary>
    /// ���������� ��ư Ŭ���̺�Ʈ
    /// </summary>
    public void OnNextButton()
    {
        proccessClass.PageIndex++;
        proccessClass.SetPageList();
    } 
    /// <summary>
    /// ó�������� ��ư Ŭ���̺�Ʈ
    /// </summary>
    public void OnMinPageButton() {
        proccessClass.PageIndex = 0;
        proccessClass.SetPageList();
    }
    /// <summary>
    /// ������������ ��ư Ŭ���̺�Ʈ
    /// </summary>
    public void OnMaxPageButton()
    {
        proccessClass.PageIndex = 99999; ///����¡�� ����ֱ� ������Ƽ���� ó���ϰ��־ ���־�ȴ�. �ִ밪����
        proccessClass.SetPageList();


    }

}
