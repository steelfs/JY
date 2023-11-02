using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    /// <summary>
    /// 플레이어
    /// </summary>
    public Player Player => player;
    MazeVisualizer mazeVisualizer;
    public static MazeVisualizer MazeVisualizer => Inst.mazeVisualizer;
    CinemachineVirtualCamera vcamera;
    /// <summary>
    /// 플레이어를 찍는 가상카메라
    /// </summary>
    public CinemachineVirtualCamera VCamera => vcamera;

    // 나중에 풀에서 가져오는 것으로 변경
    public GameObject bulletHolePrefab;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        mazeVisualizer = FindAnyObjectByType<MazeVisualizer>();
        player = FindAnyObjectByType<Player>();

        GameObject obj = GameObject.Find("PlayerFollowCamera");
        if(obj != null) 
        {
            vcamera = obj.GetComponent<CinemachineVirtualCamera>();
        }
    }
}
