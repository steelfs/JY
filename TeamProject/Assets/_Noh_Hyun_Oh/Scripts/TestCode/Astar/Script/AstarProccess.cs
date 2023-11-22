using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/*
    초기값 셋팅하는 함수 리스트 크기 미리정해두기 예측해서 
 */
/// <summary>
/// A* 맵 하나에 사용될 매니저 클래스 
/// 기능 추가:  워프가있으면 해당좌표끼리 휴리스틱 값을 조정하는 기능을 추가해야한다.
/// </summary>
public static class AstarProccess 
{ 
    /// <summary>
    /// 현재 지역의 상태값
    /// </summary>
    public enum NodeState 
    {
        None = 0,           //초기값
        Nomal   ,           //접근가능   
        Inaccessible,       //접근불가능
    }

    /// <summary>
    /// 타일 맵의 주변이 이동가능한지역인지 체크하기위한 이넘값
    /// </summary>
    [Flags]
    public enum Four_Way_Access_Area_Check :byte
    {
        NONE = 0,                           // 0000     주변4방향에 내자신과 다른값이 존재하지않는다
        UP = 1,                             // 0001     
        DOWN = 2,                           // 0010     
        LEFT = 4,                           // 0100     
        RIGHT = 8,                          // 1000     
        ALL = UP | DOWN | LEFT | RIGHT ,    // 1111     주변4방향에 내자신과 다른값이 존재한다 

    }


    /// <summary>
    /// 시작 될 노드 정보
    /// </summary>
    private static Astar_Node startNode;
    
    /// <summary>
    /// 도착 할 노드 정보
    /// </summary>
    private static Astar_Node endNode;

    /// <summary>
    /// 현재 검색중인 노드정보 
    /// </summary>
    private static Astar_Node currentNode;

    /// <summary>
    /// A* 노드리스트
    /// </summary>
    private static Astar_Node[,] nodes;
    /// <summary>
    /// 노드 오픈 리스트 
    /// </summary>
    private static List<Astar_Node> openList = new List<Astar_Node>();

    /// <summary>
    /// 노드 클로즈 리스트
    /// </summary>
    private static List<Astar_Node> closeList = new List<Astar_Node>();

    /// <summary>
    /// 정사각형의 직선 값 
    /// </summary>
    const float nomalLine = 1.0f;

    /// <summary>
    /// 정사각형의 대각선 값 1.414 배 
    /// 정사각형이라 배율이 변하지않는다.
    /// </summary>
    //readonly float diagonalLine = Mathf.Sqrt((nomalLine*nomalLine) + (nomalLine * nomalLine)); //대각선 값구하기 1.414 입력해도 되긴하다.
    readonly static float diagonalLine = 1.414f;

    public static Action<Astar_Node> onTileCreate;

    /// <summary>
    /// A* 적용할 데이터 배열을 얻어온다.
    /// </summary>
    /// <param name="horizontalSize">가로 갯수</param>
    /// <param name="verticalSize"> 세로 갯수</param>
    /// <param name="obstaclesArray">못가는지역 인덱스 배열</param>
    /// <returns>못가는지역까지 설정끝난 노드이차배열 반환</returns>
    public static Astar_Node[,] InitData(int horizontalSize, int verticalSize, int[] obstaclesArray = null)
    {
        openList.Clear(); //오픈 리스트 초기화 

        closeList.Clear(); // 클로즈 리스트 초기화 

        nodes = CreateNodeArray(horizontalSize, verticalSize); // 맵 배열만들고 

        if (nodes != null && obstaclesArray != null) //못가는 지역이있으면
        {
            SetPlaceObstacles(horizontalSize, verticalSize, obstaclesArray); //못가는지역 등록한다.
        }
        return nodes;
    }


