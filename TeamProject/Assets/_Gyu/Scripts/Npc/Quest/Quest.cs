using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    /// <summary>
    /// Npcid + ����Ʈ ��ȣQuest[]
    /// </summary>
    public int QuestIndex;

    /// <summary>
    /// Ŭ���� ���� Ȯ��
    /// </summary>
    public bool isSucess;

    /// <summary>
    /// ���� ���� Ȯ��
    /// </summary>
    public bool isProgress;

    /// <summary>
    /// ����Ʈ id
    /// </summary>
    public int Questid;

    /// <summary>
    /// ����Ʈ ����
    /// </summary>
    public string Title;

    /// <summary>
    /// ����Ʈ ����
    /// </summary>
    public string Description;

    /// <summary>
    /// ����Ʈ ��ǥ
    /// </summary>
    public string Clear;

    /// <summary>
    /// ����Ʈ Ŭ���� ����
    /// </summary>
    public QuestReward questReward;
    
    /// <summary>
    /// ����Ʈ Ŭ���� ����
    /// </summary>
    public QuestClear questClear;
}
