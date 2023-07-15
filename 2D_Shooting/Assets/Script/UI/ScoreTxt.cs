using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTxt : MonoBehaviour
{
    TextMeshProUGUI scoreUI;
    Player player;
    int targetScore = 0;// ��ǥ����
    float  currentScore = 0.0f;// ��������

    public float scoreSpeed = 50.0f;// ���� �ö󰡴� �ӵ�
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
        if (currentScore < targetScore) //Ÿ�ٽ��ھ ���� ���ھ�� Ŀ���� 
        {
            float speed = Mathf.Max((targetScore - currentScore) * 5.0f, scoreSpeed);
           
            currentScore += Time.deltaTime * speed; //�ʴ� ���ǵ��� �ӵ��� ���罺�ھ� ����
            currentScore = Mathf.Min(currentScore, targetScore);// ���� ���ھ Ÿ�ٽ��ھ�� Ŀ���� �ʵ����ϱ�
            scoreUI.text = $"Score : {currentScore:0f}";
        }
    }
    //��������Ʈ �̿��ؼ� player�� score�� ����Ǹ� scoreUI�� ������ �����ϴ� �ڵ� �ۼ��ϱ�
    // ���  "Score : 00"
}
