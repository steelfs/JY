using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_18_Exit : TestBase
{
    float cellSize = CellVisualizer.CellSize;
    public GameObject playerPrefab;
    GameObject[] cells;
    protected override void Test1(InputAction.CallbackContext context)
    {
        int x = Random.Range(0, 20);
        int y = Random.Range(0, 20);


        Vector3 pos = new(x * cellSize, 0, -y * cellSize);
        pos.x += 2.5f;
        pos.z -= 2.5f;
        GameObject obj = Instantiate(playerPrefab, pos, Quaternion.identity);
    }
   
}
