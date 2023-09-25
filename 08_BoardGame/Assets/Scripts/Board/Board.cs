using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    /// <summary>
    /// 보드의 가로 세로 한 변의 길이(칸 단위)
    /// </summary>
    public const int BoardSize = 10;

    /// <summary>
    /// 인덱스가 범위를 벗어났다는 표시
    /// </summary>
    public const int NOT_VALID_INDEX = -1;

    /// <summary>
    /// 보드에 배치되어 있는 배 정보(겹치는 것을 방지하기 위한 정보)
    /// </summary>
    ShipType[] shipInfo;
    public ShipType[] ShipInfo => shipInfo;
    /// <summary>
    /// 보드가 공격당한 위치를 시각적으로 보여주는 클래스
    /// </summary>
    BombMark bombMark;

    /// <summary>
    /// 공격 받으면 true로 설정되는 배열
    /// </summary>
    bool[] isAttacked;

    /// <summary>
    /// 배 종류별로 공격 당했을 때 실행될 델리게이트를 가지는 딕셔너리
    /// </summary>
    public Dictionary<ShipType, Action> onShipAttacked;

    private void Awake()
    {
        shipInfo = new ShipType[BoardSize * BoardSize];

        bombMark = GetComponentInChildren<BombMark>();
        isAttacked = new bool[BoardSize * BoardSize];

        onShipAttacked = new Dictionary<ShipType, Action>(ShipManager.Inst.ShipTypeCount + 1);
        onShipAttacked[ShipType.None] = null;           // 열결될 함수는 없음. 없는 키 참조로 인해 에러가 나는 것 방지
        onShipAttacked[ShipType.Carrier] = null;
        onShipAttacked[ShipType.BattleShip] = null;
        onShipAttacked[ShipType.Destroyer] = null;
        onShipAttacked[ShipType.Submarine] = null;
        onShipAttacked[ShipType.PatrolBoat] = null;
    }

    /// <summary>
    /// 보드 초기화
    /// </summary>
    /// <param name="ships"></param>
    public void ResetBoard(Ship[] ships)
    {
        foreach(var ship in ships)
        {
            UndoShipDeployment(ship);   // 배는 전부 배치 취소
        }

        // 공격 표시 초기화
        for(int i=0;i<isAttacked.Length;i++)
        {
            isAttacked[i] = false;
        }
        bombMark.ResetBombMarks();  // 폭탄 마크 리셋
    }

    /// <summary>
    /// 함선을 배치하는 함수
    /// </summary>
    /// <param name="ship">배치할 함선</param>
    /// <param name="grid">배치될 그리드 좌표(함선 머리 위치)</param>
    /// <returns>배치 성공하면 true, 배치가 불가능하면 false</returns>
    public bool ShipDeployment(Ship ship, Vector2Int grid)
    {
        Vector2Int[] girdPositions;
        bool result = IsShipDeplymentAvailable(ship, grid, out girdPositions);  // 배치 가능한지 확인
        if(result)
        {
            foreach(var pos in girdPositions)
            {
                shipInfo[GridToIndex(pos)] = ship.Type;     // 배치 가능하면 ShipInfo에 함선 배치 기록
            }

            Vector3 world = GridToWorld(grid);
            ship.transform.position = world;                // 배의 위치 변경
            ship.Deploy(girdPositions);                     // 배 배치 함수 실행
        }

        return result;
    }

    /// <summary>
    /// 함선을 배치하는 함수
    /// </summary>
    /// <param name="ship">배치할 함선</param>
    /// <param name="world">배치될 월드위치(함선 머리 위치)</param>
    /// <returns>배치 성공하면 true, 배치가 불가능하면 false</returns>
    public bool ShipDeployment(Ship ship, Vector3 world)
    {
        return ShipDeployment(ship, WorldToGrid(world));
    }

    /// <summary>
    /// 특정 배가 특정 위치에서 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배</param>
    /// <param name="grid">확인할 배의 위치(뱃머리 위치)</param>
    /// <param name="resultPos">확실하게 배치할 수 있는 위치(결과가 true일때만 사용)</param>
    /// <returns>true면 배치가능한 위치, false면 배치불가능한 위치</returns>
    public bool IsShipDeplymentAvailable(Ship ship, Vector2Int grid, out Vector2Int[] resultPos)
    {
        resultPos = new Vector2Int[ship.Size];  // 배 크기만큼 결과 저장할 배열 생성

        Vector2Int dir = Vector2Int.zero;
        switch(ship.Direction)                  // 배가 바라보는 방향에 따라 그리드 좌표를 구하기 위한 방향 결정하기
        {
            case ShipDirection.North:
                dir = Vector2Int.up;
                break;
            case ShipDirection.East:
                dir = Vector2Int.left;
                break;
            case ShipDirection.South:
                dir = Vector2Int.down;
                break;
            case ShipDirection.West:
                dir = Vector2Int.right;
                break;
        }

        // 확인할 그리드 위치들을 저장
        for (int i=0;i<ship.Size;i++)
        {
            resultPos[i] = grid + dir * i;  
        }

        bool result = true;
        foreach(var pos in resultPos)
        {
            if( !IsInBoard(pos) || IsShipDeployed(pos) )    // 한칸이라도 보드를 벗어나거나 배가 배치되어 있으면 실패
            {
                result = false;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 특정 배가 특정 위치에서 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배</param>
    /// <param name="grid">확인할 배의 위치(뱃머리 위치)</param>
    /// <returns>true면 배치가능한 위치, false면 배치불가능한 위치</returns>
    public bool IsShipDeplymentAvailable(Ship ship, Vector2Int grid)
    {
        return IsShipDeplymentAvailable(ship, grid, out _);
    }

    /// <summary>
    /// 특정 배가 특정 위치에서 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배</param>
    /// <param name="world">확인할 배의 위치(뱃머리 위치, 월드 포지션)</param>
    /// <returns>true면 배치가능한 위치, false면 배치불가능한 위치</returns>
    public bool IsShipDeplymentAvailable(Ship ship, Vector3 world)
    {
        return IsShipDeplymentAvailable(ship, WorldToGrid(world), out _);
    }

    /// <summary>
    /// 특정 위치에 배가 배치되어 있는지 확인하는 함수
    /// </summary>
    /// <param name="grid">확인할 그리드 좌표</param>
    /// <returns>true면 배가 있다. false면 배가 없다.</returns>
    private bool IsShipDeployed(Vector2Int grid)
    {
        return shipInfo[GridToIndex(grid)] != ShipType.None;
    }

    /// <summary>
    /// 함선 배치 취소 함수
    /// </summary>
    /// <param name="ship">배치를 취소할 배</param>
    public void UndoShipDeployment(Ship ship)
    {
        if (ship.IsDeployed)    // 이미 배치되어있는 경우에만 배치 해제 진행
        {
            foreach (var pos in ship.Positions)
            {
                shipInfo[GridToIndex(pos)] = ShipType.None; // 보드 기록 초기화
            }
            ship.UnDeploy();    // 함선 내부 처리
            ship.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 보드가 공격을 받았을 때 실행되는 함수
    /// </summary>
    /// <param name="grid">공격 받은 그리드 위치</param>
    /// <returns>공격이 배에 명중되었으면 true, 아니면 false</returns>
    public bool OnAttacked(Vector2Int grid)
    {
        bool result = false;

        if( IsInBoard(grid))                    // 보드 안쪽만 확인
        {
            int index = GridToIndex(grid);
            if( IsAttackable(index))            // 공격 가능한 위치인지 확인
            {
                isAttacked[index] = true;       // 공격 했다고 표시

                if (shipInfo[index] != ShipType.None)   // 배가 있으면
                {                    
                    result = true;                      // 공격 성공으로 체크
                    onShipAttacked[shipInfo[index]]?.Invoke();  // 공격당한 배의 델리게이트 실행
                }

                bombMark.SetBombMark(GridToWorld(grid), result);    // BombMark 표시
            }
        }
        return result;
    }

    /// <summary>
    /// 지정된 위치가 공격 가능한지 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 위치의 인덱스</param>
    /// <returns>공격 가능하면 true, 이미 공격 당한지점이면 false</returns>
    public bool IsAttackable(int index)
    {
        return !isAttacked[index];
    }

    /// <summary>
    /// 지정된 위치가 공격 가능한지 확인하는 함수
    /// </summary>
    /// <param name="grid">확인할 위치의 그리드 좌표</param>
    /// <returns>공격 가능하면 true, 이미 공격 당한지점이면 false</returns>
    public bool IsAttackable(Vector2Int grid)
    {
        return IsAttackable(GridToIndex(grid));
    }

    /// <summary>
    /// 특정 위치에 배치된 배 정보를 리턴하는 함수
    /// </summary>
    /// <param name="grid">확인할 그리드 좌표</param>
    /// <returns>해당 위치의 배 정보</returns>
    public ShipType GetShipType(Vector2Int grid)
    {
        
        return shipInfo[GridToIndex(grid)];
    }

    /// <summary>
    /// 특정 위치에 배치된 배 정보를 리턴하는 함수
    /// </summary>
    /// <param name="world">확인할 월드 좌표</param>
    /// <returns>해당 위치의 배 정보</returns>
    public ShipType GetShipType(Vector3 world)
    {
        return GetShipType(WorldToGrid(world));
    }


    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="worldPos">월드 좌표</param>
    /// <returns>그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        worldPos.y = transform.position.y;

        Vector3 diff = worldPos - transform.position;
        return new Vector2Int(Mathf.FloorToInt(diff.x), Mathf.FloorToInt(-diff.z));
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="x">그리드 x좌표</param>
    /// <param name="y">그리드 y좌표</param>
    /// <returns>해당 그리드의 가운데 지점 월드좌표</returns>
    public Vector3 GridToWorld(int x, int y)
    {
        return transform.position + new Vector3(x + 0.5f, 0, -(y+0.5f));
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="grid">그리드 좌표</param>
    /// <returns>해당 그리드의 가운데 지점 월드좌표</returns>
    public Vector3 GridToWorld(Vector2Int grid)
    {
        return GridToWorld(grid.x, grid.y);
    }    

    /// <summary>
    /// 인덱스를 월드 좌표로 변경해 주는 함수
    /// </summary>
    /// <param name="index">보드 이차원 배열의 인덱스</param>
    /// <returns>인덱스에 해당하는 그리드의 월드 좌표</returns>
    public Vector3 IndexToWorld(int index)
    {
        return GridToWorld(IndexToGrid(index));
    }

    /// <summary>
    /// 마우스 커서 위치의 그리드좌표를 구해주는 함수
    /// </summary>
    /// <returns>마우스 커서 위치의 그리드좌표</returns>
    public Vector2Int GetMouseGridPosition()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);

        return WorldToGrid(world);
    }

    /// <summary>
    /// 파라메터로 받은 월드 좌표가 보드 안인지 확인하는 함수
    /// </summary>
    /// <param name="worldPos">확인할 월드 좌표</param>
    /// <returns>true면 보드 안쪽, false면 보드 바깥쪽</returns>
    public bool IsInBoard(Vector3 worldPos)
    {
        worldPos.y = transform.position.y;

        Vector3 diff = worldPos - transform.position;

        return diff.x >= 0.0f && diff.x <= BoardSize && diff.z <= 0 && diff.z >= -BoardSize;
    }

    /// <summary>
    /// 인덱스 값을 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="index">인덱스 값</param>
    /// <returns>결과 그리드 좌표</returns>
    public static Vector2Int IndexToGrid(int index)
    {
        return new Vector2Int(index % BoardSize, index / BoardSize);
    }

    /// <summary>
    /// 그리드 좌표를 인덱스 값으로 변경해주는 함수
    /// </summary>
    /// <param name="x">그리드x</param>
    /// <param name="y">그리드y</param>
    /// <returns>해당 위치의 인덱스 값</returns>
    public static int GridToIndex(int x, int y)
    {
        int result = NOT_VALID_INDEX;   // 적절하지 않은 경우 -1
        if (IsInBoard(x,y))
        {
            result = x + y * BoardSize;
        }

        return result;
    }

    /// <summary>
    /// 그리드 좌표를 인덱스 값으로 변경해주는 함수
    /// </summary>
    /// <param name="grid">그리드 위치</param>
    /// <returns>해당 위치의 인덱스 값</returns>
    public static int GridToIndex(Vector2Int grid)
    {
        return GridToIndex(grid.x, grid.y);
    }



    /// <summary>
    /// 그리드 좌표가 적절한지 확인하는 함수
    /// </summary>
    /// <param name="x">x좌표</param>
    /// <param name="y">y좌표</param>
    /// <returns>true면 적절하고 false면 보드 밖의 좌표</returns>
    public static bool IsInBoard(int x, int y)
    {
        return x > -1 && x < BoardSize && y > -1 && y <BoardSize;
    }

    /// <summary>
    /// 그리드 좌표가 적절한지 확인하는 함수
    /// </summary>
    /// <param name="grid">그리드 좌표</param>
    /// <returns>true면 적절하고 false면 보드 밖의 좌표</returns>
    public static bool IsInBoard(Vector2Int grid)
    {
        return IsInBoard(grid.x, grid.y);
    }

    /// <summary>
    /// 특정한 위치가 공격 성공한 위치인지 확인하는 함수
    /// </summary>
    /// <param name="grid">확인할 위치</param>
    /// <returns>true면 공격이 성공한 위치, false면 실패</returns>
    public bool IsAttackSuccessPosition(Vector2Int grid)
    {
        int index = GridToIndex(grid);

        // 보드 안에 있는 위치 && 공격했던 위치 && 배가 있는 위치
        return index != NOT_VALID_INDEX && isAttacked[index] && (shipInfo[index] != ShipType.None);
    }

    /// <summary>
    /// 특정한 위치가 공격 실패한 위치인지 확인하는 함수
    /// </summary>
    /// <param name="grid">확인할 위치</param>
    /// <returns>true면 공격이 실패한 위치, false면 성공한 위치</returns>
    public bool IsAttackFailPosition(Vector2Int grid)
    {
        int index = GridToIndex(grid);

        // 보드 안에 있는 위치 && 공격했던 위치 && 배가 없는 위치
        return index != NOT_VALID_INDEX && isAttacked[index] && (shipInfo[index] == ShipType.None);
    }
}
