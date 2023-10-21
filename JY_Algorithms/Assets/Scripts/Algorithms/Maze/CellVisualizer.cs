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
        //ex 2�� 2�� and�� �ϸ� 1�� �ƴ϶� 2�� ������ ������ 0�� �ƴ� �������� üũ�� �ؾ��Ѵ�. != 1 �� üũ�� �ϸ� ��Ʈ�� ������ �Ǿ��־ false�� ��ȯ�Ѵ�.
    }
}
