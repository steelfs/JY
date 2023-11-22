using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̵� ������ ���� ǥ������ ������Ʈ
/// </summary>
public class MoveRange : MonoBehaviour
{
    /// <summary>
    /// û�� �������� �ٽð˻��Ҽ��ְ� üũ�ϴº���
    /// </summary>
    bool isClear = false;
   
    /// <summary>
    /// �̵� ������ Ÿ�� ��Ƶ� ����Ʈ
    /// </summary>
    List<Tile> activeMoveTiles;

    private void Awake()
    {
        SpaceSurvival_GameManager.Instance.GetMoveRangeComp = () => this; //������ �����ϱ� 
        activeMoveTiles = new List<Tile>(); //�̵������� Ÿ�� ����Ʈ �ʱ�ȭ 
    }

    /// <summary>
    /// �ٴڿ� �̵������� ������ ǥ���ϴ� ���� 
    /// </summary>
    /// <param name="mapTiles">��Ÿ�� ���� </param>
    /// <param name="currentNode">������ġ Ÿ�� ����</param>
    /// <param name="moveSize">�̵������� �Ÿ� ��</param>
    /// <param name="tileSizeX">��Ÿ���� �ִ� ���ΰ���</param>
    /// <param name="tileSizeY">��Ÿ���� �ִ� ���ΰ���</param>
    public void MoveSizeView(Tile currentNode, float moveSize)
    {
        if (!isClear)
        {
            SetMoveSize(currentNode, moveSize); //�̵� ���� ����Ʈ �����ϱ�
            OpenLineRenderer(currentNode);
        }
    }

    /// <summary>
    /// �����ִ����� �ʱ�ȭ �ϱ� ����ġ���� �ʱ�ȭ�Ǳ⶧���� ����ġ�� ���ܵд�.
    /// </summary>
    public void ClearLineRenderer(Tile currentTile) //���� ���η����� ����
    {
        if (!isClear) 
        {
            isClear = true;
            if (activeMoveTiles.Count > 0) //�ʱ�ȭ �� Ÿ������������  
            {
                Tile.TileExistType currentTileType = currentTile.ExistType; //�������� �Ź� üũ�����ʱ����� ���δ�� 
                foreach (Tile tile in activeMoveTiles)
                {

                    if (SpaceSurvival_GameManager.Instance.ItemTileList.Contains(tile))    //�̵����� �ʱ�ȭ�Ҷ� ������ ���������Ѱ��� �����Ҷ� 
                    {
                        tile.ExistType = Tile.TileExistType.Item; //���������� �ʱ�ȭ 
                    }
                    else 
                    {
                        tile.ExistType = Tile.TileExistType.None;
                    }
                }
                currentTile.ExistType = currentTileType; //������������ �����ص״� ���� ��´�.
                activeMoveTiles.Clear();//�ʱ�ȭ�������� ���� ����
            }
            isClear = false;
        }
    }

    /// <summary>
    /// �����ִ����� ǥ���ϱ� 
    /// </summary>
    private void OpenLineRenderer(Tile currentTile) //�̵������ѹ��� �� ���η����� Ű��
    {
        Tile.TileExistType currentTileType = currentTile.ExistType; //�������� �Ź� üũ�����ʱ����� ���δ�� 
        foreach (Tile tile in activeMoveTiles)
        {
            tile.ExistType = Tile.TileExistType.Move;
        }
        currentTile.ExistType = currentTileType; //������������ �����ص״� ���� ��´�.
    }

