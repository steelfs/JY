using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected Board board;//이 플레이어의 보드 
    public Board Board => board;

    protected Ship[] ships;//이 플레이어가 갖고있는 함선들 //
    public Ship[] Ships => ships;

    ShipType[] shipInfo;

    protected int remainShipCount;//남은 배 카운트
    public bool IsDefeat => remainShipCount < 1;

    bool isActionDone = false;//턴이 종료 되어있는지 여부 
    public bool IsActionDone => isActionDone;

    protected PlayerBase opponent;//상대방 

    public Action<PlayerBase> onAttackFail;//이 플레이어의 공격이 실패했으을 알리는 신호 param = 자기 자신
    public Action onActionEnd;
    public Action<PlayerBase> onDefead;// 이 플레이어가 패배했을음 알리는 신호 
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
            ships[i].on_Sinking += OnShipDestroy; //함선 침몰시 OnShipDestroy 함수 실행

            board.on_ShipAttacked[shipType] = ships[i].OnHitted;
        }
        remainShipCount = shipTypeCount;
    }
    //턴 관리용 함수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public virtual void OnPlayerTurnStart(int _)
    {
        isActionDone = false;
    }
    public virtual void OnPlayerTurnEnd()
    {

    }
    //턴 관리용 함수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    //공격 관련 함수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public void Attack(Vector2Int attackGridPos)//
    {
        Debug.Log($"{gameObject.name} 가 ({attackGridPos.x}, {attackGridPos.y}) 에 공격했습니다.");
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
    
    public void AutoAttack()//CPU, 인간 플레이어가 타임아웃 됐을 때 사용 
    {

        //1. 무작위 공격(중복공격 방지)
        //공격 성공시 다음 공격은 이전 공격 위치 상하좌우 방향 중 하나를 공격
        //공격이 두번 성공했을 때 다음 후보지역은 양 끝 바깥 중 하나를 공격
        //함선 침몰시 우선순위 후보지역 Clear;
    }
    //공격 관련 함수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    //함선 배치용 함수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public void AutoShipDeployment(bool isShowShips)//이 플레이어의 보드에 함선을 배치하는 함수 
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
                for (int i = 0; i < ship.Size; i++)
                {
                    shipIndice[i] = Board.Grid_To_Index(ship.Positions[i]);
                }
                foreach (var index in shipIndice) // 배가 배치될 인덱스를 리스트에서 지우기
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAroundPosition(ship);// 배를 감싸고있는 모든 좌표 구하기

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

        foreach (var ship in Ships)
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
                    high.RemoveAt(0);//꺼낸것 리스트에서 제거

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
                                high.Add(headIndex);//리스트에 실패한 인덱스 다시 더하기
                                failDeployment = true;
                                break;
                            }
                        }
                    }
                    counter++;//무한루프 방지용
                } while (failDeployment && counter < 10 && high.Count > 0);// 배치실패 and 카운터 10 미만 &&  high

                counter = 0;
                while (failDeployment && counter < 1000)//우선순위배치가 실패했을경우 이쪽으로 넘어오게 된다. //최악의 경우에도 루프가 종료되도록 설정// 확률이 0이 아니면 언젠가 터지게 된다.
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

                if (failDeployment)//high, low 두 과정을 모두 거쳤는데도 배치 실패가 됐을 경우(극악의 확률 . 맵을 키우거나 배 종류를 줄여야함)
                {
                    Debug.LogWarning("함선 자동배치 실패!");// 
                    return;
                }
                Board.shipDeployment(ship, grid);//실제 배치
                if (isShowShips)
                {
                    ship.gameObject.SetActive(true);//배치 성공시 활성화
                }
                else
                {
                    ship.gameObject.SetActive(false);
                }
                //배치된 위치를 high, low에서 제거
                List<int> tempList = new List<int>(shipPositions.Length);
                foreach (var pos in shipPositions)
                {
                    tempList.Add(Board.Grid_To_Index(pos));//위치를 리스트에 저장
                }
                foreach (var index in tempList)//저장한 리스트를 돌며 제거
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAroundPosition(ship);//배 주변위치 가져오기
                foreach (var index in toLow)//배치된 배 주변 위치를 high (우선순위 리스트에서 제거) 하고 low에 추가
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
    private List<int> GetShipAroundPosition(Ship ship)//함선 주변의 인덱스들을 구하는 함수 
    {
        List<int> result = new List<int>((ship.Size * 2) + 6);

        if (ship.Direction == ShipDirection.North || ship.Direction == ShipDirection.South)
        {
            foreach (var pos in ship.Positions)//세로로 세워져있을 때(뱃머리가 북쪽 /또는 남쪽)
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
    public void UndoAllShipDeployment()//모든함선의 배치를 취소하는 함수 
    {
        Board.ResetBoard(ships);
        remainShipCount = ShipManager.Inst.ShipType_Count;
    }
    //함선 배치용 함수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ


    //함선 침몰 및 패배처리 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    void OnShipDestroy(Ship ship)//내가 가진 특정배가 침몰됐을 떄 실행될 함수 
    {
        remainShipCount--;// 배가 침몰할 때마다 카운트 감소 
        Debug.Log($"{ship.ShipType} 이 침몰 했습니다. {remainShipCount} 척의 배가 남아있습니다.");
        if (remainShipCount < 1)
        {
            OnDefeat();
        }
    }

    void OnDefeat()//모든 배가 침몰되었을 때 실행될 함수 
    {
        Debug.Log($"[{this.gameObject.name}] 패배");
        onDefead?.Invoke(this);
    }

    //기타ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    public void Clear()//초기화 . 게임시작 직전 상태로 변경 
    {

    }
    public Ship GetShip(ShipType type)
    {
        return (type != ShipType.None) ? ships[(int)type - 1] : null; // None이 아니면 type - 1 리턴하고 None이면 null 리턴
    }

    public void Test_SetOpponent(PlayerBase player)
    {
        opponent = player;
    }
    //기타ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

}
