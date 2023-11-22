using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData
{
    Dictionary<int, int> detailRanks = new Dictionary<int, int>();//<subject, score>
    public Dictionary<int, int> DetailRanks => detailRanks;
    int[] subjectScores = new int[Enum.GetValues(typeof(Subject)).Length];
    public int[] SubjectScores => subjectScores;
    public PlayerData()
    {
        InitData(subjectScores);
        
    }
    public void InitData(int[] subjectScores)
    {
        for (int i = 0; i < subjectScores.Length; i++)
        {
            detailRanks[i] = subjectScores[i];
        }

    }
    public void EditData()
    {

    }
}
