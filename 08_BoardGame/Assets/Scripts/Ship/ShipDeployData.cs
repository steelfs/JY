using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeployData
{
    ShipDirection direction;//
    public ShipDirection Direction => direction;
    Vector2Int position;
    public Vector2Int Position => position;

    public ShipDeployData(ShipDirection dir, Vector2Int pos)
    {
        direction = dir;
        position = pos;
    }
}
