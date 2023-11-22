using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData
{
    Dictionary<int, int> detailRanks = new Dictionary<int, int>();//<subject(분야), score(맞춘 문제 수)>
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
    public void EditData(int data)
    {
        for (int i = 0; i < subjectScores.Length; i++)
        {
            subjectScores[i] += data;
        }
    }
}
