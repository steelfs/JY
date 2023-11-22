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
    /// 현재 포탈의 스테이지 종류
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
        // 배틀끝나면 돌아올 위치값셋팅
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

        //스테이지 관련 셋팅 
        SpaceSurvival_GameManager.Instance.CurrentStage = currentStage; //이동할 스테이지 셋팅
        LoadingScene.SceneLoading(EnumList.SceneName.TestBattleMap);
    }

    /// <summary>
    /// 해당 함수 실행시켜서 해당 스테이지가 클리어 됬는지 체크한다.
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


    // e종료버튼 안됨
    // 대화 종료시 자동으로 커서 안보이게 만들기
    // 시작시 클릭 소리 끄기
    // 마을에서 아무 상호작용 없을 때 f누르면 커서 나오는 것
    // 점프에 소리 넣기
    // 이동시 소리 중첩 안되게 하기
    // 전투 시 아이템 장비해제하면 사라짐
    // 방어 장비 착용 안됨
    // 상인 템 사는 ui 변경안됨
    // 상인 대화 종료 후 f 사라짐
    // 공격이 안됨
    // 엔딩 부분 완료하기


    // 우클릭의 용도는 무엇인가?

    // 전투 승리 시 나오는 ui 손보기
    // 대화 시 npc 애니메이션 변경
    // 배틀맵에서 행동 ui 수정
}