    /// <summary>
    /// 접근 불가능한 지역 설정 
    /// obstaclesArray 값이 좌표값 기준으로 오름차순 정렬이 된상태여야 정상작동이된다.
    /// 기능추가 : 노드에다가 현재 상태(접근가능,불가능)와 다른 값이 존재하면 체크하는 로직 추가
    /// </summary>
    /// <param name="horizontalSize">가로 크기</param>
    /// <param name="verticalSize">세로 크기</param>
    /// <param name="obstaclesArray">접근불가능한 지역 인덱스 저장해둔 배열</param>
    private static void SetPlaceObstacles(int horizontalSize, int verticalSize, int[] obstaclesArray = null)
    {
        int index = 0; // 인덱스 값 체크할 변수 
        int obstacleIndex = 0; //장애물 인덱스값 체크할 변수
        
        for (int y = 0; y < verticalSize; y++) // 전체의 가로갯수만큼 돌고
        {
            for (int x = 0; x < horizontalSize; x++) // 전체의 세로갯수만큼 돌아서 다돌린다.
            {
                if (obstaclesArray.Length > obstacleIndex && //아웃오브바운즈 에러 걸러내기용
                    obstaclesArray[obstacleIndex] == index) //인덱스값을 체크해서 
                {
                    nodes[y, x].State = NodeState.Inaccessible; //같은 인덱스값에 넣어준다.
                    obstacleIndex++;//다음 장애물 인덱스를 찾기위해 인덱스 증가
                }
                else 
                {
                    nodes[y, x].State = NodeState.Nomal; //같은 인덱스값에 넣어준다.
                }
                index++;//전체 인덱스 값 증가 
            }
        }
        // 포문에서 사용될 변수들 미리선언
        int startIndex = 0;
        int endIndex = 0;
        foreach (Astar_Node node in nodes) //접근 불가지역 노드에 근처에 갈수있는 지역있는지 체크하기
        {
            //십자 체크 시작

            //위아래 체크
            startIndex = node.Y - 1 < 0 ? 1 : - 1;  //맨 아래쪽 노드 일땐 위쪽 노드만 검색하면 되고 
            endIndex = node.Y + 2 > verticalSize    ? 0 : 2; //맨 위쪽 노드일땐 아래쪽 노드만 검색하면 된다.
            for (int y  = startIndex; y < endIndex; y += 2) // -1과 1로 반씩나눠서 체크한다.
            {
                //Debug.Log($" Y = {node.Y} , {y},{startIndex},{endIndex}");
                if (node.State != nodes[node.Y + y, node.X].State)//위아래만 체크
                {
                    // 어느방향인지 체크는 미리할수가없음으로 포문안에서 한다.
                    if (y > 0) //위쪽이냐
                    {
                        node.FourWayCheck |= Four_Way_Access_Area_Check.UP;
                        //내노드의 위에있는지 체크
                    }
                    else if (y < 0) //아래쪽이냐
                    {
                        node.FourWayCheck |= Four_Way_Access_Area_Check.DOWN;
                        //아니면 내노드의 아래에 있는지 체크
                    }
                }
            }

            //좌우 체크
            startIndex = node.X - 1 < 0 ? 1 : -1;  //맨 왼쪽노드 일땐 오른쪽 노드만 검색하면 되고 
            endIndex = node.X + 2 > horizontalSize  ? 0 : 2; //맨 오른쪽 노드 일땐 왼쪽 노드만 검색하면 된다.
            for (int x = startIndex; x < endIndex; x += 2) // -1 과 1 로 반씩 나눠 체크한다.
            {
                //Debug.Log($"x = {node.X} , {x},{startIndex},{endIndex}");
                if (node.State != nodes[node.Y, node.X + x].State)//좌우만 체크
                {
                    if (0 > x) //왼쪽이냐?
                    {
                        node.FourWayCheck |= Four_Way_Access_Area_Check.LEFT;
                        //내노드의 왼쪽에 있는지 체크
                    }
                    else if (0 < x) //오른쪽이냐?
                    {
                        node.FourWayCheck |= Four_Way_Access_Area_Check.RIGHT;
                        //아니면 내노드 오른쪽에 있는지 체크
                    }
                }
            }
        }
    }


    
    /// <summary>
    /// 위치값 두개를 가지고 가장 가까운 경로를 탐색한다.
    /// 시작위치와 도착 위치가 유효한지 체크도해서 유효안하면 근처로 바꿔야함.
    /// </summary>
    /// <param name="startIndex">시작위치 인덱스</param>
    /// <param name="endIndex">도착위치 인덱스</param>
    public static Astar_Node GetShortPath(int startIndex, int endIndex)
    {
        ResetValue();                     // 경로와 G , H 값을 리셋 시킨다.
        int lastX = nodes.GetLength(1);         //x 좌표의 최대값
        int lastY = nodes.GetLength(0);         //y 좌표의 최대값


        int startX = startIndex == 0 ? 0 : startIndex % lastX;  //시작위치 의 x좌표값 
        int startY = startIndex == 0 ? 0 : startIndex / lastY;  //시작위치 의 y좌표값

        int endY = endIndex == 0 ? 0 : endIndex / lastY;    //도착위치 의 y좌표값
        int endX = endIndex == 0 ? 0 : endIndex % lastX;    //도착위치 의 x좌표값

        //Debug.Log($"{nodes.GetLength(0)} , {nodes.GetLength(1)} , {startIndex} ,{endIndex}");
        //Debug.Log($"start : {startX},{startY} end : {endX},{endY}");
        startNode = nodes[startY, startX];  // 시작위치와
        endNode = nodes[endY, endX];        // 도착위치를 셋팅하고 
        SetHeuristicsValue(endNode); //도착 지점 노드를 이용해 노드들의 휴리스틱 값으르 셋팅한다.

        return PathFinding(); //길찾기를 실행한다.
    }


 




