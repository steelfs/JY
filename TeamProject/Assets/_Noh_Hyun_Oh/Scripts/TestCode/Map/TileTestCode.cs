using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileTestCode : TestBase
{
    [SerializeField]
    TileManager tileManager;
   
    [SerializeField]
    int startIndex = 0;
    [SerializeField]
    int endIndex = 0;
    [SerializeField]
    Astar_Node temp_node;

    [SerializeField]
    PathLine pathLine;

    [SerializeField]
    float moveSize = 0.0f;
    protected override void Awake()
    {
        base.Awake();
        pathLine = GetComponent<PathLine>();
        
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        Base_TileCell[] temp = FindObjectsOfType<Base_TileCell>();
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].OnResetData();
        }

        tileManager.GenerateTileMap();
        
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        foreach (ITileBase tile in tileManager.MapTiles)
        {
            if (tile == null) continue;
            tile.CurruntTileState = CurrentTileState.None;
        }
        Astar_Node nodeTemp = AstarProccess.GetNode(startIndex);
        if (nodeTemp == null) return;
        List<Astar_Node> tempList = AstarProccess.SetMoveSize(nodeTemp, moveSize);
        if (tempList == null) return;
        List<Vector3Int> lineTemp = new List<Vector3Int>();
        foreach (Astar_Node node in tempList)
        {
            if (tileManager.MapTiles[node.NodeIndex]
                == null) break;
            tileManager.MapTiles[node.NodeIndex].CurruntTileState = CurrentTileState.MoveArea;
            //tmpS += $"À§Ä¡ : ({node.X},{node.Y})";
            lineTemp.Add(new Vector3Int(node.X, node.Y, node.Z));
        }
        //pathLine.DrawPath(lineTemp);
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
         AstarProccess.InitData(tileManager.HorizontalGroupLength, tileManager.VerticalGroupLength, tileManager.ObstacleIndexArray);
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        temp_node = AstarProccess.GetShortPath(startIndex, endIndex);
        Debug.Log(temp_node);
    }
    protected override void Test5(InputAction.CallbackContext context)
    {
        List<Vector3Int> tempList = AstarProccess.GetPath(temp_node);

        for (int i = 0; i < tempList.Count; i++)
        {
            Vector3Int temp = tempList[i];
            temp.z += 1;
            tempList[i] = temp;
        }
        if (tempList.Count > 0) pathLine.DrawPath(tempList);
    }

    protected override void Test6(InputAction.CallbackContext context)
    {
        temp_node = AstarProccess.FindLineCheck(
   
                            startIndex, 
                            endIndex);
    }
}
