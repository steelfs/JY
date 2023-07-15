using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//축의 기준 : 왼쪽 아래가 원점 
//축의 방향 : 왼쪽 -x, 아래쪽 -y
public class Node : IComparable<Node>
{
    //rmflem aoqdptj x좌표 
    public int x;
    public int y;

    
    public float G;//확정거리 
    public float H;// 목표지점까지 남은(예상)거리 
    public float F => G + H; //g와h의 합 , 출발지에서 이 노드를 경유해 도착지까지 예상 거리

    public Node parent;//앞 노드.
    public enum NodeType
    {
        Plain,//평지(이동가능)
        Wall,// 벽(이동 불가능)
        Monster,//몬스터(이동 불가능)
    }

    public NodeType nodeType = NodeType.Plain;
    /// <summary>
    /// Node의 생성자 
    /// </summary>
    /// <param name="x">맵에서 x위치 </param>
    /// <param name="y">맵에서 y위치 </param>
    /// <param name="nodeType"></param>
    public Node(int x, int y, NodeType nodeType = NodeType.Plain)
    {
        this.x = x;
        this.y = y;
        this.nodeType = nodeType;

        ClearData();
    }

    public void ClearData()
    {
        G = float.MaxValue;
        H = float.MaxValue;
        parent = null;
    }

    public int CompareTo(Node other)// 같은 타입끼리 크기를 비교하는 함수  리턴값은 -1,0,1
    {
        //리턴이 0보다 작다 : 내가 작다(-1) (this < other)
        //리턴이 0이다  나와 같다
        //리턴이 0보다 크다 : 내가 크다(+1) (this > other)
        if (other == null) // other가 null 일 경우에 대비
        {
            return 1;
        }
        return F.CompareTo(other.F); // f값을 기준으로 크기를 결정해라
    }


    /// <summary>
    /// == 명령어 오버로딩 
    /// 왼쪽 노드의 위치와 오른쪽 벡터의 x, y 가 같은지 확인
    /// </summary>
    /// <param name="left">노드</param>
    /// <param name="right">벡터(int)</param>
    /// <returns></returns>
    public static bool operator == (Node left, Vector2Int right)
    {
        return left.x == right.x && left.y == right.y;
    }
    public static bool operator !=(Node left, Vector2Int right)
    {
        return left.x != right.x || left.y != right.y;
    }
    public override bool Equals(object obj)
    {
        return obj is Node other && this.x == other.x && this.y == other.y;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(this.x, this.y);
    }
}
