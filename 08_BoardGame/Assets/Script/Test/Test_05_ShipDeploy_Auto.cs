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
