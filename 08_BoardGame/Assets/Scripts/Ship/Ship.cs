using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 배의 종류
/// </summary>
public enum ShipType : byte
{
    None = 0,   // 아무배도 아니다(배치 정보에 사용)
    Carrier,    // 항공모함(5칸짜리)
    BattleShip, // 전함(4칸짜리)
    Destroyer,  // 구축함(3칸짜리)
    Submarine,  // 잠수함(3칸짜리)
    PatrolBoat  // 경비정(2칸짜리)
}

/// <summary>
/// 배의 방향(뱃머리가 향하는 방향)
/// </summary>
public enum ShipDirection : byte
{
    North = 0,
    East = 1,
    South = 2,
    West = 3,
}

public class Ship : MonoBehaviour
{

    /// <summary>
    /// 배의 종류
    /// </summary>
    ShipType shipType = ShipType.None;
    public ShipType Type
    {
        get => shipType;
        private set
        {
            shipType = value;
            switch(shipType)
            {
                case ShipType.Carrier:
                    size = 5;
                    shipName = "항공모함";
                    break;
                case ShipType.BattleShip:
                    size = 4;
                    shipName = "전함";
                    break;
                case ShipType.Destroyer:
                    size = 3;
                    shipName = "구축함";
                    break;
                case ShipType.Submarine:
                    size = 3;
                    shipName = "잠수함";
                    break;
                case ShipType.PatrolBoat:
                    size = 2;
                    shipName = "경비정";
                    break;
                default:
                    break;
            }
            // shipName
        }
    } 

    /// <summary>
    /// 배가 바라보는 방향.기본적으로 북->동->남->서->북 순서가 정방향 회전
    /// </summary>
    ShipDirection direction = ShipDirection.North;
    public ShipDirection Direction
    {
        get => direction;
        set
        {
            direction = value;
            model.rotation = Quaternion.Euler(0, (int)direction * 90, 0);
        }
    }

    /// <summary>
    /// 배의 이름
    /// </summary>
    string shipName;

    public string ShipName => shipName;
    public string Opponent { get; set; }
    public string Owner { get; set; }

    /// <summary>
    /// 배의 크기(=최대 HP)
    /// </summary>
    int size = 0;
    public int Size => size;

    /// <summary>
    /// 배의 현재 내구도
    /// </summary>
    int hp = 0;
    public int HP
    {
        get => hp;
        set
        {            
            hp = value;
            if(hp < 1 && IsAlive)   // 살아있는데 HP가 0이하가 되면 사망
            {
                OnSinking();
            }
            on_HpChange?.Invoke(hp, this.shipType);
        }
    }
    public Action<int, ShipType> on_HpChange;
    /// <summary>
    /// 배의 생존 여부(true면 살아있다. false면 침몰됐다.)
    /// </summary>
    bool isAlive = true;
    public bool IsAlive => isAlive;

    /// <summary>
    /// 배가 배치된 위치.
    /// </summary>
    Vector2Int[] positions;
    public Vector2Int[] Positions => positions;

    /// <summary>
    /// 배가 배치되었는지 여부(true면 배가 이미 배치되어있다. false면 아직 배치되지 않았다.)
    /// </summary>
    bool isDeployed = false;
    public bool IsDeployed => isDeployed;

    /// <summary>
    /// 배의 색상 변경 용
    /// </summary>
    Renderer shipRenderer;

    /// <summary>
    /// 배 모델 메시에 대한 트랜스폼
    /// </summary>
    Transform model;

    /// <summary>
    /// 함선이 배치되거나 배치해제 되었을 때 실행될 델리게이트(파라메터: true면 배치되었다. false면 배치해제되었다.)
    /// </summary>
    public Action<bool> onDeploy;

    /// <summary>
    /// 함선이 공격을 당했을 때 실행될 델리게이트(파라메터:자기자신)
    /// </summary>
    public Action<Ship> onHit;

    /// <summary>
    /// 함선이 침몰되었을 때 실행될 데리게이트(파라메터:자기자신)
    /// </summary>
    public Action<Ship> onSinking;

    public void Initialize(ShipType type)
    {
        Type = type;

        model = transform.GetChild(0);
        shipRenderer = model.GetComponentInChildren<Renderer>();
        
        ResetData();

        gameObject.name = $"{shipType}_{Size}";
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 공통적으로 데이터 초기화하는 함수
    /// </summary>
    void ResetData()
    {
        hp = size;                          // hp를 배의 크기로 초기화
        Direction = ShipDirection.North;    // 방향은 무조건 북쪽이 초기값
        isAlive = true;                     // 배가 살아있다고 초기화
        isDeployed = false;                 // 배치는 안되었다고 초기화
        positions = null;                   // 배치 위치는 비우기
    }

    /// <summary>
    /// 함선의 머티리얼을 선택하는 함수
    /// </summary>
    /// <param name="isNormal">true 불투명 머티리얼 사용, false면 배치모드용 반투명 머티리얼 사용</param>
    public void SetMaterialType(bool isNormal = true)
    {
        if(isNormal)
        {
            shipRenderer.material = ShipManager.Inst.NormalShipMaterial;
        }
        else
        {
            shipRenderer.material = ShipManager.Inst.DeployModeShipMaterial;
        }
    }

    /// <summary>
    /// 함선이 배치될 때 실행될 함수
    /// </summary>
    /// <param name="deployPositions">배치되는 위치들</param>
    public void Deploy(Vector2Int[] deployPositions)
    {
        SetMaterialType();              // 머티리얼 normal로 설정
        positions = deployPositions;    // 위치 기록
        isDeployed = true;              // 배치되었다고 표시
        onDeploy?.Invoke(true);         // 배치 되었음을 알림
    }

    /// <summary>
    /// 함선이 배치 해제되었을 때 실행되는 함수
    /// </summary>
    public void UnDeploy()
    {
        ResetData();                // 데이터 초기화
        onDeploy?.Invoke(false);    // 배치 해제되었음을 알림
    }

    /// <summary>
    /// 함선을 90도씩 회전 시키는 함수
    /// </summary>
    /// <param name="isCW">true면 시계방향, false 반시계방향</param>
    public void Rotate(bool isCW = true)
    {
        int dirCount = ShipManager.Inst.ShipDirectionCount;
        if(isCW)
        {
            Direction = (ShipDirection)(((int)Direction + 1) % dirCount);
        }
        else
        {
            Direction = (ShipDirection)(((int)Direction + dirCount - 1) % dirCount);
        }
    }

    /// <summary>
    /// 함선을 랜덤한 방향으로 회전시키는 함수
    /// </summary>
    public void RandomRotate()
    {
        int rotateCount = UnityEngine.Random.Range(0, ShipManager.Inst.ShipDirectionCount); // 함선을 몇번 회전시킬지 결정
        bool isCW = UnityEngine.Random.Range(0, 2) == 0;    // 정방향인지 역방향인지 결정
        for(int i = 0; i < rotateCount; i++)                // 결정된 횟수와 방향으로 회전
        {
            Rotate(isCW);
        }
    }

    /// <summary>
    /// 함선이 공격을 받았을 때 실행되는 함수
    /// </summary>
    public void OnHitted()
    {
        Debug.Log($"[{Type}]이 공격 받았다.");
        HP--;

        if(IsAlive)
        {
            onHit?.Invoke(this);
        }
    }    

    /// <summary>
    /// 함선이 침몰할 때 실행되는 함수
    /// </summary>
    /// 
    
    void OnSinking()
    {
        Debug.Log($"[{Type}]이 침몰되었다.");
        isAlive = false;
        onSinking?.Invoke(this);
    }
}
