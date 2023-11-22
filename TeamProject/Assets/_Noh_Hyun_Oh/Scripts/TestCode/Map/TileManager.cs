using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Ÿ������ ��Ѱ��� ��ġ���ִ��� ��Ȳ 
/// </summary>
public enum CurrentTileState
{
    None = 0,           // �� ĭ
    InaccessibleArea,   // ���� �Ұ��� ����
    Prop,               // ��ֹ�
    Unit,               // �̵��ϴ� ���ֵ�
    Charcter,           // ��Ʈ���ϴ� ����
    MoveArea,          // �̵������� ����
}

/// <summary>
/// Ÿ�ϱ׷��� �������� �ȵ��� ���ϴ� �̳� ���߿� �����߰�
/// </summary>
public enum CurrentTileGroupState 
{
    Nomal = 0,                  //�⺻
    InaccessibleArea,           // ���ٺҰ���
}

[Flags]
public enum CheckingObstacles : byte
{
    None = 0,
    UP = 1,
    DOWN = 2,
    LEFT = 4,
    RIGHT = 8,
    ALL = UP | DOWN | LEFT | RIGHT,
}

/// <summary>
/// Ÿ�� ������ �޴���
/// </summary>
public class TileManager : MonoBehaviour
{
    /// <summary>
    /// Ÿ�� ������
    /// </summary>
    [SerializeField]
    Base_TileCell tilePrefab;

    /// <summary>
    /// ������� Ÿ�� �迭
    /// </summary>
    [SerializeField]
    ITileBase[] mapTiles;
    public ITileBase[] MapTiles => mapTiles;

    /// <summary>
    /// Ÿ�� �ִ� ���ΰ���
    /// </summary>
    [SerializeField]
    [Range(1,100)]
    int mapMaxHorizontal = 100;
    public int MapMaxHorizontal => mapMaxHorizontal;

    /// <summary>
    /// Ÿ�� �ִ� ���ΰ���
    /// </summary>
    [SerializeField]
    [Range(1, 100)]
    int mapMaxVertical = 100;
    public int MapMaxVertical => mapMaxVertical;

    /// <summary>
    /// �ִ�ũ���� ���� ������ ������ �����ְ� ������ �����ϱ����� �ʿ��ϴ�
    /// </summary>
    [SerializeField]
    [Range(1,100)]
    int horizontalGroupLength = 5;
    public int HorizontalGroupLength => horizontalGroupLength;
    /// <summary>
    /// �ִ�ũ���� ���� ������ ������ �����ְ� ������ �����ϱ����� �ʿ��ϴ�
    /// </summary>
    [SerializeField]
    [Range(1,100)]
    int verticalGroupLength = 4;
    public int VerticalGroupLength => verticalGroupLength;

    /// <summary>
    /// ��ü������ ��ֹ��� �����ϰ��ִº��� �����  
    /// </summary>
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float obstaclesProbability = 0.0f;

    /// <summary>
    /// Ÿ�� �׷��� ������ �ش�׷��� ���� �������� �Ұ������� ������.
    /// </summary>
    TileGroupElement[] tileGroups;

    [SerializeField]
    /// <summary>
    /// ���� �Ұ����� �׷� �� ���� �迭
    /// </summary>
    int[] obstaclesIndexArray;
    public int[] ObstacleIndexArray => obstaclesIndexArray;

