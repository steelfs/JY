using EnumList;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Ǵ� ������ �̽�ũ��Ʈ�� ���Ե� ������Ʈ�ؿ� �˾��� ���� ���ְ� �װ͵鸸 ó���ǰ� �Ѵ�.
/// ���� �ٸ� ����������Ʈ�� �ڽ����� ���͵��� ����ȵ�.
///  �˾�â ���� ���� ���� Ŭ���� 
///  ����Ű 
/// </summary>
/// <typeparam name="T">�˾�â ���� �ȳ���</typeparam>
public class PopupSortManager : MonoBehaviour  
{

    LinkedList<IPopupSortWindow> popupLList;
    public LinkedList<IPopupSortWindow> PopupLList => popupLList;

    
    IPopupSortWindow[]  popupArray; //â�� ������ �������������� �迭�� ����

    public void Awake() 
    {
        popupArray = GetComponentsInChildren<IPopupSortWindow>(true);
        /*
         �˾�â �����ؼ� ���⼭ �ҷ��ͼ� ������ list �� ��´�.
         */
        foreach (var popupWindow in popupArray)
        {
            popupWindow.PopupSorting = SetWindowSort; //���˾� �̺�Ʈ�� Ŭ���̺�Ʈ����
        }

        popupLList = new LinkedList<IPopupSortWindow>(popupArray);
    }

    /// <summary>
    /// IPopupSortWindow�� ��ӹ޾Ƽ�  PopupSorting�� ������ ������Ʈ���� Ŭ���̺�Ʈ�� ����ǵ��� ���� �ϸ� ��� �����ϴ�.
    /// �˾�â���� Ŭ���� �̺�Ʈ �ߵ��ϴ³��� Ŭ�����˾�â�� �Ǿ����� ����´�.
    /// </summary>
    private void SetWindowSort(IPopupSortWindow target) 
    {
        popupLList.Remove(target);
        popupLList.AddFirst(target);
        PopupObjectChange(); //�ٲ�������� ȭ������
    }

    /// <summary>
    /// �˾� âȣ�� �� ȣ���� �Լ� 
    /// </summary>
    /// <param name="target">â�� ���µɶ��� �˾�â����</param>
    public void PopupOpen(IPopupSortWindow target)
    {
        //�˾�â�� ���� �ǵڷ� �ְ� ������Ʈ���Ľ�Ų�� �׷��� �Ǿտ����δ�.
        target.OpenWindow();
        popupLList.Remove(target);
        popupLList.AddFirst(target);
        PopupObjectChange();
    }

    /// <summary>
    /// ���� �˾�â�� �ݴ´�
    /// </summary>
    /// <param name="target">���� �˾�â</param>
    public void PopupClose(IPopupSortWindow target)
    {
        popupLList.Remove(target);
        target.CloseWindow();
    }


    /// <summary>
    /// �˾� ���� ������ �߰��� ���÷���
    /// </summary>
    /// <param name="target">â�� ���µɶ��� �˾�â����</param>
    public void PopupSortDataAppend(IPopupSortWindow target)
    {
        //�˾�â�� ���� �ǵڷ� �ְ� ������Ʈ���Ľ�Ų�� �׷��� �Ǿտ����δ�.
        popupLList.Remove(target);
        popupLList.AddFirst(target);
        PopupObjectChange();
    }

    /// <summary>
    /// �˾� ���� ������ ���� 
    /// </summary>
    /// <param name="target">���� �˾�â</param>
    public void PopupSortDataRemove(IPopupSortWindow target)
    {
        popupLList.Remove(target);
    }




    /// <summary>
    /// �Ǿ��� �˾�â�� �ݴ´� 
    /// </summary>
    public void PopupClose() 
    {
        if (popupLList.Count > 0) // ���� �˾�â�� �ִ°�� 
        {
            popupLList.First.Value.CloseWindow(); //���߰� 
            popupLList.RemoveFirst(); //����Ʈ���� ���� 
        }
    }

    /// <summary>
    /// ���� �˾�â ���� �ݱ������ �ۼ�
    /// </summary>
    public void CloseAllWindow()
    {
        foreach (IPopupSortWindow go in popupArray) //�������˾���ϵ� ������ŭ ���� �ݴ´� 
        {
            go.CloseWindow();
            popupLList.Remove(go);

        }
    }
    

    /// <summary>
    /// ���� �˾�â�߿� �����ϱ� 
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
