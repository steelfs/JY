using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class UserPlayer : PlayerBase
{

    /// <summary>
    /// 지금 배치하려는 배
    /// </summary>
    Ship selectedShip;

    Ship SelectedShip
    {
        get => selectedShip;
        set
        {
            selectedShip = value;
            if (SelectedShip != value)
            {
                if (selectedShip != null)
                {
                    selectedShip.SetMaterialType();
                    if (!selectedShip.IsDeployed)
                    {
                        selectedShip.gameObject.SetActive(false);
                    }
                }
            }
            selectedShip = value;

            if (selectedShip != null)
            {
                selectedShip.SetMaterialType(false);

                Vector2 screen = Mouse.current.position.ReadValue();
                Vector3 world = Camera.main.ScreenToWorldPoint(screen);
                world.y = board.transform.position.y;
                selectedShip.transform.position = world;
                selectedShip.gameObject.SetActive(true);
            }
        }
    }
    GameState state = GameState.Title;

    public bool IsAllDeployed
    {
        get
        {
            bool result = true;
            foreach(Ship  ship in ships)
            {
                if (!ship.IsDeployed)
                {
                    result = false; // 배가 하나라도 배치되지 않았으면 false리턴
                    break;
                }
            }
            return result;
        }
    }
    // 입력 관련 델리게이트 -------------------------------------------------------------------------
    // 상태별로 따로 처리(만약 null이면 그 상태에서 수행하는 일이 없다는 의미)
    Action<Vector2>[] onMouseClick;
    Action<Vector2>[] onMouseMove;
    Action<float>[] onMouseWheel;

    protected override void Awake()
    {
        base.Awake();
        int length = Enum.GetValues(typeof(GameState)).Length;
        onMouseClick = new Action<Vector2>[length];
        onMouseMove = new Action<Vector2>[length];
        onMouseWheel = new Action<float>[length];

        onMouseClick[(int)GameState.ShipDeployment] = OnClick_ShipDeployment;
        onMouseMove[(int)GameState.ShipDeployment] = OnMouseMove_ShipDeployment;
        onMouseWheel[(int)GameState.ShipDeployment] = OnMousewheel_ShipDeployment;
        onMouseClick[(int)GameState.Battle] = OnClick_Battle;

    }

 
    protected override void Start()
    {
        base.Start();
        GameManager.Inst.Input.onMouseClick += OnClick;
        GameManager.Inst.Input.onMouseMove += OnMouseMove;
        GameManager.Inst.Input.onMouseWheel += OnMouseWheel;

    }


    //입력관리 함수들 -------------------------------------------------------------------
    private void OnClick(Vector2 screenPos)
    {
        onMouseClick[(int)state]?.Invoke(screenPos);
    }

    private void OnMouseMove(Vector2 screenPos)
    {
        onMouseMove[(int)state]?.Invoke(screenPos);
    }

    private void OnMouseWheel(float wheelDelta)
    {
        onMouseWheel[(int)state]?.Invoke(wheelDelta);
    }

    //상태관련 함수 
    public override void OnStateChange(GameState gameState)//게임 상태 변경시
    {
        this.state = gameState;

        Initialize();
        switch (state)
        {
            case GameState.ShipDeployment:
                break;
            case GameState.Battle: 
                if (!GameManager.Inst.LoadShipDeployData(this))//로딩실패시 
                {
                    this.AutoShipDeployment(true);//자동배치
                }
                opponent = GameManager.Inst.EnemyPlayer;//적 설정
                break;
        }
    }

    // 함선 배치 씬용 입력 함수들 -------------------------------------------------------------------
    private void OnClick_ShipDeployment(Vector2 screen)
    {
        // Debug.Log($"ShipDeployment : Click {screen.x}, {screen.y}");
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        if (SelectedShip != null)   // 선택된 배가 있을 때 우선 배치 시도
        {
            if (board.ShipDeployment(SelectedShip, world))
            {
                Debug.Log("함선배치 성공");
                SelectedShip = null;          // 배 선택 해제
            }
            else
            {

            }
        }
        else
        {
            if (Board.IsInBoard(world))//선택된 배가 없을 때 보드 안쪽을 클릭했으면 
            {
                ShipType shipType = Board.GetShipType(world);

                if (shipType != ShipType.None)
                {
                    UndoShipDeploy(shipType);//배가있으면 배치 해제
                }
                else
                {
                    Debug.Log("배치할 함선이 없거나 실패했습니다.");
                }
            }
        }
    }
    public Action<ShipType> on_CancelDeploy;
    private void OnMouseMove_ShipDeployment(Vector2 screen)
    {
       // Debug.Log($"ShipDeployment : Move {screen.x}, {screen.y}");
        if (SelectedShip != null && !SelectedShip.IsDeployed)               // 선택된 배가 있고 아직 배치가 안된 상황에서만 처리
        {
            SelectedShip.gameObject.SetActive(true);
            Vector3 world = Camera.main.ScreenToWorldPoint(screen);
            world.y = board.transform.position.y;

            if (board.IsInBoard(world))        // 보드 안인지 확인
            {
                Vector2Int grid = board.GetMouseGridPosition();

                // 이동
                SelectedShip.transform.position = board.GridToWorld(grid);            // 보드 안쪽일 때만 위치 이동(칸단위로 이동)

                // 색상 변경
                bool isSuccess = board.IsShipDeplymentAvailable(selectedShip, grid);  // 배치 가능한지 확인 
                ShipManager.Inst.SetDeloyModeColor(isSuccess);                      // 결과에 따라 색상 변경

            }
            else
            {
                SelectedShip.transform.position = world;      // 자유롭게 움직이기    
                ShipManager.Inst.SetDeloyModeColor(false);  // 밖이면 무조건 빨간 색
            }
        }
    }

    private void OnMousewheel_ShipDeployment(float wheelDelta)
    {
        //  Debug.Log($"ShipDeployment : Wheel {wheelDelta}");

        bool rotateDir = true;  // 기본값은 시계방향
        if (wheelDelta < 0)         // 입력 방향 확인
        {
            rotateDir = false;  // 입력 방향이 아래쪽이면 반시계방향
        }
        if (selectedShip != null)
        {
            SelectedShip.Rotate(rotateDir);   // 선택된 배를 회전 시키기

            bool isSuccess = board.IsShipDeplymentAvailable(SelectedShip, SelectedShip.transform.position); // 지금 상태로 배치가 가능한지 확인
            ShipManager.Inst.SetDeloyModeColor(isSuccess);  // 결과에 따라 색상 변경
        }
    }

    private void OnClick_Battle(Vector2 screen)
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);
        Attack(world);
        //Debug.Log($"Battle : Click ({screen.x},{screen.y})");
    }
    // 함선 배치용 함수 ----------------------------------------------------------------------------

    /// <summary>
    /// 특정 종류의 함선을 선택하는 함수
    /// </summary>
    /// <param name="shipType"></param>
    public void SelectShipToDeploy(ShipType shipType)
    {
        SelectedShip = ships[(int)shipType - 1];
       
    }

    /// <summary>
    /// 특정 종류의 함선을 배치 취소하는 함수
    /// </summary>
    /// <param name="shipType">배치를 취소할 함선</param>
    public void UndoShipDeploy(ShipType shipType)
    {
        Board.UndoShipDeployment(ships[(int)shipType - 1]);
    }
}
