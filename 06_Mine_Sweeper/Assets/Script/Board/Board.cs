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
    Cell[] cells;

    public Sprite[] openCellImage;
    public Sprite[] closeCellImage;

    PlayerInputAction action;
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
                cell.ID = x + y * width;// y�� �߰�
                cellObj.transform.localPosition = new Vector3(x * distance, -y * distance, 0);

                cells[cell.ID] = cell; //�迭�� ����
                cellObj.name = $"Cell_{cell.ID}_({x}, {y})";
            }
        }
        ResetBoard();
    }

    private void ResetBoard()// ���忡 �����ϴ� ��� ���� �����͸� �����ϰ� ���ڸ� ���� ��ġ�ϴ� �Լ� (���� �� ���ۿ�)
    {
        //���� ������ �ʱ�ȭ
        // ���忡 MineCount��ŭ ��ġ�ϱ�
    }

}
