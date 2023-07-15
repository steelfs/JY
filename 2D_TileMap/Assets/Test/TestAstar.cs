using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TestAstar : TestBase
{
    List<int> list = new List<int>();
    List<Node> listNode = new List<Node>();

    GridMap map;
    public Tilemap backGround;
    public Tilemap obstacle;
    public PathLine pathLine;

    public Vector2Int start;
    public Vector2Int end;
    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.TestRClick.performed += TestRClick;
    }

    private void TestRClick(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();//스크린 좌표계
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);//스크린 좌표를 월드좌표로 바꿔주는 함수
        Vector2Int gridPos = map.WorldToGrid(worldPos);

        if (map.IsValidPosition(gridPos) && !map.IsWall(gridPos))//isMonster추가해야함
        {
            //Vector2 final = map.GridToWorld(gridPos);
            //end.x = (int)final.x;
            //end.y = (int)final.y;

            end = gridPos;
    
            List<Vector2Int> path = AStar.PathFind(map, start, end);
            pathLine.DrawPath(map, path);
        }
  
    }

    protected override void OnDisable()
    {
        inputActions.Test.TestRClick.performed -= TestRClick;
        base.OnDisable();
    }

    private void Start()
    {
         map = new GridMap(backGround, obstacle);
    
    }
    protected override void TestClick(InputAction.CallbackContext context)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();//스크린 좌표계
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);//스크린 좌표를 월드좌표로 바꿔주는 함수
        Vector2Int gridPos = map.WorldToGrid(worldPos);

        if (map.IsValidPosition(gridPos) && !map.IsWall(gridPos))//isMonster추가해야함
        {
            start = gridPos;
        }
        Debug.Log(gridPos);
   
        //왼쪽 클릭시 start변수에해당 그리드 좌표 설정
    }
    private void TestPathFind()
    {
        GridMap map = new GridMap(4, 3);
        Node node = map.GetNode(1, 0);
        node.nodeType = Node.NodeType.Wall;
        node = map.GetNode(2, 2);
        node.nodeType = Node.NodeType.Wall;

        List<Vector2Int> path = AStar.PathFind(map, new Vector2Int(0, 0), new Vector2Int(3, 2));

        string pathStr = "Path : ";

        foreach (var pos in path)
        {
            pathStr += $" ({pos.x}, {pos.y}) ->";
        }
        pathStr += "끝";
        Debug.Log(pathStr);
    }

    private void TestNode()
    {
        list.Add(10);
        list.Add(30);
        list.Add(50);
        list.Add(10);
        list.Add(70);
        list.Add(90);
        list.Add(80);
        list.Add(100);

        listNode.Clear();
        Node a = new Node(0, 0);
        a.H = 10;
        a.G = 0;
        Node b = new Node(0, 0);
        b.H = 5;
        b.G = 0;
        Node c = new Node(0, 0);
        c.H = 15;
        c.G = 0;
        listNode.Add(a);
        listNode.Add(b);
        listNode.Add(c);

        int i = 10;
        listNode.Sort();
        i = 20;

        Vector2Int dest = new Vector2Int(2, 5);
    }

    void PrintList(List<int> list)
    {
        string str = "";
        foreach (int item in list)
        {
            str += $"{item} ->";
        }
        Debug.Log(str);
    }
  
    protected override void Test1(InputAction.CallbackContext context)
    {
        List<Vector2Int> path = AStar.PathFind(map, start, end);
        pathLine.DrawPath(map, path);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        list.Sort();
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        
    }
 
}
