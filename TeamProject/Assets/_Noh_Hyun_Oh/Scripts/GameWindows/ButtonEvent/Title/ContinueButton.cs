using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Ÿ��Ʋ��������� ��ư �ε�ȭ�� ���۳����� 
/// �׽�Ʈ�� �ε�ȭ�鿡�� �������������� ���ѷε��˴ϴ�..
/// </summary>
public class ContinueButton : MonoBehaviour
{
   
    public void OnClickContinue()
    {
        WindowList.Instance.MainWindow.gameObject.SetActive(!WindowList.Instance.MainWindow.gameObject.activeSelf);
    }
}
