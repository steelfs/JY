
using UnityEngine;
/// <summary>
/// ������ UI�� �⺻������ ��ӹ޾ƾ��ϴ� �������̽�
/// </summary>
public interface IUIBase 
{
    public GameObject gameObject { get; }

    public void InitializeUI();
    public void ResetValue();
}