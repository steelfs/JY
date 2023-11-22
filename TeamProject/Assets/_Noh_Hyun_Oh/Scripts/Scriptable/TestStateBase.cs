using EnumList;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 기능 말고 데이터만으로 구성되야 된다.
/// </summary>
[CreateAssetMenu(fileName = "New State Data", menuName = "Scriptable Object/State Data", order = 1)]
public class TestStateBase : ScriptableObject
{
    [Header("상태이상 기본 데이터")]
    
    /// <summary>
    ///  상태이상 이름
    /// </summary>
    [SerializeField]
    string stateName = "상태이상 이름";
    public string StateName => stateName;

    /// <summary>
    /// 상태이상 설명
    /// </summary>
    [SerializeField]
    string stateInfo = "상태이상의 설명";
    public string StateInfo => stateInfo;

    /// <summary>
    /// 상태이상 지속시간 
    /// </summary>
    [SerializeField]
    float stateDuration = 3.0f;
    public float StateDuration => stateDuration;

    /// <summary>
    /// 상태이상의 아이콘
    /// </summary>
    [SerializeField]
    Sprite stateIcon;
    public Sprite StateIcon => stateIcon;   

    ///// <summary>
    ///// 상태이상 타입
    ///// </summary>
    //[SerializeField]
    //StateType stateType;
    //public StateType StateType => stateType;



}
