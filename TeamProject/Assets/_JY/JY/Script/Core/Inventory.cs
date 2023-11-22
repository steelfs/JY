using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Inventory_Tab
{
    None,
    Equip,
    Consume,
    Etc,
    Craft
}

public class Inventory : MonoBehaviour, IPopupSortWindow,IPointerClickHandler
{
    GameObject Equip_Inven;
    GameObject Consume_Inven;
    GameObject Etc_Inven;
    GameObject Craft_Inven;
    Item_Enhancer ItemEnhancer;
    Item_Mixer mixer;

    GameObject ItemDescription;

    Transform toolBar;
    Button close_Button;
    Button add_Button;
    Button equip_Button;
    Button consume_Button;
    Button etc_Button;
    Button craft_Button;
    Button sort_Button;
    Button enhance_Button;
    Button mixer_Button;

    Image equipButtonColor;
    Image consumeButtonColor;
    Image etcButtonColor;
    Image craftButtonColor;

    CanvasGroup canvasGroup;
    public delegate void Inventory_State_Changed(Inventory_Tab state); //state가 바뀌면 setter가 호출할 delegate
    Inventory_State_Changed inventory_changed;

    private  Inventory_Tab state;
    public  Inventory_Tab State
    {
        get { return state; }
        set
        {
            if (state != value)
            {
                state = value;
                inventory_changed?.Invoke(state);
            }
        }
    }

    public Action<IPopupSortWindow> PopupSorting { get ; set ; }

    private void Awake()
    {
        toolBar = transform.GetChild(4);
        close_Button = toolBar.GetChild(0).GetComponent<Button>();
        add_Button = toolBar.GetChild(1).GetComponent<Button>();
        sort_Button = toolBar.GetChild(2).GetComponent<Button>();
        enhance_Button = toolBar.GetChild(3).GetComponent<Button>();
        mixer_Button = toolBar.GetChild(4).GetComponent<Button>();

        equip_Button = transform.GetChild(5).GetComponent<Button>();
        consume_Button = transform.GetChild(6).GetComponent<Button>();
        etc_Button = transform.GetChild(7).GetComponent<Button>();
        craft_Button = transform.GetChild(8).GetComponent<Button>();


        equipButtonColor = equip_Button.GetComponent<Image>();
        consumeButtonColor = consume_Button.GetComponent<Image>();
        etcButtonColor = etc_Button.GetComponent<Image>();
        craftButtonColor = craft_Button.GetComponent<Image>();

        sort_Button.onClick.AddListener(SlotSorting);
        close_Button.onClick.AddListener(Open_Inventory);
        mixer_Button.onClick.AddListener(Open_Mixer);

        equip_Button.onClick.AddListener(SwitchTab_To_Equip);
        consume_Button.onClick.AddListener(SwitchTab_To_Consume);
        etc_Button.onClick.AddListener(SwitchTab_To_Etc);
        craft_Button.onClick.AddListener(SwitchTab_To_Craft);

        ItemDescription = transform.GetChild(9).gameObject;
        ItemDescription.SetActive(true);

        canvasGroup = GetComponent<CanvasGroup>();
    }
    void SlotSorting() //addListener 로 매개변수필요한 함수 바로 등록이 안되서 우회접근
    {
        GameManager.SlotManager.SlotSorting(ItemSortBy.Price, true);
    }
 
    private void OnEnable()
    {
        inventory_changed += Update_State;
    
    }
    private void OnDisable()
    {
        inventory_changed -= Update_State;
    }
    private void Start()
    {
        Equip_Inven = transform.GetChild(0).gameObject;
        Consume_Inven = transform.GetChild(1).gameObject;
        Etc_Inven = transform.GetChild(2).gameObject;
        Craft_Inven = transform.GetChild(3).gameObject;
        ItemEnhancer = GameManager.Enhancer;
        mixer = GameManager.Mixer;


        add_Button.onClick.AddListener(GameManager.SlotManager.Make_Slot);
        enhance_Button.onClick.AddListener(Open_Enhancer);//enable, Awake에서는 안됨
        GameManager.SlotManager.Initialize();
    }
    void Open_Enhancer()
    {
        GameManager.Enhancer.EnhancerState = EnhancerState.Open;
    }
    void Open_Mixer()
    {
        mixer.MixerState = ItemMixerState.Open;
    }

    void Update_State(Inventory_Tab state)
    {
        equipButtonColor.color = Color.white;
        consumeButtonColor.color = Color.white;
        etcButtonColor.color = Color.white;
        craftButtonColor.color = Color.white;

        
        switch (state)
        {
            case Inventory_Tab.Equip:
                Equip_Inven.SetActive(true);
                equipButtonColor.color = Color.grey;
                Consume_Inven.SetActive(false);
                Etc_Inven.SetActive(false);
                Craft_Inven.SetActive(false);
                break;
            case Inventory_Tab.Consume:
                Consume_Inven.SetActive(true);
                consumeButtonColor.color = Color.grey;
                Equip_Inven.SetActive(false);
                Etc_Inven.SetActive(false);
                Craft_Inven.SetActive(false);
                break;
            case Inventory_Tab.Etc:
                Etc_Inven.SetActive(true);
                etcButtonColor.color = Color.gray;
                Equip_Inven.SetActive(false);
                Consume_Inven.SetActive(false);
                Craft_Inven.SetActive(false);
                break;
            case Inventory_Tab.Craft:
                Craft_Inven.SetActive(true);
                craftButtonColor.color = Color.grey;
                Equip_Inven.SetActive(false);
                Consume_Inven.SetActive(false);
                Etc_Inven.SetActive(false);
                break;
            default:
                break;
        }
    }
    public void SwitchTab_To_Equip() { State = Inventory_Tab.Equip; } //버튼 누르면 호출
    public void SwitchTab_To_Consume() { State = Inventory_Tab.Consume;}
    public void SwitchTab_To_Etc() { State = Inventory_Tab.Etc;}
    public void SwitchTab_To_Craft() { State = Inventory_Tab.Craft;}
    public void Open_Inventory()
    {
        //전투중 입력 막을 함수 목록
        // 인벤토리 열기
        // 장비창
        // 퀵슬롯 팝업
        if (canvasGroup.alpha < 1.0f)
        {
            GameManager.SoundManager.PlayOneShot_OnOffToggle();
            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            PopupSorting?.Invoke(this);
        }
        else
        {
            GameManager.SoundManager.PlayOneShot_OnOffToggle();
            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
    public void RefreshOrder()
    {
        this.transform.SetAsFirstSibling();
    }

    public void OpenWindow()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void CloseWindow()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupSorting.Invoke(this);
    }
}