    /// <summary>
    /// 현재 위치지점에서 행동력 기준 이동가능한 범위 의 좌표리스트를 가져오기위한 함수
    /// </summary>
    /// <param name="currentNode">현재 노드위치 </param>
    /// <param name="moveCheck">행동력 값</param>
    /// <returns>캐릭터가 이동가능한 노드리스트</returns>
    public static List<Astar_Node> SetMoveSize(Astar_Node currentNode, float moveCheck)
    {
        List<Astar_Node> resultNode = new List<Astar_Node>(); 
        openList.Clear();   // 탐색이 필요한 노드 리스트 
        closeList.Clear();  // 이미 계산이 완료되서 더이상 탐색을 안할 리스트 

        foreach (Astar_Node node in nodes) 
        {
            node.ResetMoveCheckValue(); // H 값을 1로 고정시키고 G 값을 초기화하여 계산 을 G값으로만 할수있게 한다.
        }

        openList.Add(currentNode);

        currentNode.G = 0.0f; //위에서 값초기화 하고있음으로 처음값을 0으로 다시셋팅

        while (openList.Count > 0) 
        {
            currentNode = openList[0];
            openList.Remove(currentNode); // 탐색가능한 목록에서 현재 탐색중인 목록을 제거하고 
            closeList.Add(currentNode);   // 탐색종료한 리스트에 현재 목록을 담는다.

            if (currentNode.G > moveCheck) //G 값이 현재 이동 가능한 거리보다 높으면  더이상 탐색이 필요없음으로 
            {
                continue; //다음거 탐색 
            }
            else // 이동가능한 거리면 
            {
                resultNode.Add(currentNode); //반환 시킬 리스트로 추가한다.
            }

            OpenListAdd(currentNode); //주변 8방향의 노드를 찾아서 G값 수정하고  오픈리스트에 담을수있으면 담는다.
            openList.Sort();            //찾은 G값중 가장 작은값부터 재탐색이된다.
        }
        return resultNode;
    }

