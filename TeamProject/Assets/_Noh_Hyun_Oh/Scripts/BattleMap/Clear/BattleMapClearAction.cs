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
    /// ���� ���� 
    /// </summary>
    public void SetRewordText() 
    {
        ItemCode rewordCode = GetRewordItemCode(SpaceSurvival_GameManager.Instance.CurrentStage);
        ItemData itemData = GameManager.Itemdata[rewordCode];
        rewordText.text = $"{itemData.itemName}";
        GameManager.SlotManager.AddItem(rewordCode);
    }

    /// <summary>
    /// ������������ ��������� ���� 
    /// </summary>
    /// <param name="currentStage">�������� ����</param>
    /// <returns>��������� �ڵ�</returns>
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
