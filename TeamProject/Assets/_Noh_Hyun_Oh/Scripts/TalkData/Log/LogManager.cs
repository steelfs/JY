using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour, IPopupSortWindow
{


    /// <summary>
    /// 로그창에 로그 텍스트 부모위치
    /// </summary>
    RectTransform logTextParent;

    /// <summary>
    /// 로그 텍스트 프리팹
    /// </summary>
    [SerializeField]
    TextMeshProUGUI textPrefab;
    float textPrefabHeight;

    /// <summary>
    /// 로그목록을 가져온다.
    /// </summary>
    public Func<int, string[]> getLogTalkDataArray;

    public Action<IPopupSortWindow> PopupSorting { get; set ; }

    CanvasGroup cg;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 1.0f;

        logTextParent = transform.GetComponentInChildren<VerticalLayoutGroup>().GetComponent<RectTransform>();//content 위치
        textPrefabHeight = textPrefab.rectTransform.sizeDelta.y;
        //transform.gameObject.SetActive(false);
    }
    /// <summary>
    /// 로그박스 셋팅용 함수
    /// </summary>
    public void LogBoxSetting(int currentTalkIndex)
    {
        if (gameObject.activeSelf) //열려있으면 닫아야되고
        {
            gameObject.SetActive(false);
        }
        else //닫혀있으며 열어야한다 
        {
            gameObject.SetActive(true);
            TextMeshProUGUI[] logTexts;
            if (logTextParent.childCount > 0)
            {
                logTexts = logTextParent.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (var text in logTexts)
                {
                    text.text = "";
                }
                //기존값 존재하면 초기화 
            }
            string[] logTextArray = getLogTalkDataArray?.Invoke(currentTalkIndex);
            if (logTextArray != null) //출력할 내용이 존재하면 
            {
                int logDataLength = logTextArray.Length;
                if (logTextParent.childCount < logDataLength) //기존 로그 생성해둔 값을 비교하고 
                {
                    int createCount = logDataLength - logTextParent.childCount;
                    for (int i = 0; i < createCount; i++) //부족한만큼 
                    {
                        Instantiate<TextMeshProUGUI>(textPrefab, logTextParent); //추가생성
                    }
                }

                logTexts = logTextParent.GetComponentsInChildren<TextMeshProUGUI>();

                for (int i = 0; i < logTextArray.Length; i++)
                {
                    logTexts[i].text = $"{logTextArray[i]}\r\n";
                }
                logTextParent.sizeDelta = new Vector2(logTextParent.sizeDelta.x, textPrefabHeight * logTexts.Length);
            }
        }
    }

    public void ResetData() 
    {
        gameObject.SetActive (false);
    }

    public void OpenWindow()
    {
        gameObject.SetActive (true);
    }

    public void CloseWindow()
    {
        ResetData();
    }
}
