using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum ShipType : byte
{
    None = 0, // 배가 아닐 때
    Carrier,  // 항공모함
    BattleShip,// 전함 4칸
    Destroyer, // 구축함 3칸
    SubMarine, // 잠수함 3칸
    PatrolBoat // 경비정 2칸
}
public enum ShipDirection : byte //뱃머리가 향하는 방향
{
    North = 0,
    East,
    South,
    West

}
public class Ship : MonoBehaviour
{
    //배의 종류
    ShipType shipType = ShipType.None;
    public ShipType ShipType
    {
        get => shipType;
        private set
        {
            shipType = value;
            switch (shipType)
            {
                case ShipType.Carrier:
                    size = 5;
                    break;
                case ShipType.BattleShip:
                    size = 4;
                    break;
                case ShipType.Destroyer:
                    size = 3;
                    break;
                case ShipType.SubMarine:
                    size = 3;
                    break;
                case ShipType.PatrolBoat:
                    size = 2;
                    break;
                default:
                    break;
            }
            //shipName
        }
    }

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

    string shipName;
    public string ShipName => shipName;

    int size = 0;
    public int Size => size;

    int hp = 0;
    public int HP
    {
        get => hp;
        set
        {

        }
    }

    bool isAlive = true;
    public bool IsAlive => isAlive;

    bool isDeployed = false;//배치가 됐는가
    public bool IsDeployed => isDeployed;
    Vector2Int[] positions; // 배가 배치된 위치
    public Vector2Int[] Positions => positions;
    Vector2Int[] defaultSize;

    Board board;
    Renderer shipRenderer;

    Transform model;//배 모델 메시의 트렌스폼

    public Action<Ship> on_Hit;
    public Action<Ship> on_Sinking;
    public Action<bool> on_Deploy;// 배치 완료, 취소시

    public void Initialize(ShipType type)
    {
        board = FindObjectOfType<Board>();
        ShipType = type;
        defaultSize = new Vector2Int[Size];
        positions = defaultSize;

        model = transform.GetChild(0);
        shipRenderer = model.GetComponentInChildren<Renderer>();
        shipRenderer.material = ShipManager.Inst.DeployModeShip_Material;

        ResetData();

        gameObject.name = $"{ShipType}_{size}";
        StartCoroutine(movingCoroutine());
        gameObject.SetActive(false);
    }
    IEnumerator movingCoroutine()
    {
        Vector3 newPos = Vector3.zero;
        Vector2Int gridPos = Vector2Int.zero; 

        while (true)
        {
            gridPos = board.Get_Mouse_Grid_Pos();

            newPos.x = gridPos.x + 0.5f;
            newPos.z = -gridPos.y -(0.5f);
            newPos.y = 0;
            transform.position = newPos;
            yield return null;
        }

    }

    void ResetData()//공통으로 데이터 초기화하는 함수 
    {
        hp = size; 
        Direction = ShipDirection.North;

        isAlive = true;
        isDeployed = false; //배치 해제
        positions = null;
    }

    public void SetMaterial_Type(bool isNormal = true)// true면 불투명머티리얼, false = 배치모드용 반투명
    {
        if (isNormal)
        {
            shipRenderer.material = ShipManager.Inst.NormalShip_Material;
        }
        else
        {
            shipRenderer.material = ShipManager.Inst.DeployModeShip_Material;
        }
    }

    public void Deploy(Vector2Int[] position)//배치될 위치
    {
        positions = position;
        isDeployed = true;
        on_Deploy?.Invoke(true);

       // SetMaterial_Type(true);
       // StopAllCoroutines();
        //위치 저장
    }
    public void UnDeploy()
    {
        ResetData();
        on_Deploy?.Invoke(false);
    }
    public void Rotate(bool isCW = true)//함선을 90도씩 회전시키는 함수 true = 반시계방향 회전, false면 시계방향
    {
        int dirCount = ShipManager.Inst.ShipDirection_Count;
        if (isCW)
        {
            //Direction++;
            Direction = (ShipDirection)(((int)(Direction) + 1) % dirCount);
        }
        else
        {
            Direction = (ShipDirection)(((int)Direction + dirCount - 1) % dirCount);
            //Direction--;
        }
        
    }

    public void RandomRotate()//랜덤한 방향으로 회전
    {

    }
    public void OnHitted()
    {

    }
    void OnSinking()// 함선이 침몰할 때 실행될 함수 
    {

    }

}
