using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 타일위에 어떠한것이 배치되있는지 상황 
/// </summary>
public enum CurrentTileState
{
    None = 0,           // 빈 칸
    InaccessibleArea,   // 접근 불가능 지역
    Prop,               // 장애물
    Unit,               // 이동하는 유닛들
    Charcter,           // 컨트롤하는 유닛
    MoveArea,          // 이동가능한 지역
}

/// <summary>
/// 타일그룹이 생성될지 안될지 정하는 이넘 나중에 상태추가
/// </summary>
public enum CurrentTileGroupState 
{
    Nomal = 0,                  //기본
    InaccessibleArea,           // 접근불가능
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
/// 타일 관리할 메니저
/// </summary>
public class TileManager : MonoBehaviour
{
    /// <summary>
    /// 타일 프리팹
    /// </summary>
    [SerializeField]
    Base_TileCell tilePrefab;

    /// <summary>
    /// 만들어진 타일 배열
    /// </summary>
    [SerializeField]
    ITileBase[] mapTiles;
    public ITileBase[] MapTiles => mapTiles;

    /// <summary>
    /// 타일 최대 가로갯수
    /// </summary>
    [SerializeField]
    [Range(1,100)]
    int mapMaxHorizontal = 100;
    public int MapMaxHorizontal => mapMaxHorizontal;

    /// <summary>
    /// 타일 최대 세로갯수
    /// </summary>
    [SerializeField]
    [Range(1, 100)]
    int mapMaxVertical = 100;
    public int MapMaxVertical => mapMaxVertical;

    /// <summary>
    /// 최대크기의 맵을 구역을 나눠서 갈수있고 못가고 설정하기위해 필요하다
    /// </summary>
    [SerializeField]
    [Range(1,100)]
    int horizontalGroupLength = 5;
    public int HorizontalGroupLength => horizontalGroupLength;
    /// <summary>
    /// 최대크기의 맵을 구역을 나눠서 갈수있고 못가고 설정하기위해 필요하다
    /// </summary>
    [SerializeField]
    [Range(1,100)]
    int verticalGroupLength = 4;
    public int VerticalGroupLength => verticalGroupLength;

    /// <summary>
    /// 전체적으로 장애물이 차지하고있는비율 백분율  
    /// </summary>
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float obstaclesProbability = 0.0f;

    /// <summary>
    /// 타일 그룹을 나눠서 해당그룹은 접근 가능한지 불가능한지 나눈다.
    /// </summary>
    TileGroupElement[] tileGroups;

    [SerializeField]
    /// <summary>
    /// 접근 불가능한 그룹 을 담은 배열
    /// </summary>
    int[] obstaclesIndexArray;
    public int[] ObstacleIndexArray => obstaclesIndexArray;

