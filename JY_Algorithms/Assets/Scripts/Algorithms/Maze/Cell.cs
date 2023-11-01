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
        //~ 는 반전시키는 NOT연산자.  반전 시킨 후 And를 해서 0으로 셋팅
    }
    public void ResetPath() {path = 0; on_RefreshWall?.Invoke(path); }
    public bool IsOpened(Direction direction)
    {
        return (path & (byte)direction) != 0; //& 한 후 0이 아니면 그곳에 비트가 이미 셋팅되어있다
    }
}
