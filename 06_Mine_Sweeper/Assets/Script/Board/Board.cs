using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private int width = 16;
    private int height = 16;
    private int mineCount = 10;

    const float distance = 1.0f;//�� �� ���� ����

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
        
    }
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

                cells[cell.ID] = cell; //�迭�� ����
                cellObj.name = $"Cell_{cell.ID}_({x}, {y})";
            }
        }
        ResetBoard();
    }

    private void MineSet(int id)//Ư�� ���� ���ڰ� ��ġ�Ǿ��� �� ��������Ʈ ��ȣ�� �޾� ó���� �Լ� 
    {
        //�� ��ġ ã�� 
        //��ġ �ֺ� ���� ã�´�
        //�ֺ� ���� aroundMineCount�� 1�� ������Ų��.

        Cell cell = cells[id];

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
        shuffle(ids);
        for (int i = 0; i < mineCount; i++)
        {
            cells[ids[i]].SetMine();
        }
    }
    public void TestResetBoard()
    {
        ResetBoard();
    }
    public void shuffle(int[] source)
    {
        int loopCount = source.Length - 1;
        for (int i = 0; i < loopCount; i++)
        {
            int randomIndex = source[UnityEngine.Random.Range(0, source.Length - i)];//(0, source.Length - i) i�� ������ ������ ������ Ȯ���� ������ �ʴ´�
            int lastIndex = loopCount - i;

            (source[lastIndex], source[randomIndex]) = (source[randomIndex], source[lastIndex]);//���� 
        }
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
    //    //source�� ���� ����
    //}
}
//�����Լ� �ϼ�
//sell �� SetMine �Լ�
