using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour
{
    public RectTransform contentsRect;
    public int overCount = 0;
    public float imageSize = 60.0f;

    public TextMeshProUGUI totalScoreText;
    public Image total_TeerImage;
    public Sprite[] total_TeerSprite;

    public TextMeshProUGUI[] scoreTexts;
    public Image[] teerImage;
    public Sprite[] teerSprite;
    public Button[] detailButtons;
    private void Awake()
    {
        
    }
    public void On_ValueChanged()
    {
        if (contentsRect.anchoredPosition.y < 0)
        {
            contentsRect.anchoredPosition = Vector2.zero;
        }
        else if (contentsRect.anchoredPosition.y > (imageSize * overCount))
        {
            contentsRect.anchoredPosition = new Vector2(0, imageSize * overCount);
        }
    }

    public void UpdateScorePanel(PlayerData playerData)
    {
        int totalQuizCount = 0;
        int totalPivot = 0;
        int totalCurrent = 0;
        for (int i = 0; i < playerData.DetailRanks.Count; i++)
        {
            int total = GameManager.Inst.QuizCounts[i];
            totalQuizCount += total;
            int current = Mathf.Min(playerData.DetailRanks[i], total);
            totalCurrent += current;
            scoreTexts[i].text = $"{current} / {total}";
            int pivot = total / teerSprite.Length;
            int teer = 0;
            if (current == total)
            {
                teer = Mathf.Min((teerSprite.Length - 1), current / pivot);
            }
            else
            {
                teer = Mathf.Clamp(current / pivot, 0, teerSprite.Length - 2);
            }
            teerImage[i].sprite = teerSprite[teer];
            // 현재 푼 문제수 나누기 기준 숫자(전체문제 / 티어 수 ) = 인덱스
        }
        totalPivot = totalQuizCount / total_TeerSprite.Length;
        int totalTeer = 0;
        if (totalCurrent == totalQuizCount)
        {
            totalTeer = Mathf.Min(total_TeerSprite.Length - 1, (totalCurrent / totalPivot));
        }
        else
        {
            totalTeer = Mathf.Clamp((totalCurrent / totalPivot), 0, total_TeerSprite.Length - 2);
        }
        totalScoreText.text = $"{totalCurrent} / {totalQuizCount}";
        total_TeerImage.sprite = total_TeerSprite[totalTeer];
    }
}