    /// <summary>
    /// 셋팅된값이 도착지점에 도착할수있는지 여부를 확인하는 로직
    /// </summary>
    /// <returns>길이있으면 true 없으면 false</returns>
    private static bool IsComplite() 
    {
        while (openList.Count > 0)  //탐색 시작
        {
            currentNode = openList[0];
            if (currentNode == endNode) //탐색한 노드가 도착 위치면 
            {
                Debug.Log($"({currentNode.X},{currentNode.Y}) == ({endNode.X},{endNode.Y})길 존재함"); //길있다고 하고 
                return true; //빠져나감
            }
            else if(!closeList.Contains(currentNode)) //클로즈리스트에 없어야 
            {
                closeList.Add(currentNode);//추가한다.
                Debug.Log($"({currentNode.X},{currentNode.Y}) == ({endNode.X},{endNode.Y})길 찾는중"); //길있다고 하고 
            }
            openList.Remove(currentNode);

            OpenListAdd(currentNode);
        }
        Debug.Log($"검색마지막값의 노드 좌표는({currentNode.X},{currentNode.Y}) 이고 길이없음");
        return false;
    }
    /// <summary>
    /// 맵정보와 접근불가능한 지역 정보를 받고 
    /// 시작 위치지점과 도착 위치지점의 경로를 찾아보고 존재하면 아무짓도안하고 
    /// 존재하지않은경우 접근불가지역을 줄여가면서 경로를 탐색한다.
    /// </summary>
    /// <param name="obstacleIndexArray"></param>
    /// <param name="startIndex"></param>
    /// <param name="endIndex"></param>
    public static Astar_Node FindLineCheck(int startIndex, int endIndex)
    {


        openList.Clear(); //탐색하기위해 체크할 리스트 두개 초기화 하고 

        closeList.Clear();

        currentNode = AstarProccess.GetNode(startIndex); //시작위치의 노드값 저장하고

        currentNode.G = 0.0f; //시작지점은 0으로 안할시 길못찾는다.

        endNode =  AstarProccess.GetNode(endIndex); //도착위치의 노드값 저장 하고

        openList.Add(currentNode);//시작 노드 담아서 

        SetHeuristicsValue(endNode); //휴리스틱 셋팅 해주고 찾기 

        int debugCount = 0;
        while(!IsComplite()) //길을 찾을때 까지 계속 A Star 검색 시도  
        {
            closeList.Sort(ListCompareTo);  //휴리스틱값 기준으로 정렬을 시킨다.
            //randomCount = UnityEngine.Random.Range(0, closeList.Count); //랜덤으로 뽑기
            NextCheckNode(closeList[0]); //어느방향으로 갈지 가져오고 오픈리스트에 담을수있으면 담는다.
            debugCount++;
            if (debugCount > 1000) 
            {
                Debug.Log($"좌표값 여기가끝이야 ({currentNode.X},{currentNode.Y}) 못찾앗어 결국 ");
                break;
            }
        }

        return endNode;
    }

    /// <summary>
    /// 정렬방식 변경하기 
    /// </summary>
    /// <param name="thisNode">이전노드</param>
    /// <param name="otherNode">비교노드</param>
    /// <returns>정렬기준값</returns>
    public static int ListCompareTo(Astar_Node thisNode, Astar_Node otherNode)
    {
        // 리턴이 0보다 작다(-1)  : 내가(왼쪽) 작다(this < other)
        // 리턴이 0이다           : 나와 상대가 같다( this == other )
        // 리턴이 0보다 크다(+1)  : 내가(왼쪽) 크다(this > other)
        return thisNode.H == otherNode.H ? 0 : thisNode.H < otherNode.H ? -1 : 1;    // H 값을 기준으로 크기를 결정해라.
    }
    
