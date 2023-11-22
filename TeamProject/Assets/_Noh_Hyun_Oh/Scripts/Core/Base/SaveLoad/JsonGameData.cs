using StructList;
using System;
using System.IO;
using UnityEngine;



/// <summary>
/// ������ ������ �����͸� ���� �ϴ� Ŭ������ �����Ѵ� .
/// �ۼ���� 
/// ��
/// 1. ������ �⺻������ private �� ������ ������Ƽ�� �����Ѵ�. [�ʼ������� ĸ��ȭ]
///   1-1. ����ȭ�Ҷ� private �ϰ� �Ӽ� [SerializeField] �����°ų� public ���� �����ϴ°ų� �۾����� ���ٰ��Ѵ�. 
///         public�� ���������� ����ȭ �۾��� ���ļ� �����Ϳ��Ѹ��ٰ��Ѵ�. private �ϰ� [SerializeField] �ϴ°Ͱ� ����������� �̷������.
///         
/// 2. private �� ������ ������ �Ӽ��� [SerializeField] �� �����Ѵ�  - ����Ƽ �⺻ JsonUtility ���� ������ ����½� �����ϱ����� ���  
/// 
/// 3. ���߹迭(2�����迭�̻�)�� 1�����迭�� ����ü�� �ְ� ����ü�ȿ� �ٽ� 1�����迭�� ������ ¥�½����� ����ü�� �̿��Ͽ� ���߹迭�������� ����� ������ �ִ´�.
/// 3-1. ����ü�迭�� �ȵȴ�? �ƴ� �ȴ� ������ ������о�´�. 
/// 
/// 4. ��Ӱ����� �Ľ��� �ȵȴ�. �⺻������ �θ�A �� Ŭ������  jsonUtility �� �̿��ϸ� A ��ü�� �ڽ��ǳ������ �����Ѵ� ������ 
///         �ҷ��ö� jsonUtility�� �����͸� �޴� ����  Ŭ���������� �Ľ��ϱ⶧���� ������(�ڽ�Ŭ����������) ������ �߻��Ѵ�.
///    ��� : �������� �ϳ��� ó���Ϸ��� Ŭ�����ϳ����� ó���ؾ��Ѵ� ( ��� X )
///    - �ش系���� ���������� �����͸� ���������� �����̴�  , �޸𸮻󿡴� �ڽ��� �����ͱ��� ���� ���ִ�.
/// ex) struct A{B[] b; int i;}; struct B{C[] c; int a;};  struct C{int a;};  
/// [SerializeField]
/// A a ; 
/// public A A => a; 
/// 
/// 4. �����Ҷ� �ٲ����ʴ� ������ �����Ϸ��� SaveLoadManager.setDefaultInfo �Լ��� ���� �⺻������ ������Ƽ�� public ���� �����Ѵ�.
/// �׽�Ʈ ���
/// 1. �ش�Ŭ������ ��ӹ޾Ƽ� ����غ������� ��ӹ��� Ŭ������ �ɹ������� ������ �ǳ� �о�ö� ���� ����� �������� �ʴ´�. 
///  1-1. �ذ�: �Լ�ȣ��� ��ӹ��� Ŭ������ �ѱ�� ����� �Ľ��� �ȴ�. �ε��Ҷ��� ���� ��ü�� ����Ͽ����Ѵ�.
/// 2. JsonGameData �� ���̽��� ��ӹ��� Ŭ������   ����Ƽ �̱��� ��ü�� �����ϱ����� ���׷������� �־�f���� ���۳�Ʈ����� �̱����������������. 
///  2-1. �ش� ������ �ٸ�������� ������ �����ϰ��ְ� �׽�Ʈ���̴�.
///  
/// 3. ���� ť�� ����Ʈ ������ �׽�Ʈ�� ���غ��Ѵ�. => ť ����Ʈ ���� ���� �ڷᱸ���� ����ȭ �ϱ� ��Ʊ⶧���� �����ͷ� �ִ°� ����õ�̴�.
///  3-1 . ����Ƽ������ ����ȭ�� �������̸� ������ 10�ܰ�� �������ξ��µ� �̴� ��ȯ������ ������ �Ǽ� �׷����̴� 
///        ť�� �����ϴ� �ڷᱸ���� ���������ʰ� , ����Ʈ�ǰ�� ��ȯ������ �����ؼ� ������ ������ ����ε� ����ȭ�� �̷������.
///        ���ö��� �����ϴ� �ڷᱸ���δ� ���������ʴ�.
///        
///  ************* ĳ���� ���������ϰ� �ش�Ŭ������ ����Ͽ� �Ľ��Լ��� �����Ѵ�. 
///                 - �̶� ��ȯ������ �����ϰ� ����ؾ��ϰ� ��������(��ӵ�����)�� 10�ܰ�� �����ϱ⶧���� �����ؾ��Ѵ�.***************
///  
/// MonoBehaviour ��  [Serializable] �� ���������ʴ´�.
/// </summary>
///  
///����ȭ : ���������� �Ľ��۾��� �ʿ��ϴٰ� �Ѵ�. 
///
///PlayerPrefs �� ������ ������Ʈ���� ����ȴ�. �����Ͱ� ���µ��־ ����õ�̴�. �����ιٲܼ�������. ���ȿ� ��� 
///JsonGameData  a = new(); ���ĵ�����
///
[Serializable]
public class JsonGameData 
{
    //���嵥���� ĳ���ϱ����� �ε��� ��ȣ
    [SerializeField]
    int dataIndex;
    public int DataIndex {
        get => dataIndex;
        set{ 
            dataIndex = value;
        
        }
    }

