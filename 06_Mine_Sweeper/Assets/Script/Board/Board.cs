using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private int width = 16;
    private int height = 16;
    private int mineCount = 10;

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
        
    }
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
        //주변 셀의 aroundMineCount를 1씩 증가시킨다.

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
            int randomIndex = source[UnityEngine.Random.Range(0, source.Length - i)];//(0, source.Length - i) i를 빼주지 않으면 균일한 확률이 나오지 않는다
            int lastIndex = loopCount - i;

            (source[lastIndex], source[randomIndex]) = (source[randomIndex], source[lastIndex]);//스왑 
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
    //    //source의 순서 섞기
    //}
}
//셔플함수 완성
//sell 의 SetMine 함수
