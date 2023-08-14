using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    const int ID_NOT_VALID = -1;
    int id = ID_NOT_VALID;

    public int ID
    {
        get => id;
        set
        {
            if (id == ID_NOT_VALID)
            {
                id = value;
            }
        }
    }

    SpriteRenderer cover;
    SpriteRenderer inside;

    enum CellMarkState
    {
        None = 0,
        Flag,
        Question
    }
    CellMarkState markState = CellMarkState.None;

    int aroundMineCount = 0;
    bool hasMine = false;
    bool isOpen = false;

    Board parentBoard = null;
    public Board Board
    {
        get => parentBoard;
        set
        {
            if (parentBoard == null)
            {
                parentBoard = value;
            }
        }
    }

    public void ResetData()
    {
        markState = CellMarkState.None;
        isOpen = false;
        hasMine = false;
        aroundMineCount = 0;

        cover.gameObject.SetActive(true);// 다시 닫는 함수는 없음
    }
}
