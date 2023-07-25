using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player;

    ItemDataManager itemDataManager;
    public ItemDataManager ItemData => itemDataManager;
    InventoryUI inventoryUI;
    public InventoryUI InvenUI => inventoryUI;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        itemDataManager = GetComponent<ItemDataManager>();
        inventoryUI = FindObjectOfType<InventoryUI>();
    }
    protected override void OnInitialize()
    {
        base.OnInitialize();
        player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        ItemData data = GameManager.Inst.ItemData[ItemCode.Ruby];
        Inventory inven = new Inventory(player);
        InvenSlot slot = inven[0];
    }
}
