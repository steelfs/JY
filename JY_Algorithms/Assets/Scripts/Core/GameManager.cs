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
    public GameObject playerPrefab;

    public bool IsTestMode = true;

    Player player;
    Pools pools;
    InputBox_Test inputBox;
    MazeVisualizer_Test visualizer_Test;
    MazeVisualizer mazeVisualizer;
    Kruskal kruskal;
    QuestionPanel questionPanel;
    ToolBox toolBox;
    public static InputBox_Test InputBox => Inst.inputBox;
    public static Pools Pools => Inst.pools;
    public static MazeVisualizer_Test Visualizer_Test => Inst.visualizer_Test;
    public static MazeVisualizer Visualizer => Inst.mazeVisualizer;
    public static Player Player => Inst.player;
    public static Kruskal Kruskal => Inst.kruskal;
    public static QuestionPanel QuestionPanel => Inst.questionPanel;
    public static ToolBox ToolBox => Inst.toolBox;
    private void Awake()
    {
        toolBox = FindAnyObjectByType<ToolBox>();
        questionPanel = FindAnyObjectByType<QuestionPanel>();
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

    public int itemCount = 5;
    const int MaxItemCount = 7;
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
                    toolBox.Open();
                    break;
                case GameState.Playing:
                    mazeVisualizer.PlayerArrived(PlayerType.Player);
                    Player.InputState = InputState.Player;
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
        yield return StartCoroutine(ItemSpawn_Coroutine());

        GameState = GameState.Playing;
    }
    IEnumerator PlayerSpawn_Coroutine()
    {
        Vector2Int[] startingPoinst = Util.Get_StartingPoints();//배열의 순서를 섞어서 리턴하기 때문에 첫번째 것을 그냥 꺼내쓰면 랜덤
        Vector2Int grid = startingPoinst[0];//스타팅 포인트 네 곳 중 랜덤한 곳 선택
        Vector3 spawnPos = Util.GridToWorld(grid);//그리드좌표를 월드좌표로 변환
        spawnPos.y += 2;
        Pools.GetObject(PoolObjectType.SpawnEffect, spawnPos);// 소환. 풀에서 꺼내기
        yield return new WaitForSeconds(0.5f);
        spawnPos.y -= 2;
        Vector3 rotation = Util.GetRandomRotation(grid.x, grid.y);// 벽이 없는 방향중 랜덤한 방향으로 회전
        GameObject player_ = Instantiate(playerPrefab, spawnPos, Quaternion.Euler(rotation));
        this.player = player_.GetComponent<Player>();
        player.InputState = InputState.None;

        yield return new WaitForSeconds(1.0f);
    }
    IEnumerator ItemSpawn_Coroutine()
    {
        List<Vector2Int> spawnPositions = new List<Vector2Int>(itemCount);

        while(spawnPositions.Count < this.itemCount)
        {
            Vector2Int gridPos = Util.GetRandomGrid();//보드의 랜덤한 좌표 
            bool nearBy_Player = IsNearBy_Player(gridPos);
            bool nearBy_Others = IsNearBy_Another_Item(spawnPositions, gridPos);
            if (nearBy_Player || nearBy_Others)//or 플레이어가 가까이 있거나 다른 아이템이 가까이 있을 경우  스킵.
            {
                continue;
            }
            spawnPositions.Add(gridPos);
        }
       
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            Vector2Int spawnGridPos = spawnPositions[i];
            Vector3 position = Util.GridToWorld(spawnGridPos);
            position.y += 2;
            Pools.GetObject(PoolObjectType.SpawnEffect, position);
            yield return new WaitForSeconds(0.5f);
            position.y -= 1;
            Pools.GetObject(PoolObjectType.Item, position);
            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// 아이템이 생성될 때 다른 아이템이 특정반경 내에 있는지 확인하는 함수 
    /// </summary>
    /// <param name="spawnPositions"></param>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    bool IsNearBy_Another_Item(List<Vector2Int> spawnPositions, Vector2Int newPosition)
    {
        bool result = false;
        if (spawnPositions.Count < 1) 
        {
            return false;
        }
        if (itemCount > MaxItemCount)
            itemCount = Mathf.Clamp(MaxItemCount, 3, 7);

        int distanceMin = Mathf.Max(1, 7 / Mathf.RoundToInt(Mathf.Sqrt(itemCount)));
        foreach (Vector2Int saved_Position in spawnPositions)
        {
            if (Util.IsNeighbor(newPosition, saved_Position, distanceMin))
            {
                result = true; break;
            }
        }
        return result;
    }
    /// <summary>
    /// // 오브젝트를 생성할 때 플레이어가 특정 반경 안에 있는지 확인하는 함수
    /// </summary>
    /// <param name="spawnGridPosition"></param>
    /// <returns></returns>
    bool IsNearBy_Player(Vector2Int spawnGridPosition)
    {
        bool result = false;
        Vector2Int playerGridPos = Util.WorldToGrid(player.transform.position);
        int diffX = playerGridPos.x - spawnGridPosition.x;
        int diffY = playerGridPos.y - spawnGridPosition.y;
        if ((diffX < 3 && diffX > -3) && (diffY < 3 && diffY > -3))
        {
            result = true;
        }
        return result;
    }
    public void OpenQuestionPanel()//UI 패널 오픈
    {
        Player.InputState = InputState.UI;
        QuestionPanel.Question_Type = QuestionType.Free_Input;
        QuestionPanel.Open();
        // 일정 확률로 Free_Input or SelectAnswer
    }
    public void CloseQuestionPanel() 
    {
        QuestionPanel.Close();
        Player.InputState = InputState.Player;
    }

    
}
