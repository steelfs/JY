using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingScene : MonoBehaviour
{
    public string nextSceneName = "SeemLessBase"; //다음에 로딩할 씬의 이름

    AsyncOperation async;//비동기 명령 처리용

    float loadRatio;//로딩바의 value 가 목표로 하는 값

    public float loadingBarSpeed = 1.0f;//로딩바가 증가하는 속도 
    bool loadingDone = false;

    IEnumerator loadingTextCoroutine;//Loading Text변화용 해당 코루틴을 정지시킬때 사용

    Slider Gauge;
    TextMeshProUGUI loadingText;
    PlayerInputAction inputAction;
    private void Awake()
    {  
        inputAction = new ();

    }

    private void OnEnable()
    {
        inputAction.UI.Enable();
        inputAction.UI.Click.performed += Press;
        inputAction.UI.AnyKey.performed += Press;
    }
    private void OnDisable()
    {
        inputAction.UI.Click.performed -= Press;
        inputAction.UI.AnyKey.performed -= Press;
        inputAction.UI.Disable();
    }
    private void Press(InputAction.CallbackContext context)
    {
        if (loadingDone)
        {
            async.allowSceneActivation = true;
        }
    }

    private void Start()
    {
        Gauge = FindObjectOfType<Slider>();
        loadingText = FindObjectOfType<TextMeshProUGUI>();

        loadingTextCoroutine = LoadindTextProgress();
        StartCoroutine(loadingTextCoroutine);
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadindTextProgress()
    {
        float waitTime = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(waitTime);

        string[] texts = { "Loading", "Loading .", "Loading . .", "Loading . . .", "Loading . . . .", "Loading . . . . ." };

        int index = 0;

        while (true)
        {
            loadingText.text = texts[index];
            index++;
            index %= texts.Length;

            yield return wait;
        }
    }

    IEnumerator LoadScene()
    {
        Gauge.value = 0.0f;
        loadRatio = 0.0f;

        async = SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false;

        while(loadRatio < 1.0f)
        {
            loadRatio = async.progress + 0.1f;
            yield return null;
        }

        yield return new WaitForSeconds((loadRatio - Gauge.value) / loadingBarSpeed);

        StopCoroutine(loadingTextCoroutine);
        loadingDone = true;
        loadingText.text = "Loading \n Complete!";

    }

    private void Update()
    {
        if (Gauge.value < loadRatio)
        {
            Gauge.value += (Time.deltaTime * loadingBarSpeed);
        }
    }


}
