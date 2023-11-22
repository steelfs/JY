using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[SerializeField]
public enum ItemCode// ���� ItemData �迭�� ���� ������ �����ؾ� �˸��� �������� ��ȯ�Ѵ�.
{
    //���� ��� : Bat, Bow, Pistol, Rifle, Scythe, ShotGun, SwordLaser, TwoHandAxe,, Wand,Hammer, 
    Cash,
    HpPotion,
    MpPotion,
    SecretPotion,
    SpellBook,
    Shield,
    Shield_Extended,
    Enhancable_Bow,
    Dagger,
    Enhancable_Pistol,
    Enhancable_Rifle,
    Enhancable_shotGun,
    Enhancable_Sword,
    AquaCrystal,
    BlueCrystal,
    Intermidiate_Blue_Crystal,
    Advanced_Blue_Crystal,
    DarkCrystal,
    Intermidiate_Dark_Crystal,
    Advanced_Dark_Crystal,
    Green_Crystal,
    Intermidiate_Green_Crystal,
    Advanced_Green_Crystal,
    Pink_Crystal,
    Purple_Crystal,
    Red_Crystal,
    Intermidiate_Red_Crystal,
    Advanced_Red_Crystal,
    Unknown_Crystal,
    Intermidiate_Unknown_Crystal,
    Advanced_Unknown_Crystal,
    Yellow_Crystal,
    Bullet_Default,
    Bullet_Grade1,
    Bullet_Grade2,
    Bullet_Grade3,
    Bullet_Grade4,
    Bullet_Grade5,
    Bullet_Grade6,
    Bullet_Grade7,
    Bullet_Grade8,
    Bullet_Grade9,
    Bullet_Grade10,
    Bullet_Grade11,
    Bullet_Grade12,
    Bullet_Grade13,
    Captains_Hat,
    Crews_Hat,
    Junnkers_Helm,
    Space_Helm,
    Space_Armor,
    Big_Space_Armor
}

public enum ItemType
{
    None,
    Equip,
    Consume,
    Etc,
    Craft
}
public enum EnhanceType
{
    attack,
    defence

}
public enum ItemSortBy
{
    Code,  
    Name,  
    Price, 
}
public enum CraftType
{
    Blue_Crystal = 0,
    Dark_Crystal,
    Aqua_Crystal,
    Green_Crystal,
    Pink_Crystal,
    Purple_Crystal,
    Red_Crystal,
    Unknown_Crystal,
    Yellow_Crystal
}

public class ItemDataManager : MonoBehaviour
{

    public ItemData[] itemDatas = null;


    public ItemData this[ItemCode code]
    {
        
        get
        {
            ItemData itemdata = itemDatas[(int)code];
            BindingQuest(itemdata);
            ItemData_Enhancable enhancable_Item = itemdata as ItemData_Enhancable;

            if (enhancable_Item != null)// ��ȭ�� ������ �������̸� Ŭ���� �� �����ϰ� ��ȭ�� �Ұ����� �������̸� ������ ������
            {
                return Instantiate(itemDatas[(int)code]);
            }
            else
            {
                return itemDatas[(int)code];
            }
        }
    }
 

    public int length => itemDatas.Length;

    /// <summary>
    /// ������ �����Ϳ� ����Ʈ ������ ���ε��ϴ� �۾� 
    /// </summary>
    private void Start()
    {
        foreach (ItemData itemdata in itemDatas) 
        {
            BindingQuest(itemdata);    
        }
    }

    /// <summary>
    /// �����۱⺻�����Ϳ� ����Ʈ������ ���ε��ϴ� �Լ�
    /// </summary>
    /// <param name="itemData">���ε� üũ�� ������</param>
    private void BindingQuest(ItemData itemData) 
    {
        //��ũ���ͺ������Ʈ �ȿ� ������ �ɹ����� �����Ҷ� null ������ �ʱ�ȭ�ص� ��ü������ �⺻������ �����صּ� nullüũ�� �ȵȴ�
        if (itemData.questBinding != null) //��ũ���ͺ� ���迭������ �⺻������ ���� �־�д� . 
        {
            List<Gyu_QuestBaseData> tempQuestList = new();
            ArraySettingBindingQuest(itemData, DataFactory.Instance.QuestScriptableGenerate.MainStoryQuestArray, tempQuestList);
            ArraySettingBindingQuest(itemData, DataFactory.Instance.QuestScriptableGenerate.KillcountQuestArray, tempQuestList);
            ArraySettingBindingQuest(itemData, DataFactory.Instance.QuestScriptableGenerate.GatheringQuestArray, tempQuestList);
            if (tempQuestList.Count == 0) //��ũ���ͺ� �ʱ���� ���� ���ִ»�Ȳ�̴� �˻��ؼ� 0���� �ΰ����� �ʱ�ȭ�ϱ����� üũ
            {
                itemData.questBinding = null;//�˻��ߴµ��� ���ø��ϸ� ���������� �Ÿ������� ���Է�
                return;
            }
            itemData.questBinding = new Gyu_QuestBaseData[tempQuestList.Count];
            int i = 0;
            foreach (Gyu_QuestBaseData quest in tempQuestList)
            {
                itemData.questBinding[i++] = quest;
            }
        }
    }

    /// <summary>
    /// ����Ʈ�� ���� �˻��ؼ� ����Ʈ���� �䱸�ϴ� �������� �����ϴ��� üũ�ϰ� 
    /// �����ϸ� ����Ʈ�� ����Ʈ������ ��� �Լ�
    /// </summary>
    /// <param name="itemData">üũ�� ������</param>
    /// <param name="questArray">������ ��ü ����Ʈ���</param>
    /// <param name="bindingList">�����ۿ� ���ε��� ����Ʈ ����Ʈ</param>
    private void ArraySettingBindingQuest(ItemData itemData, Gyu_QuestBaseData[] questArray , List<Gyu_QuestBaseData> bindingList) 
    {
        foreach (Gyu_QuestBaseData quest in questArray)
        {
            foreach (ItemCode code in quest.RequestItem) 
            {
                if (code == itemData.code) 
                {
                    bindingList.Add(quest);
                    break;
                }
            }
            
        }
    }

}