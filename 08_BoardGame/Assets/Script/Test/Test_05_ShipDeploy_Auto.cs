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
    //reset��ư ������ ��ġ�Ǿ��ִ� ��� �� ��ġ����
    //random��ư ���� �� ���� ��ġ�Ǿ����� ���� ��� �谡 �ڵ� ��ġ
    // ������ġ�� ���忡 �����ڸ��� �ٸ� ���� �ֺ� ��ġ�� �켱������ ����.
    //4. ������ġ�Ǵ� ��ġ�� �켱������ ���� ��ġ�� ���� ��ġ�� �ִ�.
}
