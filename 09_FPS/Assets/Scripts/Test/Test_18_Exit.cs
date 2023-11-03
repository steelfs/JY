using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_18_Exit : TestBase
{
    
    public MazeVisualizer maze;
    public int seed = -1;
    private void Start()
    {
        if (seed != -1)
        {
            UnityEngine.Random.InitState(seed);
        }
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        int width = (int)(maze.width * 0.2f);
        int height = (int)(maze.height * 0.2f);

        int widthMin = (int)((maze.width - width) * 0.5f);
        int widthMax = (int)((maze.width + width) * 0.5f);
        int x = Random.Range(widthMin, widthMax);

        int heightMin = (int)((maze.width - width) * 0.5f);
        int heightMax = (int)((maze.width + width) * 0.5f);
        int y = Random.Range(heightMin, heightMax);

        Vector3 world = maze.GridToWorld(x, y);

        Debug.Log(x);
        Debug.Log(y);


        Player player = GameManager.Inst.Player;
        CharacterController characterController = player.GetComponent<CharacterController>();
        characterController.enabled = false;
        player.transform.position = maze.GridToWorld(x, y);
        characterController.enabled = true;

        Ray ray = new(world + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 10.0f))
        {
            CellVisualizer cellVisualizer = hitInfo.collider.gameObject.GetComponentInParent<CellVisualizer>();// 레이케스트에 성공했다는 것은 콜라이더가 무조건있다는것을 나타냄

            Direction path = cellVisualizer.GetPaths();
            Debug.Log(path);
            List<Vector3> dirList = new List<Vector3>(4);
            if ((path & Direction.North) != 0)
            {
                dirList.Add(Vector3.forward);
            }
            if((path & Direction.East) != 0)
            {
                dirList.Add(Vector3.right);
            }
            if ((path & Direction.South) != 0)
            {
                dirList.Add(Vector3.back);
            }
            if ((path & Direction.West) != 0)
            {
                dirList.Add(Vector3.left);
            }
            Vector3 dir = dirList[Random.Range(0, dirList.Count)];
            player.transform.LookAt(player.transform.position + dir);
        }
    }
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        int width = (int)(maze.width * 0.2f);
        int height = (int)(maze.height * 0.2f);

        int widthMin = (int)((maze.width - width) * 0.5f);
        int widthMax = (int)((maze.width + width) * 0.5f);
        int x = Random.Range(widthMin,widthMax);

        int heightMin = (int)((maze.width - width) * 0.5f);
        int heightMax = (int)((maze.width + width) * 0.5f);
        int y = Random.Range(heightMin, heightMax);

        Vector3 p0 = new Vector3(widthMin * CellVisualizer.CellSize, 0, -heightMin * CellVisualizer.CellSize);
        Vector3 p1 = new Vector3(widthMax * CellVisualizer.CellSize, 0, -heightMin * CellVisualizer.CellSize);
        Vector3 p2 = new Vector3(widthMax * CellVisualizer.CellSize, 0, -heightMax * CellVisualizer.CellSize);
        Vector3 p3 = new Vector3(widthMin * CellVisualizer.CellSize, 0, -heightMax * CellVisualizer.CellSize);

        Handles.color = Color.black;
        Handles.DrawLine(p0, p1, 5.0f);
        Handles.DrawLine(p1, p2, 5);
        Handles.DrawLine(p2, p3, 5);
        Handles.DrawLine(p0, p3, 5);
#endif
    }
}
