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

    //1~5 ������ �� ���� (���콺 ��ġ�� �谡 �پ �����δ�.)
    //2.  ��������� ��Ŭ���ϸ� ���õ� �谡 ��ġ�ȴ�. (���ļ� ��ġ�Ǹ� �ȵ�, ��ġ�Ұ����� ��Ȳ�� ��Ƽ���� ����)
    //3. ��Ŭ���ϸ� �� ��ġ�� �ִ� �谡 ��ġ �����ȴ�.
    // 4. �� ��ư�� �̿��ؼ� �踦 ��ġ ���� ȸ����ų �� �־�� �Ѵ�/

    // board���� ��ġ manager���� ����
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
                Target.transform.position = world; // �����̴� ��ġ������ �����Ӱ��ϱ�
                ShipManager.Inst.SetDeployMode_Color(false); //�ٱ��̸� ������
            }
        }
    }
    protected override void RClick(InputAction.CallbackContext _)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        ShipType shipType = board.GetShipType(world);//��Ŭ���� ��ġ�� �Լ� ���� ��������
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
            Debug.Log("��ġ ����");
            Target = null;
        }
        else
        {
            Debug.Log("�Լ��� ���ų� �����߽��ϴ�.");
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
            Debug.Log("�װ����� ����");
        }

    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        Ship ship = ships[(int)ShipType.BattleShip - 1];
        if (!ship.IsDeployed)
        {
            Target = ship;
            Debug.Log("���� ����");
        }
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        Ship ship = ships[(int)ShipType.Destroyer - 1];
        if (!ship.IsDeployed)
        {
            Target = ship;
            Debug.Log("������ ����");
        }
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        Ship ship = ships[(int)ShipType.SubMarine - 1];
        if (!ship.IsDeployed)
        {
            Target = ship;
            Debug.Log("�װ����� ����");
        }
    }
    protected override void Test5(InputAction.CallbackContext context)
    {
        Ship ship  = ships[(int)ShipType.PatrolBoat - 1];
        if (!ship.IsDeployed)
        {
            Target = ship;
            Debug.Log("����� ����");
        }
    }
}
