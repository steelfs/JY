using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuest : MonoBehaviour
{
    public static PlayerQuest instance;

    public int questCount = 0;  // ����Ʈ ����

    public Quest myquest;   // ����Ʈ 1���� �ޱ�
    //public List<Quest> myquests;  // ����Ʈ ������ �ޱ�

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    


}
