using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Save_PotionData
{
    public ItemData_Potion PotionData;
    public uint itemCount;
    public Slot bindingSlot;
    public SkillType skillType;
    public Save_PotionData(QuickSlot slot)
    {
        if (slot.SkillData != null)
        {
            this.PotionData = slot.ItemData;
            this.itemCount = slot.ItemCount;
            //this.bindingSlot = slot.type;
            this.skillType = slot.SkillData.SkillType;
        }
    }
}
public enum QuickSlotList
{
    Shift = 0,
    _8,
    _9,
    _0,
    Ctrl,
    Alt,
    Space,
    Insert
}
public enum QuickSlot_State
{
    None,
    Set,
    Moving
}
public class QuickSlot_Manager : MonoBehaviour, IPopupSortWindow
{
    Player_ player;
    SlotManager slotManager;
    SkillBox skillBox;

    Button popupButton;
    TextMeshProUGUI buttonText;

    RectTransform rectTransform;
    public RectTransform QuickSlotBox_RectTransform => rectTransform;
    QuickSlot[] quickSlots;
    public QuickSlot[] QuickSlots => quickSlots;

    Vector2 hidePos = Vector2.zero;
    public float popUpSpeed = 100.0f;
    bool isOpen = false;

    ItemData_Potion shiftSlot_Data;
    ItemData_Potion _8Slot_Data;
    ItemData_Potion _9Slot_Data;
    ItemData_Potion _0Slot_Data;
    ItemData_Potion ctrl_Slot_Data;
    ItemData_Potion alt_Slot_Data;
    ItemData_Potion space_Slot_Data;
    ItemData_Potion insert_Slot_Data;

    ItemData_Potion itemData;
    public ItemData_Potion ItemData
    {
        get => itemData;
        set
        {
            itemData = value;
        }
    }
    uint itemCount;
    public uint ItemCount
    {
        get => itemCount;
        set
        {
            itemCount = value;
        }
    }

    public Action<IPopupSortWindow> PopupSorting { get; set; }
    public Action<SkillData> on_Activate_Skill;

    private void Awake()
    {
        //inputAction = new InputKeyMouse();
        hidePos = new Vector2(0, -280.0f);
        rectTransform = GetComponent<RectTransform>();

        popupButton = transform.GetChild(8).GetComponent<Button>();
        buttonText = transform.GetChild(8).GetComponentInChildren<TextMeshProUGUI>();

        popupButton.onClick.AddListener(PopUp);


        quickSlots = new QuickSlot[8];
        int i = 0;
        while (i < 8)
        {
            quickSlots[i] = transform.GetChild(i).GetComponent<QuickSlot>();
            quickSlots[i].onSet_ItemData += (_, quickSlot) => Connect_Delegate(quickSlot);
            quickSlots[i].on_SkillSet += (quickSlot) => Connect_Delegate(quickSlot);
            quickSlots[i].on_Clear_Quickslot_Data += DisConnect_Delegate;
            i++;
        }

    }
   
    private void Start()
    {
        InputSystemController.Instance.OnQuickSlot_Popup += QuickSlot_PopUp;
        Init();
        StartCoroutine(Get_References());
    }
    IEnumerator Get_References()
    {
        yield return null;
        player = GameManager.Player_;
        slotManager = GameManager.SlotManager;
        skillBox = GameManager.SkillBox;
    }

