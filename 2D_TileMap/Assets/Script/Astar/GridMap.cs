using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
//���� 1ĭ  = 1
public class GridMap 
{
    Node[] nodes;//  �� �ʿ� �ִ� ��� ��� 

    int width;//���� ���� ũ�� 
    int height;//���� ����

    Vector2Int origin = Vector2Int.zero;//���� ����
    Tilemap background;//���� ����µ� ����� Ÿ�� �� (ũ�� Ȯ�� �� ����)

    const int Error_Not_Valid_Position = -1;//�ε����� �߸��ƴٴ� ���� ǥ���ϴ� ���

    Vector2Int[] movablePositions; //�̵������� ���� ��ǥ ����

    //Vector2Int origin; //���� ���� 
    public GridMap(int width, int height)// �׸���� ������
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
              
                GridToIndex(x, y, out int index); // x,y �� ������ Ȯ���� �ʿ䰡 ��� if����
                nodes[index] = new Node(x, y); // �ε����� �°� ��� ���� �� ����
                movablePositions[index] = new Vector2Int(x, y);
            }
        }
        //C# Ư���� �������迭�� ���� �ӵ��� ������.
        //ũ�Ⱑ �������� ���� �������� �迭�� ���� ��츸 ��� 
    }

    public GridMap(Tilemap backGround, Tilemap obstacle)//Ÿ�ϸ��� �̿��� �׸��� ����
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

        Vector2Int min = new(backGround.cellBounds.xMin, backGround.cellBounds.yMin);// Ÿ�ϸ��� ���� �Ʒ� ��
        Vector2Int max = new(backGround.cellBounds.xMax, backGround.cellBounds.yMax);// Ÿ�ϸ��� ������ �� ��

        origin = (Vector2Int)backGround.origin;//���� ������ origin���� ���� �Ѵ�.

        List<Vector2Int> movable = new List<Vector2Int>(width * height);

        for (int y = min.y; y < max.y; y++)
        {
            for (int x = min.x; x < max.x; x++)
            {
                GridToIndex(x, y, out int index); //�ε������� �ҷ�����
                TileBase tile = obstacle.GetTile(new(x, y)); //tile�� obstacle�� x, y ��ǥ�� ���� �� ����.
                Node.NodeType tileType = Node.NodeType.Plain;//�⺻���� Plain���� �ְ�

                if (tile != null) //���� tile�� null�� �ƴ϶�� �ش� ��ġ�� obstacle�� �ִٴ� �� �̹Ƿ� Ÿ�� Ÿ���� wall�� �����Ѵ�.
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
        this.background = backGround;//������ǥ ��꿡 �ʿ��ؼ� ����
        
    }
    public Node GetNode(int x, int y)// Ư����ġ�� �ִ� ��带 �����ϴ� �Լ� x,y �� �����ϴ� ������� Node�� ���� ����, ������ ����� null;
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
    bool GridToIndex(int x, int y, out int index) //�׸����� ��ġ���� �Ķ���ͷ� �޾� �ε����� ��ȯ���ִ� �Լ� 
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

    public bool IsValidPosition(int x, int y)//���� x, y���� ������ ����� �ʾҴٸ� true
    {
        //return x >= 0 && y >= 0 && x < width && y < height;
        return x < (width + origin.x) && y < (height + origin.y) && x >= origin.x && y >= origin.y;  // width && y < height && x >= 0 && y >= 0 &&;
    }
    public bool IsValidPosition(Vector2Int gridPos)
    {
        return IsValidPosition(gridPos.x, gridPos.y);
    }

    public bool IsWall(int x, int y)//�Է¹��� ��ġ�� ������ �ƴ��� �����ϴ� �Լ� true(��)
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


    public bool IsSpawnable(int x, int y)//���������� ��ġ���� Ȯ���ϴ� �Լ� 
    {
        Node node = GetNode(x, y);
        return node != null && node.nodeType == Node.NodeType.Plain;
    }
    public bool IsSpawnable(Vector2Int gridPos)
    {
        return IsSpawnable(gridPos.x, gridPos.y);
    }


    public Vector2Int WorldToGrid(Vector3 worldPosition)// ������ǥ�� �׸��� ��ǥ�� �ٲ��ִ� �Լ�
    {
        if (background != null)
        {
            return (Vector2Int)background.WorldToCell(worldPosition);
        }
        return new Vector2Int((int)worldPosition.x, (int)worldPosition.y);//0,0 �� �������� ���
    }
    public Vector2 GridToWorld(Vector2Int gridPos)
    {
        if (background != null)
        {
            return background.CellToWorld((Vector3Int)gridPos) + new Vector3(0.5f, 0.5f);
        }
        return new Vector2(gridPos.x + 0.5f, gridPos.y + 0.5f);
    }

    public Vector2Int GetRandomMoveablePosition()//�������� �̵������� ���� �ϳ��� �����ϴ� �Լ� 
    {
        int index = UnityEngine.Random.Range(0, movablePositions.Length);
        return movablePositions[index];
    }
    public Node GetNode(Vector3 worldPos)//Ư�� ������ǥ�� �ִ� ��带 �����ϴ� �Լ� // ������ǥ�� ��尡 �ִٸ� ���� ������ null
    {
        return GetNode(WorldToGrid(worldPos));
    }
}
