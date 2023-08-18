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

    int closeCellCount = 0;

    public bool IsBoardClear => closeCellCount == mineCount && GameManager.Inst.IsPlaying;//���尡 Ŭ���� �Ǿ����� Ȯ���ϴ� ������Ƽ //�ܺ�Ŭ������ ������ �߰��ϰԵǸ� ���յ��� �������� �������� �� �ִ�.
   // public bool IsBoardClear => closeCellCount == mineCount && !isBoardOver;
   // bool isBoardOver = false;//���尡

    const float distance = 1.0f;//�� �� ���� ����

    public GameObject cellPrefab;
    public Cell[] cells;
    List<Cell> cellWithMine;
    Cell currentCell = null;
    Cell CurrentCell
    {
        get => currentCell;
        set
        {
            if (currentCell != value)
            {
                currentCell?.RestoreCovers();//���� ���� �������·� �ǵ���
                currentCell = value;
                currentCell?.CellLeftPressed();// ���ο� �� ������
            }
        }
    }


    public Sprite[] openCellImage;
    public Sprite[] closeCellImage;

    public Action onBoardLeftPress;
    public Action onBoardLeftRelease;

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
        action.Player.LClick.canceled += On_LeftRelease;
        action.Player.RClick.performed += On_RightPress;
        action.Player.PointerMove.performed += On_PointerMove;
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

        if (cells != null)// �̹� ������ ������� �ִٸ� 
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

        GameManager gameManager = GameManager.Inst;
        for (int y = 0; y < height; y++) //�� �ϳ��� ����
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cellObj = Instantiate(cellPrefab, transform);
                Cell cell = cellObj.GetComponent<Cell>();
                cell.Board = this;

                cell.ID = x + y * width;// y�� �߰�
                cellObj.transform.localPosition = new Vector3(x * distance, -y * distance, 0);
                cell.onMineSet += MineSet;
                cell.onFlagUse += gameManager.DecreaseFlagCount;
                cell.onFlagReturn += gameManager.IncreaseFlagCount;
                cell.onCellOpen += () => closeCellCount--;
                cell.onAction += gameManager.FinishPlayerAction;
              //  cell.onExplosion += gameManager.GameOver;

               // cell.onExplosion += () => isBoardOver = true;
                   

                cells[cell.ID] = cell; //�迭�� ����
                cellObj.name = $"Cell_{cell.ID}_({x}, {y})";
            }
        }

        gameManager.onGameReady += ResetBoard;
        gameManager.onGameOver += OnGameOver;
        ResetBoard();

    }

    private void MineSet(int id)//Ư�� ���� ���ڰ� ��ġ�Ǿ��� �� ��������Ʈ ��ȣ�� �޾� ó���� �Լ� 
    {
        //�� ��ġ ã�� 
        //��ġ �ֺ� ���� ã�´�

       // List<Cell> neighbors = new List<Cell>(8);
        Vector2Int grid = IndexToGrid(id);
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                int index = GridToIndex(x + grid.x, y + grid.y);
                if (index != Cell.ID_NOT_VALID && !((x==0) && (y == 0))) // �ε����� Valid�ϰ� (0, 0)�� �ƴѰ�� ó��
                {
                    Cell cell = cells[index];
                    cell.IncreaseAroundMineCount();
                   // neighbors.Add(cells[index]);
                }
            }
        }
    }
    
    public List<Cell> GetNeighbors(int id)
    {
        List<Cell> result = new List<Cell>(8);
        Vector2Int grid = IndexToGrid(id);
        for (int y = -1; y< 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                int index = GridToIndex(x + grid.x, y + grid.y);
                if (index != Cell.ID_NOT_VALID && !(x==0 && y==0))
                {
                    result.Add(cells[index]);
                }
            }
        }

        return result;
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


    private void ResetBoard()// ���忡 �����ϴ� ��� ���� �����͸� �����ϰ� ���ڸ� ���� ��ġ�ϴ� �Լ� (���� �� ���ۿ�)
    {
        if (cellWithMine == null || cellWithMine.Count == 0)
        {
            cellWithMine = new List<Cell>(mineCount);
        }
        else
        {
            cellWithMine.Clear();
        }
        //���� ������ �ʱ�ȭ
        foreach(var cell in cells)
        {
            cell.ResetData();
            //���ڸ� �׳� �������� ��ġ�ϸ� �ߺ��Ǵ� ������ �ִ�.

        }
        // ���忡 MineCount��ŭ ��ġ�ϱ�
        int[] ids = new int[cells.Length];
        for (int i = 0; i < cells.Length; i++)
        {
            ids[i] = i;
        }
        Shuffle(ids);
        for (int i = 0; i < mineCount; i++)
        {
            cells[ids[i]].SetMine();
            cellWithMine.Add(cells[ids[i]]);
        }
        closeCellCount = cells.Length;//���� ���� ���� (����Ŭ���� ���� ���� ���� ���� == mineCount)
    }
    void OpenCellWithMine()
    {
        foreach (var cell in cellWithMine)
        {
            cell.Open();
        }
    }
    private void OnGameOver()
    {
        OpenCellWithMine();
        ShowMistake();
        //�߸�ã���� X ǥ��
        //��ã�� ������ Ŀ�� ����
    }
    public Action<int> onFixCount;
    void ShowMistake()
    {
        List<Cell> incorrectCells = new List<Cell>();
        foreach (var cell in cells)
        {
            if (!cell.HasMine && cell.IsFlaged)
            {
                incorrectCells.Add(cell);
            }
        }
        foreach (var cell in incorrectCells)
        {
            cell.SetFlagIncorrect();
        }
        onFixCount?.Invoke(incorrectCells.Count);
    }
    private void On_PointerMove(InputAction.CallbackContext context)
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 pos = context.ReadValue<Vector2>();
            Vector2Int grid = ScreenToGrid(pos);
            int index = GridToIndex(grid.x, grid.y);

            if (index != Cell.ID_NOT_VALID)
            {
                CurrentCell = cells[index];
            }
            else
            {
                CurrentCell = null;
            }
        }
  
    }
    private void On_LeftPress(InputAction.CallbackContext context)
    {
        if (GameManager.Inst.IsPlaying)
        {
            onBoardLeftPress?.Invoke();
        }
        Vector2 screenPos = Mouse.current.position.ReadValue(); //���콺��ġ �޾ƿ���
        Vector2Int grid = ScreenToGrid(screenPos); //�׸�����ǥ�� ����

        int index = GridToIndex(grid.x, grid.y);

        if (index != Cell.ID_NOT_VALID)
        {
            GameManager.Inst.GameStart();

            Cell target = cells[index];
            target.CellLeftPressed();// ��������Ʈ�� ����ϴ°� ������ ���� Ÿ�ְ̹� ���յ����鿡�� ����� �غ����Ѵ�. ��������Ʈ�� ����ϸ� ���յ��� ������ ���� �ִ�.
        }
    }
    private void On_LeftRelease(InputAction.CallbackContext _)
    {
        if (GameManager.Inst.IsPlaying)
        {
            onBoardLeftRelease?.Invoke();
        }
        Vector2 screenPos = Mouse.current.position.ReadValue(); //���콺��ġ �޾ƿ���
        Vector2Int grid = ScreenToGrid(screenPos); //�׸�����ǥ�� ����

        int index = GridToIndex(grid.x, grid.y);

        if (index != Cell.ID_NOT_VALID)
        {
            Cell target = cells[index];
            target.CellLeftRelease();// ��������Ʈ�� ����ϴ°� ������ ���� Ÿ�ְ̹� ���յ����鿡�� ����� �غ����Ѵ�. ��������Ʈ�� ����ϸ� ���յ��� ������ ���� �ִ�.
        }
    }
    private void On_RightPress(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue(); //���콺��ġ �޾ƿ���
        Vector2Int grid = ScreenToGrid(screenPos); //�׸�����ǥ�� ����

        int index = GridToIndex(grid.x, grid.y);

        if (index != Cell.ID_NOT_VALID)
        {
            GameManager.Inst.GameStart();

            Cell target = cells[index];
            target.CellRightPressed();// ��������Ʈ�� ����ϴ°� ������ ���� Ÿ�ְ̹� ���յ����鿡�� ����� �غ����Ѵ�. ��������Ʈ�� ����ϸ� ���յ��� ������ ���� �ִ�.
        }
    }

    private Vector2Int ScreenToGrid(Vector2 screenPos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);// ��ũ����ǥ�� ������ǥ��
        Vector2 diff = worldPos - (Vector2)transform.position; // ������ �Ǻ����� �󸶳� �������ִ��� Ȯ��

        return new Vector2Int(Mathf.FloorToInt(diff.x / distance), Mathf.FloorToInt(-diff.y / distance)); // ���̸� ��ĭ�� �������� ������ ���° �׸������� Ȯ�� 
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
            num += $"����{x} : ";
            for (int y = 0; y < 10; y++)
            {
                num += $" {result[x, y]}";
            }
            num += "\n";
        }
        Debug.Log(num);
    }

}

