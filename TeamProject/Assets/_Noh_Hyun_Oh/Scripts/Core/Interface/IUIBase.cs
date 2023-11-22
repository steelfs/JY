
using UnityEngine;
/// <summary>
/// 고정형 UI가 기본적으로 상속받아야하는 인터페이스
/// </summary>
public interface IUIBase 
{
    public GameObject gameObject { get; }

    public void InitializeUI();
    public void ResetValue();
}