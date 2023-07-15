using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTxt : MonoBehaviour
{
    TextMeshProUGUI scoreUI;
    Player player;
    int targetScore = 0;// 목표점수
    float  currentScore = 0.0f;// 현재점수

    public float scoreSpeed = 50.0f;// 점수 올라가는 속도
    private void Awake()
    {
        scoreUI = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        player = GameManager.Inst.Player;       
        currentScore = player.Score;
        targetScore = player.Score;
        scoreUI.text = $"Score : {currentScore:f0}";
        
        player.OnScoreChange += UpdateScoreTxt;
    }
    public void UpdateScoreTxt(int score)
    { 
        targetScore = score;
        
    }

    private void Update()
    {
        if (currentScore < targetScore) //타겟스코어가 현재 스코어보다 커지면 
        {
            float speed = Mathf.Max((targetScore - currentScore) * 5.0f, scoreSpeed);
           
            currentScore += Time.deltaTime * speed; //초당 스피드의 속도로 현재스코어 증가
            currentScore = Mathf.Min(currentScore, targetScore);// 현재 스코어가 타겟스코어보다 커지지 않도록하기
            scoreUI.text = $"Score : {currentScore:0f}";
        }
    }
    //델리게이트 이용해서 player의 score가 변경되면 scoreUI의 점수를 변경하는 코드 작성하기
    // 양식  "Score : 00"
}
