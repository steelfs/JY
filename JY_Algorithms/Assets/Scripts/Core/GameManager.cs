using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState// �ӽ�
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
                case CurrentScene.Lobby://newGame�� �ƴ� ��� playerData�� EditData�Լ� ���� �� Lobby�� ���� ����

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

        // ���� �ε�� ������ ��ٸ�
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // �� �ε��� �Ϸ�� �� ������ �ڵ�
        // ��: ���ھ� �г� ������Ʈ
        if (playerData == null)
        {
            playerData = new PlayerData();
        }
        else
        {
            playerData.InitData(playerData.SubjectScores);
        }

        if (scrollView == null)//newGame���ý�
        {
            scrollView = FindAnyObjectByType<ScrollView>();
        }
        scrollView.UpdateScorePanel(playerData);
    }
    IEnumerator LoadFieldScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Field");

        // ���� �ε�� ������ ��ٸ�
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
            playerData = new();// �ӽ�. �̰����� JsonSave������ �ҷ��;��Ѵ�
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
    /// �ɸ��� ���ý� ����� �Լ�
    /// �ν������� NewGame��ư���� ȣ��
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
        spawnedPrefabs[2].transform.position = Vector3.up * 6;//Mole ���� ������
        spawnedPrefabs[0].SetActive(true);
    }
       
    /// <summary>
    /// �ɸ��͸� �ٲٴ� �Լ� Increase = ������, Decrease = ����
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


    IEnumerator WaitCoroutine()//MazeGenerator���� �̷λ��� �Ϸ� �� ��������Ʈ�� ȣ��
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
                    toolBox.Open();//������ ���ڽ� �����ֱ�
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
        Vector2Int[] startingPoinst = Util.Get_StartingPoints();//�迭�� ������ ��� �����ϱ� ������ ù��° ���� �׳� �������� ����
        Vector2Int grid = startingPoinst[0];//��Ÿ�� ����Ʈ �� �� �� ������ �� ����
        Vector3 spawnPos = Util.GridToWorld(grid);//�׸�����ǥ�� ������ǥ�� ��ȯ
        spawnPos.y += 2;
        Pools.GetObject(PoolObjectType.SpawnEffect, spawnPos);// ��ȯ. Ǯ���� ������
        yield return new WaitForSeconds(0.5f);
        spawnPos.y -= 2;
        Vector3 rotation = Util.GetRandomRotation(grid.x, grid.y);// ���� ���� ������ ������ �������� ȸ��
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
            Vector2Int gridPos = Util.GetRandomGrid();//������ ������ ��ǥ 
            bool nearBy_Player = IsNearBy_Player(gridPos);
            bool nearBy_Others = IsNearBy_Another_Item(spawnPositions, gridPos);
            if (nearBy_Player || nearBy_Others)//or �÷��̾ ������ �ְų� �ٸ� �������� ������ ���� ���  ��ŵ.
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
    /// �������� ������ �� �ٸ� �������� Ư���ݰ� ���� �ִ��� Ȯ���ϴ� �Լ� 
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
    /// // ������Ʈ�� ������ �� �÷��̾ Ư�� �ݰ� �ȿ� �ִ��� Ȯ���ϴ� �Լ�
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
    public void OpenQuestionPanel()//UI �г� ����
    {
        Player.InputState = InputState.UI;
        QuizPanel.Question_Type = QuestionType.Select_Answer;//�� �κ��� ���� �� ��Ӵٿ� �޴��� FreeInput or Select Answer �� �� ������ �� �ְ� �ؾ���
        QuizPanel.on_QuizPopUp();
        // ���� Ȯ���� Free_Input or SelectAnswer
    }
    public void CloseQuestionPanel() 
    {
        QuizPanel.Close();
        Player.InputState = InputState.Player;
    }

    
}
