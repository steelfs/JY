using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
//리드 1칸  = 1
public class GridMap 
{
    Node[] nodes;//  이 맵에 있는 모든 노드 

    int width;//맵의 가로 크기 
    int height;//맵의 세로

    Vector2Int origin = Vector2Int.zero;//맵의 원점
    Tilemap background;//맵을 만드는데 사용한 타일 맵 (크기 확인 및 계산용)

    const int Error_Not_Valid_Position = -1;//인덱스가 잘못됐다는 것을 표시하는 상수

    Vector2Int[] movablePositions; //이동가능한 평지 좌표 모음

    //Vector2Int origin; //맵의 원점 
    public GridMap(int width, int height)// 그리드맵 생성자
    {
        this.width = width;
        this.height = height;

        nodes = new Node[width * height];
        movablePositions = new Vector2Int[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //if (GridToIndex(x, y, out int index))
                //{

                //}
              
                GridToIndex(x, y, out int index); // x,y 의 범위를 확인할 필요가 없어서 if생략
                nodes[index] = new Node(x, y); // 인덱스에 맞게 노드 생성 및 저장
                movablePositions[index] = new Vector2Int(x, y);
            }
        }
        //C# 특성상 다차원배열은 접근 속도가 느리다.
        //크기가 일정하지 않은 여러개의 배열이 있을 경우만 사용 
    }

    public GridMap(Tilemap backGround, Tilemap obstacle)//타일맵을 이용해 그리드 생성
    {
        //backGround.size.x;
        //backGround.size.y;
        //backGround.origin;
        //backGround.cellBounds.xMin;
        //backGround.cellBounds.xMax;
        //backGround.cellBounds.yMin;
        //backGround.cellBounds.yMax;
        //obstacle.GetTile()

        this.width = backGround.size.x;
        this.height = backGround.size.y;
        nodes = new Node[width * height];

        Vector2Int min = new(backGround.cellBounds.xMin, backGround.cellBounds.yMin);// 타일맵의 왼쪽 아래 끝
        Vector2Int max = new(backGround.cellBounds.xMax, backGround.cellBounds.yMax);// 타일맵의 오른쪽 위 끝

        origin = (Vector2Int)backGround.origin;//맵의 원점을 origin으로 설정 한다.

        List<Vector2Int> movable = new List<Vector2Int>(width * height);

        for (int y = min.y; y < max.y; y++)
        {
            for (int x = min.x; x < max.x; x++)
            {
                GridToIndex(x, y, out int index); //인덱스갓을 불러오기
                TileBase tile = obstacle.GetTile(new(x, y)); //tile을 obstacle의 x, y 좌표에 대입 해 본다.
                Node.NodeType tileType = Node.NodeType.Plain;//기본값은 Plain으로 주고

                if (tile != null) //만약 tile이 null이 아니라면 해당 위치에 obstacle이 있다는 뜻 이므로 타일 타입을 wall로 설정한다.
                {
                    tileType = Node.NodeType.Wall;
                }
                else
                {
                    movable.Add(new Vector2Int(x, y));
                }
                nodes[index] = new Node(x, y, tileType);
            }
        }
        movablePositions = movable.ToArray();
        this.background = backGround;//월드좌표 계산에 필요해서 저장
        
    }
    public Node GetNode(int x, int y)// 특정위치에 있는 노드를 리턴하는 함수 x,y 가 존재하는 범위라면 Node의 참조 리턴, 범위를 벗어나면 null;
    {
        Node node = null;
        if (GridToIndex(x, y, out int index))
        {
            node = nodes[index];
        }
        return node;
    }
    public Node GetNode(Vector2Int gridPos)
    {
        return GetNode(gridPos.x, gridPos.y);
    }

    public void ClearMapData()
    {
        foreach (Node node in nodes)
        {
            node.ClearData();
        }
    }
    bool GridToIndex(int x, int y, out int index) //그리드의 위치값을 파라미터로 받아 인덱스로 변환해주는 함수 
    {
        bool result = false;
        index = Error_Not_Valid_Position;
        if (IsValidPosition(x, y))
        {
            index = (x - origin.x) + (height - 1 - (y - origin.y)) * width;
            result = true;
        }
        return result;
    }

    public bool IsValidPosition(int x, int y)//만약 x, y값이 범위를 벗어나지 않았다면 true
    {
        //return x >= 0 && y >= 0 && x < width && y < height;
        return x < (width + origin.x) && y < (height + origin.y) && x >= origin.x && y >= origin.y;  // width && y < height && x >= 0 && y >= 0 &&;
    }
    public bool IsValidPosition(Vector2Int gridPos)
    {
        return IsValidPosition(gridPos.x, gridPos.y);
    }

    public bool IsWall(int x, int y)//입력받은 위치가 벽인지 아닌지 리턴하는 함수 true(벽)
    {
        Node node = GetNode(x, y);
        return node != null && node.nodeType == Node.NodeType.Wall;
    }
    public bool IsWall(Vector2Int gridPos)
    {
        return IsWall(gridPos.x, gridPos.y);
    }
    public bool IsMonster(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.nodeType == Node.NodeType.Monster;
    }
    public bool IsMonster(Vector2Int gridPos)
    {
        return IsMonster(gridPos.x, gridPos.y);
    }


    public bool IsSpawnable(int x, int y)//스폰가능한 위치인지 확인하는 함수 
    {
        Node node = GetNode(x, y);
        return node != null && node.nodeType == Node.NodeType.Plain;
    }
    public bool IsSpawnable(Vector2Int gridPos)
    {
        return IsSpawnable(gridPos.x, gridPos.y);
    }


    public Vector2Int WorldToGrid(Vector3 worldPosition)// 월드좌표를 그리드 좌표로 바꿔주는 함수
    {
        if (background != null)
        {
            return (Vector2Int)background.WorldToCell(worldPosition);
        }
        return new Vector2Int((int)worldPosition.x, (int)worldPosition.y);//0,0 을 기준으로 계산
    }
    public Vector2 GridToWorld(Vector2Int gridPos)
    {
        if (background != null)
        {
            return background.CellToWorld((Vector3Int)gridPos) + new Vector3(0.5f, 0.5f);
        }
        return new Vector2(gridPos.x + 0.5f, gridPos.y + 0.5f);
    }

    public Vector2Int GetRandomMoveablePosition()//랜덤으로 이동가능한 지역 하나를 리턴하는 함수 
    {
        int index = UnityEngine.Random.Range(0, movablePositions.Length);
        return movablePositions[index];
    }
    public Node GetNode(Vector3 worldPos)//특정 월드좌표에 있는 노드를 리턴하는 함수 // 월드좌표에 노드가 있다면 리턴 없으면 null
    {
        return GetNode(WorldToGrid(worldPos));
    }
}
