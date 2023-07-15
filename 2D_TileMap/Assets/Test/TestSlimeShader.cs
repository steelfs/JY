using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSlimeShader : TestBase
{
    public float speed = 0.5f;
    float timeElapsed = 0.0f;
    readonly int thicknessId = Shader.PropertyToID("_ThickNess");
    readonly int splitId = Shader.PropertyToID("_Split");
    readonly int fadeId = Shader.PropertyToID("_Fade");

    int id;
    int disolveId;

    enum ShaderType
    {
        OutLine,
        InnerLine,
        Phase,
        PhaseReverse,
        Disolve
    }
    

    public Renderer[] renderers;
    Material[] materials;

    private void Start()
    {

        materials = new Material[renderers.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = renderers[i].material;//sharedMaterial은 원본을 가져오고 그냥 Material은 복사본을 가져온다
        }
        id = Shader.PropertyToID("_Split");
        disolveId = Shader.PropertyToID("_Fade");
    }


    Material GetMaterial(ShaderType type)
    {
        Material material = null;
        switch (type)
        {
            case ShaderType.OutLine:
                material = materials[0];
                break;
            case ShaderType.InnerLine:
                material = materials[1];
                break;
            case ShaderType.Phase:
                material = materials[2];
                break;
            case ShaderType.PhaseReverse:
                material = materials[3];
                break;
            case ShaderType.Disolve:
                material = materials[4];
                break;
        }
        return material;
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        int id = Shader.PropertyToID("_ThickNess");
        Material mat = GetMaterial(ShaderType.InnerLine);
        mat.SetFloat(id, 0);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        int id = Shader.PropertyToID("_ThickNess");
        Material mat = GetMaterial(ShaderType.InnerLine);
        mat.SetFloat(id, 0.004f);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        float num = (Mathf.Cos(timeElapsed * speed) + 1.0f) * 0.5f;

        Material inner = GetMaterial(ShaderType.InnerLine);
        inner.SetFloat(thicknessId, num * 0.025f); //최대치 0.025
        Material phase = GetMaterial(ShaderType.Phase);
        phase.SetFloat(splitId, num);
        Material phaseReverse = GetMaterial(ShaderType.PhaseReverse);
        phaseReverse.SetFloat(splitId, num);
        Material disolve = GetMaterial(ShaderType.Disolve);
        disolve.SetFloat(fadeId, num);

        //if (isIncreasing)
        //{
        //    target += (Time.deltaTime * 0.5f);
        //    if (target  > 1.15f)
        //    {
        //        isIncreasing = false;
        //    }
        //}
        //else if (!isIncreasing)
        //{
        //    target -= (Time.deltaTime * 0.5f);
        //    if (target < 0)
        //    {
        //        isIncreasing = true;
        //    }
        //}
        //Material phaseMat = GetMaterial(ShaderType.Phase);
        //phaseMat.SetFloat(id, target);
        //Material reversePhaseMat = GetMaterial(ShaderType.PhaseReverse);
        //reversePhaseMat.SetFloat(id, target);
        //Material disolveMat = GetMaterial(ShaderType.Disolve);
        //disolveMat.SetFloat(disolveId, target);



    }
    //innerLine  반복
    // Phase 핑퐁
    // Disolve
    //
}