    /// <summary>
    /// 길이막혀있는경우 
    /// 도착지점과 가까운방향의 셀을 찾아서 다음 노드를 검색해온다.
    /// 그리고 오픈리스트에 넣는다
    /// </summary>
    /// <param name="prevNode">검색기준위치</param>
    /// <returns></returns>
    private static void NextCheckNode(Astar_Node prevNode) 
    {
        Astar_Node node; //이동한 위치값 담을 객체변수 선언
        Four_Way_Access_Area_Check tempWay = Four_Way_Access_Area_Check.NONE; // 어느쪽에서 왔는지 체크하기위한변수

        int x = prevNode.X;     //연산용 변수 두개 선언
        int y = prevNode.Y;

        int random = UnityEngine.Random.Range(0, 2);
        //가로축으로 갈건지 세로축으로 이동할건지 랜덤으로 결정
     
        if (random < 1) //세로우선 랜덤으로 결정 
        {
            //도착지점과 가까운곳으로 좌표값 셋팅
            if (endNode.Y > prevNode.Y)
            {
                y += 1;
                tempWay = Four_Way_Access_Area_Check.DOWN; //위쪽으로 가니깐 아래쪽을 저장한다
            }
            else if (endNode.Y < prevNode.Y)
            {
                y -= 1;
                tempWay = Four_Way_Access_Area_Check.UP; //아래쪽으로 가니깐 위쪽을 저장한다
            }

            node = nodes[y,prevNode.X]; 
        }
        else  // 가로우선 
        {
            //도착지점과 가까운곳으로 좌표값 셋팅
            if (endNode.X > prevNode.X)
            {
                x += 1;
                tempWay = Four_Way_Access_Area_Check.LEFT; //오른쪽으로 가니깐 왼쪽을 저장한다
            }
            else if (endNode.X < prevNode.X)
            {
                x -= 1;
                tempWay = Four_Way_Access_Area_Check.RIGHT; //왼쪽으로 가니깐 오른쪽을 저장한다
            }
            node = nodes[prevNode.Y,x];
        }

        if (node.State == NodeState.Inaccessible) //접근 불가 지역이면  
        {

            //방향 갱신
            node.FourWayCheck &= ~tempWay;  // 현재 설정된 방향에서 들어온 방향을 제외 시킨다.
            node.FourWayCheck = ~node.FourWayCheck & Four_Way_Access_Area_Check.ALL; // 속성이 바꼈음으로 전체를 뒤집는다 .
            
            // 해당지역을 이동가능지역으로 변경 
            node.State = NodeState.Nomal; // 접근불가능지역에서 접근가능지역으로 변경시킨다 
            node.G = prevNode.G + nomalLine; //G값 갱신  대각선은 안됨으로 무조건 직선라인 을 더해주면된다.
            node.PrevNode = prevNode; //여기로 오는 노드값 입력
            onTileCreate?.Invoke(node); //타일생성하라고 호출
            Debug.Log($" 이전은 :{prevNode.State}({prevNode.X}{prevNode.Y}) 좌표에서 {node.X}{node.Y} = {prevNode.State}");
        }
        if (!openList.Contains(node)) //오픈리스트에 없을때만 
        {
            openList.Add(node); //담는다.
        }
    }

    

