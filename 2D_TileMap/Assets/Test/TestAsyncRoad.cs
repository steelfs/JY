using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TestAsyncRoad : TestBase
{
    AsyncOperation async;//�񵿱� ��ɾ��� ���� ������ ������ Ŭ���� 

    protected override void Test1(InputAction.CallbackContext context)
    {
        // SceneManager.LoadScene(1);//(Synchronous)�������� �� �ε�
        StartCoroutine(LoadScene());
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = true;
    }
    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive); //Additive : ���� �߰��� ���� ��. Single : ������ �ε��Ϸ�� �͸� ���δ� 
        async.allowSceneActivation = false; // �񵿱�� �ҷ����� ���� ���� �� ��//���

        while(async.progress < 0.9f)// progress �� ���� 0~ 0.9 ����
        {
            Debug.Log($"Progress : {async.progress}");
            yield return null;
        }
        Debug.Log($"Loading Complete");
    }
}
