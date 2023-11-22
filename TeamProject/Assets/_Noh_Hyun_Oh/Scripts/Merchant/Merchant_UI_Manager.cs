using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class Merchant_UI_Manager : PopupWindowBase, IPopupSortWindow
{
   
    /// <summary>
    /// ��ũ������ ������ �ؽ�Ʈ
    /// </summary>
    TextMeshProUGUI darkForceText;

    /// <summary>
    /// ���� �� ������ �ؽ�Ʈ 
    /// </summary>
    TextMeshProUGUI coinText;

    /// <summary>
    /// ���� ��� ��ư 
    /// </summary>
    Button buyButton; 

    /// <summary>
    /// �Ǹ� ��ǰ ��ư
    /// </summary>
    Button sellButton;



    /// <summary>
    /// ��� ��ư
    /// </summary>
    Button equipButton;

    /// <summary>
    /// �Һ� ��ư
    /// </summary>
    Button consumeButton;

    /// <summary>
    /// ��Ÿ ��ư 
    /// </summary>
    Button etcButton;

    /// <summary>
    /// ���� ��ư
    /// </summary>
    Button craftButton;


    /// <summary>
    /// �Ǹ� ��� �� �������� �� ������ ��ġ 
    /// </summary>
    Transform content;

    /// <summary>
    /// ������ �����þ�� �θ������� ������ �ø������� �����´�
    /// </summary>
    RectTransform contentRect;

    /// <summary>
    /// ������ ������ �⺻ ���� �����صα� 
    /// </summary>
    float defaultItemSizeY;


    ///// <summary>
    ///// ���Ǿ� ��ó�ΰ��� ������ ��ư
    ///// </summary>
    //Button NpcTalk;




    /// <summary>
    /// ��ɺи��� ������ ����ִ� �Ŵ���
    /// </summary>
    Merchant_Manager merchant_Manager;

    /// <summary>
    /// Ȯ�� ó���� �˾�â
    /// </summary>
    MerchantModalPopup popup;

    public Action<IPopupSortWindow> PopupSorting { get; set; }


    NpcTalkController talkController;

    CanvasGroup cg;
    protected override void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 1.0f;

        base.Awake();
        
        merchant_Manager = GetComponent<Merchant_Manager>();

        talkController = FindObjectOfType<NpcTalkController>();

        popup = transform.parent.GetComponentInChildren<MerchantModalPopup>(true);

        buyButton = topPanel.GetChild(1).GetChild(0).GetComponent<Button>(); 
        buyButton.onClick.AddListener(() => {
            //Debug.Log(buyButton);
            merchant_Manager.Selected = Merchant_Selected.Buy;
            ReFresh_Merchant_Item();
        });
        
        sellButton = topPanel.GetChild(1).GetChild(1).GetComponent<Button>(); 
        sellButton.onClick.AddListener(() => { 
            //Debug.Log(sellButton);
            merchant_Manager.Selected = Merchant_Selected.Sell;
            ReFresh_Merchant_Item();
        });

        coinText = topPanel.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
      
        darkForceText = topPanel.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();



        equipButton = contentPanel.GetChild(0).GetChild(0).GetComponent<Button>();
        equipButton.onClick.AddListener(() => {
            merchant_Manager.Merchant_State = Inventory_Tab.Equip;
            ReFresh_Merchant_Item();
        });

        consumeButton = contentPanel.GetChild(0).GetChild(1).GetComponent<Button>();
        consumeButton.onClick.AddListener(() => { 
            merchant_Manager.Merchant_State = Inventory_Tab.Consume;
            ReFresh_Merchant_Item();
        });

        etcButton = contentPanel.GetChild(0).GetChild(2).GetComponent<Button>();
        etcButton.onClick.AddListener(() => { 
            merchant_Manager.Merchant_State = Inventory_Tab.Etc;
            ReFresh_Merchant_Item();
        });

        craftButton = contentPanel.GetChild(0).GetChild(3).GetComponent<Button>();
        craftButton.onClick.AddListener(() => { 
            merchant_Manager.Merchant_State = Inventory_Tab.Craft;
            ReFresh_Merchant_Item();
        });

        content = contentPanel.GetComponentInChildren<VerticalLayoutGroup>().transform;
        contentRect = content.transform.GetComponent<RectTransform>();


        //�ּ� �Ѱ��־�ΰ� �̸� ������صд� 
        defaultItemSizeY = content.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;

    }
    private void Start()
    {
        CloseWindow();
        GameManager.PlayerStatus.Base_Status.on_DarkForceChange += (value) => { ResourceSetting(0, value); };
        InputSystemController.InputSystem.Common.Esc.performed += (_) => {
            CloseWindow();
        };
    }



    public bool IsSelect() 
    {
        bool result = false;


        return result;
    }

    private void OnDisable()
    {
        popup.OnClose();
    }


    /// <summary>
    /// �����Ͱ����ؼ� ����UI ��ü�� ���÷������ִ� �Լ�
    /// </summary>
    public void ReFresh_Merchant_Item() 
    {
        Merchant_UI_Item[] merchant_UI_Items = null;
        if (merchant_Manager.Selected == Merchant_Selected.Sell)
        {
            List<Slot> invenSlots;
            InitUI();

            switch (merchant_Manager.Merchant_State) //���� ���¿� ���� ������ �������� 
            {
                case Inventory_Tab.None:
                    return;
                case Inventory_Tab.Equip:
                    invenSlots = GameManager.SlotManager.slots[merchant_Manager.Merchant_State];
                    break;
                case Inventory_Tab.Consume:
                    invenSlots = GameManager.SlotManager.slots[merchant_Manager.Merchant_State];
                    break;
                case Inventory_Tab.Etc:
                    invenSlots = GameManager.SlotManager.slots[merchant_Manager.Merchant_State];
                    break;
                case Inventory_Tab.Craft:
                    invenSlots = GameManager.SlotManager.slots[merchant_Manager.Merchant_State];
                    break;
                default:
                    return;
            }

            List<Slot> merchantSettingData = new List<Slot>(invenSlots.Count);
            foreach (Slot itemSlot in invenSlots)
            {
                if (itemSlot.ItemData != null) //���Կ� �����Ͱ� ��������� �����´� 
                {
                    merchantSettingData.Add(itemSlot);
                }
            }

            UI_MerchantItemSetting(merchantSettingData.Count);

            merchant_UI_Items = content.GetComponentsInChildren<Merchant_UI_Item>();

            int itemIndex = 0;
            foreach (Slot itemSlot in merchantSettingData)
            {
                merchant_UI_Items[itemIndex].InitData(itemSlot.ItemData, itemSlot.ItemCount,itemSlot);
                merchant_UI_Items[itemIndex].onItemClick = popup.OnPopup;// merchant_Manager.ItemClick;
                merchant_UI_Items[itemIndex].gameObject.SetActive(true);
                itemIndex++;
            }
        }
        else if (merchant_Manager.Selected == Merchant_Selected.Buy)
        {
            InitUI();
            List<ItemData> merchantList;
            int[] merchantCountArray;
            switch (merchant_Manager.Merchant_State) //���� ���¿� ���� ������ �������� 
            {
                case Inventory_Tab.None:
                    return;
                case Inventory_Tab.Equip:
                    merchantList = merchant_Manager.EquipItems;
                    merchantCountArray = merchant_Manager.EquipItemsCount;
                    break;
                case Inventory_Tab.Consume:
                    merchantList = merchant_Manager.ConsumeItems;
                    merchantCountArray = merchant_Manager.ConsumeItemsCount;
                    break;
                case Inventory_Tab.Etc:
                    merchantList = merchant_Manager.EtcItems;
                    merchantCountArray = merchant_Manager.EtcItemsCount;
                    break;
                case Inventory_Tab.Craft:
                    merchantList = merchant_Manager.CraftItems;
                    merchantCountArray = merchant_Manager.CraftItemsCount;
                    break;
                default:
                    return;
            }
            UI_MerchantItemSetting(merchantList.Count);

            merchant_UI_Items = content.GetComponentsInChildren<Merchant_UI_Item>();

            int itemIndex = 0;
            foreach (ItemData itemSlot in merchantList)
            {
                merchant_UI_Items[itemIndex].InitData(itemSlot,(uint)merchantCountArray[itemIndex]);
                merchant_UI_Items[itemIndex].onItemClick = popup.OnPopup;//merchant_Manager.ItemClick;
                merchant_UI_Items[itemIndex].gameObject.SetActive(true);
                itemIndex++;
            }
        }
        else 
        {
            return;
        }
        //�θ� ������ ������ ���� 
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, defaultItemSizeY * merchant_UI_Items.Length);
        
        ResourceSetting(0, GameManager.PlayerStatus.DarkForce);
    }
    /// <summary>
    /// �ڿ� ǥ�����ִ��Լ�
    /// </summary>
    /// <param name="coin">������</param>
    /// <param name="darkForce">��ũ����</param>
    private void ResourceSetting(float coin, float darkForce) 
    {
        coinText.text = $"{coin} G";
        darkForceText.text = $"{darkForce} Force";
    }
    /// <summary>
    /// �ʿ��� ������ŭ ������Ʈ Ǯ���� �������� �Լ�
    /// �����Ҷ� Ǯ�� ���� �������־ üũ�ϴ� �κ��� �ʿ䰡�����Ű����ϴ�..
    /// </summary>
    /// <param name="checkCount">������ ����</param>
    private void UI_MerchantItemSetting(int checkCount) 
    {
        if (checkCount > content.childCount)
        {
            Merchant_UI_Item item; 
            int forSize = checkCount - content.childCount;
            for (int i = 0; i < forSize; i++)
            {
                item = (Merchant_UI_Item)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.MERCHANT_iTEM_POLL);
                item.transform.SetParent(content);
            }
        }
    }

    /// <summary>
    /// ���� UI ������ ���½�Ű��
    /// </summary>
    private void InitUI() 
    {
        Merchant_UI_Item[] merchant_UI_Items = content.GetComponentsInChildren<Merchant_UI_Item>(); //

        foreach (Merchant_UI_Item slotItem in merchant_UI_Items)
        {
            slotItem.ResetData();
        }
    }

    private void ResetData() 
    {
        Merchant_UI_Item[] merchant_UI_Items = content.GetComponentsInChildren<Merchant_UI_Item>();
        foreach (Merchant_UI_Item item in merchant_UI_Items)
        {
            item.ResetData();
        }
        merchant_Manager.NpcTalkController.IsTalking = false;
        if (merchant_Manager.ActionUI != null && merchant_Manager.IsActionActive)
        {
            merchant_Manager.ActionUI.visibleUI();
        }
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        PopupSorting(this);
    }

    public void OpenWindow()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        ReFresh_Merchant_Item();
    }

    public void CloseWindow()
    {
        ResetData();
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }
    protected override void OnCloseButtonClick()
    {
        talkController.ResetData();
    }

}
