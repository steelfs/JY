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
    West = 8
}
public class Cell
{
    byte path;
    public byte Path
    {
        get { return path; }
        set { path = value; }
    }

    int x;
    public int X => x;
    int y;
    public int Y => y;
    public Action<byte> on_RefreshWall;
    public Cell(int x, int y)
    {
        this.x = x; 
        this.y = y;
    }
    public void OpenWall(Direction direction)
    {
        path |= (byte)direction;
        on_RefreshWall?.Invoke(path);
    }
    public void CloseWall(Direction direction)
    {
        path &= (byte)~direction;
        on_RefreshWall?.Invoke(path);
        //~ �� ������Ű�� NOT������.  ���� ��Ų �� And�� �ؼ� 0���� ����
    }
    public void ResetPath() {path = 0; on_RefreshWall?.Invoke(path); }
    public bool IsOpened(Direction direction)
    {
        return (path & (byte)direction) != 0; //& �� �� 0�� �ƴϸ� �װ��� ��Ʈ�� �̹� ���õǾ��ִ�
    }
}
