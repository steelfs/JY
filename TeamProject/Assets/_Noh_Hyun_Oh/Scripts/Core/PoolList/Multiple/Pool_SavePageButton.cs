using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_SavePageButton : Base_Pool_Multiple<SavePageButton_PoolObj>
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
                                                    GetChild(1). //PageListAndButton
                                                    GetChild(1); //PageNumber
    }


 }
