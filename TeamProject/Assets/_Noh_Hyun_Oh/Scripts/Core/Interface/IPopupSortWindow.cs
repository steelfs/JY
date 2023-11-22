
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �˾� â �������϶� Ŭ���� ������� ȭ�� �������� ���ϼ��ְ� üũ�ϴ� �̺�Ʈ 
/// </summary>
public interface IPopupSortWindow
{

    /// <summary>
    /// �������̽��� �ִ� ������ ������ ��ӹ��������� �����ؾ��ϳ� 
    /// GameObject gameObject { get;}  �� ���´� Component ���� �̹� ������ �Ǿ��־ 
    /// �������̽� ��ӹ��� Ŭ�������� MonoBehaviour �� ��ӹ޾��ִ� �����̸� �߰� ������ ���ص� ������ �ȳ���.
    /// �̸��� �������̽��� �Լ��� �ٸ� ���Ŭ���� �Լ��͵� �����Ҽ��� �ִٴ°��̴�.
    /// </summary>
    public GameObject gameObject { get;} 
    /// <summary>
    /// �˾�â Ŭ���� ��ȣ�� �޾ƿ������� �߰� 
    /// </summary>
    public Action<IPopupSortWindow> PopupSorting { get; set; }

    /// <summary>
    /// �˾�â ���� ����� ���� Ʋ���� �Լ��λ���
    /// </summary>
    public void OpenWindow();
    /// <summary>
    /// �˾�â �ݴ� ����� ���� Ʋ���� �Լ��� ���� ����
    /// </summary>
    public void CloseWindow();
}
