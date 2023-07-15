using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public float alphaChangeSpeed = 1.0f;

    CanvasGroup canvasGroup;

    TextMeshProUGUI PlayTimeText;
    TextMeshProUGUI killCountText;
    Button reStartButton;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        reStartButton = GetComponentInChildren<Button>();
        PlayTimeText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        killCountText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StopAllCoroutines();
        Player player = GameManager.Inst.Player;
        player.onDie += OnPlayerDie;
        reStartButton.onClick.AddListener(() => StartCoroutine(WaitUnLoadAll()));
    }

    IEnumerator WaitUnLoadAll()
    {
        WorldManager world = GameManager.Inst.WorldManager;
        while (!world.IsUnLoadAll)//모든 씬이 UnLoad될 때까지 대기
        {
            yield return null;
        }
        SceneManager.LoadScene("LoadingScene");
    }

    private void OnPlayerDie(float playTime, int killCount)
    {
        PlayTimeText.text = $"Play Time \n\r{playTime :f1} Sec"; // \n  = 해당 위치에서 줄바꿈   \r = 
        killCountText.text = $"Kill Count \n\r {killCount} Kill";
        StartCoroutine(StartAlphaCghange());
    }
    IEnumerator StartAlphaCghange()//canvasGroup 컴포넌트 알파값 서서히 조정하는 코루틴
    {
        while(canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.interactable = true;//상호작용 활성화
        canvasGroup.blocksRaycasts = true; //UI가 레이케스트를 막도록(감지)
    }
}
