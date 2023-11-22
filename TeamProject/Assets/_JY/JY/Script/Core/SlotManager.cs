using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour // invenSlot,invenSlotUI, SlotUIBase = Slot,     Inventory, InventoryUI = SlotManager
{
    //InputKeyMouse input; 인풋시스템 통합중

    public GameObject slot;
    Slot just_ChangeSlot;
    public Slot Just_ChangeSlot => just_ChangeSlot;
    TempSlot tempSlot;
    ItemDescription itemDescription;
    public ItemDescription ItemDescription => itemDescription;
    RectTransform inventoryRectTransform;

    public TempSlot TempSlot => tempSlot;

    Inventory inven;
    Transform equip_Below;
    Transform consume_Below;
    Transform etc_Below;
    Transform craft_Below;

    QuickSlot_Manager quickSlot_Manager;

    
    RectTransform equipboxRectTransform;
    RectTransform beforeSlotRectTransform;
    RectTransform enhancerUIRectTransform;
    RectTransform mixer_Left_slot_Transform;
    RectTransform mixer_Middle_Slot_Transform;
    RectTransform mixerUI_Transform;
    Item_Mixer_UI mixer_UI;
    EquipBox equipBox;

    ItemSplitter spliter;
    bool isShiftPress = false;

    public Action<ItemData> on_UnEquip_Item;// 장비 해제, 장비창 슬롯을 더블클릭 했을 때 호출
    public Action<ItemData_Potion, uint> onDetectQuickSlot;

    public Dictionary<Inventory_Tab, List<Slot>> slots;
    private Dictionary<Inventory_Tab, int> slotCount; //슬롯 생성후 번호를 부여하기위한 Dic
    private Dictionary<ItemData_Potion, bool> quickSlot_Binding_Table;
    List<Slot> tempList_For_QuickSlot = new();
 

    public byte Index_JustChange_Slot { get; set; }
    private void Awake()
    {
   

        //input = new InputKeyMouse(); //통합중
        tempSlot = FindObjectOfType<TempSlot>(true);
        just_ChangeSlot = tempSlot;
        itemDescription = FindObjectOfType<ItemDescription>();
        spliter = FindObjectOfType<ItemSplitter>(true);
    }
    public void Inven_Clear()
    {
        
        Transform parentTransform = GetParentTransform();
        foreach (List<Slot> slotlist in slots.Values)
        {
            slotlist.Clear();
        }
    }
    private void Start()
    {
        InputSystemController.Instance.OnUI_Inven_Click_Cancel += OnItemDrop;
        InputSystemController.Instance.OnUI_Inven_Shift += OnShiftPress;
        //GameManager.playerDummy.onUnEquipItem += UnEquip_Item;
    }
    private void OnEnable()
    {
        //통합중
        //input.UI_Inven.Enable();
        //input.UI_Inven.Click.canceled += OnItemDrop;
        //input.UI_Inven.Shift.performed += OnShiftPress;
        //input.UI_Inven.Shift.canceled += OnShiftPress;
    }
    //private void OnDisable()
    //{
    //    input.UI_Inven.Click.canceled -= OnItemDrop;
    //    input.UI_Inven.Shift.performed -= OnShiftPress;
    //    input.UI_Inven.Shift.canceled -= OnShiftPress;
    //    input.UI_Inven.Disable();
    //}
    public void Initialize()//Inventory에서 Start타이밍에 호출
    {
        equipBox = GameManager.EquipBox;
        equipboxRectTransform = equipBox.GetComponent<RectTransform>();

        mixer_Left_slot_Transform = GameManager.Mixer.MixerUI.Left_Slot.GetComponent<RectTransform>();
        mixer_Middle_Slot_Transform = GameManager.Mixer.MixerUI.Middle_Slot.GetComponent<RectTransform>();
        mixerUI_Transform = GameManager.Mixer.GetComponent<RectTransform>();
        mixer_UI = GameManager.Mixer.MixerUI;
        quickSlot_Manager = GameManager.QuickSlot_Manager;
        beforeSlotRectTransform = GameManager.Enhancer.EnhancerUI.BeforeSlot.GetComponent<RectTransform>();
        enhancerUIRectTransform = GameManager.Enhancer.EnhancerUI.AfterSlot.GetComponent<RectTransform>();

        foreach (QuickSlot quickSlot in quickSlot_Manager.QuickSlots)
        {
            quickSlot.onSet_ItemData += Binding_Slots; 
        }

        inven = GameManager.Inventory;
        equip_Below = inven.transform.GetChild(0).GetChild(0).GetChild(0);
        consume_Below = inven.transform.GetChild(1).GetChild(0).GetChild(0);
        etc_Below = inven.transform.GetChild(2).GetChild(0).GetChild(0);
        craft_Below = inven.transform.GetChild(3).GetChild(0).GetChild(0);
        inventoryRectTransform = inven.GetComponent<RectTransform>();

        itemDescription.Close();
        TempSlot.InitializeSlot(TempSlot);
        TempSlot.onTempSlotOpenClose += OnDetailPause; // TempSlot이 Open할때 true로 호출하고 Close할때 false로 호출
        spliter.onCancel += () => itemDescription.IsPause = false;   // 캔슬버턴 누르면 상세정보창 일시정지 해제
        spliter.Close();
        spliter.onOkClick += OnSpliterOk;

        


        mixer_UI.onEndSession_Success += Add_Reward_Item;

        slots = new Dictionary<Inventory_Tab, List<Slot>>
        {
            { Inventory_Tab.Equip, new List<Slot>() },
            { Inventory_Tab.Consume, new List<Slot>() },
            { Inventory_Tab.Etc, new List<Slot>() },
            { Inventory_Tab.Craft, new List<Slot>() }
        };
        slotCount = new Dictionary<Inventory_Tab, int> // 슬롯 오브젝트에 번호를 부여하기 위한 Dic
        {
            { Inventory_Tab.Equip, 0 },
            { Inventory_Tab.Consume, 0},
            { Inventory_Tab.Etc, 0},
            { Inventory_Tab.Craft, 0}
        };

        SlotInit();

    }
    /// <summary>
    /// 슬롯의 기본갯수를 셋팅한다 
    /// </summary>
    public void SlotInit() 
    {
        //슬롯갯수  초기화 시킨다
        GameManager.Inventory.State = Inventory_Tab.Equip;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Make_Slot();
            }
            switch (GameManager.Inventory.State)
            {
                case Inventory_Tab.Equip:
                    GameManager.Inventory.State = Inventory_Tab.Consume;
                    break;
                case Inventory_Tab.Consume:
                    GameManager.Inventory.State = Inventory_Tab.Etc;
                    break;
                case Inventory_Tab.Etc:
                    GameManager.Inventory.State = Inventory_Tab.Craft;
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void SaveFileLoadedResetSlots()
    {
        Slot[] findSlots = FindObjectsOfType<Slot>(true); //기존슬롯 싹다찾아서 
        foreach (Slot slot in findSlots)
        {
            if (slot != tempSlot) //템프슬롯빼고  
            {
                
                Destroy(slot.gameObject);//날려버리기 
            }
        }
        //해시테이블(딕셔너리)초기화시키고
        slots = new Dictionary<Inventory_Tab, List<Slot>>
        {
            { Inventory_Tab.Equip, new List<Slot>() },
            { Inventory_Tab.Consume, new List<Slot>() },
            { Inventory_Tab.Etc, new List<Slot>() },
            { Inventory_Tab.Craft, new List<Slot>() }
        };
        //카운트도 초기화
        slotCount = new Dictionary<Inventory_Tab, int> // 슬롯 오브젝트에 번호를 부여하기 위한 Dic
        {
            { Inventory_Tab.Equip, 0 },
            { Inventory_Tab.Consume, 0},
            { Inventory_Tab.Etc, 0},
            { Inventory_Tab.Craft, 0}
        };
        SlotInit(); //기본 슬롯갯수 셋팅
    }
    public void Make_Slot()
    {
        GameObject newSlot = Instantiate(slot);
        Slot slotComp = newSlot.GetComponent<Slot>();
        Transform parentTransform = GetParentTransform();
        if (parentTransform != null)
        {
            slotCount[GameManager.Inventory.State]++;
            newSlot.name = $"{GameManager.Inventory.State}_{slotCount[GameManager.Inventory.State]}";           
            newSlot.transform.SetParent(parentTransform.transform, true);
            slots[GameManager.Inventory.State].Add(slotComp);

            
            slotComp.InitializeSlot(slotComp);
            if (GameManager.Inventory.State == Inventory_Tab.Consume)
            {
                slotComp.onItemDataChange += BindingCheck;
            }
            slotComp.onDragBegin += OnItemMoveBegin;
            slotComp.onDragEnd += OnItemMoveEnd;
            slotComp.onClick += OnSlotClick;
            slotComp.onPointerEnter += OnItemDetailOn;
            slotComp.onPointerExit += OnItemDetailOff;
            slotComp.onPointerMove += OnSlotPointerMove;
            slotComp.Index = (uint)slots[GameManager.Inventory.State].Count - 1;
            slotComp.onSet_Just_ChangeSlot += (slot) => just_ChangeSlot = slot;

        }
    }

    /// <summary>
    /// 인벤토리 4개의 탭을 순회하며 모든 슬롯의 델리게이트를 해제, ItemData를 null로 셋팅하고
    /// slot의 갯수가 10개를 초과하는 탭을 초기 시작갯수인 10개로 되돌리는 함수
    /// </summary>
    public void Clear_Inventory()
    {
        int baseCount = 10;
        foreach(List<Slot>slotList in slots.Values)
        {
            for (int i = slotList.Count - 1; i >= baseCount; i--)//각 탭마다 슬롯의 갯수를 10개만 남기고 모두 삭제
            {
                Slot slot = slotList[i];
                SetNull_Slot_Delegate_ItemData(slot);
                Destroy(slotList[i].gameObject);
                slotList.RemoveAt(i);
            }
            for (int i = 0; i < slotList.Count; i++)//남은 10개 슬롯에 대한 처리
            {
                slotList[i].ItemCount = 0;
                slotList[i].ItemData = null;
                if (slotList[i].BindingSlot != null)
                {
                    slotList[i].BindingSlot = null;
                }
            }

        }
    }

    private void SetNull_Slot_Delegate_ItemData(Slot slot)
    {
        slot.onClick = null;
        slot.onDragBegin = null;
        slot.onDragEnd = null;
        slot.onPointerEnter = null;
        slot.onPointerExit = null;
        slot.onPointerMove = null;
        slot.onItemCountChange = null;
        slot.onItemDataChange = null;
        slot.onSet_Just_ChangeSlot = null;
        slot.ItemData = null;
        if (slot.BindingSlot != null)
        {
            slot.BindingSlot = null;
        }
    }

    /// <summary>
    /// 불러올때 사용하기위해 추가 
    /// </summary>
    /// <param name="invenTab"></param>
    public void Make_Slot(Inventory_Tab invenTab)
    {
        GameObject newSlot = Instantiate(slot);
        Slot slotComp = newSlot.GetComponent<Slot>();
        Transform parentTransform = GetParentTransform(invenTab);
        if (parentTransform != null)
        {
            slotCount[invenTab]++;
            newSlot.name = $"{invenTab}_{slotCount[invenTab]}";
            newSlot.transform.SetParent(parentTransform.transform, true);
            slots[invenTab].Add(slotComp);


            slotComp.InitializeSlot(slotComp);
            if (invenTab == Inventory_Tab.Consume)
            {
                slotComp.onItemDataChange += BindingCheck;
            }
            slotComp.onDragBegin += OnItemMoveBegin;
            slotComp.onDragEnd += OnItemMoveEnd;
            slotComp.onClick += OnSlotClick;
            slotComp.onPointerEnter += OnItemDetailOn;
            slotComp.onPointerExit += OnItemDetailOff;
            slotComp.onPointerMove += OnSlotPointerMove;
            slotComp.Index = (uint)slots[invenTab].Count - 1;
            slotComp.onSet_Just_ChangeSlot += (slot) => just_ChangeSlot = slot;
        }
    }
    private void OnItemMoveBegin(ItemData data, uint index)
    {
        Index_JustChange_Slot = (byte)index;
        MoveItem(data ,index, tempSlot.Index);    // 시작 슬롯에서 임시 슬롯으로 아이템 옮기기
        TempSlot.Open();                          // 임시 슬롯 열기
    }
    private void OnItemMoveEnd(ItemData data, uint index, bool isSuccess)
    {
        MoveItem(data, tempSlot.Index, index);    // 임시 슬롯에서 도착 슬롯으로 아이템 옮기기
        if (tempSlot.IsEmpty)          // 비었다면(같은 종류의 아이템일 때 일부만 들어가는 경우가 있을 수 있으므로)
        {
            TempSlot.Close();                     // 임시 슬롯 닫기
        }

        if (isSuccess)
        {
            List<Slot> slots = GetItemTab();
            itemDescription.Open(slots[(int)index].ItemData);     // 드래그가 성공적으로 끝났으면 상세 정보창도 열기
        }
    }
    private void OnSlotClick(ItemData data, uint index)
    {
        ItemData_Potion potion = data as ItemData_Potion;
        if (isShiftPress)
        {
            if (tempSlot.IsEmpty)
            {
                OnSpliterOpen(index);
            }
        }
        else if (potion != null)
        {
            potion.Consume(GameManager.Player_);
            RemoveItem(data, index);
        }
        else
        {
            // 임시 슬롯에 아이템이 있을 때 클릭이 되었으면
            OnItemMoveEnd(data, index, true); // 클릭된 슬롯으로 아이템 이동
        }
       
    }
    private void OnItemDetailOn( ItemData data, uint index)
    {
        List<Slot> slots = GetItemTab(data); //빈슬롯 위에 Pointer Enter시 data가 null 이되서 리스트를 가져올때 터짐
        itemDescription.Open(slots[(int)index].ItemData); // 상세정보창 열기ㅐ
    }
    private void OnItemDetailOff(uint index)
    {
        itemDescription.Close(); // 상세정보창 닫기
    }
    private void OnSlotPointerMove(Vector2 screenPos)//마우스 움직일때마다 호출
    {
        itemDescription.MovePosition(screenPos);
    }
    private void OnDetailPause(bool isPause)
    {
        itemDescription.IsPause = isPause;
    }
    /// <summary>
    /// 아이템 분리창을 여는 함수
    /// </summary>
    /// <param name="index">아이템을 분리할 슬롯의 인덱스</param>
    private void OnSpliterOpen(uint index)
    {
        List<Slot> slots = GetItemTab();
        Slot target = slots[(int)index];
        spliter.transform.position = target.transform.position + Vector3.up * 100;
        spliter.Open(target);
        itemDescription.IsPause = true;
    }

    /// <summary>
    /// 아이템 분리창에서 OK 버튼이 눌러졌을 때 실행될 함수
    /// </summary>
    /// <param name="index">아이템이 분리될 슬롯</param>
    /// <param name="count">분리된 개수</param>
    private void OnSpliterOk(uint index, uint count)
    {
        SplitItem(index, count);
        TempSlot.Open();
    }

    /// <summary>
    /// 쉬프트키가 눌려지거나 때졌을 때 실행될 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnShiftPress(InputAction.CallbackContext context)
    {
        isShiftPress = !context.canceled;   // 쉬프트키 상황 기록
    }


    /// <summary>
    /// 마우스 클릭이 떨어졌을 때 실행되는 함수(아이템 드랍용)
    /// </summary>
    private void OnItemDrop()
    {
        if (tempSlot == null)
            return;
        if (!tempSlot.IsEmpty)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector2 distance_Between_Mouse_Inven = screenPos - (Vector2)inventoryRectTransform.position;//inventoryRectTransform.position의 피봇을 기준으로 떨어진거리 계산
            Vector2 distance_Between_Mouse_BeforeSlot = screenPos - (Vector2)beforeSlotRectTransform.position;
            Vector2 distance_Between_Mouse_enhancerUI = screenPos - (Vector2)enhancerUIRectTransform.position;
            Vector2 distance_Between_Mouse_Left_Slot = screenPos - (Vector2)mixer_Left_slot_Transform.position;
            Vector2 distance_Between_Mouse_Middle_Slot = screenPos - (Vector2)mixer_Middle_Slot_Transform.position;
            Vector2 distance_Between_Mouse_MixerUI = screenPos - (Vector2)mixerUI_Transform.position;
            Vector2 distance_Between_Mouse_QuickSlot_Box = screenPos - (Vector2)GameManager.QuickSlot_Manager.QuickSlotBox_RectTransform.position;
            Vector2 distance_Between_Mouse_EquipBox = screenPos - (Vector2)equipboxRectTransform.position;

            if (beforeSlotRectTransform.rect.Contains(distance_Between_Mouse_BeforeSlot) && GameManager.Enhancer.EnhancerUI.IsOpen)//강화 슬롯의 위치이면서 강화ㅑ 가능한 아이템 일 때
            {
                ItemData_Enhancable enhancable = TempSlot.ItemData as ItemData_Enhancable;
                if (enhancable != null)
                {
                    GameManager.Enhancer.ItemData = enhancable;
                }
            }
            else if (equipboxRectTransform.rect.Contains(distance_Between_Mouse_EquipBox) && equipBox.IsOpen)//장비창 범위 안에 드롭할 때
            {
                equipBox.Set_ItemData_For_Drag(tempSlot.ItemData);
            }
            else if (quickSlot_Manager.QuickSlotBox_RectTransform.rect.Contains(distance_Between_Mouse_QuickSlot_Box))//퀵슬롯박스 안쪽일 때
            {
                //스킬이나 포션만 등록
                ItemData_Potion potion = TempSlot.ItemData as ItemData_Potion;
                slots[Inventory_Tab.Consume][Index_JustChange_Slot].ItemData = potion;//생략시 slot의 ItemData가 null 이라 델리게이트 추가가 안됨
                if (potion != null)
                {
                    if (quickSlot_Manager.Find_Slot_By_Position(out QuickSlot targetSlot))
                    {
                        quickSlot_Manager.Set_ItemDataTo_QuickSlot(potion);
                        Throw_NewCount_To_QuickSlot(targetSlot, potion);
                        targetSlot.ItemCount += tempSlot.ItemCount;
                    }
                }
            }
            else if (mixer_Left_slot_Transform.rect.Contains(distance_Between_Mouse_Left_Slot) && mixer_UI.IsOpen)//조합창의 왼쪽슬롯
            {
                if (GameManager.Mixer.LeftSlotData == null)
                {
                    GameManager.Mixer.LeftSlotData = TempSlot.ItemData;
                    RemoveItem(TempSlot.ItemData, Index_JustChange_Slot);
                }
            }
            else if (mixer_Middle_Slot_Transform.rect.Contains(distance_Between_Mouse_Middle_Slot) && mixer_UI.IsOpen)
            {
                if (GameManager.Mixer.MiddleSlotData == null)
                {
                    GameManager.Mixer.MiddleSlotData = TempSlot.ItemData;
                    RemoveItem(TempSlot.ItemData, Index_JustChange_Slot);
                }
            }
            else if (mixerUI_Transform.rect.Contains(distance_Between_Mouse_MixerUI) && mixer_UI.IsOpen)
            {
                return;
            }
            else if (!inventoryRectTransform.rect.Contains(distance_Between_Mouse_Inven))// 거리의 크기가 rect 의 크기보다 작으면 인벤토리 안쪽
            {
                if (enhancerUIRectTransform.rect.Contains(distance_Between_Mouse_enhancerUI) && GameManager.Enhancer.EnhancerState == EnhancerState.Open)//inhancerUI열려있으면 return
                {
                    return;
                }
                // 인벤토리 영역 밖이면
                TempSlot.OnDrop(screenPos);
            }
        }
    }
    public void UnEquip_Item(ItemData itemData)// 더블클릭으로 해제 할 때
    {
        bool result = false;
        List<Slot> slotList = GetItemTab(itemData);
        foreach(Slot slot in slotList)
        {
            if (slot.IsEmpty)
            {
                on_UnEquip_Item?.Invoke(itemData);
                slot.ItemData = itemData;
                result = true;
                break;
            }
        }
        if (!result)
        {
            Debug.Log("인벤토리에 빈 슬롯이 없습니다.");
        }
    }
    public void Taking_Item_From_EquipBox(ItemData data)
    {
        just_ChangeSlot.ItemData = data;

    }
    void BindingCheck(ItemData itemData)
    {
        List<QuickSlot> quickSlots = quickSlot_Manager.QuickSlots.ToList();
        foreach (QuickSlot slot in quickSlots)
        {
            if (slot.ItemData == itemData)
            {
                Binding_Slots(itemData as ItemData_Potion, slot);
                return;
            }
        }
    }
    void Binding_Slots(ItemData_Potion itemData, QuickSlot targetSlot)//퀵슬롯에 ItemData가 셋팅 됐을떄 호출
    {
        //이 시점에선 linkedSlots이 이미 Clear된 상태라서 Inventory의 아이템을 비교해서 찾아야한다.
        foreach (Slot slot in slots[Inventory_Tab.Consume])//소비창 순회
        {
            if (slot.ItemData == itemData)//퀵슬롯에서 받은itemData와 같으면 델리게이트 초기화, 재 연결
            {
                slot.BindingSlot = targetSlot;
                slot.onItemCountChange = null;
                slot.onItemCountChange += Throw_NewCount_To_QuickSlot;//인벤토리 슬롯의 카운트가 변경될 때 퀵슬롯의 카운트를 변경하는 델리게이트
            }
        }
    }

    void Throw_NewCount_To_QuickSlot(QuickSlot targetSlot, ItemData itemData)//targetSlot = 호출하는 슬롯의 BindingSlot
    {
        uint newCount = GetTotalAmount(itemData);
        targetSlot.ItemCount = newCount;//참조를 받아왔기 때문에 바로 수정 가능
    }
    private uint GetTotalAmount(ItemData itemData)//같은 아이템을가진 슬롯들의 카운트를 모두 더해 리턴하는 함수
    {
        uint newCount = 0;
        List<Slot> consumeTab = slots[Inventory_Tab.Consume];
        foreach (Slot slot in consumeTab)
        {
            if (slot.ItemData == itemData)
            {
                newCount += slot.ItemCount;
            }
        }
        return newCount;
    }

    private Transform GetParentTransform()
    {
        Transform parentTransform;
        switch (GameManager.Inventory.State)
        {
            case Inventory_Tab.Equip:
                parentTransform = equip_Below;
                break;
            case Inventory_Tab.Consume:
                parentTransform = consume_Below;
                break;
            case Inventory_Tab.Etc:
                parentTransform = etc_Below;
                break;
            case Inventory_Tab.Craft:
                parentTransform = craft_Below;
                break;
            default:
                parentTransform = null;
                break;
        }
        return parentTransform;
    }
    /// <summary>
    /// 불러올 때 사용하기 위해 추가
    /// </summary>
    /// <param name="invenTab"></param>
    /// <returns></returns>
    private Transform GetParentTransform(Inventory_Tab invenTab)
    {
        Transform parentTransform;
        switch (invenTab)
        {
            case Inventory_Tab.Equip:
                parentTransform = equip_Below;
                break;
            case Inventory_Tab.Consume:
                parentTransform = consume_Below;
                break;
            case Inventory_Tab.Etc:
                parentTransform = etc_Below;
                break;
            case Inventory_Tab.Craft:
                parentTransform = craft_Below;
                break;
            default:
                parentTransform = null;
                break;
        }
        return parentTransform;
    }

    void Add_Reward_Item(ItemData item)//조합성공시 아이템 추가
    {
        AddItem(item.code);
    }
    public bool AddItem(ItemCode code)
    {
        bool result = false;
        ItemData data = GameManager.Itemdata[code];
        if (data.ItemType == ItemType.Etc)
        {
          //  data.AddComponent<IQuest_Item>();
        }

        Slot sameDataSlot = FindSameItem(data);
        if (sameDataSlot != null && sameDataSlot.ItemCount < sameDataSlot.ItemData.maxStackCount)
        {
            // 같은 종류의 아이템이 있다.
            // 아이템 개수 1 증가시키기고 결과 받기
            result = sameDataSlot.IncreaseSlotItem(out _);  // 넘치는 개수가 의미 없어서 따로 받지 않음
        }
        else
        {
            // 같은 종류의 아이템이 없다.
            Slot emptySlot = FindEmptySlot(data);
            if (emptySlot != null)
            {
                emptySlot.AssignSlotItem(data); // 빈슬롯이 있으면 아이템 하나 할당
                result = true;
            }
            else
            {
                // 비어있는 슬롯이 없다.
                //Debug.Log("아이템 추가 실패 : 인벤토리가 가득 차있습니다.");
            }
        }

        return result;
    }
    public bool AddItem(ItemData_Enhancable data)
    {
        bool result = false;
        // 같은 종류의 아이템이 없다.
        Slot emptySlot = FindEmptySlot(data);
        if (emptySlot != null)
        {
            emptySlot.AssignSlotItem(data); // 빈슬롯이 있으면 아이템 하나 할당
            result = true;
        }
        else
        {
            // 비어있는 슬롯이 없다.
            //Debug.Log("아이템 추가 실패 : 인벤토리가 가득 차있습니다.");
        }
        return result;
    }
    public void RemoveItem(ItemData data, uint slotIndex, uint decreaseCount = 1)
    {
        List<Slot> slots = GetItemTab(data);
        if (IsValidIndex(slotIndex, data))
        {
            Slot invenSlot = slots[(int)slotIndex];
            invenSlot.DecreaseSlotItem(decreaseCount);
        }
        else
        {
            //Debug.Log($"아이템 감소 실패 : {slotIndex}는 없는 인덱스입니다.");
        }
    }
  
    //public void ClearSlot(ItemData data, uint slotIndex)
    //{
    //    List<Slot> slots = GetItemTab(data);
    //    if (IsValidIndex(slotIndex, data))
    //    {
    //        Slot invenSlot = slots[(int)slotIndex];
    //        invenSlot.ClearSlotItem();
    //    }
    //    else
    //    {
    //        //Debug.Log($"아이템 삭제 실패 : {slotIndex}는 없는 인덱스입니다.");
    //    }
    //}

    public void MoveItem(ItemData data, uint from, uint to)
    {
        if (data == null)
        {
            data = TempSlot.ItemData;
        }
        List<Slot> slots = GetItemTab(data);
        // from지점과 to지점이 다르고 from과 to가 모두 valid해야 한다.
        if ((from != to) && IsValidIndex(from, data) && IsValidIndex(to, data))
        {
            Slot fromSlot = (from == tempSlot.Index) ? tempSlot : slots[(int)from];  // 임시 슬롯을 감안해서 삼항연산자로 처리
            if (!fromSlot.IsEmpty)
            {
                Slot toSlot = (to == tempSlot.Index) ? TempSlot : slots[(int)to];
                if (fromSlot.ItemData == toSlot.ItemData)  // 같은 종류의 아이템이면
                {
                    toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);    // 일단 from이 가진 개수만큼 to 감소 시도
                    fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);          // from에서 실제로 넘어간 숫자만큼 from 감소
                    //Debug.Log($"{from}번 슬롯에서 {to}번 슬롯으로 아이템 합치기");
                }
                else
                {
                    // 다른 종류의 아이템이면 서로 스왑
                    ItemData tempData = fromSlot.ItemData;
                    uint tempCount = fromSlot.ItemCount;
                    fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);// 이때 슬롯의 데이터가 null이 된다.
                    toSlot.AssignSlotItem(tempData, tempCount);
                    //Debug.Log($"{from}번 슬롯과 {to}번 슬롯의 아이템 교체");
                }
            }
        }
    }


    public void SplitItem(uint slotIndex, uint count) // 스플릿할때는 굳이 
    {
        if (IsValidIndex(slotIndex))
        {
            List<Slot> slots = GetItemTab();
            Slot slot = slots[(int)slotIndex];
            tempSlot.AssignSlotItem(slot.ItemData, count);  // 임시 슬롯에 할당하기
            slot.DecreaseSlotItem(count);                   // 슬롯에서 덜어내고
        }
    }

    /// <summary>
    /// 인벤토리를 정렬하는 함수 
    /// </summary>
    /// <param name="sortBy">정렬 기준</param>
    /// <param name="isAcending">true면 오름차순, false면 내림차순</param>
    public void SlotSorting(ItemSortBy sortBy, bool isAcending = true)
    {
        List<Slot> slots = GetItemTab();
        List<Slot> beforeSlots = new List<Slot>(slots);   // slots 배열을 이용해서 리스트 만들기

        switch (sortBy) // 정렬 기준에 따라 다르게 처리하기(Sort 함수의 파라메터로 들어갈 람다함수를 다르게 작성)
        {
            case ItemSortBy.Code:
                beforeSlots.Sort((x, y) =>  // x, y는 서로 비교할 2개(beforeSlots에 들어있는 2개)
                {
                    if (x.ItemData == null) // itemData는 비어있을 수 있으니 비어있으면 비어있는 것이 뒤쪽으로 설정
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.code.CompareTo(y.ItemData.code);  // enum이 가지는 CompareTo 함수로 비교(오름차순일 때)
                    }
                    else
                    {
                        return y.ItemData.code.CompareTo(x.ItemData.code);  // enum이 가지는 CompareTo 함수로 비교(내림차순일 때)
                    }
                });
                break;
            case ItemSortBy.Name:
                beforeSlots.Sort((x, y) =>
                {
                    if (x.ItemData == null)
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.itemName.CompareTo(y.ItemData.itemName);
                    }
                    else
                    {
                        return y.ItemData.itemName.CompareTo(x.ItemData.itemName);
                    }
                });
                break;
            case ItemSortBy.Price:
                beforeSlots.Sort((x, y) =>
                {
                    if (x.ItemData == null)
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.price.CompareTo(y.ItemData.price);
                    }
                    else
                    {
                        return y.ItemData.price.CompareTo(x.ItemData.price);
                    }
                });
                break;
        }
        // beforeSlots은 정해진 기준에 따라 정렬 완료

        // 아이템 종류와 개수를 따로 저장하기
        List<(ItemData, uint)> sortedData = new List<(ItemData, uint)>(slots.Count);
        foreach (var slot in beforeSlots)
        {
            sortedData.Add((slot.ItemData, slot.ItemCount));
        }

        // 슬롯에 아이템 종류와 개수를 순서대로 할당하기
        int index = 0;
        foreach (var data in sortedData)
        {
            slots[index].AssignSlotItem(data.Item1, data.Item2);
            index++;
        }//수동 복사부분

        // 정렬 완료된 것을 다시 배열로 만들기
       // slots = beforeSlots;
        RefreshInventory();
    }

    /// <summary>
    /// 모든 슬롯이 변경되었음을 알리는 함수
    /// </summary>
    void RefreshInventory()
    {
        List<Slot> slots = GetItemTab();
        foreach (var slot in slots)
        {
            slot.onItemDataChange?.Invoke(slot.ItemData);
        }
    }
    public void Use_Item_On_QuickSlot(ItemData data)
    {
        Slot slot = FindSameItem(data);
        if (slot.ItemData != null && slot.ItemCount > 0)
        {
            slot.DecreaseSlotItem();
        }
    }
    Slot FindSameItem(ItemData data)
    {
        List<Slot> slots = GetItemTab(data);
        Slot findSlot = null;
        foreach (var slot in slots)  // 모든 슬롯을 다 돌면서
        {
            if (slot.ItemData != null)
            {
                if (slot.ItemData.code == data.code)  // itemData가 같고 여유 공간이 있으면 그 슬롯을 리턴한다
                {
                    findSlot = slot;
                    break;
                }
            }
            else
            {
                continue;
            }
        }

        return findSlot;
    }

    /// <summary>
    /// 인벤토리에서 비어있는 슬롯을 찾는 함수
    /// </summary>
    /// <returns>비어있는 슬롯(첫번째)</returns>
    Slot FindEmptySlot(ItemData data)
    {
        Slot findSlot = null;
        List<Slot> slots = GetItemTab(data);
        foreach (var slot in slots)     // 모든 슬롯을 다 돌면서
        {
            if (slot.IsEmpty)            // 비어있는 슬롯이 있으면 찾았다.// Slot의 IsEmpty가 초기 false인 문제  ItemData가 null 인데 왜 false인지 잘 모르겠다.
            {
                findSlot = slot;
                break;
            }
        }

        return findSlot;
    }

    /// 적절한 인덱스인지 확인하는 함수
    bool IsValidIndex(uint index, ItemData data = null)
    {
        List<Slot> slots = GetItemTab(data);
        if (index < slots.Count || index == tempSlot.Index)
        {
            return true;
        }
        return false;
    }

    private List<Slot> GetItemTab(ItemData item = null)
    {
        List<Slot> slotList = null;
        if (item != null) // 이 함수를 호출할 때 itemdata가 null 이면  인벤토리에 현재 선택된 탭의 리스트를 리턴한다.
        {
            switch (item.ItemType) // null 이 아니면 Inventory 클래스에서 현재 어떤 탭이 선택되었든 관계없이 item의 itemType에 따라 리스트를 결정 한다.
            {
                case ItemType.Equip:
                    return slotList = slots[Inventory_Tab.Equip];
                case ItemType.Consume:
                    return slotList = slots[Inventory_Tab.Consume];
                case ItemType.Etc:
                    return slotList = slots[Inventory_Tab.Etc];
                case ItemType.Craft:
                    return slotList = slots[Inventory_Tab.Craft];
                default:
                    break;
            }
        }
        else
        {
            switch (GameManager.Inventory.State)
            {
                case Inventory_Tab.Equip:
                    return slotList = slots[Inventory_Tab.Equip];
                case Inventory_Tab.Consume:
                    return slotList = slots[Inventory_Tab.Consume];
                case Inventory_Tab.Etc:
                    return slotList = slots[Inventory_Tab.Etc];
                case Inventory_Tab.Craft:
                    return slotList = slots[Inventory_Tab.Craft];
                default:
                    break;
            }
        }
        Debug.Log($"{slotList.Count} 인덱스오류왜떠");
        return null;
    }


    /// <summary>
    /// 보상 받고 퀘스트 에 들어간 아이템 삭제하기위해 아래 두함수 추가 
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    private List<Slot> GetItemTab(ItemCode code)
    {
        List<Slot> slotList = null;
        switch (GameManager.Itemdata[code].ItemType) // null 이 아니면 Inventory 클래스에서 현재 어떤 탭이 선택되었든 관계없이 item의 itemType에 따라 리스트를 결정 한다.
        {
            case ItemType.Equip:
                return slotList = slots[Inventory_Tab.Equip];
            case ItemType.Consume:
                return slotList = slots[Inventory_Tab.Consume];
            case ItemType.Etc:
                return slotList = slots[Inventory_Tab.Etc];
            case ItemType.Craft:
                return slotList = slots[Inventory_Tab.Craft];
            default:
                break;
        }
        return slotList;
    }
    public void RemoveItem(ItemCode code, int removeCount)
    {
        List<Slot> slots = GetItemTab(code);
        foreach (var item in slots)
        {
            if (item.ItemData != null && item.ItemData.code == code)
            {
                item.DecreaseSlotItem((uint)removeCount);
                break;
            }
        }

    }
}
