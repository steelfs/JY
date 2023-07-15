using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcess : MonoBehaviour
{
    Volume postProcessVolume;// 포스트프로세슥가 적용되는 볼륨
    Vignette vignette;// 볼륨/ Profile  안에 있는 비네트 제어용 객체

    private void Awake()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<Vignette>(out vignette);//profile에서 가져오기 없으면 null
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onLifeTimeChange += OnLifeTimeChange; 
    }

    private void OnLifeTimeChange(float ratio)//
    {
        vignette.intensity.value = 1.0f - ratio;
    }
}
