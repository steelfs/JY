using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Action<int> onTimeChange;// 초 단위로 시간이 변경됐을 때 실행될 델리게이트

    float elapsedTime;//타이머가 측정을 시작한 이후 시간
    public float ElapsedTime => elapsedTime;

    int visibleTime = 0;//elapsedTime 의 자연수 부분, 초가 변경되었는지 확인 

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

    public void Play()//타이머의 시간측정을 시작하는 함수 
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
    public void Stop()//타이머의 시간측정을 정지하는 함수 
    {
        StopCoroutine(timeProcesscoroutine);
    }
    void TimerReset()//타이머의 시간측정을 초기화 후 정지하는 함수 
    {
        elapsedTime = 0.0f;
        onTimeChange?.Invoke(0);
        StopCoroutine(timeProcesscoroutine);
    }
}
