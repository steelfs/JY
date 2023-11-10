using System;
using UnityEngine;
public class GameManager : Singleton<GameManager>
{
    public bool IsTestMode = true;

    Pools pools;
    InputBox_Test inputBox;
    MazeVisualizer_Test visualizer;
    public static InputBox_Test InputBox => Inst.inputBox;
    public static Pools Pools => Inst.pools;
    public static MazeVisualizer_Test Visualizer => Inst.visualizer;


    private void Awake()
    {
        visualizer = FindAnyObjectByType<MazeVisualizer_Test>();
        inputBox = FindAnyObjectByType<InputBox_Test>();
        pools = FindAnyObjectByType<Pools>();
    }
}
