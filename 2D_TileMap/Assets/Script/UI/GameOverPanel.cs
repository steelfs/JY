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
        while (!world.IsUnLoadAll)//��� ���� UnLoad�� ������ ���
        {
            yield return null;
        }
        SceneManager.LoadScene("LoadingScene");
    }

    private void OnPlayerDie(float playTime, int killCount)
    {
        PlayTimeText.text = $"Play Time \n\r{playTime :f1} Sec"; // \n  = �ش� ��ġ���� �ٹٲ�   \r = 
        killCountText.text = $"Kill Count \n\r {killCount} Kill";
        StartCoroutine(StartAlphaCghange());
    }
    IEnumerator StartAlphaCghange()//canvasGroup ������Ʈ ���İ� ������ �����ϴ� �ڷ�ƾ
    {
        while(canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.interactable = true;//��ȣ�ۿ� Ȱ��ȭ
        canvasGroup.blocksRaycasts = true; //UI�� �����ɽ�Ʈ�� ������(����)
    }
}
