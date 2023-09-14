using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_05_ShipDeploy_Auto : Test_04_ShipDeploy
{
    public Button reset;
    public Button random;


    protected override void Start()
    {
        base.Start();
        reset.onClick.AddListener(ResetBoard);
        random.onClick.AddListener(RandomDeploy);
    }
    private void ResetBoard()
    {
        foreach (Ship ship in Ships)
        {
            if (ship.IsDeployed)
            {
                Board.UndoshipDeployment(ship);
                ship.gameObject.SetActive(false);
            }
        }
    }
    void RandomDeploy()
    {
        for (int i = 0; i < Ships.Length; i++)
        {
            Target = Ships[i];
            if (!Target.IsDeployed)
            {
                Vector2Int randomPos = new Vector2Int(Random.Range(0, Board.Board_Size), Random.Range(0, Board.Board_Size));
                Target.Direction = (ShipDirection)Random.Range(0,3);
                if (!Board.shipDeployment(Target, randomPos))
                {
                    i--;
                    continue;
                }
                if (i == 4)
                {
                    Target.SetMaterial_Type();
                }
            }
        }
    }
    //reset버튼 누르면 배치되어있는 모든 배 배치해제
    //random버튼 누를 시 아직 배치되어있지 않은 모든 배가 자동 배치
    // 랜덤배치시 보드에 가장자리와 다른 배의 주변 위치는 우선순위가 낮다.
    //4. 랜덤배치되는 위치는 우선순위가 높은 위치와 낮은 위치가 있다.
}
