using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���̺� �Ҷ� �ʿ��ѵ����� 
/// id , currentCount �� �ΰ��ǵ����͸� �����Ѵ�.
/// </summary>
[CreateAssetMenu(fileName = "New Quest Data", menuName = "Scriptable Object/QuestDatas/Quest", order = 1)]
public class Gyu_QuestBaseData : ScriptableObject
{
    [Header("����Ʈ�� �⺻����")]
    /// <summary>
    /// ����Ʈ id
    /// </summary>
    int questId = -1;
    public int QuestId
    {
        get => questId;
        set
        {
            //�̼�������Ƽ�� ��ó�� �����ͼ����Ҷ� �ѹ��� �����ϵ��� ������§��.
            if (questId < 0) // ����Ʈ���̵� -1 �� �ʱⰪ�϶��� ���� �����ϵ��� �Ѵ�.
            {
                questId = value;
            }
        }
    }

    /// <summary>
    /// ����Ʈ Ÿ��
    /// </summary>
    [SerializeField]
    QuestType type;
    public QuestType QuestType => type;

    /// <summary>
    /// ����Ʈ ������
    /// </summary>
    [SerializeField]
    Sprite iconImage;
    public Sprite IconImage => iconImage;

    /// <summary>
    /// ����Ʈ ����
    /// </summary>
    [SerializeField]
    string title;
    public string Title => title;

    /// <summary>
    /// ����Ʈ ����
    /// </summary>
    [SerializeField]
    string description;
    public string Description => description;

    /// <summary>
    /// ����Ʈ ��ǥ
    /// </summary>
    [SerializeField]
    string clearObjectives;
    public string ClearObjectives => clearObjectives;

    //--------------------------- ������� ���� 
    [Header("����Ʈ�� ���� ���� ����")]
    /// <summary>
    /// ����Ʈ�� ���� �ݾ�
    /// </summary>
    [SerializeField]
    int rewardCoin;
    public int RewardCoin => rewardCoin;

    /// <summary>
    /// ����Ʈ�� ���� ������
    /// </summary> 
    [SerializeField]
    ItemCode[] rewardItem;
    public ItemCode[] RewardItem => rewardItem;

    /// <summary>
    /// ����������� ����
    /// ���� ���� ����Ʈ���� �迭�� 1:1�� �ε��� ��Ī���Ѿ��Ѵ�.
    /// </summary>
    [SerializeField]
    int[] itemCount;
    public int[] ItemCount => itemCount;



    //------------------------------- ����Ʈ ������� ���� 

    [Header("����Ʈ�� ���࿡ ���õ� ����")]
    /// <summary>
    /// ���� ����Ʈ�� �ʿ��� ������
    /// </summary> 
    [SerializeField]
    ItemCode[] requestItem;
    public ItemCode[] RequestItem => requestItem;

    /// <summary>
    /// ��� ����Ʈ�� �ʿ��� ���� ���
    /// </summary>
    [SerializeField]
    Monster_Type[] questMosters;
    public Monster_Type[] QuestMosters => questMosters;


    /// <summary>
    /// Ŭ������� �ʿ��� ī��Ʈ(����)
    /// </summary>
    [SerializeField]
    int[] requiredCount;
    public int[] RequiredCount => requiredCount;

    /// <summary>
    /// ���� ī��Ʈ(����)
    /// </summary>
    [SerializeField]
    int[] currentCount;
    public int[] CurrentCount => currentCount;

    /// <summary>
    /// ����Ʈ������ ���� ����
    /// </summary>
    Quest_State quest_State = Quest_State.None;
    public virtual Quest_State Quest_State
    {
        get => quest_State;
        set 
        {
            quest_State = value;
        }
    }


    private void Awake()
    {
       currentCount = new int[RequiredCount.Length];
    }

    /// <summary>
    /// �ҷ��ý� ����Ʈ ���൵ ó���ϱ����� �Լ�
    /// �ᱹ Setter ��.. 
    /// </summary>
    /// <param name="countArray">����� �����Ȳ������</param>
    /// <param name="state">����� ���� ������</param>
    public void SaveFileDataPasing(int[] countArray,Quest_State state) 
    {
        currentCount = countArray;
        quest_State = state;
    }

    /// <summary>
    /// ���� ����Ʈ ������ �ڵ带 ������ ����Ʈ ���ప�� �ø��� �Լ�
    /// ����Ʈ ���鶧 �ߺ��� �������� ��������� �ϸ� ���װ��߻��Ұ��̴�
    /// </summary>
    /// <param name="requestItemCode">�������ڵ�</param>
    /// <param name="requestItemCount">���� ������ ������ ����</param>
    public void SetCounting( int requestItemCount, ItemCode requestItemCode) 
    {
        for (int i = 0; i < requestItem.Length; i++)
        {
            if (requestItem[i] == requestItemCode) 
            {
                currentCount[i] = requestItemCount;
                break;
            }
        }
    }

    /// <summary>
    /// ��� ����Ʈ ī���� ������ 
    /// �̿ϼ� ���� ���ϼ��Ǿ� �ɵ��ϴ� 
    /// ���� ���������� �̳Ѱ��� ���ڷιް� ���ڸ� ����Ŭ�������ٰ� ������ ���������صΰ� �װ��� ���ϴ·����ʿ� 
    /// �⺻������ ���� ����Ʈ �񱳶� �����ϴ�.
    /// ����Ʈ ���鶧 �ߺ��� ���͸��� ó���϶�� �ϸ� ���װ��߻��Ұ��̴�
    /// </summary>
    /// <param name="monsterType">���� ����</param>
    /// <param name="addCount">�߰��� ī��Ʈ</param>
    public void SetCounting(Monster_Type monsterType, int addCount) 
    {
        for (int i = 0; i < questMosters.Length; i++)
        {
            if (questMosters[i] == monsterType)
            {
                currentCount[i] += addCount; //�ƴϸ� ī����
                break;
            }
        }
    }

    /// <summary>
    /// ����Ʈ�� ���� �������� ����Ʈ��Ȳ
    /// </summary>
    /// <returns>�Ϸ��ų� ����Ʈ�� ������  true �ƴϸ� false </returns>
    public bool IsSucess() 
    {
        int requestArrayLength = requiredCount.Length;
        for (int i = 0; i < requestArrayLength; i++)
        {
            if (requiredCount[i] > currentCount[i]) //�Ϸ� �ȉ���� üũ�ؼ�  
            {
                return false; //Ŭ���� �ȉ����� �ȉ�ٰ� ��ȯ
            }
        }
        return true; // Ŭ���� ������ true ������
    }

    /// <summary>
    /// �����Ȳ�� �ʱ�ȭ �ϴ� �Լ�
    /// </summary>
    public void ResetData() 
    {
        int length = currentCount.Length;
        for (int i = 0; i < length; i++)
        {
            currentCount[i] = 0;
        }
        quest_State = Quest_State.None;
    }
}
