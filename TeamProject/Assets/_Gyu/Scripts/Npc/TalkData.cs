using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(1000, new string[] { "1000Npc 답변1" });
        talkData.Add(2000, new string[] { "2000NPC 답변2" });
        talkData.Add(3000, new string[] { "3000NpC 답변3" });

    }

    public string GetTalk(int id)
    {
        return talkData[id][0];
    }
}
