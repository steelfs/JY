using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI player_Text;
    TextMeshProUGUI questionMark_Text;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        player_Text = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        questionMark_Text = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    void clearResultPanel()
    {
        player_Text.text = string.Empty;
        questionMark_Text.text = string.Empty;
    }


}