    /// <summary>
    /// 1. 특정노드의 주변노드를 검색해서 오픈 리스트에 담고
    /// 2. 주변노드의 G값을 갱신한다.
    /// </summary>
    /// <param name="currentNode">기준이되는 노드</param>
    private static void OpenListAdd(Astar_Node currentNode)
    {
        int horizontalSize = nodes.GetLength(1);    //가로 길이 가져오고 (2차원배열의 ? 배열의 길이 [y,?]) 오른쪽 기준이라 햇갈린다.
        int verticalSize = nodes.GetLength(0);      //세로 길이 가져오고 (2차원배열의 ? 배열의 길이 [?,x])

        //범위벗어낫는지 체크하기
        int horizontalStartIndex = currentNode.X - 1; //현재 위치의 왼쪽 값 가져오기
        horizontalStartIndex = horizontalStartIndex < 0 ? 0 : horizontalStartIndex; //왼쪽끝 값이면 0으로 셋팅

        int horizontalEndIndex = currentNode.X + 1; //현재 위치의 오른쪽 값 가져오기
        horizontalEndIndex = horizontalEndIndex == horizontalSize ? // 맨오른쪽이면  
            horizontalSize    :                                     
            horizontalEndIndex + 1;                                 // 포문 돌값셋팅하기위해 +1로 셋팅 <= 안하고 <로 체크하기위해서 값추가 

        int verticalStartIndex = currentNode.Y - 1; //현재 위치의 아래쪽 값 가져오기
        verticalStartIndex = verticalStartIndex < 0 ? 0 : verticalStartIndex; // 맨아래 값이면 0으로 셋팅 

        int verticalEndIndex = currentNode.Y + 1;                   //현재 위치의 위쪽 값 가져오기
        verticalEndIndex = verticalEndIndex == verticalSize ?       //맨 위면 
                            verticalSize    :                          
                            verticalEndIndex + 1;                       //포문 돌값셋팅하기위해 +1로 셋팅 <= 안하고 <로 체크하기위해서 값추가 
        //범위 체크 끝

        float tempG = 0.0f; //비교할 G값을 담을 임시변수

       
        Astar_Node tempNode; // 체크할 객체 선언(포문안에서 매번 스택에 메모리안잡고 한번만하기위해 밖에서 선언)
        for (int y = verticalStartIndex; y < verticalEndIndex; y++) //범위 지정한 것만큼 포문 돌리기
        {
            for (int x = horizontalStartIndex; x < horizontalEndIndex; x++) //범위 지정한만큼 포문 돌리기
            {
                tempNode = nodes[y, x]; //주변노드를 받아와서 
                if (tempNode.State == NodeState.Inaccessible) continue; //찾아온 주변노드가 못가는 지역이면 다음 반복문으로 이동
                /// G 값 체크 시작 
                /// 
                //대각선인지 직선인지 체크
                if ((currentNode.X - x) == 0 || (currentNode.Y - y) == 0) //직선인경우 
                {
                    tempG = currentNode.G + nomalLine;
                    if (tempNode.G  > tempG) //새로 갱신한 값보다 이전값이 크면  
                    {
                        tempNode.G = tempG;     //G 값을 갱신
                        tempNode.PrevNode = currentNode; //G값이 바꼈으면 어디서왔는지 이전노드도 갱신
                    }
                }
                else //대각선인 경우
                {
                    //(currentNode.X - x) //값이 - 면 오른쪽 + 면 왼쪽 
                    //(currentNode.Y - y) //값이 - 면 위쪽   + 면 아래쪽

                    //장애물 체크
                    if (nodes[
                            currentNode.Y - (currentNode.Y - y ),   // 현재위치에서 검색하는 위치의 값을 빼면 +와 -가 반전됨으로 - 로 처리 
                            currentNode.X                           // ex) 현재 (1,1) 일때  (2,2)확인시 (1,2) , (2,1)을확인해야됨 저값이되려면 +처리로안된다.
                            ].State == NodeState.Inaccessible || //세로부분에 못가는지역이 있거나
                        nodes[
                            currentNode.Y, 
                            currentNode.X - (currentNode.X -  x)
                            ].State == NodeState.Inaccessible)   //가로부분에 못가는지역이 있으면
                    {
                        Debug.Log($"현재위치({currentNode.X},{currentNode.Y}) 장애물 위치 ({x},{y})");
                        continue;//처리안하고 다음 노드를 찾는다.
                    }

                    tempG = currentNode.G + diagonalLine; //새로 생신될 G값 셋팅 대각선값으로
                    if (tempNode.G > tempG) //새로 갱신한 값보다 이전값이 크면  
                    {
                        tempNode.G = tempG;   //G 값을 갱신
                        tempNode.PrevNode = currentNode; //G값이 바꼈으면 어디서왔는지 이전노드도 갱신
                    }
                }

                //G값 체크 끝

                if (!closeList.Contains(tempNode) && !openList.Contains(tempNode))// 탐색이 끝나지 않은 노드 이고 오픈리스트에도 없어야 
                {
                   openList.Add(tempNode); //오픈리스트에 담아둔다.
                }
            }
        }
    }