    /// <summary>
    /// ���� ��ġ�������� �ൿ�� ���� �̵������� ���� �� ��ǥ����Ʈ�� ������������ �Լ�
    /// </summary>
    /// <param name="mapTiles">��Ÿ�� ���� </param>
    /// <param name="currentNode">������ġ Ÿ�� ����</param>
    /// <param name="moveCheck">�̵������� �Ÿ� ��</param>
    /// <param name="tileSizeX">��Ÿ���� �ִ� ���ΰ���</param>
    /// <param name="tileSizeY">��Ÿ���� �ִ� ���ΰ���</param>
    /// <returns>ĳ���Ͱ� �̵������� ��帮��Ʈ</returns>
    private void SetMoveSize(Tile currentNode, float moveCheck)
    {
        List<Tile> openList = new List<Tile>();   // Ž���� �ʿ��� ��� ����Ʈ 
        List<Tile> closeList = new List<Tile>();  // �̹� ����� �Ϸ�Ǽ� ���̻� Ž���� ���� ����Ʈ 
        Tile[] mapTiles = SpaceSurvival_GameManager.Instance.BattleMap;
        int tileSizeX = SpaceSurvival_GameManager.Instance.MapSizeX;
        int tileSizeY = SpaceSurvival_GameManager.Instance.MapSizeY;
        foreach (Tile node in mapTiles)
        {
            node.H = 1000.0f; //���������� ���»��¶� �ƽ��� ������ ����� �ȵ��ư���.
            node.MoveCheckG = 1000.0f;
        }

        openList.Add(currentNode);

        currentNode.MoveCheckG = 0.0f; //����ġ�� g �� 0�̴�

        while (openList.Count > 0)
        {
            currentNode = openList[0];
            openList.Remove(currentNode); // Ž�������� ��Ͽ��� ���� Ž������ ����� �����ϰ� 
            closeList.Add(currentNode);   // Ž�������� ����Ʈ�� ���� ����� ��´�.

            if (currentNode.MoveCheckG > moveCheck) //G ���� ���� �̵� ������ �Ÿ����� ������  ���̻� Ž���� �ʿ�������� 
            {
                continue; //������ Ž�� 
            }
            else // �̵������� �Ÿ��� 
            {
                activeMoveTiles.Add(currentNode); //��ȯ ��ų ����Ʈ�� �߰��Ѵ�.
            }

            OpenListAdd(mapTiles , tileSizeX, tileSizeY, currentNode, openList, closeList ); //�ֺ� 8������ ��带 ã�Ƽ� G�� �����ϰ�  ���¸���Ʈ�� ������������ ��´�.
            openList.Sort();            //ã�� G���� ���� ���������� ��Ž���̵ȴ�.
        }
    }
    private void OpenListAdd(Tile[] mapTiles, int tileSizeX, int tileSizeY,  Tile currentNode, List<Tile> open, List<Tile> close)
    {
        Tile adjoinTile;
        float sideDistance = 1.0f;
        float diagonalDistance = 1.414f;
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if (currentNode.Width + x < 0 || currentNode.Width + x > tileSizeX - 1 || // ���̵� �˻� 
                    currentNode.Length + y < 0 || currentNode.Length + y > tileSizeY - 1) //���̵� �˻�
                    continue;

                adjoinTile = Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width + x,currentNode.Length + y,tileSizeX);    // ������ Ÿ�� ��������

                if (adjoinTile == currentNode)                                          // ������ Ÿ���� (0, 0)�� ���
                    continue;
                if (adjoinTile.ExistType != Tile.TileExistType.None && adjoinTile.ExistType != Tile.TileExistType.Item) // ������ Ÿ���� �̵������Ѱ��϶� �� �ƴ� ��
                    continue;

                bool isDiagonal = (x * y != 0);                                     // �밢�� ���� Ȯ��
                if (isDiagonal &&                                                   // �밢���̰� ���� Ÿ���� �����¿찡 ���� ��
                    Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width + x, currentNode.Length, tileSizeX).ExistType == Tile.TileExistType.Prop ||
                    Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width, currentNode.Length+ y, tileSizeX).ExistType == Tile.TileExistType.Prop
                    )
                    continue;

                float distance;
                if (isDiagonal)
                {
                    distance = diagonalDistance;
                }
                else
                {
                    distance = sideDistance;
                }

                if (adjoinTile.MoveCheckG > currentNode.MoveCheckG + distance)
                {
                    open.Add(adjoinTile);
                    adjoinTile.MoveCheckG = currentNode.MoveCheckG + distance;
                }
            }
        }

    }

    /// <summary>
    /// �������� ���� ������(none�ΰ�) �� ã�Ƽ� ��ȯ�ϴ� �Լ�  ��ã���� null ��ȯ
    /// </summary>
    /// <param name="tileType">Ÿ������ �ö� Ÿ��</param>
    /// <returns>�߰������� Ÿ������ȯ �ϰų� �������� null ��ȯ</returns>
    public Tile GetRandomTile(Tile.TileExistType tileType) 
    {

        int x = SpaceSurvival_GameManager.Instance.MapSizeX;
        int y = SpaceSurvival_GameManager.Instance.MapSizeY;
        Tile[] mapTiles = SpaceSurvival_GameManager.Instance.BattleMap;
        Tile result = null;
        int maxCount = 100; //�ִ� 100��������.
        int count = 0;
        //Debug.Log($"{x},{y}");
        while (count < maxCount) //���� ���� ������ 
        {
            result = Cho_BattleMap_AStar.GetTile(mapTiles, Random.Range(0, x), Random.Range(0, y), x);
            if (result.ExistType == Tile.TileExistType.None)//�����ִ°��̸� 
            {
                result.ExistType = tileType; //�����Ǿߵ� Ÿ������ �ٲ۵� 
                return result;//����������.
            }
            else 
            {
                result = null; //������ �����̸� �ʱ�ȭ ��Ų��.   
            }

            count++; //���ѷ��� ������
        }
        Debug.Log($"{count} ��ŭ ���Ҵµ� �ٲ����ִ��� ��ã�Ҿ� ");
        return null;
    }

}
