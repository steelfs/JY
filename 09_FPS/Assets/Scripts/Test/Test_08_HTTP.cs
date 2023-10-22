using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Test_08_HTTP : TestBase
{
    public TextMeshProUGUI resultText;
    public Image resultImage;
    readonly string url = "https://atentsexample.azurewebsites.net/monster";
    //readonly string url2 = "https://www.google.com";
    private string apiUrl = "https://yesno.wtf/api";

    protected override void Test1(InputAction.CallbackContext context)
    {
        StartCoroutine(GetData());
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        StartCoroutine(GetYesNoAnswer());
    }
    IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(url); // url에 있는 데이터를 가져오는 HTTP 요청 만들기
        yield return www.SendWebRequest();              // 요청을 보내고 돌아 올때까지 대기

        if( www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            Debug.Log(json);
        }
    }
    IEnumerator GetYesNoAnswer()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                string result = webRequest.downloadHandler.text;
                string[] arr = result.Split('"');//1, 9
                string answer = arr[3];
                string imagePath = arr[9];
                resultText.text = answer;
                Debug.Log("Response: " + webRequest.downloadHandler.text);
                using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imagePath))
                {
                    yield return uwr.SendWebRequest();

                    if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.Log(uwr.error);
                    }
                    else
                    {
                        // Get downloaded texture once the web request completes
                        Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                        // Set the sprite to the Image component
                        resultImage.sprite = sprite;
                    }
                }
            }
        }
    }
    //text로 UI출력하기
}
