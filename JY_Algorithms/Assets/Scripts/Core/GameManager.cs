using System;
using System.Collections;
using UnityEngine;

public enum GameState
{
    None,
    Start,
    Playing,
    End
}
public class GameManager : Singleton<GameManager>
{
    GameState gameState = GameState.None;
    GameState GameState
    {
        get => gameState;
        set
        {
            gameState = value;
            switch (gameState)
            {
                case GameState.Start:
                    Vector3 spawnPos = Visualizer.GetRandomPos(out Vector3 rotation);
                    Instantiate(playerPrefab, spawnPos, Quaternion.Euler(rotation));
                    break;
                case GameState.Playing:

                    break;
                case GameState.End:
                    break;
                default:
                    break;
            }
        }
    }

    public GameObject playerPrefab;

    public bool IsTestMode = true;

    Pools pools;
    InputBox_Test inputBox;
    MazeVisualizer_Test visualizer_Test;
    MazeVisualizer mazeVisualizer;
    Kruskal kruskal;
    public static InputBox_Test InputBox => Inst.inputBox;
    public static Pools Pools => Inst.pools;
    public static MazeVisualizer_Test Visualizer_Test => Inst.visualizer_Test;
    public static MazeVisualizer Visualizer => Inst.mazeVisualizer;


    private void Awake()
    {
        visualizer_Test = FindAnyObjectByType<MazeVisualizer_Test>();
        mazeVisualizer = FindAnyObjectByType<MazeVisualizer>();
        inputBox = FindAnyObjectByType<InputBox_Test>();
        pools = FindAnyObjectByType<Pools>();
    }
    private void Start()
    {
        Visualizer.MazeType = MazeType.kruskal;
        Visualizer.MakeBoard(10, 10);
        Visualizer.MakeMaze();
        kruskal = Visualizer.Kruskal;
        kruskal.on_DoneWithMakeMaze += () => StartCoroutine(WaitCoroutine());
    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(1);
        GameState = GameState.Start;
    }

}