    private void Awake()
    {
        AstarProccess.onTileCreate += AddTileGroup; //타일생성 로직 연결
    }
    /// <summary>
    /// 
    /// </summary>
    public void GenerateTileMap() 
    {
        int arrayGroupSize = horizontalGroupLength * verticalGroupLength; //그룹 총갯수를 구한다.
        
        //접근 불가지역 인덱스 셋팅 시작

        int[] gridGroupArray = new int[arrayGroupSize]; //그룹 인덱스 갯수를 배열로 잡고
        
        for (int i = 0; i < arrayGroupSize; i++) 
        {
            gridGroupArray[i] = i; //그룹 인덱스에다가 인덱스를 0번부터 순차적으로 셋팅한다.     
        }

        Shuffle(gridGroupArray); //그룹 인덱스 의 순번을 랜덤으로 섞는다. 

        int obstaclesLength = (int)(arrayGroupSize * obstaclesProbability); //접근 불가능한 그룹 갯수를 구한다. float => int 라서 정확하지않음 

        obstaclesIndexArray = new int[obstaclesLength]; //접근 불가능한 지역 인덱스 배열을 생성한다.

        for (int i = 0; i < obstaclesLength; i++)
        {
            obstaclesIndexArray[i] = gridGroupArray[i]; //접근불가능한 지역을 따로  담아둔다.
        }

        Array.Sort(obstaclesIndexArray); //정렬을 한뒤에 

        // 랜덤으로 접근불가지역 셋팅 끝


        //시작 위치와 끝위치는 길이 무조건 이어져있어야 함으로 그것에대한 처리 시작


       /*
        출발 도착 지점 길이 연결되는것을 확인하기위해 
        openList 내용 을 다뒤진뒤 도착지점안나올시 
        H값 기준으로 제일 작은값의 목록을뽑고 
        그안에 서 랜덤으로 돌려서 
        가까운위치를 생성
        생성한 위치값을 오픈리스트에 다시 넣고 돌리고 무한반복 .
        */




        //접근 불가지역 인덱스 셋팅


        tileGroups = new TileGroupElement[arrayGroupSize];  //그룹관리할 클래스의 배열크기만큼 메모리 공간을 잡고 



        int gridGroupIndex  = 0; // 타일 그룹 인덱스 담아둘 변수 값

        int gridCheckingIndex = 0; // 생성불가지역 체크할 변수 
        
        Vector2Int gridPosX = Vector2Int.zero; //전체 셀정보에서 x좌표의 셀위치(시작 x,끝 y)위치를 담아둔다
        
        Vector2Int gridPosY = Vector2Int.zero; //전체 셀정보에서 y좌표의 셀위치(시작 x,끝 y)위치를 담아둔다

        Vector2Int gridPosZ = Vector2Int.zero; //전체 셀정보에서 해당그룹의 높이 정보(x 바닥, y 천장)를 담는다

        int groupCellSizeX = mapMaxHorizontal / horizontalGroupLength;    //그룹이 가지고있는 가로 셀 갯수를 구한다 

        int groupCellSizeY = mapMaxVertical / verticalGroupLength;  //그룹이 가지고있는 세로 셀 갯수를 구한다.

        int groupCellSize = (groupCellSizeX * groupCellSizeY); //한 그룹에 담길 셀갯수


        //int cellGenerateLength = (mapMaxHorizontal * mapMaxVertical)                           // 맵 전체크기에서 
        //                            -                                                   // 뺀다
        //                         (groupCellSize * obstaclesLength); // 접근불가능한 셀 (생성안되도될셀) 만큼


        mapTiles = new ITileBase[mapMaxHorizontal * mapMaxVertical]; //생성이 필요한 셀만큼 크기를 잡는다.

        int groupCellIndex = 0; //포문안에서 사용할 임시변수 (타일 인덱스 정할변수)

        int tempGroupCellEndIndexY = 0;  //포문안에서 사용할 임시변수
        int tempGroupCellEndIndexX = 0;  //포문안에서 사용할 임시변수
        int tempGroupCellIndexZ = 0;  //포문안에서 사용할 임시변수

        for (int group_y = 0; group_y < verticalGroupLength; group_y++) // 그룹의 세로 길이만큼 돌리고
        {
            for (int group_x = 0; group_x < horizontalGroupLength; group_x++) //그룹의 가로 길이 만큼 돌리고
            {

                tempGroupCellEndIndexY = ((group_y + 1) * groupCellSizeY); //Y좌표 끝나는지점 셋팅 (셀 셋팅할때 사용)
                tempGroupCellEndIndexX = ((group_x + 1) * groupCellSizeX); //X좌표 끝나는지점 셋팅 (셀 셋팅할때 사용)


                gridPosX.x =  group_x * groupCellSizeX;             //그룹 X좌표 의 셀의 시작위치 
                gridPosX.y =  tempGroupCellEndIndexX - 1;           //그룹 X좌표 의 셀의 끝나는 위치 
                gridPosY.x =  group_y * groupCellSizeY;             //그룹 Y좌표 의 셀의 시작위치
                gridPosY.y =  tempGroupCellEndIndexY- 1;            //그룹 X좌표 의 셀의 끝나는 위치 
                /*
                 높이설정도 여기서 해줘야하는데 일단 나중에 생각하자.
                 */

                if (obstaclesIndexArray.Length > 0 && gridCheckingIndex < obstaclesIndexArray.Length &&  //IndexOutOfRangeException 에러 방지용 
                    gridGroupIndex == obstaclesIndexArray[gridCheckingIndex]) //접근불가능한 그룹 갯수만큼 체크하고 
                {
                    //접근불가능한 지역에대한 설정


                    tileGroups[gridGroupIndex] = new TileGroupElement(
                                                    gridPosX,
                                                    gridPosY,
                                                    Vector2Int.one,
                                                    CurrentTileGroupState.InaccessibleArea
                                                    ); 
                    

                    gridCheckingIndex++; //접근불가지역 인덱스
                }
                else 
                {
                    //접근 가능한 지역에 대한 설정 


                    tileGroups[gridGroupIndex] = new TileGroupElement(
                                                    gridPosX,
                                                    gridPosY,
                                                    Vector2Int.one,
                                                    CurrentTileGroupState.Nomal
                                                    ); //랜덤한 인덱스값 위치에 그룹 데이터를 생성한다.



                    tempGroupCellIndexZ = 1; //높이값 셋팅용 변수   ******** 나중에 높이도 설정할때 사용 ********

                    /*
                     높이 설정하려면 여기서 작업 
                     */


                    //생성이 필요한 셀을 지정한다.
                    for (int cellY = tempGroupCellEndIndexY - groupCellSizeY; cellY < tempGroupCellEndIndexY; cellY++)
                    {
                        for (int cellX = tempGroupCellEndIndexX - groupCellSizeX ; cellX < tempGroupCellEndIndexX; cellX++)
                        {
                            //int groupCellIndex = (gridGroupIndex * groupCellSize) + tempGroupCellIndexCount; //셀의인덱스 
                            groupCellIndex = (horizontalGroupLength * cellY) + cellX; //셀의인덱스 

                            mapTiles[groupCellIndex] = (ITileBase)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.TILE_POOL);
                            //mapTiles[groupCellIndex] =  Instantiate(tilePrefab); //팩토리 연결 필요 

                            mapTiles[groupCellIndex].OnInitData(
                                    groupCellIndex,
                                    new Vector3Int(  cellX,  
                                                     cellY, 
                                                    tempGroupCellIndexZ), //위치값 셋팅 
                                    CurrentTileState.None       //기본값 이동가능하게 셋팅
                                    ); //위치값 및 상태 초기화 
                            mapTiles[groupCellIndex].OnClick += TileOnClick;
                        }
                    }

                }
                gridGroupIndex++; //그룹 인덱스값

            }
        }
    }

