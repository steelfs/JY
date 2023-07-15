using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_SlimePath : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;
    public Slime slime;

    GridMap map;

    private void Start()
    {
        map = new GridMap(background, obstacle);

        if (slime == null)
        {
            slime = FindObjectOfType<Slime>();
        }
         slime.Initialize(map,new Vector3(-3,0));
    }

    protected override void TestClick(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();//��ũ�� ��ǥ��
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);//��ũ�� ��ǥ�� ������ǥ�� �ٲ��ִ� �Լ�
        Vector2Int gridPos = map.WorldToGrid(worldPos);

        if (map.IsValidPosition(gridPos) && !map.IsWall(gridPos) &&  !map.IsMonster(gridPos))//isMonster�߰��ؾ���// 
        {
            slime.SetDestination(gridPos);
        }
        Debug.Log(gridPos);
    }
}
