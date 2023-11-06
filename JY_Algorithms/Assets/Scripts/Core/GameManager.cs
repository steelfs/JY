using System;
using UnityEngine;
public class GameManager : Singleton<GameManager>
{
    public bool IsTestMode = true;

    Pools pools;
    InputBox inputBox;
    MazeVisualizer visualizer;
    public static InputBox InputBox => Inst.inputBox;
    public static Pools Pools => Inst.pools;
    public static MazeVisualizer Visualizer => Inst.visualizer;


    private void Awake()
    {
        visualizer = FindAnyObjectByType<MazeVisualizer>();
        inputBox = FindAnyObjectByType<InputBox>();
        pools = FindAnyObjectByType<Pools>();
    }
}
