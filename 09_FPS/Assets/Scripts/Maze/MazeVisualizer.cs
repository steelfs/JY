using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    public GameObject cellPrefab;

    Vector3[] enemySpawnPos = null;
    public Player player;

    private void Start()
    {
        enemySpawnPos = new Vector3[transform.childCount];
        for (int i = 0; i < enemySpawnPos.Length; i++)
        {
            Transform child = transform.GetChild(i);
            enemySpawnPos[i] = new Vector3(child.position.x + 2.5f, 1, child.position.z - 2.5f);
        }
    }

//    public Vector3 GetRandomSpawnPos()
//    {
//        Vector3 result = enemySpawnPos[Random.Range(0, enemySpawnPos.Length)];
//        Vector3 distance = new();
//        while (distance.x < 10 && distance.z < 10)
//        {
//            result = enemySpawnPos[Random.Range(0, enemySpawnPos.Length)];
//w            distance = result - player.transform.position;
//        }
      
//        return result;
//    }
    public void Draw(Cell[] data)
    {
        foreach (Cell cell in data)
        {
            GameObject obj = Instantiate(cellPrefab, transform);
            obj.transform.Translate(cell.X * CellVisualizer.CellSize, 0, -cell.Y * CellVisualizer.CellSize);
            obj.gameObject.name = $"Cell_({cell.X},{cell.Y})";

            CellVisualizer displayer = obj.GetComponent<CellVisualizer>();
            displayer.RefreshWall(cell.Path);
        }
    }

    public void Clear()
    {
        while(transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }


}
