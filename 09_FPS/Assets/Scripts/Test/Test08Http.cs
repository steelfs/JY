using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class Test08Http : TestBase
{
    string url = "https://atentsexample.azurewebsites.net/monster";
    string imageUrl = "https://biz.heraldcorp.com/view.php?ud=20230107000106";
    public GameObject plane;
    protected override void Test1(InputAction.CallbackContext context)
    {
        StartCoroutine(GetImage());
    }
    IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);//url에 있는 데이터를 가져오는 HTTP 요청 만들기
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            Debug.Log(json);
        }

        //string result = www.
    }
    IEnumerator GetImage()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.downloadHandler.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            plane.GetComponent<Renderer>().material.mainTexture = texture;
        }
    }
}
