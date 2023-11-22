using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cho_SceneLoading : MonoBehaviour
{
    public GameObject prop;
    public GameObject clearLight;
    Material clearRay;

    InteractionUI interaction;
    Cho_PlayerMove player;

    ParticleSystem shortRay;
    ParticleSystem longRay;
    AudioSource audioSource;
    SphereCollider sphereCollider;

    /// <summary>
    /// ���� ��Ż�� �������� ����
    /// </summary>
    [SerializeField]
    StageList currentStage;

    private void Awake()
    {
        prop.SetActive(false);
        clearRay = clearLight.GetComponent<SkinnedMeshRenderer>().materials[0];

        interaction = FindObjectOfType<InteractionUI>();
        player = FindObjectOfType<Cho_PlayerMove>();
        Transform parent = transform.parent;
        shortRay = parent.GetChild(0).GetComponent<ParticleSystem>();
        longRay = parent.GetChild(5).GetComponent<ParticleSystem>();
        longRay.Stop();
        audioSource = GetComponent<AudioSource>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        if (IsStageClear())
        {
            shortRay.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IsStageClear())
            {
                player.interaction = Warp;
                interaction.visibleUI?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.interaction = null;
            interaction.invisibleUI?.Invoke();
            if (IsStageClear())
            {
                shortRay.Stop();
                StartCoroutine(ClearLightVisible());
            }
        }
    }

    private void Warp()
    {
        player.Controller.enabled = false;
        player.transform.position = shortRay.transform.position;
        player.Controller.enabled = true;
        // ��Ʋ������ ���ƿ� ��ġ������
        SpaceSurvival_GameManager.Instance.ShipStartPos = player.transform.position;
        StartCoroutine(WarpCoroutine());
    }

    IEnumerator WarpCoroutine()
    {
        player.InputActions.Disable();
        sphereCollider.enabled = false;
        interaction.invisibleUI?.Invoke();
        player.interaction = null;
        player.Cinemachine.Priority = 20;
        audioSource.Play();
        shortRay.Stop();
        longRay.Play();
        yield return new WaitForSeconds(3.0f);

        //�������� ���� ���� 
        SpaceSurvival_GameManager.Instance.CurrentStage = currentStage; //�̵��� �������� ����
        LoadingScene.SceneLoading(EnumList.SceneName.TestBattleMap);
    }

    /// <summary>
    /// �ش� �Լ� ������Ѽ� �ش� ���������� Ŭ���� ����� üũ�Ѵ�.
    /// </summary>
    /// <returns></returns>
    private bool IsStageClear()
    {
        return (SpaceSurvival_GameManager.Instance.StageClear & currentStage) > 0;
    }

    IEnumerator ClearLightVisible()
    {
        float value = 0.2705882f * Time.fixedDeltaTime * 0.75f;
        float alpha = 0.0f;
        while (clearRay.GetColor("_TintColor").a < 0.2705882f)
        {
            alpha += value;
            clearRay.SetColor("_TintColor", new Color(0.09077621f, 0.5367647f, 0.4444911f, alpha));
            yield return null;
        }
        prop.SetActive(true);
    }


    // e�����ư �ȵ�
    // ��ȭ ����� �ڵ����� Ŀ�� �Ⱥ��̰� �����
    // ���۽� Ŭ�� �Ҹ� ����
    // �������� �ƹ� ��ȣ�ۿ� ���� �� f������ Ŀ�� ������ ��
    // ������ �Ҹ� �ֱ�
    // �̵��� �Ҹ� ��ø �ȵǰ� �ϱ�
    // ���� �� ������ ��������ϸ� �����
    // ��� ��� ���� �ȵ�
    // ���� �� ��� ui ����ȵ�
    // ���� ��ȭ ���� �� f �����
    // ������ �ȵ�
    // ���� �κ� �Ϸ��ϱ�


    // ��Ŭ���� �뵵�� �����ΰ�?

    // ���� �¸� �� ������ ui �պ���
    // ��ȭ �� npc �ִϸ��̼� ����
    // ��Ʋ�ʿ��� �ൿ ui ����
}
