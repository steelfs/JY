using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이동 가능한 범위 표시해줄 컴포넌트
/// </summary>
public class MoveRange : MonoBehaviour
{
    /// <summary>
    /// 청소 끝나고나서 다시검색할수있게 체크하는변수
    /// </summary>
    bool isClear = false;
   
    /// <summary>
    /// 이동 가능한 타일 담아둘 리스트
    /// </summary>
    List<Tile> activeMoveTiles;

    private void Awake()
    {
        SpaceSurvival_GameManager.Instance.GetMoveRangeComp = () => this; //데이터 연결하기 
        activeMoveTiles = new List<Tile>(); //이동가능한 타일 리스트 초기화 
    }

    /// <summary>
    /// 바닥에 이동가능한 범위를 표시하는 로직 
    /// </summary>
    /// <param name="mapTiles">맵타일 정보 </param>
    /// <param name="currentNode">현재위치 타일 정보</param>
    /// <param name="moveSize">이동가능한 거리 값</param>
    /// <param name="tileSizeX">맵타일의 최대 가로갯수</param>
    /// <param name="tileSizeY">맵타일의 최대 세로갯수</param>
    public void MoveSizeView(Tile currentNode, float moveSize)
    {
        if (!isClear)
        {
            SetMoveSize(currentNode, moveSize); //이동 가능 리스트 설정하기
            OpenLineRenderer(currentNode);
        }
    }

    /// <summary>
    /// 갈수있는지역 초기화 하기 내위치까지 초기화되기때문에 내위치는 남겨둔다.
    /// </summary>
    public void ClearLineRenderer(Tile currentTile) //기존 라인렌더러 끄기
    {
        if (!isClear) 
        {
            isClear = true;
            if (activeMoveTiles.Count > 0) //초기화 할 타일이있을때만  
            {
                Tile.TileExistType currentTileType = currentTile.ExistType; //포문에서 매번 체크하지않기위해 따로담고 
                foreach (Tile tile in activeMoveTiles)
                {

                    if (SpaceSurvival_GameManager.Instance.ItemTileList.Contains(tile))    //이동범위 초기화할때 아이템 정보에대한값이 존재할때 
                    {
                        tile.ExistType = Tile.TileExistType.Item; //아이템으로 초기화 
                    }
                    else 
                    {
                        tile.ExistType = Tile.TileExistType.None;
                    }
                }
                currentTile.ExistType = currentTileType; //수정끝났으면 저장해뒀던 값을 담는다.
                activeMoveTiles.Clear();//초기화끝낫으면 내용 비우기
            }
            isClear = false;
        }
    }

    /// <summary>
    /// 갈수있는지역 표시하기 
    /// </summary>
    private void OpenLineRenderer(Tile currentTile) //이동가능한범위 의 라인렌더러 키기
    {
        Tile.TileExistType currentTileType = currentTile.ExistType; //포문에서 매번 체크하지않기위해 따로담고 
        foreach (Tile tile in activeMoveTiles)
        {
            tile.ExistType = Tile.TileExistType.Move;
        }
        currentTile.ExistType = currentTileType; //수정끝났으면 저장해뒀던 값을 담는다.
    }

