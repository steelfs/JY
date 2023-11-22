using EnumList;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ��� ���� �����͸����� �����Ǿ� �ȴ�.
/// </summary>
[CreateAssetMenu(fileName = "New State Data", menuName = "Scriptable Object/State Data", order = 1)]
public class TestStateBase : ScriptableObject
{
    [Header("�����̻� �⺻ ������")]
    
    /// <summary>
    ///  �����̻� �̸�
    /// </summary>
    [SerializeField]
    string stateName = "�����̻� �̸�";
    public string StateName => stateName;

    /// <summary>
    /// �����̻� ����
    /// </summary>
    [SerializeField]
    string stateInfo = "�����̻��� ����";
    public string StateInfo => stateInfo;

    /// <summary>
    /// �����̻� ���ӽð� 
    /// </summary>
    [SerializeField]
    float stateDuration = 3.0f;
    public float StateDuration => stateDuration;

    /// <summary>
    /// �����̻��� ������
    /// </summary>
    [SerializeField]
    Sprite stateIcon;
    public Sprite StateIcon => stateIcon;   

    ///// <summary>
    ///// �����̻� Ÿ��
    ///// </summary>
    //[SerializeField]
    //StateType stateType;
    //public StateType StateType => stateType;



}
