using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어 턴 
/// </summary>
public class PlayerTurnObject : TurnBaseObject
{

    /// <summary>
    /// 테스트용 변수 
    /// </summary>
    [SerializeField]
    int testPlayerLength = 1;

    /// <summary>
    /// 배틀맵에서 이벤트핸들러를 정의할 컴포넌트
    /// </summary>
    [SerializeField]
    BattleMap_Player_Controller bpc;

    [SerializeField]
    CameraOriginTarget cot;

    /// <summary>
    /// 캐릭터 데이터는 외부에서 셋팅하기때문에 해당 델리게이트 연결해줘야함
    /// </summary>
    public Func<ICharcterBase[]> initPlayer;

    MiniMapCamera miniMapCam;

    /// <summary>
    /// 데이터 초기화 함수
    /// </summary>
    public override void InitData()
    {
        //InputSystemController.InputSystem
        //해당오브젝트는 팩토리에서 생성하지만 
        bpc = FindObjectOfType<BattleMap_Player_Controller>();   // 컨트롤러는 배틀맵에서만 있는 컴포넌트라서 초기화 할때 찾아온다
        cot = FindObjectOfType<CameraOriginTarget>(true);        // 컨트롤러는 배틀맵에서만 있는 컴포넌트라서 초기화 할때 찾아온다
        miniMapCam = FindObjectOfType<MiniMapCamera>(true);      // 컨트롤러는 배틀맵에서만 있는 컴포넌트라서 초기화 할때 찾아온다
        bpc.onClickPlayer = OnClickPlayer;                       // 타일을 클릭했을때 플레이어 가있는타일(타일속성이 몬스터) 이면 실행될 함수를 연결한다. 
        //bpc.onClickItem = OnClickItem;                           // 타일을 클릭했을때 아이템이 있는 타일이면 실행될 함수를 연결한다.
        bpc.onMoveActive = OnUnitMove;                           // 타일을 클릭했을때 플레이어가 움직이도록 로직연결
        bpc.GetPlayerTurnObject = () => this;                    // 초기값 데이터 연결 

        SpaceSurvival_GameManager.Instance.GetPlayerTeam  = () =>  charcterList.Cast<BattleMapPlayerBase>().ToArray(); //일반포문돌려서 새로배열만들어서 집어넣는방법을하는게 조금더 가볍다.
        if (initPlayer != null) //외부 함수가 연결되 있으면
        {
            ICharcterBase[] playerList = initPlayer(); //데이터 요청을 하고 
            if (playerList != null && playerList.Length > 0) //데이터가 존재하면  
            {
                foreach (ICharcterBase player in playerList) //데이터 갯수만큼 
                {
                    charcterList.Add(player); //턴관리할 캐릭터로 셋팅
                    player.GetCurrentTile = () => SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Charcter); //타일 셋팅연결
                    player.transform.position = player.CurrentTile.transform.position;//셋팅된 타일위치로 이동시킨다.
                }
                WindowList.Instance.TeamBorderManager.ViewTeamInfo(playerList.Length);//팀 상시 유아이 보여주기 
            }
            else
            {
                Debug.LogWarning($"{name} 오브젝트호출  \n 외부 플레이어 데이터가 셋팅이 안되있습니다.");
            }
        }
        else //외부함수가 연결안되있는경우  
        {
            BattleMapPlayerBase go;
            //테스트 데이터 생성
            for (int i = 0; i < testPlayerLength; i++)//캐릭터들 생성해서 셋팅 
            {
                go = (BattleMapPlayerBase)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.CHARCTER_PLAYER_POOL);
                charcterList.Add(go);
                go.name = $"Player_{i}";
                go.SetTile(SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Charcter));
                go.transform.position = go.CurrentTile.transform.position; //셋팅된 타일위치로 이동시킨다.
                bpc.onAttackAction = (_,_) => { 
                    go.CharcterData.SkillPostProcess(); 
                };
                go.onMoveRangeClear = (currentTile, currentMoveSize) => {
                    if (SpaceSurvival_GameManager.Instance.MoveRange != null) 
                    {
                        SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(currentTile);
                        SpaceSurvival_GameManager.Instance.MoveRange.MoveSizeView(currentTile, currentMoveSize);//이동범위표시해주기 
                    }
                };

            }
            WindowList.Instance.TeamBorderManager.ViewTeamInfo(testPlayerLength); //팀 상시 유아이 보여주기 

        }


    }

    /// <summary>
    /// 턴메니저에서 자신의 턴일때 실행해주는 함수 
    /// </summary>
    public override void TurnStartAction()
    {
        isTurn = true; // 자신의 턴인지 체크한다. 해제는 델리게이트에 연결해두었고 델리는 턴종료버튼 에서 실행된다.
        //Debug.Log($"{name} 오브젝트는 턴이 시작되었다 행동력 : {TurnActionValue}");
        currentUnit = charcterList[0]; //플레이어 설정을하고 
        currentUnit.IsControll = true; //컨트롤 할수있게 설정한다.
        cot.Target = currentUnit.transform; //카메라 포커스 맞추기 

        //캐릭터쪽으로 스테미나 데이터 넘기기
        BattleMapPlayerBase currentCharcter = (BattleMapPlayerBase)currentUnit;
        Base_Status playerData = GameManager.PlayerStatus.Base_Status;
        playerData.Current_Stamina += TurnActionValue;
        float moveSize = currentUnit.MoveSize < TurnActionValue ? currentUnit.MoveSize : TurnActionValue;//이동범위 최대 크기잡아놓은만큼만 표시하기위한 값
        //Debug.Log(TurnActionValue);
        //상시유아이 갱신

        TeamBorderStateUI uiComp = WindowList.Instance.TeamBorderManager.TeamStateUIs[0];
        uiComp.SetHpGaugeAndText(playerData.CurrentHP, playerData.Base_MaxHP);
        uiComp.SetStmGaugeAndText(playerData.Current_Stamina, playerData.Base_MaxStamina);



        SelectControllUnit(); //유닛 선택로직 실행

        // 첫로딩시 생성타이밍안맞음 
        if (currentUnit.BattleUI != null)
        {

            currentUnit.BattleUI.TrunActionStateChange(); //턴시작시 상태이상 들을 게이지 진행시킨다
            currentUnit.BattleUI.stmGaugeSetting(playerData.Current_Stamina, playerData.Base_MaxStamina);
            currentUnit.BattleUI.hpGaugeSetting(playerData.CurrentHP, playerData.Base_MaxHP);
        }
        SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(currentUnit.CurrentTile);
        SpaceSurvival_GameManager.Instance.MoveRange.MoveSizeView(currentUnit.CurrentTile, moveSize);//이동범위표시해주기 

        SpaceSurvival_GameManager.Instance.AttackRange.ClearLineRenderer();     //공격범위 초기화 
        SpaceSurvival_GameManager.Instance.AttackRange.InitDataSet(currentCharcter); //턴시작될때 공격 범위 적용 할 유닛으로 셋팅
    }

    /// <summary>
    /// 현재 컨트롤 중인 유닛이 있을때 컨트롤중인유닛 이동로직 연결하기 
    /// </summary>
    /// <param name="seletedTile">선택된 타일</param>
    private void OnUnitMove(Tile seletedTile)
    {

        if (currentUnit != null && currentUnit.IsControll && !currentUnit.IsMoveCheck) //현재 컨트롤인경우만 
        {
            //이동로직 실행
            StartCoroutine(currentUnit.CharcterMove(seletedTile));

        }
    }

    /// <summary>
    /// 아군을 클릭했을때 처리할 로직 
    /// </summary>
    /// <param name="clickedTile">클릭한 타일</param>
    public void OnClickPlayer(Tile clickedTile)
    {
        //if (currentUnit == null) //플레이어가 설정안되있으면 
        //{
        //    foreach (ICharcterBase playerUnit in charcterList) //플레이어 유닛위치인지 체크하기위해 플레이어를 뒤진다.
        //    {
        //        if (clickedTile.width == playerUnit.CurrentTile.width &&
        //            clickedTile.length == playerUnit.CurrentTile.length) //클릭한 타일이 플레이어 유닛 위치면 
        //        {
        //            currentUnit = playerUnit; //플레이어 설정을하고 
        //            currentUnit.IsControll = true; //컨트롤 할수있게 설정한다.
        //            cot.Target = currentUnit.transform; //카메라 포커스 맞추기 
        //            SelectControllUnit();
        //            return;
        //        }
        //    }
        //}
        currentUnit.IsControll = true; //컨트롤 할수있게 설정한다.
        cot.Target = currentUnit.transform; //카메라 포커스 맞추기 
        if (!currentUnit.IsMoveCheck)// 캐릭터 이동중인지 체크해서 이동끝날때만 로직 실행 
        {
            if (currentUnit == null || //컨트롤중인 유닛이 없거나 
                clickedTile.width != currentUnit.CurrentTile.width ||
                clickedTile.length != currentUnit.CurrentTile.length
                )//컨트롤 중인 유닛의 위치와 다를경우 
            {
                foreach (ICharcterBase playerUnit in charcterList) //플레이어 유닛위치인지 체크하기위해 플레이어를 뒤진다.
                {
                    if (clickedTile.width == playerUnit.CurrentTile.width &&
                        clickedTile.length == playerUnit.CurrentTile.length) //클릭한 타일이 플레이어 유닛 위치면 
                    {
                        if (currentUnit != null) //기존에 컨트롤 중인 유닛이 있을때  
                        {
                            currentUnit.IsControll = false; //기존값은 컨트롤 해제하고 
                            //SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(currentUnit.CurrentTile); //이동범위 리셋시킨다.
                        }
                        TurnActionValue -= currentUnit.CurrentTile.MoveCheckG;  //이동한값만큼 감소시키기
                        currentUnit = playerUnit; //다른 아군을 담고
                        currentUnit.IsControll = true; //컨트롤 할수있게 설정한다.
                        cot.Target = currentUnit.transform; //카메라 포커스 맞추기 
                        miniMapCam.player = currentUnit.transform;
                        SelectControllUnit();
                        return;
                    }
                }
            }
            else //현재 컨트롤 중인 유닛이 있는 타일이 클릭됬을경우  
            {
                PlayerSelect();
            }
        }
        else
        {
            Debug.LogWarning("캐릭터가 이동중입니다.");
        }
    }

    /// <summary>
    /// 캐릭터가 선택된 상태에서 다시 선택될때 처리할로직 
    /// </summary>
    private void PlayerSelect()
    {
        Debug.Log($"컨트롤유닛 {currentUnit.transform.name} 을 다시 선택했다.");
    }

    /// <summary>
    /// 컨트롤 유닛으로 선택 될때 처리할로직 
    /// </summary>
    private void SelectControllUnit()
    {
        //currentUnit.MoveSize = TurnActionValue; //새로운캐릭터 이동가능범위 셋팅
        MoveActionButton.IsMoveButtonClick = false; //귀찮아서 스태틱
        //Debug.Log($"컨트롤유닛 : {currentUnit.transform.name} 선택했다.");
    }

    public override void ResetData()
    {
        if (currentUnit != null) //현재 진행중인 유닛이 있는경우 
        {
            currentUnit.IsControll = false; //컨트롤 해제 한다.
            currentUnit = null;
        }
        base.ResetData();//그리고 데이터 초기화 한다.
    }

}
