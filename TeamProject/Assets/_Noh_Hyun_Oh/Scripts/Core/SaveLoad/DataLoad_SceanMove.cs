using StructList;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataLoad_SceanMove : MonoBehaviour
{
    SlotManager slotManager;
    private void Awake()
    {
        slotManager = FindObjectOfType<SlotManager>(true);
    }
    private void Start()
    {
        SaveLoadManager.Instance.loadedSceanMove = FileLoadAction;
    }
    /// <summary>
    /// 로드 눌렀을때 화면이동과 데이터 셋팅 함수
    /// </summary>
    /// <param name="data">로드된 데이터</param>
    private void FileLoadAction(JsonGameData data)
    {
        //여기에 파싱작업이필요하다 실제로사용되는 작업
        if (data != null)
        {
           
            SaveLoadManager.Instance.ParsingProcess.LoadParsing(data);
            //Debug.Log($"{data} 파일이 정상로드됬습니다 , {data.SceanName} 파싱작업후 맵이동 작성을 해야하니 맵이 필요합니다.");
            LoadingScene.SceneLoading(data.SceanName);
            if (TurnManager.Instance.TurnIndex > 0) //배틀맵에서 로드한거면 
            {
                if (data.SceanName == EnumList.SceneName.TestBattleMap) // 불러오는곳이 배틀맵이면  
                {
                    SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestReset(true);  //배틀맵 데이터 초기화 
                    SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestInit();  //데이터 리셋
                }
                else
                {
                    SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestReset();  //배틀맵 데이터 초기화 
                }
            }
            else 
            {

                SpaceSurvival_GameManager.Instance.ResetData(false);
            }
            if (SceneManager.GetActiveScene().buildIndex  == (int)EnumList.SceneName.SpaceShip //현재씬이 함선인지 체크하고  
                &&  data.SceanName == EnumList.SceneName.SpaceShip) //로드했을때 함선내부에서 같은맵로드시 맵이동이없음으로 
            {
                BattleShipInitData bsd = FindObjectOfType<BattleShipInitData>(true);
                bsd.CharcterMove(SpaceSurvival_GameManager.Instance.ShipStartPos);  //캐릭터 위치변경을 강제로 시킨다.
            }

        }
    }
 
}