using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapChoiceController : MonoBehaviour
{
    [SerializeField]
    EnumList.SceneName sceneName = EnumList.SceneName.TestBattleMap;


    [SerializeField]
    bool IsBoss = false;

    Image icon;

    Button actionBtn;

    [SerializeField]
    string mapNameText;
    [SerializeField]
    int mapLevelValue;
    [SerializeField]
    string mapInfoText;
    [SerializeField]
    bool mapClearValue;

    TextMeshProUGUI mapName;

    TextMeshProUGUI mapInfo;

    TextMeshProUGUI mapLevel;

    TextMeshProUGUI mapClear;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        
        icon = child.GetComponent<Image>();
        
        child = transform.GetChild(2);

        mapName = child.GetChild(0).GetComponent<TextMeshProUGUI>();

        mapLevel = child.GetChild(1).GetComponent<TextMeshProUGUI>();
        
        mapInfo = child.GetChild(2).GetComponent<TextMeshProUGUI>();
        
        mapClear = child.GetChild(3).GetComponent<TextMeshProUGUI>();

        actionBtn = GetComponent<Button>();
        actionBtn.onClick.AddListener(OnButtonClick);
        InitData(null,mapNameText,mapInfoText,sceneName,mapLevelValue,mapClearValue);
    }



    public void InitData(Sprite iconImg, string name, string info, EnumList.SceneName sceneName,  int level = 1 , bool clear = false) 
    {
        icon.sprite = iconImg;
        mapName.text = $"{name}";
        mapLevel.text = $"{level}";
        mapInfo.text = $"{info}";
        mapClear.text = clear ? "Clear" : "None";
        this.sceneName = sceneName;

    }

    private void OnButtonClick() 
    {
        LoadingScene.SceneLoading(sceneName);
        SpaceSurvival_GameManager.Instance.IsBoss = IsBoss;
    }
}
