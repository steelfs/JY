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
    /// 이 셀에 연결된 길을 기록하는 변수 (북 동 남 서 ) 순서대로 비트에 1이 세팅되어 있으면 길이 있다. 0이 세팅되어 있으면 길이 없다.
    /// </summary>
    byte path = 0;
    public byte Path => path;
    /// <summary>
    /// 그리드상에서 x 좌표 왼쪽 => 오른쪽
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
        path |= (byte)direction; // | 연산자 이용해서 비트를 추가하는 함수 

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
