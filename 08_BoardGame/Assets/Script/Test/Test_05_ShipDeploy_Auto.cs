using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_05_ShipDeploy_Auto : Test_04_ShipDeploy
{
    public Button reset;
    public Button random;
    public Button reset_Random;

    protected override void Start()
    {
        base.Start();
        reset.onClick.AddListener(ClearBoard);
        random.onClick.AddListener(AutoShipDeployment);
        reset_Random.onClick.AddListener(ResetAndRandom);

    }
    void ResetAndRandom()
    {
        ClearBoard();
        AutoShipDeployment();
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

        foreach(var ship in Ships)
        {
            if (!ship.IsDeployed)// ��ġ���� ���� �踸
            {
                ship.RandomRotate(); // ȸ����Ű��

                bool failDeployment = false;
                int counter = 0;
                Vector2Int grid;
                Vector2Int[] shipPositions;
                do
                {
                    int headIndex = high[0]; //������� high �� ù��° ������
                    high.RemoveAt(0);

                    grid = Board.Index_To_Grid(headIndex);
                    failDeployment = !Board.IsShipDeployment_Available(ship, grid, out shipPositions);//��ġ �����ϸ�  ���� ������ �޾ƿ���

                    if (failDeployment) //��ġ�Ұ��� �ϸ� �ٽ� ���ϱ�
                    {
                        high.Add(headIndex);
                    }
                    else// ��ġ�����ϸ� 
                    {
                        for (int i = 1; i < shipPositions.Length; i++)
                        {
                            int bodyIndex = Board.Grid_To_Index(shipPositions[i]);//����κ� �ε��� ��������
                            if (!high.Contains(bodyIndex))// ���� �ε����� high�� ���ٸ� ��ġ ���� ó��
                            {
                                high.Add(headIndex);
                                failDeployment = true;
                                break;
                            }
                        }
                    }
                    counter++;//���ѷ��� ������
                } while (failDeployment && counter < 10 && high.Count > 0);// ��ġ���� and ī���� 10 �̸� &&  high

                counter = 0;
                while (failDeployment && counter < 1000)//�־��� ��쿡�� ������ ����ǵ��� ����// Ȯ���� 0�� �ƴϸ� ������ ������ �ȴ�.
                {
                    int headIndex = low[0];
                    low.RemoveAt(0);
                    grid = Board.Index_To_Grid(headIndex);

                    failDeployment = !Board.IsShipDeployment_Available(ship, grid, out shipPositions);
                    if (failDeployment)
                    {
                        low.Add(headIndex);
                    }
                    counter++;
                }

                if (failDeployment)
                {
                    Debug.LogWarning("�Լ� �ڵ���ġ ����!");// �ؾ��� Ȯ�� . ���� Ű��ų� �� ������ �ٿ�����
                    return;
                }
                Board.shipDeployment(ship, grid);//���� ��ġ
                ship.gameObject.SetActive(true);//��ġ ������ Ȱ��ȭ

                //��ġ�� ��ġ�� high, low���� ����
                List<int> tempList = new List<int>(shipPositions.Length);
                foreach(var pos in shipPositions)
                {
                    tempList.Add(Board.Grid_To_Index(pos));//��ġ�� ����Ʈ�� ����
                }
                foreach (var index in tempList)//������ ����Ʈ�� ���� ����
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAround_Positions(ship);//�� �ֺ���ġ ��������
                foreach (var index in toLow)
                {
                    if (high.Contains(index))
                    {
                        low.Add(index);
                        high.Remove(index);
                    }
                }
            }
        }

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
