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
}
