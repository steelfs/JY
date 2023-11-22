using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[SerializeField]
public enum ItemCode// 밑의 ItemData 배열과 같은 순서를 유지해야 알맞은 프리팹을 소환한다.
{
    //지울 목록 : Bat, Bow, Pistol, Rifle, Scythe, ShotGun, SwordLaser, TwoHandAxe,, Wand,Hammer, 
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

            if (enhancable_Item != null)// 강화가 가능한 아이템이면 클론을 찍어서 리턴하고 강화가 불가능한 아이템이면 참조만 리턴함
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
    /// 프리팹 데이터에 퀘스트 정보를 바인딩하는 작업 
    /// </summary>
    private void Start()
    {
        foreach (ItemData itemdata in itemDatas) 
        {
            BindingQuest(itemdata);    
        }
    }

    /// <summary>
    /// 아이템기본데이터에 퀘스트정보를 바인딩하는 함수
    /// </summary>
    /// <param name="itemData">바인딩 체크할 아이템</param>
    private void BindingQuest(ItemData itemData) 
    {
        //스크립터블오브젝트 안에 값들은 맴버변수 선언할때 null 값으로 초기화해도 자체적으로 기본값으로 생성해둬서 null체크가 안된다
        if (itemData.questBinding != null) //스크립터블에 들어간배열변수라 기본적으로 값을 넣어둔다 . 
        {
            List<Gyu_QuestBaseData> tempQuestList = new();
            ArraySettingBindingQuest(itemData, DataFactory.Instance.QuestScriptableGenerate.MainStoryQuestArray, tempQuestList);
            ArraySettingBindingQuest(itemData, DataFactory.Instance.QuestScriptableGenerate.KillcountQuestArray, tempQuestList);
            ArraySettingBindingQuest(itemData, DataFactory.Instance.QuestScriptableGenerate.GatheringQuestArray, tempQuestList);
            if (tempQuestList.Count == 0) //스크립터블 초기셋팅 값이 들어가있는상황이니 검색해서 0개면 널값으로 초기화하기위해 체크
            {
                itemData.questBinding = null;//검색했는데도 셋팅못하면 위에포문을 거르기위해 널입력
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
    /// 퀘스트를 전부 검색해서 퀘스트에서 요구하는 아이템이 존재하는지 체크하고 
    /// 존재하면 리스트에 퀘스트정보를 담는 함수
    /// </summary>
    /// <param name="itemData">체크할 아이템</param>
    /// <param name="questArray">종류별 전체 퀘스트목록</param>
    /// <param name="bindingList">아이템에 바인딩될 퀘스트 리스트</param>
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