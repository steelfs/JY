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
                low.Add(i);//위 조건에 해당하는 좌표는 낮은확률로 배치될 좌표
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
                foreach (var index in shipIndice) // 배가 배치될 인덱스를 리스트에서 지우기
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAround_Positions(ship);// 배를 감싸고있는 모든 좌표 구하기

                foreach (int index in toLow)//구한 좌표도 역시 낮은확률로 배치될 좌표에 추가
                {
                    high.Remove(index);
                    low.Add(index);
                }
            }
        }

        //high와 low 내부 순서 섞기
        int[] temp = high.ToArray();
        Util.Shuffle(temp);
        high = new(temp);

        temp = low.ToArray();
        Util.Shuffle(temp);
        low = new(temp);

        foreach(var ship in Ships)
        {
            if (!ship.IsDeployed)// 배치되지 않은 배만
            {
                ship.RandomRotate(); // 회전시키기

                bool failDeployment = false;
                int counter = 0;
                Vector2Int grid;
                Vector2Int[] shipPositions;
                do
                {
                    int headIndex = high[0]; //섞어놓은 high 의 첫번째 꺼내기
                    high.RemoveAt(0);

                    grid = Board.Index_To_Grid(headIndex);
                    failDeployment = !Board.IsShipDeployment_Available(ship, grid, out shipPositions);//배치 가능하면  배의 포지션 받아오기

                    if (failDeployment) //배치불가능 하면 다시 더하기
                    {
                        high.Add(headIndex);
                    }
                    else// 배치가능하면 
                    {
                        for (int i = 1; i < shipPositions.Length; i++)
                        {
                            int bodyIndex = Board.Grid_To_Index(shipPositions[i]);//몸통부분 인덱스 가져오기
                            if (!high.Contains(bodyIndex))// 몸통 인덱스가 high에 없다면 배치 실패 처리
                            {
                                high.Add(headIndex);
                                failDeployment = true;
                                break;
                            }
                        }
                    }
                    counter++;//무한루프 방지용
                } while (failDeployment && counter < 10 && high.Count > 0);// 배치실패 and 카운터 10 미만 &&  high

                counter = 0;
                while (failDeployment && counter < 1000)//최악의 경우에도 루프가 종료되도록 설정// 확률이 0이 아니면 언젠가 터지게 된다.
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
                    Debug.LogWarning("함선 자동배치 실패!");// 극악의 확률 . 맵을 키우거나 배 종류를 줄여야함
                    return;
                }
                Board.shipDeployment(ship, grid);//실제 배치
                ship.gameObject.SetActive(true);//배치 성공시 활성화

                //배치된 위치를 high, low에서 제거
                List<int> tempList = new List<int>(shipPositions.Length);
                foreach(var pos in shipPositions)
                {
                    tempList.Add(Board.Grid_To_Index(pos));//위치를 리스트에 저장
                }
                foreach (var index in tempList)//저장한 리스트를 돌며 제거
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAround_Positions(ship);//배 주변위치 가져오기
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
            foreach(var pos in ship.Positions)//세로로 세워져있을 때(뱃머리가 북쪽 /또는 남쪽)
            {
                result.Add(Board.Grid_To_Index(pos + Vector2Int.right)); //배를 감싸고있는 양 옆의 좌표 모두 추가
                result.Add(Board.Grid_To_Index(pos + Vector2Int.left));
            }

            Vector2Int head;
            Vector2Int tail;
            if (ship.Direction == ShipDirection.North)
            {
                head = ship.Positions[0] + Vector2Int.down;//머리가 북쪽일때 -y 가 북쪽
                tail = ship.Positions[^1] + Vector2Int.up;
            }
            else
            {
                head = ship.Positions[0] + Vector2Int.up;
                tail = ship.Positions[^1] + Vector2Int.down;
            }
            result.Add(Board.Grid_To_Index(head)); //머리와 머리 양 옆
            result.Add(Board.Grid_To_Index(head + Vector2Int.left));
            result.Add(Board.Grid_To_Index(head + Vector2Int.right));
            result.Add(Board.Grid_To_Index(tail));//꼬리와 꼬리 양 옆
            result.Add(Board.Grid_To_Index(tail + Vector2Int.left));
            result.Add(Board.Grid_To_Index(tail + Vector2Int.right));
        }
        else
        {
            foreach (var pos in ship.Positions)//
            {
                result.Add(Board.Grid_To_Index(pos + Vector2Int.up));//뱃머리가 왼쪽 또는 오른 쪽일 때  위, 아래로 감사고있는 좌표 모두 추가
                result.Add(Board.Grid_To_Index(pos + Vector2Int.down));
            }

            Vector2Int head;
            Vector2Int tail;
            if (ship.Direction == ShipDirection.East)
            {
                head = ship.Positions[0] + Vector2Int.right; //뱃머리가 오른쪽일때 x+ 방향이 머리
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
        result.RemoveAll((x) => x == Board.NOT_VALID_INDEX);// 조건에 해당하는 것만 remove 

        
        //result.TrimExcess();

        return result;
    }

    //reset버튼 누르면 배치되어있는 모든 배 배치해제
    //random버튼 누를 시 아직 배치되어있지 않은 모든 배가 자동 배치
    // 랜덤배치시 보드에 가장자리와 다른 배의 주변 위치는 우선순위가 낮다.
    //4. 랜덤배치되는 위치는 우선순위가 높은 위치와 낮은 위치가 있다.
}
