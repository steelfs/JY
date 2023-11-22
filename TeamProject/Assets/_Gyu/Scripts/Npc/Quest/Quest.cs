using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    /// <summary>
    /// Npcid + 퀘스트 번호Quest[]
    /// </summary>
    public int QuestIndex;

    /// <summary>
    /// 클리어 여부 확인
    /// </summary>
    public bool isSucess;

    /// <summary>
    /// 진행 여부 확인
    /// </summary>
    public bool isProgress;

    /// <summary>
    /// 퀘스트 id
    /// </summary>
    public int Questid;

    /// <summary>
    /// 퀘스트 제목
    /// </summary>
    public string Title;

    /// <summary>
    /// 퀘스트 내용
    /// </summary>
    public string Description;

    /// <summary>
    /// 퀘스트 목표
    /// </summary>
    public string Clear;

    /// <summary>
    /// 퀘스트 클리어 보상
    /// </summary>
    public QuestReward questReward;
    
    /// <summary>
    /// 퀘스트 클리어 조건
    /// </summary>
    public QuestClear questClear;
}