    private void Awake()
    {
        AstarProccess.onTileCreate += AddTileGroup; //Ÿ�ϻ��� ���� ����
    }
    /// <summary>
    /// 
    /// </summary>
    public void GenerateTileMap() 
    {
        int arrayGroupSize = horizontalGroupLength * verticalGroupLength; //�׷� �Ѱ����� ���Ѵ�.
        
        //���� �Ұ����� �ε��� ���� ����

        int[] gridGroupArray = new int[arrayGroupSize]; //�׷� �ε��� ������ �迭�� ���
        
        for (int i = 0; i < arrayGroupSize; i++) 
        {
            gridGroupArray[i] = i; //�׷� �ε������ٰ� �ε����� 0������ ���������� �����Ѵ�.     
        }

        Shuffle(gridGroupArray); //�׷� �ε��� �� ������ �������� ���´�. 

        int obstaclesLength = (int)(arrayGroupSize * obstaclesProbability); //���� �Ұ����� �׷� ������ ���Ѵ�. float => int �� ��Ȯ�������� 

        obstaclesIndexArray = new int[obstaclesLength]; //���� �Ұ����� ���� �ε��� �迭�� �����Ѵ�.

        for (int i = 0; i < obstaclesLength; i++)
        {
            obstaclesIndexArray[i] = gridGroupArray[i]; //���ٺҰ����� ������ ����  ��Ƶд�.
        }

        Array.Sort(obstaclesIndexArray); //������ �ѵڿ� 

        // �������� ���ٺҰ����� ���� ��


        //���� ��ġ�� ����ġ�� ���� ������ �̾����־�� ������ �װͿ����� ó�� ����


       /*
        ��� ���� ���� ���� ����Ǵ°��� Ȯ���ϱ����� 
        openList ���� �� �ٵ����� ���������ȳ��ý� 
        H�� �������� ���� �������� ������̰� 
        �׾ȿ� �� �������� ������ 
        �������ġ�� ����
        ������ ��ġ���� ���¸���Ʈ�� �ٽ� �ְ� ������ ���ѹݺ� .
        */




        //���� �Ұ����� �ε��� ����


        tileGroups = new TileGroupElement[arrayGroupSize];  //�׷������ Ŭ������ �迭ũ�⸸ŭ �޸� ������ ��� 



        int gridGroupIndex  = 0; // Ÿ�� �׷� �ε��� ��Ƶ� ���� ��

        int gridCheckingIndex = 0; // �����Ұ����� üũ�� ���� 
        
        Vector2Int gridPosX = Vector2Int.zero; //��ü ���������� x��ǥ�� ����ġ(���� x,�� y)��ġ�� ��Ƶд�
        
        Vector2Int gridPosY = Vector2Int.zero; //��ü ���������� y��ǥ�� ����ġ(���� x,�� y)��ġ�� ��Ƶд�

        Vector2Int gridPosZ = Vector2Int.zero; //��ü ���������� �ش�׷��� ���� ����(x �ٴ�, y õ��)�� ��´�

        int groupCellSizeX = mapMaxHorizontal / horizontalGroupLength;    //�׷��� �������ִ� ���� �� ������ ���Ѵ� 

        int groupCellSizeY = mapMaxVertical / verticalGroupLength;  //�׷��� �������ִ� ���� �� ������ ���Ѵ�.

        int groupCellSize = (groupCellSizeX * groupCellSizeY); //�� �׷쿡 ��� ������


        //int cellGenerateLength = (mapMaxHorizontal * mapMaxVertical)                           // �� ��üũ�⿡�� 
        //                            -                                                   // ����
        //                         (groupCellSize * obstaclesLength); // ���ٺҰ����� �� (�����ȵǵ��ɼ�) ��ŭ


        mapTiles = new ITileBase[mapMaxHorizontal * mapMaxVertical]; //������ �ʿ��� ����ŭ ũ�⸦ ��´�.

        int groupCellIndex = 0; //�����ȿ��� ����� �ӽú��� (Ÿ�� �ε��� ���Һ���)

        int tempGroupCellEndIndexY = 0;  //�����ȿ��� ����� �ӽú���
        int tempGroupCellEndIndexX = 0;  //�����ȿ��� ����� �ӽú���
        int tempGroupCellIndexZ = 0;  //�����ȿ��� ����� �ӽú���

        for (int group_y = 0; group_y < verticalGroupLength; group_y++) // �׷��� ���� ���̸�ŭ ������
        {
            for (int group_x = 0; group_x < horizontalGroupLength; group_x++) //�׷��� ���� ���� ��ŭ ������
            {

                tempGroupCellEndIndexY = ((group_y + 1) * groupCellSizeY); //Y��ǥ ���������� ���� (�� �����Ҷ� ���)
                tempGroupCellEndIndexX = ((group_x + 1) * groupCellSizeX); //X��ǥ ���������� ���� (�� �����Ҷ� ���)


                gridPosX.x =  group_x * groupCellSizeX;             //�׷� X��ǥ �� ���� ������ġ 
                gridPosX.y =  tempGroupCellEndIndexX - 1;           //�׷� X��ǥ �� ���� ������ ��ġ 
                gridPosY.x =  group_y * groupCellSizeY;             //�׷� Y��ǥ �� ���� ������ġ
                gridPosY.y =  tempGroupCellEndIndexY- 1;            //�׷� X��ǥ �� ���� ������ ��ġ 
                /*
                 ���̼����� ���⼭ ������ϴµ� �ϴ� ���߿� ��������.
                 */

                if (obstaclesIndexArray.Length > 0 && gridCheckingIndex < obstaclesIndexArray.Length &&  //IndexOutOfRangeException ���� ������ 
                    gridGroupIndex == obstaclesIndexArray[gridCheckingIndex]) //���ٺҰ����� �׷� ������ŭ üũ�ϰ� 
                {
                    //���ٺҰ����� ���������� ����


                    tileGroups[gridGroupIndex] = new TileGroupElement(
                                                    gridPosX,
                                                    gridPosY,
                                                    Vector2Int.one,
                                                    CurrentTileGroupState.InaccessibleArea
                                                    ); 
                    

                    gridCheckingIndex++; //���ٺҰ����� �ε���
                }
                else 
                {
                    //���� ������ ������ ���� ���� 


                    tileGroups[gridGroupIndex] = new TileGroupElement(
                                                    gridPosX,
                                                    gridPosY,
                                                    Vector2Int.one,
                                                    CurrentTileGroupState.Nomal
                                                    ); //������ �ε����� ��ġ�� �׷� �����͸� �����Ѵ�.



                    tempGroupCellIndexZ = 1; //���̰� ���ÿ� ����   ******** ���߿� ���̵� �����Ҷ� ��� ********

                    /*
                     ���� �����Ϸ��� ���⼭ �۾� 
                     */


                    //������ �ʿ��� ���� �����Ѵ�.
                    for (int cellY = tempGroupCellEndIndexY - groupCellSizeY; cellY < tempGroupCellEndIndexY; cellY++)
                    {
                        for (int cellX = tempGroupCellEndIndexX - groupCellSizeX ; cellX < tempGroupCellEndIndexX; cellX++)
                        {
                            //int groupCellIndex = (gridGroupIndex * groupCellSize) + tempGroupCellIndexCount; //�����ε��� 
                            groupCellIndex = (horizontalGroupLength * cellY) + cellX; //�����ε��� 

                            mapTiles[groupCellIndex] = (ITileBase)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.TILE_POOL);
                            //mapTiles[groupCellIndex] =  Instantiate(tilePrefab); //���丮 ���� �ʿ� 

                            mapTiles[groupCellIndex].OnInitData(
                                    groupCellIndex,
                                    new Vector3Int(  cellX,  
                                                     cellY, 
                                                    tempGroupCellIndexZ), //��ġ�� ���� 
                                    CurrentTileState.None       //�⺻�� �̵������ϰ� ����
                                    ); //��ġ�� �� ���� �ʱ�ȭ 
                            mapTiles[groupCellIndex].OnClick += TileOnClick;
                        }
                    }

                }
                gridGroupIndex++; //�׷� �ε�����

            }
        }
    }

    public void AddTileGroup(Astar_Node addNode) 
    {
        int groupCellIndex = (horizontalGroupLength * addNode.Y) + addNode.X; //�����ε��� 

        mapTiles[groupCellIndex] = (ITileBase)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.TILE_POOL);
        //mapTiles[groupCellIndex] =  Instantiate(tilePrefab); //���丮 ���� �ʿ� 

        mapTiles[groupCellIndex].OnInitData(
                groupCellIndex,
                new Vector3Int(addNode.X,
                                 addNode.Y,
                                1), //��ġ�� ���� 
                CurrentTileState.None       //�⺻�� �̵������ϰ� ����
                ); //��ġ�� �� ���� �ʱ�ȭ 
        mapTiles[groupCellIndex].OnClick += TileOnClick;
    }

    /// <summary>
    /// �迭�� ���� �Լ� 
    /// ���� : ���� �ڽ��� ��ġ�� �����Ҽ����⶧���� Ȯ���� �յ��� Ȯ���� �����Եȴ�.
    /// ���� : ���� �ڽ��� ��ġ���� �������̵��� �Ǵ� �Լ��� 0���ε������� 0����ġ�� ������������.
    /// </summary>
    /// <param name="source">���� �迭</param>
    private void Shuffle(int[] source)
    {
        // source�� ���� ����(�Ǽ�-������ �˰��� ���)
        int loopCount = source.Length - 1; 
        for (int i = 0; i < loopCount; i++) 
        {
            int randomIndex = UnityEngine.Random.Range(0, source.Length - i);   // ��ü �������� ��� 1�� �����ϴ� ����
            int lastIndex = loopCount - i;  // ���������� ��� 1�� �����ϴ� ����

            (source[lastIndex], source[randomIndex]) = (source[randomIndex], source[lastIndex]);    // �����ϱ�
        }
    }

    private void TileOnClick(int index) 
    {
        Debug.Log(index);
    }

    public void TileLineCheck(int startIndex , int endindex) 
    {
        
        
    }
}