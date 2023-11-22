using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestClear
{
    public enum ClearType
    {
        Killcount,
        Gathering,
        Comunication
    }

    /// <summary>
    /// 퀘스트 타입
    /// </summary>
    public ClearType clearType;

    /// <summary>
    /// 클리어까지 필요한 카운트(갯수)
    /// </summary>
    public int requiredCount;

    /// <summary>
    /// 현재 카운트(갯수)
    /// </summary>
    public int currentCount;

    /// <summary>
    /// 클리어 Npcid
    /// </summary>
    public int ClearNpcid;

    public bool isSucess()
    {
        return (requiredCount <= currentCount);
    }

    // 퀘스트.Type = "적 처치하기"
    public void EnemyKilled()
    {
        if (clearType == ClearType.Killcount)
        {
            currentCount++;
        }
    }

    // 퀘스트.Type = "아이템 모으기"
    public void ItemCollected()
    {
        if(clearType == ClearType.Gathering)
        {
            currentCount++;
        }
    }
}
