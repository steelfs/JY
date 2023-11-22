using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSpawner : TestBase
{
    public GameObject[] prefabs;
  //  public float dropRate;
    private Dictionary<ItemCode, GameObject> prefabDict = new Dictionary<ItemCode, GameObject>();
    private Dictionary<Monster_Type, List<(ItemCode, float)>> enemyDropTable = new Dictionary<Monster_Type, List<(ItemCode, float)>>();//������̺� ����
    // Enemy�� Ÿ�Կ� ���� PrefabName(������) �� float Ȯ���� ����ض�. �� PrefabName��  dictionary�� GameObject�� ���ε��ؼ� �������ش�.

    public uint index = 0;
    public ItemCode itemCode;
    public ItemSortBy sortBy;
    public bool IsAccending;
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    void Initialize()
    {
        SetDropTable();//���� ������� ����
    }
    private void Start()
    {
        StartCoroutine(TestInit());
    }

    IEnumerator TestInit() 
    {
        yield return null;
        player = GameManager.Player_;
        //GetItem_For_Test();
        SetSkillData_For_Test();
    }

    private void SetSkillData_For_Test()
    {
        SkillBox skillBox = FindObjectOfType<SkillBox>();
        for (int  i = 0; i < 5; i++)
        {
            SkillData skillData = skillBox.transform.GetChild(i + 2).GetComponent<SkillData>();
            GameManager.QuickSlot_Manager.QuickSlots[i].SkillData = skillData;
        }
    }

    void SetDropTable()
    {
        if (prefabs.Length != Enum.GetValues(typeof(ItemCode)).Length)
        {
            Debug.LogError("Enum �� ������ �����չ迭�� ������ �ٸ��ϴ�.");
            return;
        }
        // Make sure the prefabs array and the PrefabName enum have the same length
        if (prefabs.Length != Enum.GetValues(typeof(ItemCode)).Length)
        {
            Debug.LogError("The prefabs array and the PrefabName enum do not match!");
            return;
        }

        // Initialize the prefab dictionary
        for (int i = 0; i < prefabs.Length; i++)
        {
            prefabDict.Add((ItemCode)i , prefabs[i]);
        }

        // Initialize the enemy drop table
        enemyDropTable.Add(Monster_Type.Size_S, new List<(ItemCode, float)>
            {
                (ItemCode.SecretPotion, 1.0f),
                //(ItemCode.Cash, 0.1f),
                //(ItemCode.Enhancable_shotGun, 0.9f),
                //(ItemCode.Enhancable_Rifle, 0.99f),
                
            });

        enemyDropTable.Add(Monster_Type.Size_M, new List<(ItemCode, float)>
            {
                (ItemCode.Enhancable_shotGun, 1.0f),
                //(ItemCode.Enhancable_Bow, 0.9f),
                //(ItemCode.Purple_Crystal, 0.01f)
            });
        enemyDropTable.Add(Monster_Type.Size_L, new List<(ItemCode, float)>
            {
                (ItemCode.Enhancable_Rifle, 1.0f),
                //(ItemCode.Enhancable_Bow, 0.9f),
                //(ItemCode.Purple_Crystal, 0.01f)
            });
        enemyDropTable.Add(Monster_Type.Boss, new List<(ItemCode, float)>
            {
                (ItemCode.Enhancable_Bow, 0.9f),
                (ItemCode.Purple_Crystal, 0.01f)
            });
    }
    public void SpawnItem(BattleMapEnemyBase enemy)//ū �������� �з��� �ƴ϶� ��Ȯ�� � ������ �˾ƾ��Ѵ�
    {
        List<(ItemCode, float)> dropTable = enemyDropTable[enemy.EnemyData.mType];

        foreach (var (itemtype, droprate)in dropTable)
        {
            if (UnityEngine.Random.value <= droprate)
            {
                ItemObject ItemObj = Instantiate(prefabDict[itemtype], enemy.transform.position, Quaternion.identity).GetComponent<ItemObject>();
                ItemObj.itemData = GameManager.Itemdata.itemDatas[(int)itemtype];
                enemy.currentTile.ExistType = Tile.TileExistType.Item;

                ///��Ʋ�ʿ� ������ ǥ�ÿ����� �ʿ��� ������ ����
                ItemObj.CurrentTile = enemy.currentTile;
                SpaceSurvival_GameManager.Instance.ItemTileList.Add(enemy.currentTile);
            }
        }
    }
 
    public void GetItem()
    {
        GameManager.SlotManager.AddItem(itemCode);
    }
   
    public void ClearSlot()
    {
    }
    public void GetItem_For_Test()
    {
        int i = 0;
        while(i < 50)
        {
            GameManager.SlotManager.AddItem(ItemCode.HpPotion);
            GameManager.SlotManager.AddItem(ItemCode.MpPotion);
            i++;
        }
        GameManager.SlotManager.AddItem(ItemCode.Enhancable_shotGun);
        GameManager.SlotManager.AddItem(ItemCode.Enhancable_Pistol);
        GameManager.SlotManager.AddItem(ItemCode.Enhancable_Rifle);
        GameManager.SlotManager.AddItem(ItemCode.Captains_Hat);
        GameManager.SlotManager.AddItem(ItemCode.Crews_Hat);
        GameManager.SlotManager.AddItem(ItemCode.Big_Space_Armor);
        GameManager.SlotManager.AddItem(ItemCode.Space_Armor);
        GameManager.SlotManager.AddItem(ItemCode.Intermidiate_Green_Crystal);
        GameManager.SlotManager.AddItem(ItemCode.Advanced_Red_Crystal);
        GameManager.SlotManager.AddItem(ItemCode.Purple_Crystal);
        GameManager.SlotManager.AddItem(ItemCode.Pink_Crystal);
        GameManager.SlotManager.AddItem(ItemCode.Red_Crystal);
        GameManager.SlotManager.AddItem(ItemCode.Red_Crystal);
    }
    public void GetItemH()
    {
        GameManager.SlotManager.AddItem(ItemCode.HpPotion);
    }
    public void RemoveItem()
    {
        ItemData data = GameManager.Itemdata[itemCode];
        GameManager.SlotManager.RemoveItem(data, index);
    }
    public void GetItemMpPotion()
    {
        GameManager.SlotManager.AddItem(ItemCode.MpPotion);
    }
    public void SlotSorting()
    {
       // ItemData data = GameManager.Itemdata[itemCode];
        GameManager.SlotManager.SlotSorting(ItemSortBy.Price, false);
    }
    //protected override void TestClick(InputAction.CallbackContext context)
    //{
    //    if (GameManager.SlotManager.IsSlotMoving)
    //    {
    //        RectTransform inventoryRectTransform = GameManager.Inventory.GetComponent<RectTransform>();
    //        Vector2 localMousePosition;
    //        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryRectTransform, Input.mousePosition, null, out localMousePosition))
    //        {
    //            if (!inventoryRectTransform.rect.Contains(localMousePosition))
    //            {
    //               // GameManager.SlotManager.DropItem();
    //            }
    //        }
    //    }
    //}

    public void SpawnItemPrefab()
    {
        ItemFactory.MakeItem(itemCode);
    }
    public bool IsCritical;
    protected override void Test1(InputAction.CallbackContext _)
    {
       // GameManager.Player_.Defence(UnityEngine.Random.Range(100, 200), IsCritical);
        GameManager.SlotManager.AddItem(itemCode);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
       // GameManager.SlotManager.Clear_Inventory();
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
    }
    protected  void OpenInven(InputAction.CallbackContext _)
    {

    }

    protected override void Test4(InputAction.CallbackContext context)
    {
       // GameManager.SlotManager.AddItem(ItemCode.SecretPotion);
       // GameManager.SlotManager.AddItem(ItemCode.SpellBook);
    }
    Player_ player;
    protected override void Test5(InputAction.CallbackContext context)
    {
       // GameManager.SlotManager.Inven_Clear();

    }
    protected override void Test6(InputAction.CallbackContext context)
    {
    }
    //protected override void Test7(InputAction.CallbackContext context)
    //{
    //    ClearSlot();
    //}
    //protected override void Test8(InputAction.CallbackContext context)
    //{
    //    ClearInventory();
    //}
    //protected override void Test9(InputAction.CallbackContext context)
    //{
    //    SlotSorting();
    //}
}
