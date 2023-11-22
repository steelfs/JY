using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour, IPopupSortWindow
{


    /// <summary>
    /// �α�â�� �α� �ؽ�Ʈ �θ���ġ
    /// </summary>
    RectTransform logTextParent;

    /// <summary>
    /// �α� �ؽ�Ʈ ������
    /// </summary>
    [SerializeField]
    TextMeshProUGUI textPrefab;
    float textPrefabHeight;

    /// <summary>
    /// �α׸���� �����´�.
    /// </summary>
    public Func<int, string[]> getLogTalkDataArray;

    public Action<IPopupSortWindow> PopupSorting { get; set ; }

    CanvasGroup cg;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 1.0f;

        logTextParent = transform.GetComponentInChildren<VerticalLayoutGroup>().GetComponent<RectTransform>();//content ��ġ
        textPrefabHeight = textPrefab.rectTransform.sizeDelta.y;
        //transform.gameObject.SetActive(false);
    }
    /// <summary>
    /// �α׹ڽ� ���ÿ� �Լ�
    /// </summary>
    public void LogBoxSetting(int currentTalkIndex)
    {
        if (gameObject.activeSelf) //���������� �ݾƾߵǰ�
        {
            gameObject.SetActive(false);
        }
        else //���������� ������Ѵ� 
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
                //������ �����ϸ� �ʱ�ȭ 
            }
            string[] logTextArray = getLogTalkDataArray?.Invoke(currentTalkIndex);
            if (logTextArray != null) //����� ������ �����ϸ� 
            {
                int logDataLength = logTextArray.Length;
                if (logTextParent.childCount < logDataLength) //���� �α� �����ص� ���� ���ϰ� 
                {
                    int createCount = logDataLength - logTextParent.childCount;
                    for (int i = 0; i < createCount; i++) //�����Ѹ�ŭ 
                    {
                        Instantiate<TextMeshProUGUI>(textPrefab, logTextParent); //�߰�����
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