    /// <summary>
    /// 최적의 경로를 찾는다.
    /// </summary>
    /// <returns>경로가 저장된 노드를 반환한다.</returns>
    private static Astar_Node PathFinding()
    {
        openList.Clear();   // 탐색이 필요한 노드 리스트 
        closeList.Clear();  // 이미 계산이 완료되서 더이상 탐색을 안할 리스트 

        startNode.G = 0.0f;      // 맨 처음 시작지점은 G 값이 0이래야 된다.
        openList.Add(startNode); // 맨 처음에는 시작 지점 등록해서 A* 로직 시작

        while (openList.Count > 0) //오픈리스트의 값이 있으면 계속 탐색 
        {
            currentNode = openList[0]; //현재 탐색 중인 노드를 담아둔다.

            if (currentNode == endNode) // 목적지에 도착했는지 체크한다.
            {
                return currentNode;     //도착했으면 경로가 담긴 노드를 반환한다.
            }

            openList.Remove(currentNode); // 탐색가능한 목록에서 현재 탐색중인 목록을 제거하고 
            closeList.Add(currentNode);   // 탐색종료한 리스트에 현재 목록을 담는다.

            OpenListAdd(currentNode); //현재위치에서 주변 노드를 찾아 G 값을 수정시키고 오픈리스트에 담는다.

            //오름차순 정렬 
            openList.Sort();
        }
        return null; //경로못찾으면 널을 리턴한다.
    }

    /// <summary>
    /// A* 에 사용할 다중배열을 생성한다.
    /// </summary>
    /// <param name="horizontalSize">가로 크기</param>
    /// <param name="verticalSize">세로 크기</param>
    private static Astar_Node[,] CreateNodeArray(int horizontalSize, int verticalSize)
    {
        Astar_Node[,] nodes = new Astar_Node[verticalSize, horizontalSize]; //전체크기만큼 배열잡고 
        int i = 0; // 인덱스 계산할 변수선언하고 
        for (int y = 0; y < verticalSize; y++) //세로 만큼
        {
            for (int x = 0; x < horizontalSize; x++)//가로만큼
            {
                nodes[y, x] = new Astar_Node(i , x, y); //2차원 배열 만들기  ///풀로 변경필요 --
                i++; //인덱스 증가
            }
        }
        return nodes;
    }

    /// <summary>
    /// 노드들의 G,H 값 을 초기화 하는 함수
    /// 경로 재탐색시 필요함
    /// </summary>
    private static void ResetValue()
    {
        foreach (var node in nodes)
        {
            node.AstarDataReset();
        }
    }

