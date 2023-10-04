using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    Button openButton;
    GameObject resultBoard;
    CanvasGroup canvasGroup;
    Result_Analysis[] analysis;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        openButton = transform.GetChild(0).GetComponent<Button>();
        resultBoard = transform.GetChild(1).gameObject;

        openButton.onClick.AddListener(ToggleOpenClose);
        analysis = GetComponentsInChildren<Result_Analysis>();
    }
    void ToggleOpenClose()
    {
        if (resultBoard.activeSelf)
        {
            resultBoard.SetActive(false);
        }
        else
        {
            resultBoard.SetActive(true);
        }
    }
    void Open()
    {
        canvasGroup.alpha = 0.9f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        foreach (Result_Analysis analysis in analysis)
        {
            analysis.Refresh();
        }
    }
    void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void Start()
    {
        GameManager.Inst.EnemyPlayer.onDefeat += (_) => Open();
        GameManager.Inst.UserPlayer.onDefeat += (_) => Open();
        Close();
    }
}
