using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public int itemCount = 5;
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
                    StartCoroutine(SpawnPlayerAndItems());
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
    IEnumerator SpawnPlayerAndItems()
    {
        yield return StartCoroutine(PlayerSpawn_Coroutine());
        yield return StartCoroutine(ItemSpawn_Coroutine(itemCount));
    }
    IEnumerator PlayerSpawn_Coroutine()
    {
        Vector3 spawnPos = Visualizer.GetRandomPos(out Vector3 rotation);
        spawnPos.y += 2;
        Pools.GetObject(PoolObjectType.SpawnEffect, spawnPos);
        yield return new WaitForSeconds(0.5f);
        spawnPos.y -= 2;
        GameObject player_ = Instantiate(playerPrefab, spawnPos, Quaternion.Euler(rotation));
        this.player = player_.GetComponent<Player>();
        yield return new WaitForSeconds(1.0f);
    }
    IEnumerator ItemSpawn_Coroutine(int itemCount)
    {
        List<Vector3> spawnPositions = new List<Vector3>(itemCount);

        for (int i = 0; i < itemCount; i++)
        {
            //UnityEngine.Random.InitState(i);
            Vector3 spawnPos = Visualizer.GetRandomPos(out _);
            bool contains = spawnPositions.Contains(spawnPos);
            bool IsNearBy = IsNearByPlayer(spawnPos);
            if (contains || IsNearBy)//or 플레이어와 가까울 경우
            {
                i--;
                continue;
            }
            spawnPositions.Add(spawnPos);
        }
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            Vector3 position = spawnPositions[i];
            position.y += 2;
            Pools.GetObject(PoolObjectType.SpawnEffect, position);
            yield return new WaitForSeconds(0.5f);
            position.y -= 1;
            Pools.GetObject(PoolObjectType.Item, position);
            yield return new WaitForSeconds(0.05f);
        }
    }
    bool IsNearByPlayer(Vector3 spawnPosition)
    {
        //float distanceSquared = Vector3.SqrMagnitude(player.transform.position - spawnPosition);
        //float maxDistanceSquared = 5 * 5 * MazeVisualizer_Test.CellSize * MazeVisualizer_Test.CellSize; // 5칸 거리의 제곱

        return false;
    }
    // 플레이어와 떨어진 위치
    // 위치가 겹치지 않게
    public GameObject playerPrefab;

    public bool IsTestMode = true;

    Player player;
    Pools pools;
    InputBox_Test inputBox;
    MazeVisualizer_Test visualizer_Test;
    MazeVisualizer mazeVisualizer;
    Kruskal kruskal;
    public static InputBox_Test InputBox => Inst.inputBox;
    public static Pools Pools => Inst.pools;
    public static MazeVisualizer_Test Visualizer_Test => Inst.visualizer_Test;
    public static MazeVisualizer Visualizer => Inst.mazeVisualizer;
    public static Player Player => Inst.player;


    private void Awake()
    {

        visualizer_Test = FindAnyObjectByType<MazeVisualizer_Test>();
        mazeVisualizer = FindAnyObjectByType<MazeVisualizer>();
        inputBox = FindAnyObjectByType<InputBox_Test>();
        pools = FindAnyObjectByType<Pools>();
    }
    private void Start()
    {
        if (!IsTestMode)
        {
            Visualizer.MazeType = MazeType.kruskal;
            Visualizer.MakeBoard(10, 10);
            kruskal = Visualizer.Kruskal;
            kruskal.on_DoneWithMakeMaze += () => StartCoroutine(WaitCoroutine());
            Visualizer.MakeMaze();
        }
    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(1);
        GameState = GameState.Start;
    }

}
