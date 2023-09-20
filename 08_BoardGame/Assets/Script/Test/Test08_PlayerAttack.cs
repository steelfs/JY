using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Test08_PlayerAttack : TestBase
{
    public Button reset;
    public Button random;
    public Button reset_Random;
    public PlayerBase user;
    public PlayerBase enemy;

    Board userBoard;
    Board enemyBoard;

    private void Start()
    {
        reset.onClick.AddListener(user.UndoAllShipDeployment);
        random.onClick.AddListener(() => user.AutoShipDeployment(true));
        reset_Random.onClick.AddListener(() =>
        {
            enemy.UndoAllShipDeployment();
            enemy.AutoShipDeployment(true);
        });
        enemyBoard = enemy.Board;
        userBoard = user.Board;

        enemy.Test_SetOpponent(user);
        user.Test_SetOpponent(enemy);

        enemy.AutoShipDeployment(true);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

    }
    protected override void LClick(InputAction.CallbackContext obj)
    {
        //����
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        user.Attack(world);
    }
    protected override void RClick(InputAction.CallbackContext obj)
    {
        //��Ŭ���� ������ �ִ� �Լ� ��ġ ���
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        ShipType shipType = userBoard.GetShipType(world);//��Ŭ���� ��ġ�� �Լ� ���� ��������
        if (shipType != ShipType.None)
        {
            Ship ship = user.GetShip(shipType);
            userBoard.UndoshipDeployment(ship);
            ship.gameObject.SetActive(false);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        inputActions.Test.RClick.performed -= RClick;
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        user.ActiveMarks();
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        user.DeActiveMarks();
    }
}
