using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    public int width = 16;
    public int height = 16;
    public int mineCount = 10;

    const float distance = 1.0f;//셀 한 변의 길이

    public GameObject cellPrefab;
    public Cell[] cells;

    public Sprite[] openCellImage;
    public Sprite[] closeCellImage;

    PlayerInputAction action;

    public Sprite this[OpenCellType type] => openCellImage[(int)type];
    public Sprite this[CloseCellType type] => closeCellImage[(int)type];

    private void Awake()
    {
        action = new PlayerInputAction();
    }
    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.LClick.performed += On_LeftPress;
        action.Player.RClick.performed += On_RightPress;
    }



    private void OnDisable()
    {
        action.Player.LClick.performed -= On_LeftPress;
        action.Player.Disable();
    }
 

    //protected override void LeftClick(InputAction.CallbackContext context)
    //{
    //    Vector2 mousePos = Mouse.current.position.ReadValue();
    //    Debug.Log(mousePos);
    //    Vector2 fixedPos = Camera.main.ScreenToWorldPoint(mousePos);
    //    Debug.Log(fixedPos);

    //    Vector2Int fixedMousePos = Vector2Int.zero;
    //    fixedMousePos.x = (int)fixedPos.x;
    //    fixedMousePos.y = (int)fixedPos.y;

    //    if (IsValidGrid(fixedMousePos))
    //    {
    //        Debug.Log(fixedMousePos);
    //    }
    //   // if (IsValidGrid((Vector2Int)fixedPos))
    //}
    public void Initialize(int newWidth, int newHeight, int newMineCount)
    {
        width = newWidth;
        height = newHeight;
        mineCount = newMineCount;

        if (cells != null)// 이미 셀들이 만들어져 있다면 
        {
            foreach (var cell in cells)
            {
                Destroy(cell.gameObject);
            }
            cells = null;
        }

        //Vector2 origin = Vector2.zero;
        //float offsetX = 1.0f;
        //float offsetY = 1.0f;
        cells = new Cell[width * height];

        for (int y = 0; y < height; y++) //셀 하나씩 찍어내기
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cellObj = Instantiate(cellPrefab, transform);
                Cell cell = cellObj.GetComponent<Cell>();
                cell.Board = this;

                cell.ID = x + y * width;// y값 추가
                cellObj.transform.localPosition = new Vector3(x * distance, -y * distance, 0);
                cell.onMineSet += MineSet;

                cells[cell.ID] = cell; //배열에 저장
                cellObj.name = $"Cell_{cell.ID}_({x}, {y})";
            }
        }
        ResetBoard();
    }

    private void MineSet(int id)//특정 셀에 지뢰가 설치되었을 때 델리게이트 실호를 받아 처리할 함수 
    {
        //셀 위치 찾기 
        //위치 주변 셀을 찾는다

       // List<Cell> neighbors = new List<Cell>(8);
        Vector2Int grid = IndexToGrid(id);
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                int index = GridToIndex(x + grid.x, y + grid.y);
                if (index != Cell.ID_NOT_VALID && !((x==0) && (y == 0))) // 인덱스가 Valid하고 (0, 0)이 아닌경우 처리
                {
                    Cell cell = cells[index];
                    cell.IncreaseAroundMineCount();
                   // neighbors.Add(cells[index]);
                }
            }
        }

        //주변 셀의 aroundMineCount를 1씩 증가시킨다.

       
    }
    private Vector2Int IndexToGrid(int index)
    {
        return new Vector2Int(index % width, index / width);
    }
    private int GridToIndex(int x, int y)
    {
        int result = Cell.ID_NOT_VALID;
        if (IsValidGrid(x, y))
            result = x + y * width;

        return result;
    }
    private bool IsValidGrid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
    private bool IsValidGrid(Vector2Int grid)
    {
        return IsValidGrid(grid.x, grid.y);
    }


    private void ResetBoard()// 보드에 존재하는 모든 셀의 데이터를 리셋하고 지뢰를 새로 배치하는 함수 (게임 재 시작용)
    {
        //셀의 데이터 초기화
        foreach(var cell in cells)
        {
            cell.ResetData();
            //지뢰를 그냥 랜덤으로 배치하면 중복되는 문제가 있다.

        }
        // 보드에 MineCount만큼 배치하기
        int[] ids = new int[cells.Length];
        for (int i = 0; i < cells.Length; i++)
        {
            ids[i] = i;
        }
        Shuffle(ids);
        for (int i = 0; i < mineCount; i++)
        {
            cells[ids[i]].SetMine();
        }
    }

    private void On_LeftPress(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue(); //마우스위치 받아오기
        Vector2Int grid = ScreenToGrid(screenPos); //그리드좌표로 변경

        int index = GridToIndex(grid.x, grid.y);

        if (index != Cell.ID_NOT_VALID)
        {
            Debug.Log(index);
            Cell target = cells[index];
            target.CellLeftPressed();// 델리게이트를 사용하는게 유용한 경우는 타이밍과 결합도측면에서 고민을 해봐야한다. 델리게이트를 사용하면 결합도가 낮아질 수는 있다.
        }
    }
    private void On_RightPress(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue(); //마우스위치 받아오기
        Vector2Int grid = ScreenToGrid(screenPos); //그리드좌표로 변경

        int index = GridToIndex(grid.x, grid.y);

        if (index != Cell.ID_NOT_VALID)
        {
            Debug.Log(index);
            Cell target = cells[index];
            target.CellRightPressed();// 델리게이트를 사용하는게 유용한 경우는 타이밍과 결합도측면에서 고민을 해봐야한다. 델리게이트를 사용하면 결합도가 낮아질 수는 있다.
        }
    }

    private Vector2Int ScreenToGrid(Vector2 screenPos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);// 스크린좌표를 월드좌표로
        Vector2 diff = worldPos - (Vector2)transform.position; // 보드의 피봇에서 얼마나 떨어져있는지 확인

        return new Vector2Int(Mathf.FloorToInt(diff.x / distance), Mathf.FloorToInt(-diff.y / distance)); // 차이를 한칸당 간격으로 나눠서 몇번째 그리드인지 확인 
    }


    //--------------------------------------------------------------------------------------------
    public void TestResetBoard()
    {
        ResetBoard();
    }
    public void Shuffle(int[] source)
    {
        int loofCount = source.Length - 1;
        for (int i = 0; i < loofCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, source.Length - i);
            int lastIndex = loofCount - i;

            (source[randomIndex], source[lastIndex]) = (source[lastIndex], source[randomIndex]);
        }
    }
    public void Test_ResetBoard()
    {
        ResetBoard();
    }

    public void Test_Shuffle()
    {
        int[,] result = new int[10, 10];
        for (int i = 0; i < 1000000; i++)
        {
            int[] source = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Shuffle(source);
            for (int j = 0; j < source.Length; j++)
            {
                result[source[j], j]++;
            }
        }

        string num = "";
        for (int x = 0; x < 10; x++)
        {
            num += $"숫자{x} : ";
            for (int y = 0; y < 10; y++)
            {
                num += $" {result[x, y]}";
            }
            num += "\n";
        }
        Debug.Log(num);
    }
    //public void shuffleMine(int[] source)
    //{
    //    for (int i = 0; i < source.Length; i++)
    //    {
    //        int randomIndex = source[UnityEngine.Random.Range(0, source.Length - 1)];
    //        int tempValue = source[randomIndex];
    //        int origin = source[i];
    //        source[i] = tempValue;
    //        source[randomIndex] = origin;
    //    }
    //    //source의 순서 섞기
    //}
}
//셔플함수 완성
//sell 의 SetMine 함수
