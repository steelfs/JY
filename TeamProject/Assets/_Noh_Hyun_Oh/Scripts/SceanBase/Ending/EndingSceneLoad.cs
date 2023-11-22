using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingSceneLoad : MonoBehaviour
{
    Image backGroundImg;

    [Range(0.0f,3.0f)]
    [SerializeField]
    float cutImgTime = 1.0f;

    [SerializeField]
    AsyncOperation ao;

    public AsyncOperation AsyncSceneLoader 
    {
        get 
        {
            return ao ??= getAsyncSceneLoader?.Invoke();
        }
    }
    public Func<AsyncOperation> getAsyncSceneLoader;
    private void Start()
    {
        backGroundImg =  WindowList.Instance.transform.GetChild(WindowList.Instance.transform.childCount-1).GetComponent<Image>();
    }

    public void EndingCutScene() 
    {
        StartCoroutine(BackImageCutOut());
    }

    IEnumerator BackImageCutOut() 
    {
        float timeElaspad = 0.0f;
        while (timeElaspad < cutImgTime)
        {
            timeElaspad += Time.deltaTime;
            backGroundImg.color = new Color( 0,0,0,timeElaspad / cutImgTime);
            yield return null;
        }
        backGroundImg.color = Color.black;
        AsyncSceneLoader.allowSceneActivation = true;
        //LoadingScene.SceneLoading(EnumList.SceneName.ENDING);
    }
    public void ReSetImage() 
    {
        backGroundImg.color = Color.clear;
    }
}
