using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Direction : byte
{
    North = 1,
    East = 2,
    South = 4,
    West = 8,
}
public class Cell 
{
    /// <summary>
    /// �� ���� ����� ���� ����ϴ� ���� (�� �� �� �� ) ������� ��Ʈ�� 1�� ���õǾ� ������ ���� �ִ�. 0�� ���õǾ� ������ ���� ����.
    /// </summary>
    byte path = 0;
    public byte Path => path;
    /// <summary>
    /// �׸���󿡼� x ��ǥ ���� => ������
    /// </summary>
    protected int x;
    public int X => x;
    protected int y;
    public int Y => y;

    public Cell(int x, int y)
    {
        this.path = 0;
        this.x = x;
        this.y = y;
    }
    public void MakePath(Direction direction)//or & |
    {
        path |= (byte)direction; // | ������ �̿��ؼ� ��Ʈ�� �߰��ϴ� �Լ� 

    }

    public bool IsPath(Direction direction)
    {
        return (path & (byte)direction) == 1;
    }
    public bool IsWall(Direction direction)
    {
        return (path & (byte)direction) != 1;
    }
}
