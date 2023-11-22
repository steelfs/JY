using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI 기준으로 스크립트만듬 
/// </summary>
public class BattleActionButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //버튼은 하위단에 하나밖에존재안함
    Button bt;
    //컨트롤러도 하나만존재
    protected BattleActionUIController uiController;
    protected virtual void Awake()
    {
        Transform parent = transform.parent;
        uiController = parent.GetComponentInChildren<BattleActionUIController>(true);
        bt = GetComponentInChildren<Button>(true);
        bt.onClick.AddListener(OnClick);
    }
    protected virtual void OnClick() 
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //SpaceSurvival_GameManager.Instance.IsUICheck = true;
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //SpaceSurvival_GameManager.Instance.IsUICheck = false;
        OnMouseExit();
    }

    protected virtual void OnMouseEnter()
    {
        
    }
    protected virtual void OnMouseExit() 
    {
    
    }

}
