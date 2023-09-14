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
        reset.onClick.AddListener(ClearBoard);
        random.onClick.AddListener(AutoShipDeployment);
    }
    private void ClearBoard()
    {
        foreach (Ship ship in Ships)
        {
            if (ship.IsDeployed)
            {
                Board.UndoshipDeployment(ship);
            }
        }
    }
    void AutoShipDeployment()
    {
        int maxCapacity = Board.Board_Size * Board.Board_Size;
        List<int> high = new(maxCapacity);
        List<int> low = new(maxCapacity);

        for (int  i = 0; i < maxCapacity; i++)
        {
            //i % Board.Board_Size == 0 // = 10,20,30 ...//  i % Board.Board_Size == Board.Board_Size - 1 // = 9, 19, 29 ..
            // i > 0 && i < Board.Board_Size - 1  // = 1,2,3 ~ 8 //
            if (i % Board.Board_Size == 0 || i % Board.Board_Size == Board.Board_Size - 1 || i > 0 && i < Board.Board_Size - 1 || Board.Board_Size * (Board.Board_Size - 1) < i && i < (Board.Board_Size * Board.Board_Size - 1))
            {
                low.Add(i);//�� ���ǿ� �ش��ϴ� ��ǥ�� ����Ȯ���� ��ġ�� ��ǥ
            }
            else
            {
                high.Add(i);
            }
        }

        foreach (var ship in Ships)
        {
            if (ship.IsDeployed)
            {
                int[] shipIndice = new int[ship.Size];
                for (int  i = 0; i < ship.Size; i++)
                {
                    shipIndice[i] = Board.Grid_To_Index(ship.Positions[i]);
                }
                foreach (var index in shipIndice) // �谡 ��ġ�� �ε����� ����Ʈ���� �����
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAround_Positions(ship);// �踦 ���ΰ��ִ� ��� ��ǥ ���ϱ�

                foreach (int index in toLow)//���� ��ǥ�� ���� ����Ȯ���� ��ġ�� ��ǥ�� �߰�
                {
                    high.Remove(index);
                    low.Add(index);
                }
            }
        }

        //high�� low ���� ���� ����
        int[] temp = high.ToArray();
        Util.Shuffle(temp);
        high = new(temp);

        temp = low.ToArray();
        Util.Shuffle(temp);
        low = new(temp);
    }

    List<int> GetShipAround_Positions(Ship ship)
    {
        List<int> result = new List<int>((ship.Size * 2) + 6);

        if (ship.Direction == ShipDirection.North || ship.Direction == ShipDirection.South)
        {
            foreach(var pos in ship.Positions)//���η� ���������� ��(��Ӹ��� ���� /�Ǵ� ����)
            {
                result.Add(Board.Grid_To_Index(pos + Vector2Int.right)); //�踦 ���ΰ��ִ� �� ���� ��ǥ ��� �߰�
                result.Add(Board.Grid_To_Index(pos + Vector2Int.left));
            }

            Vector2Int head;
            Vector2Int tail;
            if (ship.Direction == ShipDirection.North)
            {
                head = ship.Positions[0] + Vector2Int.down;//�Ӹ��� �����϶� -y �� ����
                tail = ship.Positions[^1] + Vector2Int.up;
            }
            else
            {
                head = ship.Positions[0] + Vector2Int.up;
                tail = ship.Positions[^1] + Vector2Int.down;
            }
            result.Add(Board.Grid_To_Index(head)); //�Ӹ��� �Ӹ� �� ��
            result.Add(Board.Grid_To_Index(head + Vector2Int.left));
            result.Add(Board.Grid_To_Index(head + Vector2Int.right));
            result.Add(Board.Grid_To_Index(tail));//������ ���� �� ��
            result.Add(Board.Grid_To_Index(tail + Vector2Int.left));
            result.Add(Board.Grid_To_Index(tail + Vector2Int.right));
        }
        else
        {
            foreach (var pos in ship.Positions)//
            {
                result.Add(Board.Grid_To_Index(pos + Vector2Int.up));//��Ӹ��� ���� �Ǵ� ���� ���� ��  ��, �Ʒ��� ������ִ� ��ǥ ��� �߰�
                result.Add(Board.Grid_To_Index(pos + Vector2Int.down));
            }

            Vector2Int head;
            Vector2Int tail;
            if (ship.Direction == ShipDirection.East)
            {
                head = ship.Positions[0] + Vector2Int.right; //��Ӹ��� �������϶� x+ ������ �Ӹ�
                tail = ship.Positions[^1] + Vector2Int.left;
            }
            else
            {
                head = ship.Positions[0] + Vector2Int.left;
                tail = ship.Positions[^1] + Vector2Int.right;
            }
            result.Add(Board.Grid_To_Index(head));
            result.Add(Board.Grid_To_Index(head + Vector2Int.up));
            result.Add(Board.Grid_To_Index(head + Vector2Int.down));
            result.Add(Board.Grid_To_Index(tail));
            result.Add(Board.Grid_To_Index(tail + Vector2Int.up));
            result.Add(Board.Grid_To_Index(tail + Vector2Int.down));
        }
        result.RemoveAll((x) => x == Board.NOT_VALID_INDEX);// ���ǿ� �ش��ϴ� �͸� remove 

        
        //result.TrimExcess();

        return result;
    }

    //reset��ư ������ ��ġ�Ǿ��ִ� ��� �� ��ġ����
    //random��ư ���� �� ���� ��ġ�Ǿ����� ���� ��� �谡 �ڵ� ��ġ
    // ������ġ�� ���忡 �����ڸ��� �ٸ� ���� �ֺ� ��ġ�� �켱������ ����.
    //4. ������ġ�Ǵ� ��ġ�� �켱������ ���� ��ġ�� ���� ��ġ�� �ִ�.
}
