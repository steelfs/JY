using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    const int ID_NOT_VALID = -1;
    int id = ID_NOT_VALID;

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

    Board parentBoard;
}
