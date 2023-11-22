using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 배틀맵 관련 초기화 하는 컴퍼넌트
/// </summary>

public class InitCharcterSetting : MonoBehaviour
{
    /// <summary>
    /// 팀은 갯수 
    /// </summary>
    [SerializeField]
    int teamLength = 2;


    /// <summary>
    /// 턴을 진행할 팀 배열
    /// </summary>
    ITurnBaseData[] teamArray;


    /// <summary>
    /// 미니맵 카메라 가져오기 미완성 - 나중에 타겟도 옮겨야되서 수정이 필요하다 
    /// </summary>
    MiniMapCamera miniCam;

    /// <summary>
    /// 미니맵과 마찬가지로 나중에 타겟이 바뀌면 이것도 바껴야됨으로 수정이 필요 
    /// </summary>
    CameraOriginTarget cameraOriginTarget;


    CinemachineBrain brainCam;

    Camera_Move cameraMoveComp;

    /// <summary>
    /// 초기화하기위해 연결
    /// </summary>
    Transform battleActionButtons;
    private void Awake()
    {
        miniCam = FindObjectOfType<MiniMapCamera>(true);
        cameraOriginTarget = FindObjectOfType<CameraOriginTarget>(true);
        cameraMoveComp = FindObjectOfType<Camera_Move>(true);
        brainCam = Camera.main.GetComponent<CinemachineBrain>();//브레인 카메라 찾고 
        cameraMoveComp.GetCineBrainCam = () => brainCam; //브레인카메라 찾아서 연결해주기
        cameraMoveComp.gameObject.SetActive(true); //카메라 이동은 배틀맵에서만 사용하기때문에 따로안뺏다.


    }
    private void Start()
    {
        TestInit();
        
    }
    /// <summary>
    /// 테스트용 데이터 생성 
    /// </summary>
    public void TestInit()
    {
        SpaceSurvival_GameManager.Instance.GetBattleMapInit = () => this; //배틀맵 데이터 연결
        if (teamArray == null) 
        {
            teamArray = new ITurnBaseData[teamLength]; //배열 크기잡고
        }
        if (TurnManager.Instance.IsViewGauge) // 게이지 보여줄지 체크해서 
        {
            WindowList.Instance.TurnGaugeUI.gameObject.SetActive(true); //보여주면 표시
        }
        ITurnBaseData tbo; //임시로 담을 변수 선언하고 
        for (int i = 0; i < teamLength; i++)
        {
            if (i == 0) // 첫번째는 무조껀 아군 턴을 집어넣고 
            {
                tbo = (PlayerTurnObject)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_PLAYER_POOL); //
                tbo.UnitBattleIndex = i; 
                tbo.gameObject.name = $"Player_Team_{i}";
                tbo.InitData();

                miniCam.player = tbo.CharcterList[0].transform; //미니맵 활성화용 수정필요 
                cameraOriginTarget.Target = tbo.CharcterList[0].transform;
               
            }
            else //그이외에는 몬스터나 중립 을 넣으면된다 조건문 추가필요 
            {
                tbo = (EnemyTurnObject)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_ENEMY_POOL); //가져오고
                tbo.UnitBattleIndex = i;
                tbo.gameObject.name = $"ENEMY_Team_{i}";
                tbo.InitData();
                
            }
            //tbo.TurnActionValue = 10.0f; // -테스트값 설정
            teamArray[i] = tbo;
        }

        //데이터 초기화끝나면 턴시작 

        battleActionButtons = WindowList.Instance.BattleActionButtons;

        int childCount = battleActionButtons.childCount;

        for (int i = 0; i < childCount; i++)
        {
            battleActionButtons.GetChild(i).gameObject.SetActive(true);
        }
        miniCam.gameObject.SetActive(true); //미니맵 활성화 수정필요 
        cameraOriginTarget.gameObject.SetActive(true);


        TurnManager.Instance.InitTurnData(teamArray);
        
        GameManager.QuickSlot_Manager.gameObject.SetActive(true);

        WindowList.Instance.Gyu_QuestManager.InitDataSetting();
    }

    /// <summary>
    /// 배틀맵 데이터를 초기화 하는 함수
    /// </summary>
    /// <param name="isBattleLoaded">씬이동이 배틀맵에서 배틀맵으로 데이터 초기화시 체크하는 변수</param>
    public void TestReset(bool isBattleLoaded = false) 
    {
        miniCam.gameObject.SetActive(false); //미니맵 활성화 수정필요 
        cameraOriginTarget.gameObject.SetActive(false);
        WindowList.Instance.TeamBorderManager.UnView(); //팀 상시 유아이 끄기 

        int childCount = battleActionButtons.childCount;
        for (int i = 0; i < childCount; i++)
        {
            battleActionButtons.GetChild(i).gameObject.SetActive(false); //배틀맵 액션버튼 끄기 
        }
     

        TurnManager.Instance.ResetBattleData(); //턴데이터 초기화 
        if (TurnManager.Instance.IsViewGauge) // 게이지 보여줄지 체크해서 
        {
            WindowList.Instance.TurnGaugeUI.gameObject.SetActive(false); //비활성화 처리
        }
        SpaceSurvival_GameManager.Instance.ResetData(isBattleLoaded);
    }

}
     //UI 끄기
        //for (int i = 1; i<uiParent.childCount - 1; i++)
        //{
        //    for (int j = 0; j<uiParent.GetChild(i).childCount; j++)
        //    {
        //        uiParent.GetChild(i).GetChild(j).gameObject.SetActive(false); //액션버튼 끄기 
        //    }
        //}
        //turnGaugeUI.gameObject.SetActive(false); //턴게이지 끄기

/*======================================== 테스트용 ==================================================*/


/// <summary>
/// 턴리스트의 값확인용
/// </summary>
//public void ViewTurnList()
//{
//    foreach (ITurnBaseData j in turnObjectList)
//    {
//        Debug.Log($"{j} 값 : {j.TurnActionValue}");
//    }
//    Debug.Log(turnObjectList.Count);
//}
///// <summary>
///// 현재 진행중인 턴유닛 찾아오기 
///// </summary>
///// <returns></returns>
//public ITurnBaseData GetNode()
//{
//    ITurnBaseData isTurnNode = null;
//    foreach (ITurnBaseData node in turnObjectList)
//    {
//        if (node.TurnEndAction != null) //현재 턴진행중이면 endAction 이 등록되있다 
//        {
//            Debug.Log(node.transform.name);
//            isTurnNode = node;//진행중인 노드 담아서 
//        }
//    }
//    return isTurnNode;//반환
//}
///// <summary>
///// 테스트용 랜덤한 유닛 가져오기 
///// </summary>
///// <returns></returns>
//public ITurnBaseData RandomGetNode()
//{
//    ITurnBaseData isRandNode = null;
//    int randomIndex = UnityEngine.Random.Range(0, turnObjectList.Count); //리스트의 랜덤한 인덱스 값 가져오기 
//    LinkedListNode<ITurnBaseData> node = turnObjectList.First;
//    for (int i = 0; i < turnObjectList.Count; i++)
//    {
//        if (randomIndex == i)
//        {
//            isRandNode = node.Value;
//            break;
//        }
//        node = node.Next;
//    }
//    return isRandNode;
//}
//}
