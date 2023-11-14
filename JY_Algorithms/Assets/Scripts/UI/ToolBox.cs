using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolBox : MonoBehaviour
{
    public Button backButton;
    public TextMeshProUGUI cashText;
    CanvasGroup canvasGroup;
    float cash;
    const float duration = 1;
    public AnimationCurve cashFontSize;
 
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
    private void Start()
    {
        backButton.onClick.AddListener(GameManager.Inst.CloseQuestionPanel);
    }
 
    IEnumerator IncreaseCashText(float total_Increase)
    {
        float time = 0;
        float increase = 0;
        float result = cash + total_Increase;
        while(time < duration)
        {
            time += Time.deltaTime;
            increase = total_Increase * Time.deltaTime;
            cash += increase;
            cashText.text = $"{cash:n0}";//n0 시 f0 할 필요 없이 소숫점 자동 제거
            cashText.fontSize = Mathf.Min(100, cashFontSize.Evaluate(time)); 
            yield return null;
        }
        cash = result;
        cashText.text = $"{result:n0}";
    }
    public void IncreaseCash(float total_Increase)
    {
        StartCoroutine(IncreaseCashText(total_Increase));
    }
}