    public void AddTileGroup(Astar_Node addNode) 
    {
        int groupCellIndex = (horizontalGroupLength * addNode.Y) + addNode.X; //셀의인덱스 

        mapTiles[groupCellIndex] = (ITileBase)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.TILE_POOL);
        //mapTiles[groupCellIndex] =  Instantiate(tilePrefab); //팩토리 연결 필요 

        mapTiles[groupCellIndex].OnInitData(
                groupCellIndex,
                new Vector3Int(addNode.X,
                                 addNode.Y,
                                1), //위치값 셋팅 
                CurrentTileState.None       //기본값 이동가능하게 셋팅
                ); //위치값 및 상태 초기화 
        mapTiles[groupCellIndex].OnClick += TileOnClick;
    }

    /// <summary>
    /// 배열을 섞는 함수 
    /// 장점 : 원래 자신의 위치에 존재할수없기때문에 확율상 균등한 확율이 가지게된다.
    /// 단점 : 원래 자신의 위치에서 무조건이동이 되는 함수라 0번인덱스값이 0번위치에 있을수가없다.
    /// </summary>
    /// <param name="source">섞을 배열</param>
    private void Shuffle(int[] source)
    {
        // source의 순서 섞기(피셔-예이츠 알고리즘 사용)
        int loopCount = source.Length - 1; 
        for (int i = 0; i < loopCount; i++) 
        {
            int randomIndex = UnityEngine.Random.Range(0, source.Length - i);   // 전체 개수에서 계속 1이 감소하는 범위
            int lastIndex = loopCount - i;  // 마지막에서 계속 1씩 감소하는 숫자

            (source[lastIndex], source[randomIndex]) = (source[randomIndex], source[lastIndex]);    // 스왑하기
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