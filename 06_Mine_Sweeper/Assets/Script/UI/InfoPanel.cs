using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    TextMeshProUGUI actionCount;
    TextMeshProUGUI findCount;
    TextMeshProUGUI notFindCount;

    private void Awake()
    {
        actionCount = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        findCount = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        notFindCount = transform.GetChild(7).GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        GameManager.Inst.onActionCountChange += RefreshActionCount;
        GameManager.Inst.onUpdateUI += ShowResult;
        GameManager.Inst.Board.onFixCount += SubstrateFindText;
    }
    void RefreshActionCount(int count)
    {
        actionCount.text = count.ToString();
    }

    void ShowResult(int mineCount, int findCount_)
    {
        int result = mineCount - findCount_;
        findCount.text = findCount_.ToString();
        notFindCount.text = result.ToString();
    }
    void SubstrateFindText(int mistakecount)//이 함수보다 위 ShowResult 가 먼저 실행되어야 한다
    {
        int originFindCount = int.Parse(findCount.text);
        int originNotFindCount = int.Parse(notFindCount.text);
        int newFindCount = originFindCount - mistakecount;
        int newNotFindCount = originNotFindCount + mistakecount;
        findCount.text = newFindCount.ToString();
        notFindCount.text = newNotFindCount.ToString();
    }
}
