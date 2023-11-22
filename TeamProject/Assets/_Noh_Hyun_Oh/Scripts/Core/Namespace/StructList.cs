
using System;
using UnityEngine;
/// <summary>
/// ������ ������ �����͸� ���� �ϴ� Ŭ������ �����Ѵ� .
/// �ۼ���� 
/// 1. ������ �⺻������ private �� ������ ������Ƽ�� �����Ѵ�. [�ʼ������� ĸ��ȭ]
/// 2. private �� ������ ������ �Ӽ��� [SerializeField] �� �����Ѵ�  - ����Ƽ �⺻ JsonUtility ���� ������ ����½� �����ϱ����� ���  
///  2-1. ����Ƽ������ public ����� ������ �⺻������ ����ȭ �۾����Ѵٰ��Ѵ� �׷��Ƿ� private �� [SerializeField] �� �����ѰŶ� �۾����� ����.
///         - https://docs.unity3d.com/ScriptReference/SerializeField.html ����
/// 3. ���߹迭(2�����迭�̻�)�� 1�����迭�� ����ü�� �ְ� ����ü�ȿ� �ٽ� 1�����迭�� ������ ¥�½����� ����ü�� �̿��Ͽ� ���߹迭�������� ����� ������ �ִ´�. �׽�Ʈ�ڵ�� 
/// ex) struct A{B[] b; int i;}; struct B{C[] c; int a;};  struct C{int a;};  
/// A a ; 
/// public A A => a; 
/// 4. JsonUtility ������ �ڷ��� ���̸� 10�ܰ�� ���ϰ��ִ� ���̻�Ǵ°��� �Ľ��� �ȵȴ�.
///     ex ) A ����ü �ȿ� �ɹ� B ����ü   B����ü �ȿ� �ɹ� C ����ü �̷�������  �����ؼ����µ� �ٽ������ؼ� �����ؾߵǴ� ���̸� ���Ѵ�.
///     �⺻������ Vector3 �� ����ü�ȿ��� ����ϸ� ���̰� 10�̳Ѿ ���ɼ��� �־ ������ �߻��Ѵ�.
///     �׷��� �ʿ��� ���� �����ϵ������� 
/// �׽�Ʈ ���
/// 1. �ش�Ŭ������ ��ӹ޾Ƽ� ����غ������� ��ӹ��� Ŭ������ �ɹ������� ������ �ǳ� �о�ö� ���� ����� �������� �ʴ´�. 
///  1-1. �ذ�: �Լ�ȣ��� ��ӹ��� Ŭ������ �ѱ�� ����� �Ľ��� �ȴ�. �ε��Ҷ��� ���� ��ü�� ����Ͽ����Ѵ�.
/// 2. JsonGameData �� ���̽��� ��ӹ��� Ŭ������   ����Ƽ �̱��� ��ü�� �����ϱ����� ���׷������� �־�f���� ���۳�Ʈ����� �̱����������������. 
///  2-1. �ش� ������ �ٸ�������� ������ �����ϰ��ְ� �׽�Ʈ���̴�.
///  
/// 3. ���� ť�� ����Ʈ ������ �׽�Ʈ�� ���غ��Ѵ�.
/// 4. 
///  ************* ĳ���� ���������ϰ� �ش�Ŭ������ ����Ͽ� �Ľ��Լ��� �����Ѵ�.***************
/// 
/// MonoBehaviour ��  [Serializable] �� ���������ʴ´�.
/// </summary>
namespace StructList {
    /// <summary>
    /// ĳ���� ���������� ������ ���� ����ü �����ʿ��ϸ� ���⿡�߰�
    /// </summary>
    [Serializable]
    public struct CharcterItems
    {
        /// <summary>
        /// �����ۼ���
        /// </summary>
        [SerializeField]
        uint values; 
        public uint Values
        {
            get => values;
            set
            {
                values = value;
            }
        }

        /// <summary>
        /// ������ ���� Ű��
        /// </summary>
        [SerializeField]
        ItemCode itemIndex;
        public ItemCode ItemIndex
        {
            get => itemIndex;
            set
            {
                itemIndex = value;
            }
        }
        /// <summary>
        /// ������ ���Թ�ȣ 
        /// </summary>
        [SerializeField]
        uint slotIndex;
        public uint SlotIndex 
        {
            get=> slotIndex;
            set 
            {
                slotIndex = value;
            }
        }
        /// <summary>
        /// ������ ��ȭ ��
        /// </summary>
        [SerializeField]
        byte itemEnhanceValue;
        public byte ItemEnhanceValue 
        {
            get => itemEnhanceValue;
            set => itemEnhanceValue = value;    
        }
    }

