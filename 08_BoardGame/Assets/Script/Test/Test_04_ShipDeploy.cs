using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_04_ShipDeploy : TestBase
{
    public BombMark bombMark;

    ShipType shipType;
    Ship[] ships;
    protected Ship[] Ships => ships;
    protected Ship targetShip;
    protected Ship Target
    {
        get => targetShip; 
        set
        {
            if (targetShip != value)
            {
                if (targetShip != null)
                {
                    targetShip.SetMaterial_Type();
                    if (!targetShip.IsDeployed)
                    {
                        targetShip.gameObject.SetActive(false);
                    }
                }
            }
            targetShip = value;
            if (targetShip != null)
            {
                targetShip.SetMaterial_Type(false);

                Vector2 screen = Mouse.current.position.ReadValue();
                Vector3 world = Camera.main.ScreenToWorldPoint(screen);
                
                world.y = transform.position.y;
                targetShip.transform.position = world;
                targetShip.gameObject.SetActive(true);
            }
      
        }
    }

    Board board;
    protected Board Board => board;

    //1~5 눌러서 배 선택 (마우스 위치에 배가 붙어서 움직인다.)
    //2.  보드안쪽을 좌클릭하면 선택된 배가 배치된다. (겹쳐서 배치되면 안됨, 배치불가능한 상황에 머티리얼 변경)
    //3. 우클릭하면 그 위치에 있는 배가 배치 해제된다.
    // 4. 휠 버튼을 이용해서 배를 배치 전에 회전시킬 수 있어야 한다/

    // board에서 배치 manager에서 생성
    protected virtual void Start()
    {
        ships = new Ship[ShipManager.Inst.ShipType_Count];
        ships[(int)ShipType.Carrier - 1] = ShipManager.Inst.MakeShip(ShipType.Carrier, transform);
        ships[(int)ShipType.BattleShip - 1] = ShipManager.Inst.MakeShip(ShipType.BattleShip, transform);
        ships[(int)ShipType.Destroyer - 1] = ShipManager.Inst.MakeShip(ShipType.Destroyer, transform);
        ships[(int)ShipType.SubMarine - 1] = ShipManager.Inst.MakeShip(ShipType.SubMarine, transform);
        ships[(int)ShipType.PatrolBoat - 1] = ShipManager.Inst.MakeShip(ShipType.PatrolBoat, transform);

        board = FindObjectOfType<Board>();
    }

    protected override void MouseMove(InputAction.CallbackContext context)
    {
        if (Target != null && !Target.IsDeployed)
        {
            Vector2 screen = Mouse.current.position.ReadValue();
            Vector3 world = Camera.main.ScreenToWorldPoint(screen);
            world.y = board.transform.position.y;

            if (board.Is_In_Board(world))
            {
                Vector2Int grid = board.Get_Mouse_Grid_Pos();
                Target.transform.position = board.Grid_To_World(grid);

                bool isSuccess = board.IsShipDeployment_Available(Target,grid);
                ShipManager.Inst.SetDeployMode_Color(isSuccess);
            }
            else
            {
                Target.transform.position = world; // 움직이는 위치단위를 자유롭게하기
                ShipManager.Inst.SetDeployMode_Color(false); //바깥이면 빨간색
            }
        }
    }
    protected override void RClick(InputAction.CallbackContext _)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        ShipType shipType = board.GetShipType(world);//우클릭한 위치의 함선 정보 가져오기
        if (shipType != ShipType.None)
        {
            Ship ship = ships[(int)shipType - 1];
            board.UndoshipDeployment(ship);
        }


    }
    protected override void LClick(InputAction.CallbackContext obj)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        if (Target != null && board.shipDeployment(Target, world))
        {
            Debug.Log("배치 성공");
            Target = null;
        }
        else
        {
            Debug.Log("함선이 없거나 실패했습니다.");
        }

    }

    protected override void Wheel(InputAction.CallbackContext context)
    {
        float delta = context.ReadValue<float>();
        bool rotateDir = true;
        if (delta < 0)
        {
            rotateDir = false;
        }
        if (Target != null)
        {
            Target.Rotate(rotateDir);

            bool isSuccess = board.IsShipDeployment_Available(Target, Target.transform.position);
            ShipManager.Inst.SetDeployMode_Color(isSuccess);
        }
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        Ship ship = ships[(int)ShipType.Carrier - 1];
        if (!ship.IsDeployed)
        {
            Target = ship;
            Debug.Log("항공모함 선택");
        }

    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        Ship ship = ships[(int)ShipType.BattleShip - 1];
        if (!ship.IsDeployed)
        {
            Target = ship;
            Debug.Log("전함 선택");
        }
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        Ship ship = ships[(int)ShipType.Destroyer - 1];
        if (!ship.IsDeployed)
        {
            Target = ship;
            Debug.Log("전투함 선택");
        }
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        Ship ship = ships[(int)ShipType.SubMarine - 1];
        if (!ship.IsDeployed)
        {
            Target = ship;
            Debug.Log("항공모함 선택");
        }
    }
    protected override void Test5(InputAction.CallbackContext context)
    {
        Ship ship  = ships[(int)ShipType.PatrolBoat - 1];
        if (!ship.IsDeployed)
        {
            Target = ship;
            Debug.Log("경비정 선택");
        }
    }
}
