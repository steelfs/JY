using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellVisualizer : MonoBehaviour
{
    GameObject[] walls;
    private void Awake()
    {
        walls = new GameObject[transform.childCount - 1];
        for (int i = 1; i < transform.childCount; i++)
        {
            walls[i - 1] = transform.GetChild(i).gameObject;    
        }
    }
    public void RefreshWalls(int data)
    {
        for (int i = 0; i < walls.Length; i++)
        {
            int mask = 1 << i;
            walls[i].gameObject.SetActive(!((data & mask) != 0));
        }
        //ex 2와 2를 and를 하면 1이 아니라 2가 나오기 때문에 0이 아닌 조건으로 체크를 해야한다. != 1 로 체크를 하면 비트가 세팅이 되어있어도 false를 반환한다.
    }
}
