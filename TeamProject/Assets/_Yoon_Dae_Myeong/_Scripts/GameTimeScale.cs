using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeScale : MonoBehaviour
{
    void Start ( )
    {
        //3.5초후에 게임 속도 조절
        Invoke ( "ChangeTimeScale" , 3.5f );
    }

    void ChangeTimeScale ( )
    {
        Time . timeScale = 0.3f; // 게임 속도를 0.3배속으로 설정합니다.
    }
}
