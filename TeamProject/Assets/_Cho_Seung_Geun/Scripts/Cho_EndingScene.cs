using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cho_EndingScene : MonoBehaviour
{
    public Transform endingEffect;

    ParticleSystem particle;
    AudioSource audioSource;

    /// <summary>
    /// 이펙트 기다리는 시간
    /// </summary>
    [SerializeField]
    float waitEffect = 2.0f;

    private void Awake()
    {
        particle = endingEffect.GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            endingEffect.gameObject.SetActive(true);
            particle.Play();
            audioSource.Play();
            StartCoroutine(Effect());
        }
    }

    IEnumerator Effect()
    {
        Vector3 value = 0.5f * Time.deltaTime * new Vector3(1, 1, 1);
        while (endingEffect.localScale.x < waitEffect)
        {
            endingEffect.localScale += value;
            yield return null;
        }
        yield return EndSceneLoading();
    }

    IEnumerator EndSceneLoading()
    {
        yield return null;
        WindowList.Instance.EndingCutImageFunc.EndingCutScene();
    }
}
