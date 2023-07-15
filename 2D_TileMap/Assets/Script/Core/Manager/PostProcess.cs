using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcess : MonoBehaviour
{
    Volume postProcessVolume;// ����Ʈ���μ����� ����Ǵ� ����
    Vignette vignette;// ����/ Profile  �ȿ� �ִ� ���Ʈ ����� ��ü

    private void Awake()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<Vignette>(out vignette);//profile���� �������� ������ null
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
