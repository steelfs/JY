using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMapClearAction : PopupWindowBase
{
    Button confimBtn;

    TextMeshProUGUI rewordText;

    protected override void Awake()
    {
        base.Awake();
        Transform child = transform.GetChild(0).GetChild(0);
        rewordText = child.GetChild(2).GetComponent<TextMeshProUGUI>();
        confimBtn = child.GetChild(3).GetComponent<Button>();
        confimBtn.onClick.AddListener(() => {
            SpaceSurvival_GameManager.Instance.IsBattleMapClear = true;
            SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestReset();
            LoadingScene.SceneLoading(EnumList.SceneName.SpaceShip);
            gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 보상 셋팅 
    /// </summary>
    public void SetRewordText() 
    {
        ItemCode rewordCode = GetRewordItemCode(SpaceSurvival_GameManager.Instance.CurrentStage);
        ItemData itemData = GameManager.Itemdata[rewordCode];
        rewordText.text = $"{itemData.itemName}";
        GameManager.SlotManager.AddItem(rewordCode);
    }

    /// <summary>
    /// 스테이지별로 보상아이템 설정 
    /// </summary>
    /// <param name="currentStage">스테이지 종류</param>
    /// <returns>보상아이템 코드</returns>
    private ItemCode GetRewordItemCode(StageList currentStage) 
    {
        switch (currentStage)
        {
            case StageList.stage1:
                return ItemCode.Red_Crystal;
            case StageList.stage2:
                return ItemCode.Pink_Crystal;
            case StageList.stage3:
                return ItemCode.Yellow_Crystal;
            default:
                return ItemCode.Cash;
        }
    }

}
