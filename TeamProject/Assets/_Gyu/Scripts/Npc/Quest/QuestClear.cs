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
    /// ����Ʈ Ÿ��
    /// </summary>
    public ClearType clearType;

    /// <summary>
    /// Ŭ������� �ʿ��� ī��Ʈ(����)
    /// </summary>
    public int requiredCount;

    /// <summary>
    /// ���� ī��Ʈ(����)
    /// </summary>
    public int currentCount;

    /// <summary>
    /// Ŭ���� Npcid
    /// </summary>
    public int ClearNpcid;

    public bool isSucess()
    {
        return (requiredCount <= currentCount);
    }

    // ����Ʈ.Type = "�� óġ�ϱ�"
    public void EnemyKilled()
    {
        if (clearType == ClearType.Killcount)
        {
            currentCount++;
        }
    }

    // ����Ʈ.Type = "������ ������"
    public void ItemCollected()
    {
        if(clearType == ClearType.Gathering)
        {
            currentCount++;
        }
    }
}
