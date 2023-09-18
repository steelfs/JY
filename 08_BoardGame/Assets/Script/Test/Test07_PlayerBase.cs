using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Test07_PlayerBase : TestBase
{
    public Button reset;
    public Button random;
    public Button reset_Random;
    public PlayerBase player;

    Board board;

    private void Start()
    {
        reset.onClick.AddListener(player.UndoAllShipDeployment);
        random.onClick.AddListener(() => player.AutoShipDeployment(true));
        reset_Random.onClick.AddListener(() =>
        {
            player.UndoAllShipDeployment();
            player.AutoShipDeployment(true);
        });
        board = player.Board;
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

        player.Board.OnAttacked(player.Board.World_To_Grid(world));
    }
    protected override void RClick(InputAction.CallbackContext obj)
    {
        //��Ŭ���� ������ �ִ� �Լ� ��ġ ���
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        ShipType shipType = board.GetShipType(world);//��Ŭ���� ��ġ�� �Լ� ���� ��������
        if (shipType != ShipType.None)
        {
            Ship ship = player.GetShip(shipType);
            board.UndoshipDeployment(ship);
            ship.gameObject.SetActive(false);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        inputActions.Test.RClick.performed -= RClick;
    }
}
