using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TestAsyncRoad : TestBase
{
    AsyncOperation async;//비동기 명령어의 여러 정보를 가지는 클래스 

    protected override void Test1(InputAction.CallbackContext context)
    {
        // SceneManager.LoadScene(1);//(Synchronous)동기방식의 씬 로딩
        StartCoroutine(LoadScene());
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = true;
    }
    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive); //Additive : 씬을 추가로 만들어서 엶. Single : 마지막 로딩완료된 것만 보인다 
        async.allowSceneActivation = false; // 비동기로 불러오는 씬을 열지 말 것//대기

        while(async.progress < 0.9f)// progress 의 범위 0~ 0.9 사이
        {
            Debug.Log($"Progress : {async.progress}");
            yield return null;
        }
        Debug.Log($"Loading Complete");
    }
}
