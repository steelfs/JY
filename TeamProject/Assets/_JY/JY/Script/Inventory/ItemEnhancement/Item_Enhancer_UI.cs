using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Item_Enhancer_UI : MonoBehaviour, IPopupSortWindow, IPointerClickHandler
{ 
    CanvasGroup canvasGroup;
    Item_Enhancer itemEnhancer;

    public bool IsOpen => canvasGroup.alpha == 1.0f;

    public UnityEngine.UI.Button closeButton;
    public UnityEngine.UI.Button cancelButton;
    public UnityEngine.UI.Button plusButton;
    public UnityEngine.UI.Button minusButton;
    public UnityEngine.UI.Button confirmButton;
    public TextMeshProUGUI successRateText;
    public TextMeshProUGUI beforelevelText;
    public TextMeshProUGUI afterlevelText;
    public Animator proceedAnim;

    public TMP_InputField amountText;
    public UnityEngine.UI.Slider amountSlider;
    
    Enhancer_Slot_Before beforeSlot;
    Enhancer_Slot_After afterSlot;

    public Action<ItemData_Enhancable> onDarkForceValueChange;
    public Action onTriggerLevelUp;

    const uint MinDarkForceCount = 0;
    uint darkForceCount = MinDarkForceCount;
    byte itemLevel = 0;
    public uint DarkForceCount
    {
        get => darkForceCount;
        set
        {
            darkForceCount = Math.Clamp(value, MinDarkForceCount, (uint)GameManager.PlayerStatus.DarkForce);
            amountText.text = darkForceCount.ToString();    // 인풋 필드에 적용
            amountSlider.value = darkForceCount;
            onDarkForceValueChange?.Invoke(itemEnhancer.ItemData);
        }
    }
    public Enhancer_Slot_Before BeforeSlot => beforeSlot;
    public Enhancer_Slot_After AfterSlot => afterSlot;

    public Action<IPopupSortWindow> PopupSorting { get; set; }

    private void Awake()
    {
        WarningBox warningBox = FindObjectOfType<WarningBox>();
        warningBox.onWarningBoxClose += OpenInteractable;

        itemEnhancer = GetComponent<Item_Enhancer>();
        canvasGroup = GetComponent<CanvasGroup>();
        beforeSlot = GetComponentInChildren<Enhancer_Slot_Before>();
        afterSlot = GetComponentInChildren<Enhancer_Slot_After>();

        closeButton.onClick.AddListener(() => itemEnhancer.EnhancerState = EnhancerState.Close);// 수정필요
        cancelButton.onClick.AddListener(() => itemEnhancer.EnhancerState = EnhancerState.Close);

        confirmButton.onClick.AddListener(() => itemEnhancer.EnhancerState = EnhancerState.Confirm);
     
        plusButton.onClick.AddListener(() => DarkForceCount++);
        minusButton.onClick.AddListener(() => DarkForceCount--);


        amountText.onValueChanged.AddListener((text) =>
        {
            if (uint.TryParse(text, out uint result))
            {
                DarkForceCount = result;
            }
            else
            {
                DarkForceCount = MinDarkForceCount;
            }
        });
        amountSlider.onValueChanged.AddListener((ratio) =>
        {
            DarkForceCount = (uint)ratio;
        });
    }
    private void Start()
    {

        itemEnhancer.onOpen += Open;
        itemEnhancer.onClose += Close;
        itemEnhancer.onSetItem += RefreshEnhancerUI; //tempSlot의 아이템을 드롭했을 때
        itemEnhancer.onClearItem += ClearEnhancerUI; // beforeSlot을 클릭했을 때
        itemEnhancer.onConfirmButtonClick += BlockInteractable;// enhancerUI창의 체크버튼 클맀했을 때
        onDarkForceValueChange += UpdateSuccessRate; // InputField의 Darkforce의 값이 바뀔때
        itemEnhancer.onWaitforResult += WaitForResult; // warningBox의 체크버튼을 누를 때 
       // itemEnhancer.onWaitforResult += BlockInteractable;
        itemEnhancer.onSuccess += () => StartCoroutine(PopUp_ProceedBox(true)); //WaitForResult에서 호출
        itemEnhancer.onFail += () => StartCoroutine(PopUp_ProceedBox(false));



        Close();

    }
    public void Open()
    {
        GameManager.SoundManager.PlayOneShot_OnOffToggle();
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        DarkForceCount = 0;

        ClearEnhancerUI();
        PopupSorting?.Invoke(this);
    }
    public void Close()
    {
        GameManager.SoundManager.PlayOneShot_OnOffToggle();
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        itemEnhancer.ItemData = null;
        BeforeSlot.ItemData = null;
        AfterSlot.ItemData = null;
        successRateText.text = "0";
    }
    void RefreshEnhancerUI(ItemData_Enhancable itemData)
    {
        if (itemData != null)
        {
            itemLevel = itemEnhancer.ItemData.Itemlevel;
            beforelevelText.text = $"{itemLevel}";
            afterlevelText.text = $"{itemLevel + 1}";
            amountSlider.maxValue = GameManager.PlayerStatus.DarkForce;
            UpdateSuccessRate(itemData);
        }
    }

    private void ClearEnhancerUI()
    {
        //이미지, 성공률 text
        beforelevelText.text = string.Empty;
        afterlevelText.text = string.Empty;
        amountSlider.maxValue = 0.0f;
        amountSlider.value = 0;
        amountText.text = "0";
        successRateText.text = $"{0}";
        beforeSlot.ItemData = null;
        afterSlot.ItemData = null;
        itemEnhancer.ItemData = null;
    }
    void BlockInteractable()//Enhancer에서 신호 받음 
    {
        beforeSlot.imageComp.raycastTarget = false;
        beforeSlot.itemIcon.raycastTarget = false;
        confirmButton.interactable = false;
        cancelButton.interactable = false;
        closeButton.interactable = false;
        amountSlider.interactable = false;
        amountText.interactable= false;
        plusButton.interactable = false;
        minusButton.interactable = false;
    }
    void OpenInteractable()//warningbox에서 신호받음
    {

        beforeSlot.imageComp.raycastTarget = true;
        beforeSlot.itemIcon.raycastTarget = true;
        confirmButton.interactable = true;
        cancelButton.interactable = true;
        closeButton.interactable = true;
        amountSlider.interactable = true;
        amountText.interactable = true;
        plusButton.interactable = true;
        minusButton.interactable = true;
    }
    void UpdateSuccessRate(ItemData item)//확률 계산은 IEnhancable 에서 직접하는게 좋을것 같다. 필요한 데이터가 모두 거기있기때문이다.
    {
        if (item == null)
        {
            return;
        }
          
        ItemData_Enhancable enhancable = item as ItemData_Enhancable;
        if (enhancable != null)
        {
            successRateText.text = enhancable.CalculateSuccessRate(DarkForceCount).ToString("f1");
        }
    }
    void WaitForResult()//confirm 버튼을 누른 후
    {
        if (itemEnhancer.ItemData.LevelUp(DarkForceCount))//
        {
            itemEnhancer.EnhancerState = EnhancerState.Success;
        }
        else
        {
            itemEnhancer.EnhancerState = EnhancerState.Fail;
        }
        GameManager.PlayerStatus.Base_Status.Base_DarkForce -= DarkForceCount;
    }
    IEnumerator PopUp_ProceedBox(bool levelUp)
    {
        if (levelUp)
        {
            proceedAnim.SetTrigger("Success");
            yield return new WaitForSeconds(3.0f);// Success clip의 재생시간을 고려한 딜레이
            onTriggerLevelUp?.Invoke();
            itemEnhancer.EnhancerState = EnhancerState.ClearItem;
            Debug.Log("State 변경 ");
        }
        else
        {
            proceedAnim.SetTrigger("Fail");
            yield return new WaitForSeconds(3.0f);//대기시간이 없으면 버튼 활성화가 너무 빨리된다.
        }
        OpenInteractable();
    }

    public void OpenWindow()
    {
        Open();
        PopupSorting?.Invoke(this);
    }

    public void CloseWindow()
    {
        Close();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupSorting?.Invoke(this);
    }
}
