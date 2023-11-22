using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState// 임시
{
    None,
    Start,
    Playing,
    End
}
public enum CurrentScene
{
    Title,
    Lobby,
    Field
}
public enum TotalRank
{
    Silver,
    Gold,
    Sapphire,
    Platinum
}
public enum Detail_Rank
{
    Silver_01,
    Silver_02,
    Silver_03,
    Silver_04,
    Gold_01,
    Gold_02,
    Gold_03,
    Gold_04,
}
public enum Subject
{
    Eng,
    Math,
    Coding,
    History,
    Common,
    Japanese
}
public class GameManager : Singleton<GameManager>
{
    int[] quizCounts = new int[] { 800, 800, 800, 800, 800,1000 };
    public int GetTotalQuizCount()
    {
        int result = 0;
        foreach(int count in quizCounts)
        {
            result += count;
        }
        return result;
    }
    public int[] QuizCounts => quizCounts;
    public GameObject[] playerPrefabs;
    GameObject[] spawnedPrefabs;
    public GameObject playerPrefab;
    int activeOrder = 0;
    int ActiveOrder
    {
        get => activeOrder;
        set
        {
            activeOrder = value;
            foreach(GameObject prefab in spawnedPrefabs)
            {
                prefab.SetActive(false);
            }
            spawnedPrefabs[activeOrder].SetActive(true);
        }
    }

    public bool IsTestMode = true;


    public ScrollView scrollView;
    TitlePanel titlePanel;

    PlayerData playerData = null;
    Player player;
    Pools pools;
    InputBox_Test inputBox;
    MazeVisualizer_Test visualizer_Test;
    MazeVisualizer mazeVisualizer;
    Kruskal kruskal;
    QuizPanel quizPanel;
    ToolBox toolBox;
    QuizData_English quizData_English;
    public static InputBox_Test InputBox => Inst.inputBox;
    public static Pools Pools => Inst.pools;
    public static MazeVisualizer_Test Visualizer_Test => Inst.visualizer_Test;
    public static MazeVisualizer Visualizer => Inst.mazeVisualizer;
    public static Player Player => Inst.player;
    public static Kruskal Kruskal => Inst.kruskal;
    public static QuizPanel QuizPanel => Inst.quizPanel;
    public static QuizData_English QuizData_English => Inst.quizData_English;
    public static ToolBox ToolBox => Inst.toolBox;

    public Action<PlayerData> on_UpdateScorePanel;

    CurrentScene currentScene = CurrentScene.Title;
    public CurrentScene CurrentScene
    {
        get => currentScene;
        set
        {
            currentScene = value;
            switch (currentScene)
            {
                case CurrentScene.Title:
                    if (SceneManager.GetActiveScene().name != "Title")
                    {
                        SceneManager.LoadScene("Title");
                    }
                        titlePanel.Open();
                    break;
                case CurrentScene.Lobby://newGame이 아닐 경우 playerData의 EditData함수 실행 후 Lobby로 상태 변경

                    StartCoroutine(LoadLobbyScene());
                    break;
                case CurrentScene.Field:
                    StartCoroutine(LoadFieldScene());
                    break;
            }
        }
    }
    IEnumerator LoadLobbyScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lobby");

        // 씬이 로드될 때까지 기다림
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 씬 로딩이 완료된 후 실행할 코드
        // 예: 스코어 패널 업데이트
        if (playerData == null)
        {
            playerData = new PlayerData();
        }
        else
        {
            playerData.InitData(playerData.SubjectScores);
        }

        if (scrollView == null)//newGame선택시
        {
            scrollView = FindAnyObjectByType<ScrollView>();
        }
        scrollView.UpdateScorePanel(playerData);
    }
    IEnumerator LoadFieldScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Field");

        // 씬이 로드될 때까지 기다림
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Init_FieldScene();
    }
    public void NewGame() { CurrentScene = CurrentScene.Lobby; }
    public void LoadGame()
    {
        if (playerData == null)
        {
            playerData = new();// 임시. 이곳에서 JsonSave파일을 불러와야한다
        }
        playerData.EditData(50);
        CurrentScene = CurrentScene.Lobby;
    }

    protected override void Awake()
    {
        base.Awake();
        titlePanel = FindAnyObjectByType<TitlePanel>();
        CurrentScene = CurrentScene.Title;
        spawnedPrefabs = new GameObject[playerPrefabs.Length];
    }

    /// <summary>
    /// 케릭터 선택시 실행될 함수
    /// 인스펙터의 NewGame버튼에서 호출
    /// </summary>
    public void SpawnPlayerPrefabs()
    {
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            GameObject obj = Instantiate(playerPrefabs[i], Vector3.zero, Quaternion.Euler(0, 180, 0));
            obj.AddComponent<ChoosePanel_Animal>();
            spawnedPrefabs[i] = obj;
            obj.SetActive(false);
        }
        spawnedPrefabs[2].transform.position = Vector3.up * 6;//Mole 물개 프리팹
        spawnedPrefabs[0].SetActive(true);
    }
       
    /// <summary>
    /// 케릭터를 바꾸는 함수 Increase = 오른쪽, Decrease = 왼쪽
    /// </summary>
    public void IncreaseActiveOrder() { ActiveOrder = (activeOrder + 1) % spawnedPrefabs.Length; }
    public void DecreaseActiveOrder() 
    {
        if (activeOrder < 1)
        {
            ActiveOrder = spawnedPrefabs.Length - 1;
        }
        else
        {
            ActiveOrder = (activeOrder - 1) % spawnedPrefabs.Length; 
        }
    }

    private void Init_FieldScene()
    {
        toolBox = FindAnyObjectByType<ToolBox>();
        quizPanel = FindAnyObjectByType<QuizPanel>();
        visualizer_Test = FindAnyObjectByType<MazeVisualizer_Test>();
        mazeVisualizer = FindAnyObjectByType<MazeVisualizer>();
        inputBox = FindAnyObjectByType<InputBox_Test>();
        pools = FindAnyObjectByType<Pools>();

        if (!IsTestMode)
        {
            Visualizer.MazeType = MazeType.kruskal;
            Visualizer.MakeBoard(10, 10);
            kruskal = Visualizer.Kruskal;
            kruskal.on_DoneWithMakeMaze += () => StartCoroutine(WaitCoroutine());
            Visualizer.MakeMaze();
        }
        quizData_English = new();
        quizData_English.InitQuizData();
    }


    IEnumerator WaitCoroutine()//MazeGenerator에서 미로생성 완료 후 델리게이트로 호출
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
                    toolBox.Open();//오른쪽 툴박스 보여주기
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
        this.player = player_.AddComponent<Player>();
        player.AddComponent<Rigidbody>();
        player.AddComponent<BoxCollider>();
        player.GetComponents();
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
        QuizPanel.Question_Type = QuestionType.Select_Answer;//이 부분은 시작 전 드롭다운 메뉴로 FreeInput or Select Answer 둘 중 선택할 수 있게 해야함
        QuizPanel.on_QuizPopUp();
        // 일정 확률로 Free_Input or SelectAnswer
    }
    public void CloseQuestionPanel() 
    {
        QuizPanel.Close();
        Player.InputState = InputState.Player;
    }

    
}
