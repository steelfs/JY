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
        talkData.Add(1000, new string[] { "1000Npc �亯1" });
        talkData.Add(2000, new string[] { "2000NPC �亯2" });
        talkData.Add(3000, new string[] { "3000NpC �亯3" });

    }

    public string GetTalk(int id)
    {
        return talkData[id][0];
    }
}
