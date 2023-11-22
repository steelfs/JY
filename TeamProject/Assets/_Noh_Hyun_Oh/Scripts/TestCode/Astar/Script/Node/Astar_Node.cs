using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A* 에 사용될 노드 값
/// </summary>
public class Astar_Node : IComparable<Astar_Node>
{
    /// <summary>
    /// 주변노드가 갈수있는지역있지 아닌지 체크
    /// </summary>
    private AstarProccess.Four_Way_Access_Area_Check fourWayCheck = AstarProccess.Four_Way_Access_Area_Check.NONE;
    public AstarProccess.Four_Way_Access_Area_Check FourWayCheck
    {
        get => fourWayCheck;
        set 
        {
            fourWayCheck = value;
        }
    }
    private int nodeIndex = -1;
    public int NodeIndex => nodeIndex;
    /// <summary>
    /// 해당노드의 접근 가능여부 
    /// </summary>
    AstarProccess.NodeState state = AstarProccess.NodeState.None;
    public AstarProccess.NodeState State 
    {
        get => state;
        set 
        {
            state = value; //셋팅 된다.
        }
    }
    /// <summary>
    /// 해당 노드로 오기전의 노드위치값
    /// </summary>
    Astar_Node prevNode;
    public Astar_Node PrevNode 
    {
        get => prevNode;
        set
        {
            prevNode = value;
        }
    }
    
    /// <summary>
    /// 해당노드가 포함하고있는 세로 사이즈값 (시작좌표와 끝의좌표간의 사이값).
    /// 타일간의 간격을 이값으로 설정한다.
    /// </summary>
    //float nodeHorizontalSize = 1.0f;

    /// <summary>
    /// 해당노드가 포함하고있는 가로 사이즈값 (시작좌표와 끝의좌표간의 사이값).
    /// 타일간의 간격을 이값으로 설정한다.
    /// </summary>
    //float nodeVerticalSize = 1.0f;

    /// <summary>
    /// 해당노드가 포함하고있는 높이 사이즈값 (시작좌표와 끝의좌표간의 사이값).
    /// 타일간의 간격을 이값으로 설정한다.
    /// </summary>
    //float nodeDepthSize = 1.0f;


    /// <summary>
    /// 현재 인덱스 위치 좌표값
    /// </summary>
    int x ,  // 가로 라인 인덱스 
        y ,  // 높이 인덱스
        z;   // 세로 라인 인덱스
    public int X => x;
    public int Y => y;
    public int Z => z;



    public float G = float.MaxValue;
    /// <summary>
    /// 출발 지점에서 현재지점까지 온거리
    /// </summary>


    public float H  = float.MaxValue;

    /// <summary>
    /// 현재지점에서 도착지점까지 남은 거리(추정)
    /// </summary>
    
    /// <summary>
    /// 최종적으로 예상 도착값
    /// </summary>
    public float F => G + H;

    /// <summary>
    /// 생성자 
    /// </summary>
    /// <param name="tile3DPos"></param>
    public Astar_Node(int index , Vector3Int tile3DPos)
    {
        nodeIndex = index;
        InitData(tile3DPos.x, tile3DPos.y, tile3DPos.z);
    }
    public Astar_Node(int index, int x , int y , int z  = 1)
    {
        nodeIndex = index;
        InitData(x,y,z);
    }
    /// <summary>
    /// 데이터 초기화용 함수
    /// </summary>
    /// <param name="x">x인덱스 값</param>
    /// <param name="y">y인덱스 값</param>
    /// <param name="z">z인덱스 값</param>
    public void InitData(int x , int y , int z ) 
    {
        this.x = x;
        this.y = y;
        this.z = z;

        TileSizeSetting();
    }
    
    /// <summary>
    /// 타일의 크기 셋팅
    /// </summary>
    private void TileSizeSetting() 
    {
        ///타일값 셋팅하는방법 찾아서 넣어두자 .
    }

    /// <summary>
    /// 데이터 리셋용 함수
    /// </summary>
    public void AstarDataReset() 
    {
        G = float.MaxValue;
        H = float.MaxValue;
        PrevNode = null;
    }

    /// <summary>
    /// 모든데이터 리셋 함수
    /// </summary>
    public void ResetValue() 
    {
        AstarDataReset();
        nodeIndex = -1;
        x = int.MinValue;
        y = int.MinValue;
        z = int.MinValue;
        state = AstarProccess.NodeState.Nomal;
    }

    /// <summary>
    /// 이동범위 체크하기전용 리셋함수 
    /// </summary>
    public void ResetMoveCheckValue() 
    {
        H = 1;
        G = float.MaxValue;
    }

    /// <summary>
    /// 타일의 상태(이동가능여부)를 초기화 하는 함수 
    /// </summary>
    public void ReverseState() 
    {
        state = AstarProccess.NodeState.Nomal;
    }

    /// <summary>
    /// 같은 타입 간의 크기 비교를 하는 함수
    /// </summary>
    /// <param name="other">비교 대상</param>
    /// <returns>-1,-0, 1 중 하나</returns>
    public int CompareTo(Astar_Node other)
    {
        // 리턴이 0보다 작다(-1)  : 내가(왼쪽) 작다(this < other)
        // 리턴이 0이다           : 나와 상대가 같다( this == other )
        // 리턴이 0보다 크다(+1)  : 내가(왼쪽) 크다(this > other)

        if (other == null)      // other가 null일 수 있으니 그것에 대한 대비
            return 1;

        return F.CompareTo(other.F);    // F 값을 기준으로 크기를 결정해라.
    }

    /// <summary>
    /// == 명령어 오버로딩. 왼쪽 노드의 위치와 오른쪽 벡터의 (x,y,z)가 같은지 확인
    /// </summary>
    /// <param name="left">노드</param>
    /// <param name="right">벡터(int)</param>
    /// <returns>true면 같고 false면 다르다.</returns>
    public static bool operator ==(Astar_Node left, Vector3Int right)
    {
        return left.x == right.x && left.y == right.y && left.z == right.z;
    }

    public static bool operator !=(Astar_Node left, Vector3Int right)
    {
        return left.x != right.x || left.y != right.y || left.z != right.z;
    }

    public override bool Equals(object obj)
    {
        return obj is Astar_Node other && this.x == other.x && this.y == other.y && this.z == other.z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.x, this.y, this.z);
    }
}