using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A* �� ���� ��� ��
/// </summary>
public class Astar_Node : IComparable<Astar_Node>
{
    /// <summary>
    /// �ֺ���尡 �����ִ��������� �ƴ��� üũ
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
    /// �ش����� ���� ���ɿ��� 
    /// </summary>
    AstarProccess.NodeState state = AstarProccess.NodeState.None;
    public AstarProccess.NodeState State 
    {
        get => state;
        set 
        {
            state = value; //���� �ȴ�.
        }
    }
    /// <summary>
    /// �ش� ���� �������� �����ġ��
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
    /// �ش��尡 �����ϰ��ִ� ���� ����� (������ǥ�� ������ǥ���� ���̰�).
    /// Ÿ�ϰ��� ������ �̰����� �����Ѵ�.
    /// </summary>
    //float nodeHorizontalSize = 1.0f;

    /// <summary>
    /// �ش��尡 �����ϰ��ִ� ���� ����� (������ǥ�� ������ǥ���� ���̰�).
    /// Ÿ�ϰ��� ������ �̰����� �����Ѵ�.
    /// </summary>
    //float nodeVerticalSize = 1.0f;

    /// <summary>
    /// �ش��尡 �����ϰ��ִ� ���� ����� (������ǥ�� ������ǥ���� ���̰�).
    /// Ÿ�ϰ��� ������ �̰����� �����Ѵ�.
    /// </summary>
    //float nodeDepthSize = 1.0f;


    /// <summary>
    /// ���� �ε��� ��ġ ��ǥ��
    /// </summary>
    int x ,  // ���� ���� �ε��� 
        y ,  // ���� �ε���
        z;   // ���� ���� �ε���
    public int X => x;
    public int Y => y;
    public int Z => z;



    public float G = float.MaxValue;
    /// <summary>
    /// ��� �������� ������������ �°Ÿ�
    /// </summary>


    public float H  = float.MaxValue;

    /// <summary>
    /// ������������ ������������ ���� �Ÿ�(����)
    /// </summary>
    
    /// <summary>
    /// ���������� ���� ������
    /// </summary>
    public float F => G + H;

    /// <summary>
    /// ������ 
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
    /// ������ �ʱ�ȭ�� �Լ�
    /// </summary>
    /// <param name="x">x�ε��� ��</param>
    /// <param name="y">y�ε��� ��</param>
    /// <param name="z">z�ε��� ��</param>
    public void InitData(int x , int y , int z ) 
    {
        this.x = x;
        this.y = y;
        this.z = z;

        TileSizeSetting();
    }
    
    /// <summary>
    /// Ÿ���� ũ�� ����
    /// </summary>
    private void TileSizeSetting() 
    {
        ///Ÿ�ϰ� �����ϴ¹�� ã�Ƽ� �־���� .
    }

    /// <summary>
    /// ������ ���¿� �Լ�
    /// </summary>
    public void AstarDataReset() 
    {
        G = float.MaxValue;
        H = float.MaxValue;
        PrevNode = null;
    }

    /// <summary>
    /// ��絥���� ���� �Լ�
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
    /// �̵����� üũ�ϱ����� �����Լ� 
    /// </summary>
    public void ResetMoveCheckValue() 
    {
        H = 1;
        G = float.MaxValue;
    }

    /// <summary>
    /// Ÿ���� ����(�̵����ɿ���)�� �ʱ�ȭ �ϴ� �Լ� 
    /// </summary>
    public void ReverseState() 
    {
        state = AstarProccess.NodeState.Nomal;
    }

    /// <summary>
    /// ���� Ÿ�� ���� ũ�� �񱳸� �ϴ� �Լ�
    /// </summary>
    /// <param name="other">�� ���</param>
    /// <returns>-1,-0, 1 �� �ϳ�</returns>
    public int CompareTo(Astar_Node other)
    {
        // ������ 0���� �۴�(-1)  : ����(����) �۴�(this < other)
        // ������ 0�̴�           : ���� ��밡 ����( this == other )
        // ������ 0���� ũ��(+1)  : ����(����) ũ��(this > other)

        if (other == null)      // other�� null�� �� ������ �װͿ� ���� ���
            return 1;

        return F.CompareTo(other.F);    // F ���� �������� ũ�⸦ �����ض�.
    }

    /// <summary>
    /// == ��ɾ� �����ε�. ���� ����� ��ġ�� ������ ������ (x,y,z)�� ������ Ȯ��
    /// </summary>
    /// <param name="left">���</param>
    /// <param name="right">����(int)</param>
    /// <returns>true�� ���� false�� �ٸ���.</returns>
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