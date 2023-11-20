using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Quiz_Option_Base : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int optionNumber;
    TextMeshProUGUI optionText;
    protected WaitForSeconds moment;
    StringBuilder sb;
    private void Awake()
    {
        sb = new StringBuilder(50);
        optionText  = GetComponent<TextMeshProUGUI>();
        moment = new WaitForSeconds(0.03f);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        optionText.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        optionText.color = Color.black;
    }
    public void SetOptionText(string option)
    {
        StartCoroutine(SetOptionText_Coroutine(option));
    }
    IEnumerator SetOptionText_Coroutine(string option)
    {
        for (int i = 0; i < option.Length; i++)
        {
            sb.Append(option[i]);
            optionText.text = sb.ToString();
            yield return moment;
        }
    }
}
