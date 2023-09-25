using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test11 : MonoBehaviour
{
    public GameState gameState = GameState.ShipDeployment;

    GameManager gameManager;

    public Button random;
    public Button finish;
    Ship[] ships;
    UserPlayer player;
    Board board;
    private void OnValidate()
    {
        if (gameManager != null)
        {
            GameManager.Inst.GameState = gameState;
        }
    }
    void Start()
    {
        gameManager = GameManager.Inst;


        //reset.onClick.AddListener(ClearBoard);
        random.onClick.AddListener(AutoShipDeplyment);
        //resetAndRandom.onClick.AddListener(() =>
        //{
        //    ClearBoard();
        //    AutoShipDeplyment();
        //});
        player = GameManager.Inst.UserPlayer;
        this.ships = player.Ships;
        board = player.Board;
    }

    private void AutoShipDeplyment()
    {
        int maxCapacity = Board.BoardSize * Board.BoardSize;
        List<int> high = new(maxCapacity);
        List<int> low = new(maxCapacity);

        // �����ڸ� �κ��� low�� ����
        for (int i = 0; i < maxCapacity; i++)
        {
            if (i % Board.BoardSize == 0                            // 0,10,20,30,40,50,60,70,80,90
                || i % Board.BoardSize == (Board.BoardSize - 1)     // 9,19,29,39,49,59,69,79,89,99
                || i > 0 && i < (Board.BoardSize - 1)                 // 1~8
                || (Board.BoardSize * (Board.BoardSize - 1) < i && i < (Board.BoardSize * Board.BoardSize - 1))) // 91~98
            {
                low.Add(i);
            }
            else
            {
                high.Add(i);
            }
        }

        // �̹� ��ġ�� �� �ֺ��� low�� ����
        foreach (var ship in ships)
        {
            if (ship.IsDeployed)
            {
                int[] shipIndice = new int[ship.Size];
                for (int i = 0; i < ship.Size; i++)
                {
                    shipIndice[i] = Board.GridToIndex(ship.Positions[i]);
                }

                foreach (var index in shipIndice)
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAroundPositions(ship);
                foreach (var index in toLow)
                {
                    high.Remove(index);
                    low.Add(index);
                }
            }
        }

        // high�� low ���� ���� ����(����)
        int[] temp = high.ToArray();
        Util.Shuffle(temp);
        high = new(temp);
        temp = low.ToArray();
        Util.Shuffle(temp);
        low = new(temp);

        // �踦 �ϳ��� ��ġ ����
        foreach (var ship in ships)
        {
            if (!ship.IsDeployed)    // �谡 ��ġ���� ���� �͸� ó��
            {
                ship.RandomRotate();            // �踦 ������ ȸ�� ��Ű��

                bool failDeployment = true;
                int counter = 0;
                Vector2Int grid;
                Vector2Int[] shipPositions;

                // �켱 �켱������ ���� ���� ����
                do
                {
                    int headIndex = high[0];                // high�� ù��° ������ headIndex�� ����
                    high.RemoveAt(0);

                    grid = Board.IndexToGrid(headIndex);

                    // headIndex�� ��ġ�� �������� Ȯ��
                    failDeployment = !board.IsShipDeplymentAvailable(ship, grid, out shipPositions);
                    if (failDeployment)
                    {
                        high.Add(headIndex);    // �Ұ����ϸ� headIndex�� high�� �ǵ�����
                    }
                    else
                    {
                        // ����κ��� ��� high�� �ִ��� Ȯ��
                        for (int i = 1; i < shipPositions.Length; i++)
                        {
                            int bodyIndex = Board.GridToIndex(shipPositions[i]);    // ����κ� �ε��� �����ͼ�
                            if (!high.Contains(bodyIndex))   // high�� ����κ��� �ִ��� Ȯ��
                            {
                                high.Add(headIndex);        // high�� ����κ��� ������ ���з� ó��
                                failDeployment = true;
                                break;
                            }
                        }
                    }
                    counter++;  // ���ѷ��� ������

                    // �����ϰ�, �õ�Ƚ���� 10�� �̸��̰�, high�� �ĺ������� ���������� �ݺ�
                } while (failDeployment && counter < 10 && high.Count > 0);

                // �ʿ��� ��� �켱������ ���� �� ó��
                counter = 0;
                while (failDeployment && counter < 1000)
                {
                    int headIndex = low[0];                 // low���� �ϳ� ������
                    low.RemoveAt(0);
                    grid = Board.IndexToGrid(headIndex);

                    failDeployment = !board.IsShipDeplymentAvailable(ship, grid, out shipPositions);    // ��ġ �õ��ϰ�
                    if (failDeployment)
                    {
                        low.Add(headIndex);                 // �����ϸ� low�� �ǵ�����
                    }
                    counter++;
                }

                // high, low �Ѵ� �������� ��
                if (failDeployment)
                {
                    Debug.LogWarning("�Լ� �ڵ���ġ ����!!!!!");    // �̰� ������ �ִ�(���� Ű��ų�, �� ������ �ٿ��� ��)
                    return;
                }

                // ���� ��ġ
                board.ShipDeployment(ship, grid);
                ship.gameObject.SetActive(true);

                // ��ġ�� ��ġ�� high�� low���� ����
                List<int> tempList = new List<int>(shipPositions.Length);
                foreach (var pos in shipPositions)
                {
                    tempList.Add(Board.GridToIndex(pos));   // ��ġ�� ��ġ�� �ε����� ��ȯ�ؼ� ����
                }
                foreach (var index in tempList)
                {
                    high.Remove(index);     // high���� �ε��� ����
                    low.Remove(index);      // low���� �ε��� ����
                }

                // �Լ� �ֺ� ��ġ�� low�� ������
                List<int> toLow = GetShipAroundPositions(ship); // ��ġ�� �� �ֺ� ��ġ ���ϱ�
                foreach (var index in toLow)
                {
                    if (high.Contains(index))        // high�� �ش� ��ġ�� ������
                    {
                        low.Add(index);             // low�� �ְ�
                        high.Remove(index);         // high���� ����
                    }
                }
            }
        }
    }

    private List<int> GetShipAroundPositions(Ship ship)
    {
        List<int> result = new List<int>(ship.Size * 2 + 6);

        if (ship.Direction == ShipDirection.North || ship.Direction == ShipDirection.South)
        {
            foreach (var pos in ship.Positions)
            {
                result.Add(Board.GridToIndex(pos + Vector2Int.right));
                result.Add(Board.GridToIndex(pos + Vector2Int.left));
            }

            Vector2Int head;
            Vector2Int tail;
            if (ship.Direction == ShipDirection.North)
            {
                head = ship.Positions[0] + Vector2Int.down;
                tail = ship.Positions[^1] + Vector2Int.up;
            }
            else
            {
                head = ship.Positions[0] + Vector2Int.up;
                tail = ship.Positions[^1] + Vector2Int.down;
            }
            result.Add(Board.GridToIndex(head));
            result.Add(Board.GridToIndex(head + Vector2Int.left));
            result.Add(Board.GridToIndex(head + Vector2Int.right));
            result.Add(Board.GridToIndex(tail));
            result.Add(Board.GridToIndex(tail + Vector2Int.left));
            result.Add(Board.GridToIndex(tail + Vector2Int.right));
        }
        else
        {
            foreach (var pos in ship.Positions)
            {
                result.Add(Board.GridToIndex(pos + Vector2Int.up));
                result.Add(Board.GridToIndex(pos + Vector2Int.down));
            }

            Vector2Int head;
            Vector2Int tail;
            if (ship.Direction == ShipDirection.East)
            {
                head = ship.Positions[0] + Vector2Int.right;
                tail = ship.Positions[^1] + Vector2Int.left;
            }
            else
            {
                head = ship.Positions[0] + Vector2Int.left;
                tail = ship.Positions[^1] + Vector2Int.right;
            }
            result.Add(Board.GridToIndex(head));
            result.Add(Board.GridToIndex(head + Vector2Int.up));
            result.Add(Board.GridToIndex(head + Vector2Int.down));
            result.Add(Board.GridToIndex(tail));
            result.Add(Board.GridToIndex(tail + Vector2Int.up));
            result.Add(Board.GridToIndex(tail + Vector2Int.down));
        }
        result.RemoveAll((x) => x == Board.NOT_VALID_INDEX);

        return result;
    }

    private void ClearBoard()
    {
        board.ResetBoard(ships);
    }
    //�ǽ�
    //���õ� �谡 ���� ��  ��ġ�� �踦 Ŭ���� ��ġ ����
    //������ġ��ư ������ ���� ��ġ���� ���� �� ��� ��ġ
    //��� �谡 ��ġ �Ϸ�Ǹ� ��ġ�Ϸ��ư Ȱ��ȭ
}
