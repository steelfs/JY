using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Action<int> onTimeChange;// �� ������ �ð��� ������� �� ����� ��������Ʈ

    float elapsedTime;//Ÿ�̸Ӱ� ������ ������ ���� �ð�
    public float ElapsedTime => elapsedTime;

    int visibleTime = 0;//elapsedTime �� �ڿ��� �κ�, �ʰ� ����Ǿ����� Ȯ�� 

    IEnumerator timeProcesscoroutine;

    private void Start()
    {
        timeProcesscoroutine = timeProcess();

        GameManager manager = GameManager.Inst;
        manager.onGameReady += TimerReset;
        manager.onGamePlay += TimerReset;
        manager.onGamePlay += Play;
        manager.onGameClear += Stop;
        manager.onGameOver += Stop;
    }

    public void Play()//Ÿ�̸��� �ð������� �����ϴ� �Լ� 
    {
        StartCoroutine(timeProcesscoroutine);
    }
    IEnumerator timeProcess()
    {
        while (true)
        {
            elapsedTime += Time.deltaTime;
            if ((int)elapsedTime != visibleTime)
            {
                visibleTime = (int)elapsedTime;
                onTimeChange?.Invoke(visibleTime);
            }
            yield return null;
        }
    }
    public void Stop()//Ÿ�̸��� �ð������� �����ϴ� �Լ� 
    {
        StopCoroutine(timeProcesscoroutine);
    }
    void TimerReset()//Ÿ�̸��� �ð������� �ʱ�ȭ �� �����ϴ� �Լ� 
    {
        elapsedTime = 0.0f;
        onTimeChange?.Invoke(0);
        StopCoroutine(timeProcesscoroutine);
    }
}
