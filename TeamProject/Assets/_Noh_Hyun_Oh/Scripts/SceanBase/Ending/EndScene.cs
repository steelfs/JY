using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndScene : MonoBehaviour
{
    CanvasGroup cg;
    [SerializeField]
    [Range(1.0f, 5.0f)]
    float fadeOutTime = 3.0f;
    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();

    }
    private void Start()
    {
        WindowList.Instance.EndingCutImageFunc.ReSetImage();
    }
    public void AppendInputAnykey()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut() 
    {
        float timeElaspad = 0.0f;
        InputSystemController.InputSystem.Common.AnyKey.performed += OnTitleMove;
        while (timeElaspad < fadeOutTime) 
        {
            timeElaspad += Time.deltaTime;
            cg.alpha = timeElaspad / fadeOutTime;
            yield return null;
        }
        cg.alpha = 1.0f;
    }
    private void OnDisable()
    {
        InputSystemController.InputSystem.Common.AnyKey.performed -= OnTitleMove;
    }
    private void OnTitleMove(InputAction.CallbackContext context)
    {

        LoadingScene.SceneLoading(EnumList.SceneName.TITLE);
    }

}