    /// <summary>
    /// ���� �������� ��Ʋ�� 
    /// </summary>
    [SerializeField]
    StageList currentStage = StageList.None;
    public StageList CurrentStage 
    {
        get => currentStage;
        set => currentStage = value;
    }

    /// <summary>
    /// �������� Ŭ���� ���� ����
    /// </summary>
    [SerializeField]
    StageList stageClear;
    public StageList StageClear 
    {
        get => stageClear;
        set => stageClear = value;
    }
    /// <summary>
    /// ���������� ������ġ�� ����
    /// </summary>
    [SerializeField]
    Vector3 startPos;
    public Vector3 StartPos 
    {
        get => startPos;
        set => startPos = value;
    }

    /// <summary>
    /// ����ð� �־�α� 
    /// </summary>
    [SerializeField]
    string saveTime;
    public string SaveTime { 
        get => saveTime;
        set { 
            saveTime = value;
        }
    }
    
    /// <summary>
    /// �ҷ������ ���� ������ 
    /// </summary>
    [SerializeField]
    EnumList.SceneName sceanName;
    public EnumList.SceneName SceanName
    {
        get => sceanName;
        set
        {
            sceanName = value;
        }
    }
    [SerializeField]
    Base_Status playerData;
    public Base_Status PlayerData 
    {
        get => playerData;
        set => playerData = value;
    }
    [SerializeField]
    Equipments_Data_Server equipments_Data;
    public Equipments_Data_Server Equipments_Data
    {
        get => equipments_Data;
        set => equipments_Data = value;
    }

    /// <summary>
    /// ��� ���԰��� 
    /// </summary>
    [SerializeField]
    int equipSlotLength;
    public int EquipSlotLength 
    {   
        get => equipSlotLength;
        set => equipSlotLength = value;
    }

    /// <summary>
    /// ĳ���� ���������� ����Ʈ -���
    /// </summary>
    [SerializeField]
    CharcterItems[] equipData;
    public CharcterItems[] EquipData
    {
        get => equipData;
        set => equipData = value;
    }
    /// <summary>
    /// �Ҹ� ���԰��� 
    /// </summary>
    [SerializeField]
    int consumeSlotLength;
    public int ConsumeSlotLength
    {
        get => consumeSlotLength;
        set => consumeSlotLength = value;
    }
    /// <summary>
    /// ĳ���� ���������� ����Ʈ -�Һ�
    /// </summary>
    [SerializeField]
    CharcterItems[] consumeData;
    public CharcterItems[] ConsumeData
    {
        get => consumeData;
        set => consumeData = value;
    }
    /// <summary>
    /// ��Ÿ ���԰��� 
    /// </summary>
    [SerializeField]
    int etcSlotLength;
    public int EtcSlotLength
    {
        get => etcSlotLength;
        set => etcSlotLength = value;
    }
    /// <summary>
    /// ĳ���� ���������� ����Ʈ -��Ÿ
    /// </summary>
    [SerializeField]
    CharcterItems[] etcData;
    public CharcterItems[] EtcData
    {
        get => etcData;
        set => etcData = value;
    }
    /// <summary>
    /// ���� ���԰��� 
    /// </summary>
    [SerializeField]
    int craftSlotLength;
    public int CraftSlotLength
    {
        get => craftSlotLength;
        set => craftSlotLength = value;
    }
    /// <summary>
    /// ĳ���� ���������� ����Ʈ -����
    /// </summary>
    [SerializeField]
    CharcterItems[] craftData;
    public CharcterItems[] CraftData
    {
        get => craftData;
        set => craftData = value;
    }

    /// <summary>
    /// �߿뾾�� ���� Ŭ���� ���� 
    /// </summary>
    [SerializeField]
    Save_SkillData[] skillDatas;
    public Save_SkillData[] SkillDatas 
    {
        get => skillDatas;
        set => skillDatas = value;
    }
    /// <summary>
    /// ĳ���� ����Ʈ���� ����Ʈ
    /// </summary>
    [SerializeField]
    StructList.CharcterQuest[] questList;
    public StructList.CharcterQuest[] QuestList
    {
        get => questList;
        set
        {
            questList = value;
        }
    }

  
}