    /// <summary>
    /// 현재 위치지점에서 행동력 기준 이동가능한 범위 의 좌표리스트를 가져오기위한 함수
    /// </summary>
    /// <param name="mapTiles">맵타일 정보 </param>
    /// <param name="currentNode">현재위치 타일 정보</param>
    /// <param name="moveCheck">이동가능한 거리 값</param>
    /// <param name="tileSizeX">맵타일의 최대 가로갯수</param>
    /// <param name="tileSizeY">맵타일의 최대 세로갯수</param>
    /// <returns>캐릭터가 이동가능한 노드리스트</returns>
    private void SetMoveSize(Tile currentNode, float moveCheck)
    {
        List<Tile> openList = new List<Tile>();   // 탐색이 필요한 노드 리스트 
        List<Tile> closeList = new List<Tile>();  // 이미 계산이 완료되서 더이상 탐색을 안할 리스트 
        Tile[] mapTiles = SpaceSurvival_GameManager.Instance.BattleMap;
        int tileSizeX = SpaceSurvival_GameManager.Instance.MapSizeX;
        int tileSizeY = SpaceSurvival_GameManager.Instance.MapSizeY;
        foreach (Tile node in mapTiles)
        {
            node.H = 1000.0f; //도착지점이 없는상태라서 맥스값 넣으니 제대로 안돌아간다.
            node.MoveCheckG = 1000.0f;
        }

        openList.Add(currentNode);

        currentNode.MoveCheckG = 0.0f; //내위치는 g 가 0이다

        while (openList.Count > 0)
        {
            currentNode = openList[0];
            openList.Remove(currentNode); // 탐색가능한 목록에서 현재 탐색중인 목록을 제거하고 
            closeList.Add(currentNode);   // 탐색종료한 리스트에 현재 목록을 담는다.

            if (currentNode.MoveCheckG > moveCheck) //G 값이 현재 이동 가능한 거리보다 높으면  더이상 탐색이 필요없음으로 
            {
                continue; //다음거 탐색 
            }
            else // 이동가능한 거리면 
            {
                activeMoveTiles.Add(currentNode); //반환 시킬 리스트로 추가한다.
            }

            OpenListAdd(mapTiles , tileSizeX, tileSizeY, currentNode, openList, closeList ); //주변 8방향의 노드를 찾아서 G값 수정하고  오픈리스트에 담을수있으면 담는다.
            openList.Sort();            //찾은 G값중 가장 작은값부터 재탐색이된다.
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
                if (currentNode.Width + x < 0 || currentNode.Width + x > tileSizeX - 1 || // 사이드 검색 
                    currentNode.Length + y < 0 || currentNode.Length + y > tileSizeY - 1) //사이드 검색
                    continue;

                adjoinTile = Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width + x,currentNode.Length + y,tileSizeX);    // 인접한 타일 가져오기

                if (adjoinTile == currentNode)                                          // 인접한 타일이 (0, 0)인 경우
                    continue;
                if (adjoinTile.ExistType != Tile.TileExistType.None && adjoinTile.ExistType != Tile.TileExistType.Item) // 인접한 타일이 이동가능한곳일때 이 아닐 때
                    continue;

                bool isDiagonal = (x * y != 0);                                     // 대각선 유무 확인
                if (isDiagonal &&                                                   // 대각선이고 현재 타일의 상하좌우가 벽일 때
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
    /// 랜덤으로 셋팅 가능한(none인값) 을 찾아서 반환하는 함수  못찾으면 null 반환
    /// </summary>
    /// <param name="tileType">타일위에 올라갈 타입</param>
    /// <returns>추가가능한 타일을반환 하거나 못잡으면 null 반환</returns>
    public Tile GetRandomTile(Tile.TileExistType tileType) 
    {

        int x = SpaceSurvival_GameManager.Instance.MapSizeX;
        int y = SpaceSurvival_GameManager.Instance.MapSizeY;
        Tile[] mapTiles = SpaceSurvival_GameManager.Instance.BattleMap;
        Tile result = null;
        int maxCount = 100; //최대 100번만돈다.
        int count = 0;
        //Debug.Log($"{x},{y}");
        while (count < maxCount) //무한 루프 방지용 
        {
            result = Cho_BattleMap_AStar.GetTile(mapTiles, Random.Range(0, x), Random.Range(0, y), x);
            if (result.ExistType == Tile.TileExistType.None)//갈수있는곳이면 
            {
                result.ExistType = tileType; //설정되야될 타입으로 바꾼뒤 
                return result;//빠져나간다.
            }
            else 
            {
                result = null; //못가는 지역이면 초기화 시킨다.   
            }

            count++; //무한루프 방지용
        }
        Debug.Log($"{count} 만큼 돌았는데 다꽉차있는지 못찾았어 ");
        return null;
    }

}
