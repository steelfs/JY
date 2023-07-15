using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���� ���� : ���� �Ʒ��� ���� 
//���� ���� : ���� -x, �Ʒ��� -y
public class Node : IComparable<Node>
{
    //rmflem aoqdptj x��ǥ 
    public int x;
    public int y;

    
    public float G;//Ȯ���Ÿ� 
    public float H;// ��ǥ�������� ����(����)�Ÿ� 
    public float F => G + H; //g��h�� �� , ��������� �� ��带 ������ ���������� ���� �Ÿ�

    public Node parent;//�� ���.
    public enum NodeType
    {
        Plain,//����(�̵�����)
        Wall,// ��(�̵� �Ұ���)
        Monster,//����(�̵� �Ұ���)
    }

    public NodeType nodeType = NodeType.Plain;
    /// <summary>
    /// Node�� ������ 
    /// </summary>
    /// <param name="x">�ʿ��� x��ġ </param>
    /// <param name="y">�ʿ��� y��ġ </param>
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

    public int CompareTo(Node other)// ���� Ÿ�Գ��� ũ�⸦ ���ϴ� �Լ�  ���ϰ��� -1,0,1
    {
        //������ 0���� �۴� : ���� �۴�(-1) (this < other)
        //������ 0�̴�  ���� ����
        //������ 0���� ũ�� : ���� ũ��(+1) (this > other)
        if (other == null) // other�� null �� ��쿡 ���
        {
            return 1;
        }
        return F.CompareTo(other.F); // f���� �������� ũ�⸦ �����ض�
    }


    /// <summary>
    /// == ��ɾ� �����ε� 
    /// ���� ����� ��ġ�� ������ ������ x, y �� ������ Ȯ��
    /// </summary>
    /// <param name="left">���</param>
    /// <param name="right">����(int)</param>
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
