using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;
using EnumList;

/// <summary>
/// 로딩씬 관리할 클래스
/// 고려사항 :
///     1. 씬이동시 중복된 컴포넌트를 가지고있을경우 
///        싱글톤 사용시 Awake 함수에서 맴버 변수에 접근하여 로직수행시 제대로 안될수있다.
///     2. 팩토리 사용시 
/// </summary>
public class LoadingScene : MonoBehaviour
{
    /// <summary>
    /// 로딩상황을 체크하는변수
    /// </summary>
    static bool isLoading = false;
    public static bool IsLoading => isLoading;
    
    /// <summary>
    /// 배틀 맵인지 체크하는 변수 
    /// </summary>
    static bool isBattleMap = false;
    public static bool IsBattleMap => isBattleMap;

    /// <summary>
    /// 다음씬으로 넘어갈 씬이름
    /// 다음씬이 입력안되면 타이틀로넘어간다.
    /// </summary>
    //static  EnumList.SceanName nextSceanName = EnumList.SceanName.TITLE;
    static int nextSceanindex = -1;

     /// <summary>
    /// 로딩 진행도 이미지 종류
    /// EnumList 인터페이스에 정의해놓은 값을 참고한다.
    /// </summary>
    static EnumList.ProgressType progressType;

    /// <summary>
    /// 진행도를 처리할 이미지파일
    /// </summary>
    Image progressImg;

    /// <summary>
    /// 페이크 로딩 시간설정 
    /// 로딩의 최소시간이라보면된다.
    /// </summary>
    [Range(1.0f, 5.0f)]
    public float fakeTimer = 3.0f;

    /// <summary>
    /// 메인카메라에 붙어있는 시네머신 값 수정을위해 찾아두기
    /// </summary>
    static CinemachineBrain brainCamera;

    /// <summary>
    /// 씬로딩의 진행도를 보여주는씬으로 넘어가는 함수 비동기로진행
    /// 로딩씬으로 잠시 넘어갔다가 이동한다.
    /// </summary>
    /// <param name="sceneName">이동할 씬 이름</param>
    /// <param name="type">진행 상황 표기할 progressType  EnumList의 값을확인</param>
    public static void SceneLoading(EnumList.SceneName sceneName = EnumList.SceneName.TITLE, 
                EnumList.ProgressType type = EnumList.ProgressType.BAR)
    {
        if (sceneName != EnumList.SceneName.NONE) { //씬 셋팅이 되어있고
            if (!isLoading) { //로딩이 안됬을경우 
                isLoading = true;//로딩 시작플래그
                nextSceanindex = (int)sceneName; //다음씬 인덱스 셋팅하고 
                InputSystemController.Instance.DisableHotKey(HotKey_Use.None); //열려있는 액션 전부 닫고 기본만열자 
                WindowList.Instance.PopupSortManager.CloseAllWindow(); //화면 전환시 열려있는창 전부닫자.  
                if (SceneManager.GetActiveScene().buildIndex != nextSceanindex) //현재씬이아닌 다른씬갈때는 로딩창을 가도록 수정 
                {
                   
                    progressType = type; //프로그래스 타입설정.
                    SceneManager.LoadSceneAsync((int)EnumList.SceneName.LOADING);

                }
                else 
                {
                    isLoading = false; //로딩 화면전환이없음으로 바로 끄기
                    SetInputSetting();

                }

            }
        }
        
    }



    /// <summary>
    /// 로딩화면 로딩시 바로 코루틴 실행하여 다음씬에대해 비동기로 로딩을 하고
    /// 그에대한 정보를 받아온다.
    /// </summary>
    void Start()
    {
        isLoading = true; // 로딩화면에서부터 게임시작하면 필요한 구문
        SetDisavleObjects(); //열려있는창 닫아버리기
        StopAllCoroutines();//로딩이 연속으로 이러나는경우에 기존코루틴을 멈추고 새로시작한다.
        StartCoroutine(LoadSceneProcess());
        brainCamera = Camera.main.GetComponent<CinemachineBrain>();
    }


    /// <summary>
    /// 로딩창왔을때 열려있는창 닫아버리기위한 함수
    /// 내용추가 필요
    /// </summary>
    private void SetDisavleObjects() { 
        WindowList.Instance.MainWindow.gameObject.SetActive(false);
    }





    /// <summary>
    /// 로딩화면에서 다음씬이 로딩이 완료됬는지 확인하기위해 처리하는작업
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadSceneProcess()
    {
        switch ((EnumList.SceneName)nextSceanindex)
        {
            case EnumList.SceneName.TITLE:
                Time.timeScale = 1.0f;
                TitleSceneMove();       //타이틀로 갈때 데이터 초기화용 함수
                break;
            case EnumList.SceneName.ENDING:
                break;
            case EnumList.SceneName.TestBattleMap:
                break;
            case EnumList.SceneName.SpaceShip:
                GameManager.PlayerStatus.Base_Status.CurrentHP = GameManager.PlayerStatus.Base_Status.Base_MaxHP;
                break;
            default:
                break;
        }
        //비동기 씬로딩정보를 받기위해 가져오는 변수
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceanindex, LoadSceneMode.Single); //기본 single => 마지막 로딩완료된것만 열린다
                                                                                                  // additive는 여러씬이 같이열린다..
        //AsyncOperation 이 한개가있지만 여러개선언해도 하나를 참조해서 
        //allowSceneActivation 값이 공유된다.; 