    public void Set_ItemDataTo_QuickSlot(ItemData_Potion data)
    {
        Find_Slot_By_Position(out QuickSlot targetSlot);
        if (targetSlot != null)
        {
            targetSlot.ItemData = data;
        }
    }
    void Connect_Delegate(QuickSlot slot)
    {
        switch (slot.type)
        {
            case QuickSlot_Type.Shift:
                //inputAction.QuickSlot.Shift.performed += Shift_performed;
                InputSystemController.InputSystem.QuickSlot.Shift.performed += Shift_performed; //인풋시스템에 직접 등록하면 중복등록을 막을 수 있음
                shiftSlot_Data = slot.ItemData;// shiftSlot_Data = 이 클래스에서 저장해둘 데이터
                break;
            case QuickSlot_Type._8:
                InputSystemController.InputSystem.QuickSlot.Eight.performed += Eight_performed;
                _8Slot_Data = slot.ItemData;
                break;
            case QuickSlot_Type._9:
                InputSystemController.InputSystem.QuickSlot.Nine.performed += Nine_performed;
                _9Slot_Data = slot.ItemData;
                break;
            case QuickSlot_Type._0:
                InputSystemController.InputSystem.QuickSlot.Zero.performed += Zero_performed;
                _0Slot_Data = slot.ItemData;
                break;
            case QuickSlot_Type.Ctrl:
                InputSystemController.InputSystem.QuickSlot.Ctrl.performed += Ctrl_performed;
                ctrl_Slot_Data = slot.ItemData;
                break;
            case QuickSlot_Type.Alt:
                InputSystemController.InputSystem.QuickSlot.Alt.performed += Alt_performed;
                alt_Slot_Data = slot.ItemData;
                break;
            case QuickSlot_Type.Space:
                InputSystemController.InputSystem.QuickSlot.Space.performed += Space_performed;
                space_Slot_Data = slot.ItemData;
                break;
            case QuickSlot_Type.Insert:
                InputSystemController.InputSystem.QuickSlot.Insert.performed += Insert_performed;
                insert_Slot_Data = slot.ItemData;
                break;
            default:
                break;
        }
    }
    void DisConnect_Delegate(QuickSlot slot)
    {
        if (slot.ItemData == null && slot.SkillData == null)
        {
            switch (slot.type)
            {
                case QuickSlot_Type.Shift:
                    //inputAction.QuickSlot.Shift.performed -= Shift_performed;
                    InputSystemController.InputSystem.QuickSlot.Shift.performed -= Shift_performed;
                    break;
                case QuickSlot_Type._8:
                    //inputAction.QuickSlot.Eight.performed -= Eight_performed;
                    InputSystemController.InputSystem.QuickSlot.Eight.performed -= Eight_performed;
                    break;
                case QuickSlot_Type._9:
                    //inputAction.QuickSlot.Nine.performed -= Nine_performed;
                    InputSystemController.InputSystem.QuickSlot.Nine.performed -= Nine_performed;
                    break;
                case QuickSlot_Type._0:
                    //inputAction.QuickSlot.Zero.performed -= Zero_performed;
                    InputSystemController.InputSystem.QuickSlot.Zero.performed -= Zero_performed;
                    break;
                case QuickSlot_Type.Ctrl:
                    //inputAction.QuickSlot.Ctrl.performed -= Ctrl_performed;
                    InputSystemController.InputSystem.QuickSlot.Ctrl.performed -= Ctrl_performed;
                    break;
                case QuickSlot_Type.Alt:
                    //inputAction.QuickSlot.Alt.performed -= Alt_performed;
                    InputSystemController.InputSystem.QuickSlot.Alt.performed -= Alt_performed;
                    break;
                case QuickSlot_Type.Space:
                    //inputAction.QuickSlot.Space.performed -= Space_performed;
                    InputSystemController.InputSystem.QuickSlot.Space.performed -= Space_performed;
                    break;
                case QuickSlot_Type.Insert:
                    //inputAction.QuickSlot.Insert.performed -= Insert_performed;
                    InputSystemController.InputSystem.QuickSlot.Insert.performed -= Insert_performed;
                    break;
                default:
                    break;
            }
        }
    }
    public bool Find_Slot_By_Position(out QuickSlot findSlot)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        findSlot = null;
        bool result = false;
        foreach (var targetSlot in quickSlots)
        {
            RectTransform rectTransform = targetSlot.GetComponent<RectTransform>();
            Vector2 distance = mousePos - (Vector2)rectTransform.position;
            if (rectTransform.rect.Contains(distance))
            {
                findSlot = targetSlot;
                result = true;
                break;
            }
        }
        return result;
    }

    IEnumerator PopUpCoroutine()
    {
        Vector2 newPos = Vector2.zero;
        if (isOpen)
        {
            while (transform.position.y > hidePos.y)
            {
                transform.position += popUpSpeed * Time.deltaTime * -Vector3.up;
                yield return null;
            }
            newPos = transform.position;
            newPos.y = -280.0f;
            transform.position = newPos;
            isOpen = false;
        }
        else
        {
            while (transform.position.y < 0.0f)
            {
                transform.position += popUpSpeed * Time.deltaTime * Vector3.up;
                yield return null;
            }
            newPos = transform.position;
            newPos.y = 0;
            transform.position = newPos;
            isOpen = true;
        }
    }
    void PopUp()
    {
        StartCoroutine(PopUpCoroutine());
    }
    //private void QuickSlot_PopUp(InputAction.CallbackContext _)
    private void QuickSlot_PopUp()
    {
        StartCoroutine(PopUpCoroutine());
    }
    private void Space_performed(InputAction.CallbackContext _)
    {
        if (quickSlots[(int)QuickSlotList.Space].SkillData != null)
        {
            on_Activate_Skill?.Invoke(quickSlots[(int)QuickSlotList.Space].SkillData);
        }
        else if (quickSlots[(int)QuickSlotList.Space].ItemCount > 0)
        {
            space_Slot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(space_Slot_Data);
        }
    }

    private void Alt_performed(InputAction.CallbackContext _)
    {
        if (quickSlots[(int)QuickSlotList.Alt].SkillData != null)
        {
            on_Activate_Skill?.Invoke(quickSlots[(int)QuickSlotList.Alt].SkillData);
        }
        else if (quickSlots[(int)QuickSlotList.Alt].ItemCount > 0)
        {
            alt_Slot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(alt_Slot_Data);
        }
    }

    private void Ctrl_performed(InputAction.CallbackContext _)
    {
        if (quickSlots[(int)QuickSlotList.Ctrl].SkillData != null)
        {
            on_Activate_Skill?.Invoke(quickSlots[(int)QuickSlotList.Ctrl].SkillData);
        }
        else if (quickSlots[(int)QuickSlotList.Ctrl].ItemCount > 0)
        {
            ctrl_Slot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(ctrl_Slot_Data);
        }
    
    }

    private void Zero_performed(InputAction.CallbackContext _)
    {
        if (quickSlots[(int)QuickSlotList._0].SkillData != null)
        {
            on_Activate_Skill?.Invoke(quickSlots[(int)QuickSlotList._0].SkillData);
        }
        else if (quickSlots[(int)QuickSlotList._0].ItemCount > 0)
        {
            _0Slot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(_0Slot_Data);
        }
    }
    private void Nine_performed(InputAction.CallbackContext _)
    {
        if (quickSlots[(int)QuickSlotList._9].SkillData != null)
        {
            on_Activate_Skill?.Invoke(quickSlots[(int)QuickSlotList._9].SkillData);
        }
        else if (quickSlots[(int)QuickSlotList._9].ItemCount > 0)
        {
            _9Slot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(_9Slot_Data);
        }
    }

    private void Eight_performed(InputAction.CallbackContext _)
    {
        if (quickSlots[(int)QuickSlotList._8].SkillData != null)
        {
            on_Activate_Skill?.Invoke(quickSlots[(int)QuickSlotList._8].SkillData);
        }
        else if (quickSlots[(int)QuickSlotList._8].ItemCount > 0)
        {
            _8Slot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(_8Slot_Data);
        }
    }

    private void Insert_performed(InputAction.CallbackContext context)
    {
        if (quickSlots[(int)QuickSlotList.Insert].SkillData != null)
        {
            on_Activate_Skill?.Invoke(quickSlots[(int)QuickSlotList.Insert].SkillData);
        }
        else if (quickSlots[(int)QuickSlotList.Insert].ItemCount > 0)
        {
            shiftSlot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(insert_Slot_Data);
        }
    }

    //private void Shift_performed(InputAction.CallbackContext context)
    private void Shift_performed(InputAction.CallbackContext _)
    {
        if (quickSlots[(int)QuickSlotList.Shift].SkillData != null)
        {
            on_Activate_Skill?.Invoke(quickSlots[(int)QuickSlotList.Shift].SkillData);
        }
        else if (quickSlots[(int)QuickSlotList.Shift].ItemCount > 0)
        {
            shiftSlot_Data.Consume(player);
            slotManager.Use_Item_On_QuickSlot(shiftSlot_Data);
        }
    }


    void Init()
    {
        TempSlot_For_QuickSlot_ tempSlot = transform.GetChild(9).GetComponent<TempSlot_For_QuickSlot_>();

        // quickSlots = new QuickSlot[8];
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = transform.GetChild(i).GetComponent<QuickSlot>();
            quickSlots[i].Index = i;
            quickSlots[i].QuickSlot_Key_Value = Enum.GetName(typeof(QuickSlotList), i);//i번째 인덱스를 문자열로 바꿔서 변수에 할당

            quickSlots[i].on_BeginDrag_With_Potion += tempSlot.OpenWith_Potion_Data;
            quickSlots[i].on_BeginDrag_With_Skill += tempSlot.OpenWith_Skill_Data;
            quickSlots[i].on_Drag += tempSlot.MovePosition;
        }
    }

    public void OpenWindow()
    {
     
        if (isOpen)
        {
            StartCoroutine(PopUpOpenCoroutine());
            PopupSorting(this);
        }
    }

    public void CloseWindow()
    {
        if (!isOpen)
        {
            StartCoroutine(PopUpCloseCoroutine());
        }
    }
    IEnumerator PopUpCloseCoroutine()
    {
        isOpen = false;
        while (transform.position.y > hidePos.y)
        {
            transform.Translate((Time.deltaTime * popUpSpeed) * -Vector2.up);
           // transform.position += popUpSpeed * -Vector3.up;
            yield return null;
        }
    }
    IEnumerator PopUpOpenCoroutine()
    {
        isOpen = true;
        while (transform.position.y < 0.0f)
        {
            transform.Translate((Time.deltaTime * popUpSpeed) * Vector2.up);
            //transform.position += popUpSpeed * Vector3.up;
            yield return null;
        }
    }

  
    public void QuickSlots_Clear()
    {
        foreach(var slot in quickSlots)
        {
            slot.SkillData = null;
        }
        foreach(var skillData in GameManager.SkillBox.SkillDatas)
        {
            skillData.TestInit();
        }
    }
}
