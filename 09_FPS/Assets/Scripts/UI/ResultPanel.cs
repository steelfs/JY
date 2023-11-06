using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    TextMeshProUGUI title;
    TextMeshProUGUI killCountText;
    TextMeshProUGUI timeText;
    Button restartButton;

    private void Awake()
    {
        title = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        killCountText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        timeText = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        restartButton = transform.GetChild(5).GetComponent<Button>();
        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
   

    }
    public void Open(int killCount, bool isClear, float liveTime)
    {
        gameObject.SetActive(true);
        if (isClear)
        {
            title.text = "Game Clear";
        }
        else
        {
            title.text = "Game Over";
        }
        killCountText.text = killCount.ToString();
        timeText.text = $"{liveTime:f2}";
        Cursor.lockState = CursorLockMode.None;
    }

}
