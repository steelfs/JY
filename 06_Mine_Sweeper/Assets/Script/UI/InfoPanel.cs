using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    TextMeshProUGUI actionCount;
    TextMeshProUGUI findCount;
    TextMeshProUGUI notFindCount;

    private void Awake()
    {
        Transform child = transform.GetChild(2);
        actionCount = child.GetChild(1).GetComponent<TextMeshProUGUI>();
        findCount = child.GetChild(4).GetComponent<TextMeshProUGUI>();
        notFindCount = child.GetChild(7).GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        GameManager.Inst.onActionCountChange += RefreshActionCount;
        GameManager.Inst.onUpdateUI += ShowResult;
    }
    void RefreshActionCount(int Count)
    {
        actionCount.text = Count.ToString();
    }

    void ShowResult(int mineCount, int findCount_)
    {
        int result = mineCount - findCount_;
        findCount.text = findCount_.ToString();
        notFindCount.text = result.ToString();
    }
}
