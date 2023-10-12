using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SelfDistruct : MonoBehaviour
{
    VisualEffect effect;

    private void Awake()
    {
        effect = GetComponent<VisualEffect>();
        int id = Shader.PropertyToID("Duration");

        float  duration = effect.GetFloat(id);
        Destroy(this.gameObject, duration);


    }

}
