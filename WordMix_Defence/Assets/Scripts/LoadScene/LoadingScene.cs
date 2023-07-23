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
    public string nextSceneName = "SeemLessBase"; //������ �ε��� ���� �̸�

    AsyncOperation async;//�񵿱� ��� ó����

    float loadRatio;//�ε����� value �� ��ǥ�� �ϴ� ��

    public float loadingBarSpeed = 1.0f;//�ε��ٰ� �����ϴ� �ӵ� 
    bool loadingDone = false;

    IEnumerator loadingTextCoroutine;//Loading Text��ȭ�� �ش� �ڷ�ƾ�� ������ų�� ���

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
