using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuest : MonoBehaviour
{
    public static PlayerQuest instance;

    public int questCount = 0;  // 퀘스트 갯수

    public Quest myquest;   // 퀘스트 1개만 받기
    //public List<Quest> myquests;  // 퀘스트 여러개 받기

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    


}
