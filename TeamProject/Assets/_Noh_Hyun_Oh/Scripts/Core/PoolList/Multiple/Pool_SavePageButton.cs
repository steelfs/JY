using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_SavePageButton : Base_Pool_Multiple<SavePageButton_PoolObj>
{
    /// <summary>
    /// 풀이 생성될 부모위치를 바꾸기위해 추가함
    /// 함수는 풀이 초기화 하가전에 사전작업이필요할경우 사용하면된다.
    /// </summary>
    protected override void StartInitialize()
    {
        
        setPosition = FindObjectOfType<SaveWindowManager>(true).transform.
                                                    GetChild(0). //ContentParent
                                                    GetChild(0). //Contents
                                                    GetChild(0). //SaveLoadWindow
                                                    GetChild(1). //PageListAndButton
                                                    GetChild(1); //PageNumber
    }


 }
