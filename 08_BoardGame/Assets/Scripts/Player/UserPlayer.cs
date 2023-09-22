using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        }
    }
    GameState state;
    // 입력 관련 델리게이트 -------------------------------------------------------------------------
    // 상태별로 따로 처리(만약 null이면 그 상태에서 수행하는 일이 없다는 의미)
    Action<Vector2>[] onMouseClick;
    Action<Vector2>[] onMouseMove;
    Action<float>[] onMouseWheel;

    private void OnMouseMove(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    protected override void Start()
    {
        base.Start();
        GameManager.Inst.onStateChange += OnStateChange;//gamemanager의 OnInitialize가 실행된 후 실행되어야함.

        onMouseClick = new Action<Vector2>[] { OnClick_ShipDeployment , OnClick_Battle };
        onMouseMove = new Action<Vector2>[] { OnMouseMove_ShipDeployment, OnMouseMove_Battle };
        onMouseWheel = new Action<float>[] { OnMousewheel_ShipDeployment, OnMousewheel_Battle };
    }

    //상태관련 함수 
    public void OnStateChange(GameState gameState)
    {
        this.state = gameState; 
    }

    // 함선 배치 씬용 입력 함수들 -------------------------------------------------------------------
    private void OnClick_ShipDeployment(Vector2 screen)
    {

    }

    private void OnMouseMove_ShipDeployment(Vector2 screen)
    {
        if (selectedShip != null && !selectedShip.IsDeployed)               // 선택된 배가 있고 아직 배치가 안된 상황에서만 처리
        {
            selectedShip.gameObject.SetActive(true);
            Vector3 world = Camera.main.ScreenToWorldPoint(screen);
            world.y = board.transform.position.y;

            if (board.IsInBoard(world))        // 보드 안인지 확인
            {
                Vector2Int grid = board.GetMouseGridPosition();

                // 이동
                selectedShip.transform.position = board.GridToWorld(grid);            // 보드 안쪽일 때만 위치 이동(칸단위로 이동)

                // 색상 변경
                bool isSuccess = board.IsShipDeplymentAvailable(selectedShip, grid);  // 배치 가능한지 확인 
                ShipManager.Inst.SetDeloyModeColor(isSuccess);                      // 결과에 따라 색상 변경

            }
            else
            {
                selectedShip.transform.position = world;      // 자유롭게 움직이기    
                ShipManager.Inst.SetDeloyModeColor(false);  // 밖이면 무조건 빨간 색
            }
        }
    }

    private void OnMousewheel_ShipDeployment(float wheelDelta)
    {

    }

    // 전투 씬용 입력 함수들 ------------------------------------------------------------------------
    private void OnClick_Battle(Vector2 screen)
    {

    }

    private void OnMouseMove_Battle(Vector2 screen)
    {

    }

    private void OnMousewheel_Battle(float wheelDelta)
    {

    }

    // 함선 배치용 함수 ----------------------------------------------------------------------------
    
    /// <summary>
    /// 특정 종류의 함선을 선택하는 함수
    /// </summary>
    /// <param name="shipType"></param>
    public void SelectShipToDeploy(ShipType shipType)
    {
        SelectedShip = ships[(int)shipType - 1];
        GameManager.Inst.Input.onMouseMove = onMouseMove[0];
        GameManager.Inst.Input.onMouseClick = onMouseClick[0];
        GameManager.Inst.Input.onMouseWheel = onMouseWheel[0];
    }

    /// <summary>
    /// 특정 종류의 함선을 배치 취소하는 함수
    /// </summary>
    /// <param name="shipType">배치를 취소할 함선</param>
    public void UndoShipDeploy(ShipType shipType)
    {

    }
}