    /// <summary>
    /// ĳ���� ���������� ��ų ���� ����ü �����ʿ��ϸ� ���⿡�߰�
    /// </summary>
    [Serializable]
    public struct CharcterSkills
    {
        /// <summary>
        /// ��ų ���� ���� 
        /// </summary>
        [SerializeField]
        int levelValue;
        public int LevelValue
        {
            get => levelValue;
            set
            {
                levelValue = value;
            }
        }
        /// <summary>
        /// ��ų ������ȣ 
        /// </summary>
        [SerializeField]
        int skillIndex;
        public int SkillIndex
        {
            get => skillIndex;
            set
            {
                skillIndex = value;
            }
        }
    }


    /// <summary>
    /// ĳ���� �� ���� ���⿡ �߰�
    /// </summary>
    [Serializable]
    public struct CharcterInfo
    {
        /// <summary>
        /// ĳ�����̸�
        /// </summary>
        [SerializeField]
        string charcterName;
        public String CharcterName
        {
            get => charcterName;
            set
            {
                charcterName = value;
            }
        }
        /// <summary>
        /// ĳ������ ����
        /// </summary>
        [SerializeField]
        int level;
        public int Level
        {
            get => level;
            set
            {
                level = value;
            }
        }
       
        /// <summary>
        /// ĳ������ ����ġ
        /// </summary>
        [SerializeField]
        float exp;
        public float EXP
        {
            get => exp;
            set
            {
                exp = value;
            }
        }

        /// <summary>
        /// ĳ���� �����ݾ�
        /// </summary>
        [SerializeField]
        long money;
        public long Money
        {
            get => money;
            set
            {
                money = value;
            }
        }
        [SerializeField]
        int[] flagList;
        public int[] FlagList 
        {
            get => flagList;
            set => flagList = value;
        }
        /// <summary>
        /// ĳ���� ����ʿ��� ��ǥ�� X ���� ��ǥ�� �����Ű���
        /// </summary>
        [SerializeField]
        float sceanPositionX;
        public float SceanPositionX 
        {
            get => sceanPositionX;
            set => sceanPositionX = value;
        }
        /// <summary>
        /// ĳ���� ����ʿ��� ��ǥ��  Y ���� ��ǥ�� �����Ű���
        /// </summary>
        [SerializeField]
        float sceanPositionY;
        public float SceanPositionY 
        { 
            get => sceanPositionY;
            set => sceanPositionY = value;  
        }
        /// <summary>
        /// ĳ���� ����ʿ��� ��ǥ�� Z ���� ��ǥ�� �����Ű���
        /// </summary>
        [SerializeField]
        float sceanPositionZ;
        public float SceanPositionZ 
        {
            get => sceanPositionZ; 
            set => sceanPositionZ = value;
        }

    }


    /// <summary>
    /// ĳ���� �� �������� �̺�Ʈ(����Ʈ) ����
    /// </summary>
    [Serializable]
    public struct CharcterQuest
    {
        /// <summary>
        /// ����Ʈ ����ȭ�鿡���� ����
        /// </summary>
        [SerializeField]
        string questInfo;
        public string QuestInfo 
        {
            get => questInfo;
            set => questInfo = value;
        }
        /// <summary>
        /// �̺�Ʈ ������ȣ 
        /// </summary>
        [SerializeField]
        int questIndex;
        public int QuestIndex
        {
            get => questIndex;
            set => questIndex = value;
        }
        
        /// <summary>
        /// ����Ʈ Ÿ�� �Ľ��� ���ݴ� �����ϰ� �ϱ���������
        /// </summary>
        [SerializeField]
        QuestType questType;
        public QuestType QuestType 
        {
            get => questType;
            set => questType = value;
        }
        /// <summary>
        /// ����Ʈ ���� ����
        /// </summary>
        [SerializeField]
        Quest_State questState;
        public Quest_State QuestState 
        {
            get => questState;
            set => questState = value;
        }
        /// <summary>
        /// �̺�Ʈ ���൵
        /// </summary>
        [SerializeField]
        int[] questIProgress;
        public int[] QuestIProgress
        {
            get => questIProgress;
            set => questIProgress = value;
        }


    }

}