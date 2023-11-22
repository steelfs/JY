using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapTest : MonoBehaviour
{
    public ChoClickTest player;             // 플레이어
    public Material material;               // 필요없을 것 같아 아마 지울 예정

    public GameObject centerTile;           // 중앙에 사용할 타일
    public GameObject sideTile;             // 외곽에 배치될 타일
    public GameObject vertexTile;           // 꼭지점 타일
    public GameObject wall;                 // 기본 벽
    public GameObject pillar;               // 기둥

    public int sizeX = 0;                   // 타일 가로 갯수
    public int sizeY = 0;                   // 타일 세로 갯수
    public int tileCount = 0;               // 타일의 수

    public List<GameObject> singleProps;    // 1칸만 차지하는 물체
    public List<GameObject> multiProps;     // 2칸 이상의 타일을 차지하는 물체

    bool isExist = false;                   // 타일 존재 여부
    bool isPropExist = false;

    Vector3 mainTileSize = Vector3.zero;    // 중앙 타일 사이즈

    Tile[] mapTiles;                        // 타일 오브젝트 객체를 담을 배열
    List<GameObject> props;                 // 지형 지물을 담을 배열

    GameObject[] pillars;                   // 기둥
    Tile[] standardPos;                // 기준 위치(조명과 기둥이 있을 위치)

    private void Start()
    {
        // 중앙 타일 사이즈 반환     - 바닥에 깔리는 모든 타일들은 사이즈 동일
        mainTileSize = centerTile.GetComponentInChildren<BoxCollider>().size;
    }

    /// <summary>
    /// 타일 랜덤 생성
    /// </summary>
    public void Test1()
    {
        if (!isExist)                   // 타일이 존재하지 않을 경우에만 생성
        {
            sizeX = Random.Range(20, 31);       // 타일 가로 갯수 랜덤 생성
            sizeY = Random.Range(20, 31);       // 타일 세로 갯수 랜덤 생성
            tileCount = sizeX * sizeY;          // 총 타일 갯수

            MapInstantiate();                       // 메인 맵 생성


            PillarInstantiate();                     // 조명 및 기둥 생성
            //MiniMapInstantiate();                 // 미니맵 판자 생성(필요없을 것 같아 나중에 지울 예정)

            RandomPlayerMove();

            isExist = true;         // 중복 맵 생성 방지

        }
    }

    /// <summary>
    /// 타일 제거
    /// </summary>
    public void Test2()
    {
        if (isExist && !isPropExist)
        {
            MapDestroy();

            isExist = false;
        }
    }

    public void Test3()
    {
        if (isExist)
        {
            PropInstantiate();
            RandomPlayerMove();
            isPropExist = true;
        }
    }

    public void Test4()
    {
        if (isPropExist)
        {
            PropDestroy();
            isPropExist = false;
        }
    }

    /// <summary>
    /// 메인 맵 생성하는 함수
    /// </summary>
    private void MapInstantiate()
    {

        mapTiles = new Tile[tileCount];   // 배열 동적 생성
        GameObject wallObject;          // 벽 오브젝트

        for (int i = 0; i < tileCount; i++)
        {
            int width = i % sizeX;              // 가로 인덱스 번호
            int length = i / sizeX;             // 세로 인덱스 번호

            // 타일 생성
            if ((width == 0 && length == 0) || (width == 0 && length == sizeY - 1) || (width == sizeX - 1 && length == 0) || (width == sizeX - 1 && length == sizeY - 1))
            {
                // 꼭지점인 경우
                TileInstantiate(i, vertexTile, Tile.MapTileType.vertexTile, width, length);      // 꼭지점 타일 생성
                wallObject = Instantiate(wall, mapTiles[i].transform);                      // 측면 벽1 생성
                wallObject.transform.Translate(new Vector3(1.0f, 0.0f, -1.75f));            // 측면 벽1 이동
                wallObject = Instantiate(wall, mapTiles[i].transform);                      // 측면 벽2 생성
                wallObject.transform.Rotate(new Vector3(0, -90.0f, 0));                     // 측면 벽2 회전
                wallObject.transform.Translate(new Vector3(1.0f, 0.0f, -1.75f));            // 측면 벽2 이동
                wallObject = Instantiate(wall, mapTiles[i].transform);                      // 꼭지점 벽 생성
                wallObject.transform.Rotate(new Vector3(0, -45.0f, 0));                     // 꼭지점 벽 회전
                wallObject.transform.Translate(new Vector3(1.0f, 0.0f, -2.0f));             // 꼭지점 벽 이동


                if (width == 0 && length == 0)                                      // 왼쪽 위
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 90.0f, 0));
                }
                else if (width == 0 && length == sizeY - 1)                         // 왼쪽 아래
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 180.0f, 0));
                }
                else if (width == sizeX - 1 && length == 0)                         // 오른쪽 위
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 0.0f, 0));
                }
                else if (width == sizeX - 1 && length == sizeY - 1)                 // 오른쪽 아래
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 270.0f, 0));
                }
            }
            else if (width == 0 || width == sizeX - 1 || length == 0 || length == sizeY - 1)
            {
                // 가장자리일 경우
                TileInstantiate(i, sideTile, Tile.MapTileType.sideTile, width, length);             // 사이드 타일 생성
                wallObject = Instantiate(wall, mapTiles[i].transform);                              // 측면 벽 생성
                wallObject.transform.Translate(new Vector3(1, 0.0f, -1.75f));                       // 측면 벽 이동

                if (width == 0)                                                                     // 왼쪽 세로줄
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 90.0f, 0));
                }
                else if (width == sizeX - 1)                                                        // 오른쪽 세로줄
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 270.0f, 0));
                }
                else if (length == 0)                                                               // 맨 윗줄
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 0.0f, 0));
                }
                else if (length == sizeY - 1)                                                       // 맨 아랫줄
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 180.0f, 0));
                }
            }
            else
            {
                // 가장자리가 아닌 경우
                TileInstantiate(i, centerTile, Tile.MapTileType.centerTile, width, length);              //중앙 타일 생성
                mapTiles[i].transform.Rotate(new Vector3(0, 90.0f * Random.Range(0, 4), 0));        // 중앙 타일 랜덤 회전(그냥 미관상)
            }

            mapTiles[i].transform.position = new Vector3(mainTileSize.x * width, 0, mainTileSize.z * length);
        }
    }

    /// <summary>
    /// 맵 제거하는 함수
    /// </summary>
    private void MapDestroy()
    {
        for (int i = 0; i < tileCount; i++)
        {
            Destroy(mapTiles[i].gameObject);
        }

        for (int i = 0; i < 4; i++)
        {
            Destroy(pillars[i]);
        }
    }

    /// <summary>
    /// 이차원 좌표를 타일로 반환하는 함수
    /// </summary>
    /// <param name="width">가로 인덱스</param>
    /// <param name="length">세로 인덱스</param>
    /// <returns></returns>
    public Tile GetTile(int width, int length)
    {
        int index = sizeX * length + width;
        return mapTiles[index];
    }

    public Tile GetTile(Vector2Int pos)
    {
        int index = sizeX * pos.y + pos.x;
        return mapTiles[index];
    }

    /// <summary>
    /// 타입에 따른 타일 생성
    /// </summary>
    /// <param name="i">맵타일 인덱스</param>
    /// <param name="type">생성할 타일의 타입</param>
    /// <param name="tileType">타일 스크립트에 저장할 타입</param>
    /// <param name="width">타일의 가로 인덱스</param>
    /// <param name="length">타일의 세로 인덱스</param>
    private void TileInstantiate(int i, GameObject type, Tile.MapTileType tileType, int width, int length)
    {
        mapTiles[i] = Instantiate(type, gameObject.transform).GetComponent<Tile>();      // type에 따른 타일 생성
        mapTiles[i].GetComponent<Tile>().TileType = tileType;                            // 타일 스크립트에 타입 저장
        mapTiles[i].GetComponent<Tile>().Width = width;                                  // 타일 가로 인덱스 저정
        mapTiles[i].GetComponent<Tile>().Length = length;                                // 타일 세로 인덱스 저정
        mapTiles[i].GetComponent<Tile>().Index = i;
    }

    /// <summary>
    /// 조명과 기둥 생성 및 이동
    /// </summary>
    private void PillarInstantiate()
    {
        standardPos = new Tile[4];         // 기준 위치 생성
        pillars = new GameObject[4];            // 기둥 동적 생성

        standardPos[0] = GetTile(sizeX / 3 - 1, sizeY / 3 - 1).GetComponent<Tile>();
        standardPos[1] = GetTile(sizeX - sizeX / 3 + 1, sizeY / 3 - 1).GetComponent<Tile>();
        standardPos[2] = GetTile(sizeX / 3 - 1, sizeY - sizeY / 3 + 1).GetComponent<Tile>();
        standardPos[3] = GetTile(sizeX - sizeX / 3 + 1, sizeY - sizeY / 3 + 1).GetComponent<Tile>();

        for (int i = 0; i < 4; i++)
        {
            standardPos[i].GetComponent<Tile>().ExistType = Tile.TileExistType.Prop;                                 // 기둥이 있는 타일의 타입 지정

            pillars[i] = Instantiate(pillar, gameObject.transform);                                               // 기둥 생성
            pillars[i].transform.position = standardPos[i].transform.position;                                    // 기둥 이동

        }
    }

    /// <summary>
    /// 미니맵 판자 생성(지울 예정)
    /// </summary>
    private void MiniMapInstantiate()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // 미니맵 위치를 위해 0번 인덱스로 초기화
        Vector3 miniMapPos = mapTiles[0].transform.position;

        if (sizeX % 2 == 0)         // 가로축이 짝수인 경우
        {
            miniMapPos.x = (mapTiles[(sizeX / 2) - 1].transform.position.x + mapTiles[sizeX / 2].transform.position.x) * 0.5f;
        }
        else                        // 가로축이 홀수인 경우
        {
            miniMapPos.x = mapTiles[sizeX / 2].transform.position.x;
        }

        if (sizeY % 2 == 0)         // 세로축이 짝수인 경우
        {
            miniMapPos.z = (mapTiles[((sizeY / 2) - 1) * sizeX].transform.position.z + mapTiles[(sizeY / 2) * sizeX].transform.position.z) * 0.5f;
        }
        else                        // 세로축이 홀수인 경우
        {
            miniMapPos.z = mapTiles[(sizeY / 2) * sizeX].transform.position.z;
        }

        miniMapPos.y += (mainTileSize.y * 0.5f) + 40.0f;
        cube.transform.position = miniMapPos;
        cube.transform.localScale = new Vector3(mainTileSize.x * sizeX, mainTileSize.y, mainTileSize.z * sizeY);
        cube.GetComponent<MeshRenderer>().material = material;
    }

    /// <summary>
    /// 구조물 생성 함수
    /// </summary>
    private void PropInstantiate()
    {
        if (props == null)
        {
            props = new List<GameObject>(16);               // 구조물이 비어있으면 생성. 16은 임의로 넣은 숫자.
        }
        else
        {
            return;                         // 구조물이 생성돼 있으면 더 이상 생성하지 않음
        }

        int chooseProp;     // 구조물 종류 중 랜덤 생성

        // 기둥을 기준으로 구역을 나누기 위해 생성한 두 개의 임시 배열(각각 가로와 세로)
        int[] tempArrayX = new int[4] { 0, standardPos[0].Width, standardPos[1].Width, sizeX };
        int[] tempArrayY = new int[4] { 0, standardPos[0].Length, standardPos[2].Length, sizeY };

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // 가로 혹은 세로가 2 이상의 길이를 가진 구조물 생성
                chooseProp = Random.Range(0, multiProps.Count);
                PropMultiMaking(chooseProp, tempArrayX[i], tempArrayX[i + 1], tempArrayY[j], tempArrayY[j + 1]);    
                if (i == 1 && j == 1)
                {
                    // 조금 더 빽빽하게 해주기 위해 중앙에 하나 더 생성
                    PropMultiMaking(chooseProp, tempArrayX[i], tempArrayX[i + 1], tempArrayY[j], tempArrayY[j + 1]);    
                }
            }
        }

        for (int i = 0; i < Random.Range(1, singleProps.Count + 1); i++)
        {
            chooseProp = Random.Range(0, singleProps.Count);
            PropSingleMaking(chooseProp);   // 가로와 세로가 각각 1인 구조물 생성
        }

    }

    /// <summary>
    /// 가로와 세로가 각각 1인 구조물을 생성하는 함수
    /// </summary>
    /// <param name="chooseProp">구조물 종류 인덱스</param>
    private void PropSingleMaking(int chooseProp)
    {
        GameObject obj = Instantiate(singleProps[chooseProp]);      // 구조물 생성

        while (true)
        {
            Tile tile = GetTile(Random.Range(0, sizeX), Random.Range(0, sizeY));    // 임의의 타일 지정
            if (tile.ExistType != Tile.TileExistType.None)          // 임의의 타일이 비어있는 게 아닐 경우
            {
                continue;                                           // 다시 뽑기(될 때까지 무한 반복). 만약 됐으면 아래로 내려감
            }
            obj.transform.position = tile.transform.position;       // 구조물을 타일의 위치로 이동
            obj.transform.GetChild(0).rotation = Quaternion.Euler(0.0f, 90.0f * Random.Range(0, 4), 0.0f);  // 구조물 회전시켜 주기
            tile.ExistType = Tile.TileExistType.Prop;               // 구조물이 있는 타일 구조물이 있다고 표시
            break;                  // 무한 루프 탈출
        }
        
        props.Add(obj);             // 구조물 배열에 추가

    }

    /// <summary>
    /// 가로 혹은 세로의 인덱스가 2 이상인 구조물을 생성하는 함수
    /// </summary>
    /// <param name="chooseProp">구조물 종류 인덱스</param>
    /// <param name="index1">가로 인덱스의 최소 범위</param>
    /// <param name="index2">가로 인덱스의 최대 범위</param>
    /// <param name="index3">세로 인덱스의 최소 범위</param>
    /// <param name="index4">세로 인덱스의 최대 범위</param>
    private void PropMultiMaking(int chooseProp, int index1, int index2, int index3, int index4)
    {
        GameObject obj = Instantiate(multiProps[chooseProp]);           // 구조물 생성
        PropData objData = obj.GetComponent<PropData>();                // 구조물의 데이터 반환
        Tile[] tempTile = new Tile[objData.width * objData.length];     // 타일 체크 시 담아놓을 임시 배열(구조물의 가로와 세로 길이의 곱과 같다)
        bool isSuccess = false;                                         // 구조물 이동이 가능한지 여부

        // 생성 가능한 위치에 구조물 이동 및 배열 추가
        while (!isSuccess)
        {
            Tile tile = GetTile(Random.Range(index1, index2), Random.Range(index3, index4));        // 구조물을 놓을 랜덤 위치의 타일을 가져오고
            int randomRotation = Random.Range(0, 4);                                                // 생성 시 배치될 랜덤 회전 (0 ~ 3). y축 기준.
            for (int count = 0; count < 4; count++)                         // (0, 90, 180, 270)도의 회전을 해야하기에 4번 돌린다.
            {
                randomRotation++;                                           // 나중에 회전 각을 맞추기 위해 앞으로 당겨 놓음(중요하지 않음)
                randomRotation %= 4;                                        // 랜덤로테이션 값이 계속 0~3 이 되도록 지정
                int tileCount = 0;                                          // 체크한 타일의 개수를 세기 위한 값
                for (int i = 0; i < objData.width; i++)         // 구조물의 가로 길이 만큼 반복 돌리기
                {
                    for (int j = 0; j < objData.length; j++)    // 구조물의 세로 길이 만큼 반복 돌리기
                    {
                        switch (randomRotation)         // 회전 정도에 따라 체크해야할 타일의 인덱스가 달라지기 때문에 각자 맞춰 계산하도록 돌림
                        {
                            case 0:         // 회전이 0도일 때
                                if (GetTile(tile.Width + i, tile.Length + j).ExistType == Tile.TileExistType.Prop ||    // 타일에 구조물이 놓여있거나
                                    GetTile(tile.Width + i, tile.Length + j).TileType == Tile.MapTileType.sideTile ||   // 타일이 사이드 타일이거나
                                    GetTile(tile.Width + i, tile.Length + j).TileType == Tile.MapTileType.vertexTile)   // 꼭지점 타일인 경우
                                {
                                    i = objData.width;          // 가로축 for문 탈출을 위한 최대값 지정
                                    j = objData.length;         // 세로축 for문 탈을을 위한 최대값 지정
                                    break;                      // switch문 탈출
                                }
                                tempTile[i * objData.length + j] = GetTile(tile.Width + i, tile.Length + j);    // 위의 경우에 해당하지 않으면 임시 배열에 타일 저장
                                tileCount++;                                                           // 몇 개의 타일을 체크했는지 확인하기 위해 타일 카운트 증가
                                break;                                                                 // switch문 탈출
                            case 1:         // 회전이 90도 일 때
                                if (GetTile(tile.Width + j, tile.Length - i).ExistType == Tile.TileExistType.Prop ||        // 위와 동일
                                    GetTile(tile.Width + j, tile.Length - i).TileType == Tile.MapTileType.sideTile ||
                                    GetTile(tile.Width + j, tile.Length - i).TileType == Tile.MapTileType.vertexTile)
                                {
                                    i = objData.width;
                                    j = objData.length;
                                    break;
                                }
                                tempTile[i * objData.length + j] = GetTile(tile.Width + j, tile.Length - i);
                                tileCount++;
                                break;
                            case 2:         // 회전이 180도 일 때
                                if (GetTile(tile.Width - i, tile.Length - j).ExistType == Tile.TileExistType.Prop ||        // 위와 동일
                                    GetTile(tile.Width - i, tile.Length - j).TileType == Tile.MapTileType.sideTile ||
                                    GetTile(tile.Width - i, tile.Length - j).TileType == Tile.MapTileType.vertexTile)
                                {
                                    i = objData.width;
                                    j = objData.length;
                                    break;
                                }
                                tempTile[i * objData.length + j] = GetTile(tile.Width - i, tile.Length - j);
                                tileCount++;
                                break;
                            case 3:         // 회전이 270도 일 때
                                if (GetTile(tile.Width - j, tile.Length + i).ExistType == Tile.TileExistType.Prop ||        // 위와 동일
                                    GetTile(tile.Width - j, tile.Length + i).TileType == Tile.MapTileType.sideTile ||
                                    GetTile(tile.Width - j, tile.Length + i).TileType == Tile.MapTileType.vertexTile)
                                {
                                    i = objData.width;
                                    j = objData.length;
                                    break;
                                }
                                tempTile[i * objData.length + j] = GetTile(tile.Width - j, tile.Length + i);
                                tileCount++;
                                break;
                            default:
                                break;
                        }       // switch문 끝
                    }       // 세로 for문 (j) 끝
                }       // 가로 for문 (i) 끝

                if (tileCount == objData.width * objData.length)    // 만약 체크한 타일의 개수와 체크해야 할 타일의 개수가 같다면
                {
                    isSuccess = true;           // 이동이 가능한 것으로 변경
                    count = 4;                  // 회전을 위해 돌리는 for문을 탈출하기 위해 최대치인 4로 설정
                }
            }           // 회전을 위해 사용하는 for문 (count) 끝

            if (isSuccess)      // 만약 이동이 가능하다면
            {
                obj.transform.position = tile.transform.position;                                   // 그 타일의 위치로 구조물을 이동시켜 주고
                obj.transform.rotation = Quaternion.Euler(0.0f, 90.0f * randomRotation, 0.0f);      // 구조물을 회전시킨다.
                props.Add(obj);                                                                     // 구조물 배열에 추가
                break;                  // while 반복문 탈출
            }
        }       // while 반복문 끝

        for (int i = 0; i < tempTile.Length; i++)           // 필요한 타일을 담아놓은 배열을 순환시키며
        {
            tempTile[i].ExistType = Tile.TileExistType.Prop;        // 그 타일은 구조물이 있음을 표시
        }
    }

    /// <summary>
    /// 구조물을 제거하는 함수
    /// </summary>
    private void PropDestroy()
    {
        foreach (var obj in props)
        {
            Destroy(obj);           // 구조물 배열 순회하며 제거
        }
        props.Clear();
        props = null;               // 비우고 null로 초기화

        if (isExist)                // 맵이 있을 때만 가능
        {
            for (int i = 0; i < mapTiles.Length; i++)
            {
                mapTiles[i].GetComponent<Tile>().ExistType = Tile.TileExistType.None;   // 타일의 타입을 None으로 초기화
            }
        }

        for (int i = 0; i < standardPos.Length; i++)
        {
            standardPos[i].ExistType = Tile.TileExistType.Prop;     // 기둥이 있는 타일은 다시 Prop으로 변경
        }
    }

    /// <summary>
    /// A*에 대한 부분 초기화
    /// </summary>
    public void ClearTile()
    {
        for (int i = 0; i < mapTiles.Length; i++)
        {
            mapTiles[i].Clear();
        }
    }

    public bool IsWall(int x, int y)
    {
        return GetTile(x, y).ExistType != Tile.TileExistType.None;
    }

    
    private void RandomPlayerMove()
    {
        int count = 0;
        while (true)
        {
            if (GetTile(sizeX / 2, sizeY / 3 + count).ExistType == Tile.TileExistType.None)
            {
                player.CurrentPos = GetTile(sizeX / 2, sizeY / 3 + count);     // 플레이어 위치 이동(임시)
                break;
            }
            else
            {
                count++;
            }
        }
    }

}
