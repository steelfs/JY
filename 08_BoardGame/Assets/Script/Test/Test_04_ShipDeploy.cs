using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_04_ShipDeploy : TestBase
{
    ShipType shipType;
    Ship[] ships;
    Ship targetShip;
    Ship Target
    {
        get => targetShip; 
        set
        {
            if (targetShip != value)
            {
                if (targetShip != null)
                {
                    targetShip.SetMaterial_Type();
                    targetShip.gameObject.SetActive(false);
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


    //1~5 눌러서 배 선택 (마우스 위치에 배가 붙어서 움직인다.)
    //2.  보드안쪽을 좌클릭하면 선택된 배가 배치된다. (겹쳐서 배치되면 안됨, 배치불가능한 상황에 머티리얼 변경)
    //3. 우클릭하면 그 위치에 있는 배가 배치 해제된다.
    // 4. 휠 버튼을 이용해서 배를 배치 전에 회전시킬 수 있어야 한다/

    // board에서 배치 manager에서 생성
    private void Start()
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
            }
            else
            {
                Target.transform.position = world;
            }
        }
    }
    protected override void RClick(InputAction.CallbackContext _)
    {

    }
    protected override void LClick(InputAction.CallbackContext obj)
    {
        Vector2Int mouseGridPos = board.Get_Mouse_Grid_Pos();
        ships[0].Deploy(mouseGridPos);
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
        }
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        Target = ships[(int)ShipType.Carrier - 1];
        Debug.Log("항공모함 선택");

    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        Target = ships[(int)ShipType.BattleShip - 1];
        Debug.Log("함선 선택");
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        Target = ships[(int)ShipType.Destroyer - 1];
        Debug.Log("전투함 선택");
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        Target = ships[(int)ShipType.SubMarine - 1];
        Debug.Log("2 선택");
    }
    protected override void Test5(InputAction.CallbackContext context)
    {
        Target = ships[(int)ShipType.PatrolBoat - 1];
        Debug.Log("1 선택");
    }
}
