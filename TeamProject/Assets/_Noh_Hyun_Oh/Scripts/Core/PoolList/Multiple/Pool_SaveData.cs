using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 저장데이터화면에 보여줄 오브젝트 생산클래스
/// 오브젝트풀에 없다고 동작안하는것은 아니다.
/// </summary>
public class Pool_SaveData : Base_Pool_Multiple<SaveGameObject>
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
                                                    GetChild(0). //SaveFileList
                                                    GetChild(0). //Scroll View
                                                    GetChild(0). //Viewport
                                                    GetChild(0);//Content 

    }


  

}
