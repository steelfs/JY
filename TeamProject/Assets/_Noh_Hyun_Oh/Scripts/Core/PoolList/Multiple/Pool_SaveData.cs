using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���嵥����ȭ�鿡 ������ ������Ʈ ����Ŭ����
/// ������ƮǮ�� ���ٰ� ���۾��ϴ°��� �ƴϴ�.
/// </summary>
public class Pool_SaveData : Base_Pool_Multiple<SaveGameObject>
{

    /// <summary>
    /// Ǯ�� ������ �θ���ġ�� �ٲٱ����� �߰���
    /// �Լ��� Ǯ�� �ʱ�ȭ �ϰ����� �����۾����ʿ��Ұ�� ����ϸ�ȴ�.
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