        //op.allowSceneActivation 값이 true 이면 씬 로딩이 90%(0.9f)이상이 되면 자동으로 다음씬으로 넘어가진다 추가- 씬로드가완료되면 0.9값을 유지한다고한다.
        op.allowSceneActivation = false;
        //맵이 작은경우 로딩시간이 짧아서 로딩 화면을 순식간에 지나갈수있어서 일단 false 로 
        //로딩씬에서 다음씬으로 이동을 멈춰두고 페이크로딩을 밑에 제작한다.

        float timer = 0.0f; //로딩 진행도를 담을 변수 (함수를 사용했기때문에 값을 처리하기위해 사용했다.)
        float loadingTime = 0.0f; //프로그래스바 진행시간체크
        switch (progressType)
        {
            case EnumList.ProgressType.BAR:
                //진행바이미지 셋팅을위해 가져오기
                progressImg = ProgressList.Instance.GetProgress(EnumList.ProgressType.BAR, transform)
                                .transform.GetChild(0).GetComponent<Image>(); //Prefab 에 순서1번째로 지정해두었다  

                while (!op.isDone)  //isDone 으로 다음씬 로딩이 끝낫는지 체크할수있다.
                {
                    yield return null; //진행바가 바뀔수있게 제어권을 넘긴다.
                    loadingTime += Time.unscaledDeltaTime; //로딩시간 체크

                    // 진행도 표시를 바꾸려면 밑에 로직을 추가
                    if (op.progress < 0.9f) //로드 완료되면 0.9 로유지한다 1로 유지가안된다.
                    {
                        //정상 로딩 로직 
                        progressImg.fillAmount = op.progress; //화면 이미지에 진행상황값 전달
                    }
                    else
                    {
                        //페이크로딩 로직

                        timer += Time.unscaledDeltaTime; //Lerp 함수의 진행 상황저장
                        progressImg.fillAmount = Mathf.Lerp(0.9f, 1f, timer); // 0.9 ~ 1.0 사이의 데이터 표시

                        //로딩창이 너무빠르게 넘어가는것을 방지하기위해 페이크 타임 체크
                        if (fakeTimer < loadingTime) //에디터에서 페이크로딩시간을 조절한다.
                        {

                            isLoading = false;//로딩끝났다고 설정
                            op.allowSceneActivation = true; //해당 변수가 true면 progress 값이 0.9(90%)값이 넘어가는순간 다음씬을 로딩한다.
                            SetInputSetting();
                             yield break; //제어권넘기기
                        }
                    }
                }
                break;
            //로딩이미지 추가시 이위치에서 작성
            default: //타입값을 잘못입력했을경우 이곳으로 이동
                Debug.LogWarning($"{this.name} 의 프로그래스(progress)바 타입설정을 잘못했습니다. ");
                yield break;
        }



    }

    /// <summary>
    /// 씬전환시 액션값 셋팅용 함수 
    /// </summary>
    private static void SetInputSetting()
    {
        brainCamera.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;

        EnumList.SceneName nextSceanName = (EnumList.SceneName)nextSceanindex;
        switch (nextSceanName)
        {
            case EnumList.SceneName.TestBattleMap:
                Cursor.visible = true;
                InputSystemController.Instance.EnableHotKey(HotKey_Use.Use_BattleMap);
                InputSystemController.Instance.EnableHotKey(HotKey_Use.Use_InvenView);
                InputSystemController.Instance.EnableHotKey(HotKey_Use.QuickSlot);
                break;
            case EnumList.SceneName.SpaceShip:
                Cursor.visible = false;
                brainCamera.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
                InputSystemController.Instance.EnableHotKey(HotKey_Use.Use_TownMap);
                InputSystemController.Instance.EnableHotKey(HotKey_Use.Use_InvenView);
                break;
            case EnumList.SceneName.BattleShip:
                InputSystemController.Instance.EnableHotKey(HotKey_Use.Use_TownMap);
                InputSystemController.Instance.EnableHotKey(HotKey_Use.Use_InvenView);
                break;
            default:
                break;
        }

        Cursor.lockState = CursorLockMode.None;
    }
    /// <summary>
    /// 타이틀로 갈때 데이터를 초기화 하기위한 함수 
    /// </summary>
    private static void TitleSceneMove() 
    {
        SpaceSurvival_GameManager.Instance.ResetData();     //공용으로 들고있는값 초기화
        SpaceSurvival_GameManager.Instance.ShipStartPos = Vector3.zero; //저장,로드하거나 함선에서 배틀맵 넘어갈때 저장된값 초기화

        SpaceSurvival_GameManager.Instance.PlayerQuest.ResetData(); //퀘스트 데이터 초기화 

        GameManager.EquipBox.ClearEquipBox();                       // 장비 초기화 

        SkillData[] skillDatas = GameManager.SkillBox.SkillDatas;
        foreach (var skillData in skillDatas)
        {
            skillData.InitSkillData(); //기본값 돌리기 
        }

        GameManager.SlotManager.SaveFileLoadedResetSlots(); //인벤 비우고 초기화 


        GameManager.PlayerStatus.Base_Status.Init();                // 캐릭터 능력치 초기화 
        GameManager.PlayerStatus.Reset_Status();                    // 캐릭터 능력치 초기화 

        ///타이틀로 돌아갈시 배틀맵 현재정보 초기화 
        SpaceSurvival_GameManager.Instance.IsBattleMapClear = false;
        SpaceSurvival_GameManager.Instance.CurrentStage = StageList.None;
        SpaceSurvival_GameManager.Instance.StageClear = StageList.None; //스테이지 클리어정보 초기화
        Cursor.visible = true;
    }
}
