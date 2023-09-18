using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected Board board;//�� �÷��̾��� ���� 
    public Board Board => board;

    protected Ship[] ships;//�� �÷��̾ �����ִ� �Լ��� //
    public Ship[] Ships => ships;

    ShipType[] shipInfo;

    protected int remainShipCount;//���� �� ī��Ʈ
    public bool IsDefeat => remainShipCount < 1;

    bool isActionDone = false;//���� ���� �Ǿ��ִ��� ���� 
    public bool IsActionDone => isActionDone;

    protected PlayerBase opponent;//���� 

    public Action<PlayerBase> onAttackFail;//�� �÷��̾��� ������ ���������� �˸��� ��ȣ param = �ڱ� �ڽ�
    public Action onActionEnd;
    public Action<PlayerBase> onDefead;// �� �÷��̾ �й������� �˸��� ��ȣ 
    protected virtual void Awake()
    {
        board = GetComponentInChildren<Board>();
        shipInfo = new ShipType[Board.Board_Size * Board.Board_Size];
    }
    protected virtual void Start()
    {
        int shipTypeCount = ShipManager.Inst.ShipType_Count;
        ships = new Ship[shipTypeCount];

        for (int i = 0; i < shipTypeCount; i++)
        {
            ShipType shipType = (ShipType)(i + 1);
            ships[i] = ShipManager.Inst.MakeShip(shipType, transform);
            ships[i].on_Sinking += OnShipDestroy; //�Լ� ħ���� OnShipDestroy �Լ� ����

            board.on_ShipAttacked[shipType] = ships[i].OnHitted;
        }
        remainShipCount = shipTypeCount;
    }
    //�� ������ �Լ��ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    public virtual void OnPlayerTurnStart(int _)
    {
        isActionDone = false;
    }
    public virtual void OnPlayerTurnEnd()
    {

    }
    //�� ������ �Լ��ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

    //���� ���� �Լ��ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    public void Attack(Vector2Int attackGridPos)//
    {
        Debug.Log($"{gameObject.name} �� ({attackGridPos.x}, {attackGridPos.y}) �� �����߽��ϴ�.");
        opponent.Board.OnAttacked(attackGridPos);
    }
    public void Attack(int index)
    {
        Attack(Board.Index_To_Grid(index));
    }
    public void Attack(Vector3 worldPos)
    {
        Attack(opponent.Board.World_To_Grid(worldPos));
    }
    
    public void AutoAttack()//CPU, �ΰ� �÷��̾ Ÿ�Ӿƿ� ���� �� ��� 
    {

        //1. ������ ����(�ߺ����� ����)
        //���� ������ ���� ������ ���� ���� ��ġ �����¿� ���� �� �ϳ��� ����
        //������ �ι� �������� �� ���� �ĺ������� �� �� �ٱ� �� �ϳ��� ����
        //�Լ� ħ���� �켱���� �ĺ����� Clear;
    }
    //���� ���� �Լ��ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

    //�Լ� ��ġ�� �Լ��ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    public void AutoShipDeployment(bool isShowShips)//�� �÷��̾��� ���忡 �Լ��� ��ġ�ϴ� �Լ� 
    {
        int maxCapacity = Board.Board_Size * Board.Board_Size;
        List<int> high = new(maxCapacity);
        List<int> low = new(maxCapacity);

        for (int i = 0; i < maxCapacity; i++)
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
                for (int i = 0; i < ship.Size; i++)
                {
                    shipIndice[i] = Board.Grid_To_Index(ship.Positions[i]);
                }
                foreach (var index in shipIndice) // �谡 ��ġ�� �ε����� ����Ʈ���� �����
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAroundPosition(ship);// �踦 ���ΰ��ִ� ��� ��ǥ ���ϱ�

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

        foreach (var ship in Ships)
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
                    high.RemoveAt(0);//������ ����Ʈ���� ����

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
                                high.Add(headIndex);//����Ʈ�� ������ �ε��� �ٽ� ���ϱ�
                                failDeployment = true;
                                break;
                            }
                        }
                    }
                    counter++;//���ѷ��� ������
                } while (failDeployment && counter < 10 && high.Count > 0);// ��ġ���� and ī���� 10 �̸� &&  high

                counter = 0;
                while (failDeployment && counter < 1000)//�켱������ġ�� ����������� �������� �Ѿ���� �ȴ�. //�־��� ��쿡�� ������ ����ǵ��� ����// Ȯ���� 0�� �ƴϸ� ������ ������ �ȴ�.
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

                if (failDeployment)//high, low �� ������ ��� ���ƴµ��� ��ġ ���а� ���� ���(�ؾ��� Ȯ�� . ���� Ű��ų� �� ������ �ٿ�����)
                {
                    Debug.LogWarning("�Լ� �ڵ���ġ ����!");// 
                    return;
                }
                Board.shipDeployment(ship, grid);//���� ��ġ
                if (isShowShips)
                {
                    ship.gameObject.SetActive(true);//��ġ ������ Ȱ��ȭ
                }
                else
                {
                    ship.gameObject.SetActive(false);
                }
                //��ġ�� ��ġ�� high, low���� ����
                List<int> tempList = new List<int>(shipPositions.Length);
                foreach (var pos in shipPositions)
                {
                    tempList.Add(Board.Grid_To_Index(pos));//��ġ�� ����Ʈ�� ����
                }
                foreach (var index in tempList)//������ ����Ʈ�� ���� ����
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAroundPosition(ship);//�� �ֺ���ġ ��������
                foreach (var index in toLow)//��ġ�� �� �ֺ� ��ġ�� high (�켱���� ����Ʈ���� ����) �ϰ� low�� �߰�
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
    private List<int> GetShipAroundPosition(Ship ship)//�Լ� �ֺ��� �ε������� ���ϴ� �Լ� 
    {
        List<int> result = new List<int>((ship.Size * 2) + 6);

        if (ship.Direction == ShipDirection.North || ship.Direction == ShipDirection.South)
        {
            foreach (var pos in ship.Positions)//���η� ���������� ��(��Ӹ��� ���� /�Ǵ� ����)
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
    public void UndoAllShipDeployment()//����Լ��� ��ġ�� ����ϴ� �Լ� 
    {
        Board.ResetBoard(ships);
        remainShipCount = ShipManager.Inst.ShipType_Count;
    }
    //�Լ� ��ġ�� �Լ��ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�


    //�Լ� ħ�� �� �й�ó�� �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    void OnShipDestroy(Ship ship)//���� ���� Ư���谡 ħ������ �� ����� �Լ� 
    {
        remainShipCount--;// �谡 ħ���� ������ ī��Ʈ ���� 
        Debug.Log($"{ship.ShipType} �� ħ�� �߽��ϴ�. {remainShipCount} ô�� �谡 �����ֽ��ϴ�.");
        if (remainShipCount < 1)
        {
            OnDefeat();
        }
    }

    void OnDefeat()//��� �谡 ħ���Ǿ��� �� ����� �Լ� 
    {
        Debug.Log($"[{this.gameObject.name}] �й�");
        onDefead?.Invoke(this);
    }

    //��Ÿ�ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

    public void Clear()//�ʱ�ȭ . ���ӽ��� ���� ���·� ���� 
    {

    }
    public Ship GetShip(ShipType type)
    {
        return (type != ShipType.None) ? ships[(int)type - 1] : null; // None�� �ƴϸ� type - 1 �����ϰ� None�̸� null ����
    }

    public void Test_SetOpponent(PlayerBase player)
    {
        opponent = player;
    }
    //��Ÿ�ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

}