    /// <summary>
    /// 전체 셋팅  단일 셋팅도 만들어야한다.
    /// 도착 위치 기준으로 노드리스트의 휴리스틱 값을 셋팅하는 함수 
    /// </summary>
    /// <param name="endNode">도착 노드 값</param>
    private static void SetHeuristicsValue(Astar_Node endNode) 
    {
        float tempX = 0.0f;    //대각선 계산을위해 임시로 사용할 변수 
        float tempY = 0.0f;    //대각선 계산을위해 임시로 사용할 변수 
        float tempLine = 0.0f; //대각선 계산을위해 임시로 사용할 변수 

        foreach (Astar_Node node in nodes) //전체 리스트를 찾아서
        {
            node.H = float.MaxValue; //휴리스틱값을 다시셋팅할때 H 값을 초기화시킨다.

            if (node.X != endNode.X && node.Y != endNode.Y) //대각선인경우 ( x값이나 ,y 값이 같으면 같은라인인것을 이용해 체크)
            {
                // x 와 y 의 차이중 작은 값을 가지고 대각선을 긋고 두값의 차이만큼 더하면 끝 (ㅁ/ 이런모양)

                tempX = Math.Abs(endNode.X - node.X); //가로 라인의 거리값을 구하고 

                tempY = Math.Abs(endNode.Y - node.Y); //세로 라인의 거리값을 구하고

                tempLine = tempX == tempY ?                                         // 거리값이 같으면 정사각형임으로 
                            tempX * diagonalLine                                    // 한변에다가 대각선값을 곱하면끝
                            : tempX > tempY ?                                       // 가로 거리값이 더크면 
                            (tempY * diagonalLine) + ((tempX - tempY) * nomalLine)  // 세로 길이만큼 대각선길이를 곱하고 남은 가로 거리를 직선길이로 곱한다
                            :                                                       // 세로 거리값이 더크면 
                            (tempX * diagonalLine) + ((tempY - tempX) * nomalLine); // 가로 길이만큼 대각선길이를 곱하고 남은 세로 거리를 직선길이로 곱한다.

                node.H = tempLine; //계산된 값을 집어넣는다.
            }
            else if (node.X == endNode.X) //같은 가로 라인에 존재하면
            {
                node.H = Math.Abs(endNode.Y - node.Y) * nomalLine; //세로 차이만큼만 계산하면됨으로 직선계산   
            }
            else if (node.Y == endNode.Y) //같은 세로 라인에 존재하면 
            {
                node.H = Math.Abs(endNode.X - node.X) * nomalLine;  //가로 차이만큼만 직선계산
            }
            else 
            {
                Debug.Log($"무슨경우냐? 들어올일 없을거같은데  : node({node.X},{node.Y}) _ curruntNode({endNode.X},{endNode.Y})");
            }
            Debug.Log($"휴리스틱({node.X},{node.Y}) : {node.H} ");
        }
    }
    
    /// <summary>
    /// 노드가 가지고있는 경로를 찾아온다.
    /// </summary>
    /// <param name="node">경로 찾을 노드</param>
    /// <returns>노드까지 설정된 경로 </returns>
    public static List<Vector3Int> GetPath(Astar_Node node)
    {
        List<Vector3Int> path = new List<Vector3Int>();

        path.Add(new Vector3Int(node.X, node.Y, node.Z));
        Astar_Node prevNode = node.PrevNode;

        while (prevNode != null) //이전경로가 있을때까지 돌려!
        {
            path.Add(new Vector3Int(prevNode.X, prevNode.Y, prevNode.Z)); //노드 정보를 담는다.
            prevNode = prevNode.PrevNode; //이전노드 정보 찾아오기
        }
        path.Reverse(); //들어간 값을 꺼꾸로 뒤집는다.
        return path;
    }
    /// <summary>
    /// 인덱스에 해당하는 노드값 가져오기
    /// </summary>
    /// <param name="startIndex">인덱스값</param>
    /// <returns>인덱스에 해당하는 노드정보</returns>
    public static Astar_Node GetNode(int startIndex)
    {
        if (nodes != null && nodes.Length > 0 && startIndex < nodes.Length)
        {
            if (startIndex < 1)
            {
                return nodes[0, 0];
            }
            int startX = startIndex == 0 ? 0 : startIndex % nodes.GetLength(0);  //시작위치 의 x좌표값 
            int startY = startIndex == 0 ? 0 : startIndex / nodes.GetLength(1);  //시작위치 의 y좌표값

            return nodes[startY, startX];
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 인덱스에 해당하는 노드값 가져오기
    /// <param name="x">x인덱스 좌표</param>
    /// <param name="y">y인덱스 좌표</param>
    /// <param name="z">z인덱스 좌표? 값없음</param>
    /// <returns>인덱스에 해당하는 노드정보</returns>
    public static Astar_Node GetNode( int x , int y , int z = 1)
    {
        if (x  < 0 || y < 0  || x > nodes.GetLength(0) || y >  nodes.GetLength(1))
        {
            return nodes[x, y];
        }
        else
        {
            return null;
        }
    }

    //----------- 테스트용 함수




    public static void TestGLog()
    {
        string str = "";
        foreach (Astar_Node node in nodes)
        {
            str += $"좌표({node.X},{node.Y}) : G 값 : {node.G} \r\n";
        }
        Debug.Log(str);
    }

   

}