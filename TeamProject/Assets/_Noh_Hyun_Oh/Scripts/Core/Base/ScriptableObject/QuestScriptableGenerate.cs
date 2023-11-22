using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ӳ����� �ѹ��� �����ǰ� ����Ʈ �� ������ ������ Ŭ���� 
/// </summary>
public class QuestScriptableGenerate : BaseScriptableObjectGenerate<Gyu_QuestBaseData>
{
    /// <summary>
    /// ���ӽ��۽� ����Ʈ������ �ε����� �����ϱ����� �� 
    /// ����Ʈ �ε��� �� ���� ������
    /// </summary>
    int questIndex = 0;

    /// <summary>
    /// ����Ʈ ������ ���̵�� �����ϱ����� �ε��� ���� �ش�. 
    /// ����Ʈ �������� �ش� ���̻����� ��������� �ϱ����� ����
    /// </summary>
    int questIndexGap = 1000;
    public int QuestIndexGap => questIndexGap;
    
    /// <summary>
    /// ����Ʈ�� ������ �ľ��� ������
    /// </summary>
    //[SerializeField]
    //int questCount = 0;

    /// <summary>
    /// ���� ����Ʈ ��ũ���ͺ� ������ ���� �迭
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] originMainStoryQuestArray;

    /// <summary>
    /// ��� ����Ʈ ��ũ���ͺ� ������ ���� �迭
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] originKillcountQuestArray;

    /// <summary>
    /// ���� ����Ʈ ��ũ���ͺ� ������ ���� �迭
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] originGatheringQuestArray;


    [Header("���ӻ��� ��� ����Ʈ �迭")]
    /// <summary>
    /// ���� ����Ʈ ��Ƶ� �迭 
    /// </summary>
    Gyu_QuestBaseData[] mainStoryQuestArray;
    public Gyu_QuestBaseData[] MainStoryQuestArray => mainStoryQuestArray;

    /// <summary>
    /// ��� ����Ʈ ��Ƶ� �迭 
    /// </summary>
    Gyu_QuestBaseData[] killcountQuestArray;
    public Gyu_QuestBaseData[] KillcountQuestArray => killcountQuestArray;

    /// <summary>
    /// ���� ����Ʈ ��Ƶ� �迭
    /// </summary>
    Gyu_QuestBaseData[] gatheringQuestArray;
    public Gyu_QuestBaseData[] GatheringQuestArray => gatheringQuestArray;

    private void Awake()
    {
        mainStoryQuestArray = QuestDataGenerate(originMainStoryQuestArray);
        killcountQuestArray = QuestDataGenerate(originKillcountQuestArray);
        gatheringQuestArray = QuestDataGenerate(originGatheringQuestArray);
        SetAllDataIndexing();
    }
    /// <summary>
    /// ���� �ʱ�ȭ�� ����Ʈ�� �ε����� �����ϱ����� �����ų���� 
    /// </summary>
    private void SetAllDataIndexing() 
    {
        int questTypeLength = Enum.GetValues(typeof(QuestType)).Length;     //�̳� ������ ã�Ƽ�  

        for (int i = 0; i < questTypeLength; i++) 
        {
            questIndex = questIndexGap * i; 
            switch ((QuestType)i)
            {
                case QuestType.Story:
                    ArrayIndexsing(mainStoryQuestArray);
                    break;
                case QuestType.Killcount:
                    ArrayIndexsing(killcountQuestArray);
                    break;
                case QuestType.Gathering:
                    ArrayIndexsing(gatheringQuestArray);
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// �ʱ�ȭ�� ����Ǹ�
    /// �ε����� �����ϱ����� �Լ�
    /// </summary>
    /// <param name="resetDataArray">�ε��� ������ ����Ʈ �迭</param>
    private void ArrayIndexsing(Gyu_QuestBaseData[] resetDataArray) 
    {

        foreach (Gyu_QuestBaseData questData in resetDataArray)
        {
            questData.QuestId = questIndex;
            questIndex++;
            
        }
    }
    /// <summary>
    /// ���������ʹ� ���ΰ� �������� ���鸸 ���� �ʱ�ȭ �ϴ� �Լ� 
    /// </summary>
    public void ResetData() 
    {
        ArrayDataReset(mainStoryQuestArray);
        ArrayDataReset(killcountQuestArray);
        ArrayDataReset(gatheringQuestArray);
    }
    /// <summary>
    /// ������ �ȵǾ� �� ���� ���ΰ� 
    /// ��������� �����Ǵ� �����͸� ���½�Ű�� �Լ� 
    /// </summary>
    /// <param name="resetDataArray">������ �迭</param>
    private void ArrayDataReset(Gyu_QuestBaseData[] resetDataArray)
    {
        foreach (Gyu_QuestBaseData questData in resetDataArray)
        {
            questData.ResetData();
        }
    }

    /// <summary>
    /// ������ ������ ����Ǹ� ����� �Լ� 
    /// </summary>
    public void InventoryItemCountSetting(int itemCount, ItemCode itemCode) 
    {
        //��� ����Ʈ�� ������ �����Ű��.
        int questTypeLength = Enum.GetValues(typeof(QuestType)).Length;     //�̳� ������ ã�Ƽ�  

        for (int i = 0; i < questTypeLength; i++)
        {
            switch ((QuestType)i)
            {
                case QuestType.Story:
                    QuestCurrentCountingSetting(mainStoryQuestArray);
                    break;
                case QuestType.Killcount:
                    QuestCurrentCountingSetting(killcountQuestArray);
                    break;
                case QuestType.Gathering:
                    QuestCurrentCountingSetting(gatheringQuestArray);
                    break;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="questArray"></param>
    private void QuestCurrentCountingSetting(Gyu_QuestBaseData[] questArray) 
    {

    }
}
