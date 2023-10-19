using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CellVisualizer : MonoBehaviour
{
    GameObject[] walls;
    /// <summary>
    /// 바닥 한 변의 사이즈
    /// </summary>
    public const int CellSize = 5;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        walls = new GameObject[child.childCount];
        for (int i = 0; i < child.childCount; i++)
        {
            walls[i] = child.GetChild(i).gameObject;
        }
    }
    public void RefreshWall(int data)
    {
        //data 에 자릿수 별로 비트가1 이면 경로가 있다(열려있다.) 0이면 막혀있다. 
        for (int i = 0; i < walls.Length; i++)
        {
            int mask = 1 << i;
            walls[i].gameObject.SetActive((data & mask) == 0);
        }
        //if ((data & 1) != 0)
        //    walls[0].gameObject.SetActive(false);555
        //else
        //    walls[0].gameObject.SetActive(true);
        //if ((data & 2) != 0)
        //    walls[1].gameObject.SetActive(false);
        //else
        //    walls[1].gameObject.SetActive(true);
        //if ((data & 4) != 0)
        //    walls[2].gameObject.SetActive(false);
        //else
        //    walls[2].gameObject.SetActive(true);
        //if ((data & 8) != 0)
        //    walls[3].gameObject.SetActive(false);
        //else
        //    walls[3].gameObject.SetActive(true);
    }

}
