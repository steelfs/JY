using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDisplayer : MonoBehaviour
{
    public GameObject cellPrefab;

    public void Draw(Cell[] data)
    {
        foreach (Cell cell in data)
        {
            GameObject obj = Instantiate(cellPrefab, transform);
            obj.transform.Translate(cell.X * CellDisplayer.CellSize, 0, -cell.Y * CellDisplayer.CellSize);
            obj.gameObject.name = $"Cell_{cell.X}_{cell.Y}";

            CellDisplayer displayer = obj.GetComponent<CellDisplayer>();
            displayer.RefreshWall(cell.Path);
        }
    }
    public void Clear()
    {
        while(transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.SetParent(null);// 생략시 무한루프 때는데 시간이 오래걸려서 child에서 제외되지 않음
            Destroy(child.gameObject);
        }
    }
}
