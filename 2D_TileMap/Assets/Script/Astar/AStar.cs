using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
//static은 new 사용 불가능 
public static class AStar //단순 계산용 클래스
{

    /// <summary>
    /// // Astar 알고리즘으로 길을 탐색하는 함수//기본적으로 길찾기알고리즘은 무겁다. 따라서 그룹으로 묶어 실행하면 해당그룹의 중심점을 기준으로 한번만 실행한다.
    /// </summary>
    /// <param name="map">길을 찾을 맵</param>
    /// <param name="start">시작위치</param>
    /// <param name="end">도착위치</param>
    /// <returns> 출발 -> 도착으로 가는 경로 길찾기 실패시 null</returns>
    public static List<Vector2Int> PathFind(GridMap map, Vector2Int start, Vector2Int end)
    {
        const float sideDistance = 1.0f;
        const float diagonalDistance = 1.4f;//대각선으로 이동할 경우 거리

       
        List<Vector2Int> path = null;

        if (map.IsValidPosition(start) && map.IsValidPosition(end))//파라미터로 받은 좌표가 범위 안에 있다면 
        {
            map.ClearMapData();//재사용할 때 를 대비해 초기화 한번 실행

            List<Node> open = new List<Node>();// 앞으로 탐색할 후보노드의 리스트
            List<Node> close = new List<Node>();//탐색이 완료된 리스트

            Node current = map.GetNode(start); //A* 시작 // start를 가져와서
            current.G = 0;
            current.H = GetHeuristic(current, end); //G, H 초기 설정 후 
            open.Add(current);// openList에 추가

            while(open.Count > 0)//오픈리스트(후보) 에 노드가 들어있다면 계속 반복, openList가 비면 길찾기 실패
            {
                open.Sort();// //F값 기준으로  정렬 후
                current = open[0];//F값이 가장 작은 노드를 current로 설정
                open.RemoveAt(0); // 꺼낸 노드를 OpenList에서 제거
                if (current != end) // 현재위치가 도착지점인지 확인
                {
                    close.Add(current);//close리스트에 넣어서 이 노드는 탐색했음을 표시

                    for (int y = -1; y<2; y++) //current의 주변 8방향 노드를 openList에 넣거나 G값 갱신
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            Node node = map.GetNode(current.x + x, current.y + y);//  주변노드 가져와서

                            //스킵할 노드 체크
                            if (node == null) continue; //노드가 맵 범위 밖이면 continue
                            if (node == current) continue; //자기자신
                            if (node.nodeType == Node.NodeType.Wall) continue; //벽일 때 
                            if (close.Exists((x) => x == node)) continue;//노드가 클로즈리스트에 있을 때 
                            bool isDiagonal = (x * y) != 0;   //대각선 조건
                            //대각선은 어떻게 확인할 수 있는가  //isDiagonal = Mathf.Abs(x) != Mathf.Abs(y);

                            if (isDiagonal && (map.IsWall(current.x + x, current.y) || map.IsWall(current.x, current.y + y))) //대각선일 때 양 옆의 노드 중 하나가 벽 일때  
                                continue;

                            float distance;//거리 설정 
                            if (isDiagonal) distance = diagonalDistance; //대각선이면 1

                            else
                                distance = sideDistance;

                            if (node.G > current.G + distance) //노드의 g값이 current를 거쳐서 노드로 가는 G값보다 크면 갱신
                            {
                                if (node.parent == null) //노드의 부모가 없으면 아직 openList에 안들어간 것으로 판단
                                {
                                    //openList에 아직 안들어가 있다면 
                                    node.H = GetHeuristic(node, end); //휴리스틱 계산
                                    open.Add(node);//open 리스트에 추가
                                }
                                node.G = current.G + distance;//G값 갱신
                                node.parent = current;//current를 거쳐서 도착했다고 표시
                            }
                            



                            //(-1, 1), (0,  1), (1,  1)
                            //(-1, 0), (0,  0), (1,  0)
                            //(-1,-1), (0, -1), (1, -1)
                        }
                    }
                }
                else//도착지점이라면
                {
                    break;
                }
            }
            //마무리작업
            if (current == end)//목적지에 도착했다면 
            {
                path = new List<Vector2Int>();//경로 만들기
                Node result = current;
                while(result != null) //시작위치에 도달할때 까지 계속 추가
                {
                    path.Add(new Vector2Int(result.x, result.y));
                    result = result.parent;
                }
                path.Reverse(); //도착 -> 출발로 오는 리스트를 뒤집기 
            }
        }
        return path;
    }

    /// <summary>
    /// 휴리스틱값을 계싼하는 함수  (현재위치에서 목적지까지 예상거리 구하기)
    /// </summary>
    /// <param name="current">현재노드</param>
    /// <param name="end">도착지점</param>
    /// <returns>current 에서 end까지 예상거리</returns>
    private static float GetHeuristic(Node current, Vector2Int end)
    {
        return Mathf.Abs(current.x - end.x) + Mathf.Abs(current.y - end.y);//절대값 음수 리턴 방지
    }
}
