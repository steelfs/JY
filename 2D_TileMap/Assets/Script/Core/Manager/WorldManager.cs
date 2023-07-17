using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WorldManager : MonoBehaviour
{
    //서브맵의 가로 
    const int HeightCount = 3;
    const int WidthCount = 3;

    const float mapheightLength = 20.0f; //서브맵 하나의 가로길이
    const float mapwidthLength = 20.0f;

    readonly Vector2 worldOrigin = new Vector2(-mapwidthLength * WidthCount * 0.5f, - mapheightLength * HeightCount * 0.5f);//모든 월드맵의 원점 (좌측 아래)

    const string SceneNameBase = "SeemLess";//씬이름 조합용 기본 이름

    string[] sceneNames; //조합이 완료된 모든 씬의 이름 배열
    
    enum SceneLoadState : byte// 씬의 로딩 상태를 나타낼 enum
    {
        UnLoad = 0, //로딩이 안되어있음
        PendingUnLoad, // 로딩 해제 진행중
        PendingLoad,// 로딩 진행중
        Loaded//로딩 완료됨
            
    }

    private void Start()
    {
        Debug.Log(worldToGrid(new Vector3(41, 30, 0)));
        
    }

    SceneLoadState[] sceneLoadState;

    public bool IsUnLoadAll// 모든 씬이 언로드 되었음을 확인하기 위한 프로퍼티
    {
        get
        {
            bool result = true;
            foreach(SceneLoadState state in sceneLoadState)
            {
                if (state != SceneLoadState.UnLoad)
                {
                    result = false; //하나라도 UnLoad가 아니면 false
                    break;
                }
            }
            return result;
        }
    }

    List<int> loadWork = new List<int>(); //로딩을 시도할 목록
    List<int> loadWorkComplete = new List<int>(); // 로딩 시도가 완료된 목록

    List<int> unloadWork = new List<int>(); //로딩해제를 시도할 목록
    List<int> unloadWorkComplete = new List<int>(); // 로딩해제가 완료된 목록
    public void PreInitialize()//싱글톤이 만들어 질때 단 한번만 호출
    {
        sceneNames = new string[HeightCount * WidthCount ];  //배열크기 확보
        sceneLoadState = new SceneLoadState[HeightCount * WidthCount];

        for (int y = 0; y < HeightCount; y++)
        {
            for (int x = 0; x < WidthCount; x++)
            {
                int index = GetIndex(x, y);
                sceneNames[index] = $"{SceneNameBase}{x}_{y}"; //배열 채워넣기 
                sceneLoadState[index] = SceneLoadState.UnLoad;
            }
        }
    }
    public void Initialize()// 씬이 single모드로 로딩될때마다 호출되는 함수 
    {
        for (int i = 0; i < sceneLoadState.Length; i++)
        {
            sceneLoadState[i] = SceneLoadState.UnLoad; // 씬이 불러졌을 때 서브맵들의 로딩상태 초기화 
        }

        Player player = GameManager.Inst.Player;
        if (player != null)
        {
            player.onDie += (_, _) => //플레이어 죽었을 때 실행될 람다 함수 , 파라미터는 무시
            {
                for (int y = 0; y < HeightCount; y++)
                {
                    for (int x = 0; x < WidthCount; x++)
                    {
                        RequestAsyncSceneUnLoad(x, y);//모든 씬 로딩 해제 요청
                    }
                }
            };
            player.onMapMoved += (gridPos) =>// 플레이어가 맵을 이동했을 때 실행될 람다 함수 
            {
                RefreshScenes(gridPos.x, gridPos.y);//플레이어 주변 맵 갱신 요청
            };

            Vector2Int grid = worldToGrid(player.transform.position); // 플레이어가 있는 서브맵 그리드 가져오기
            RequestAsyncSceneLoad(grid.x, grid.y); // 플레이어가 위치한 맵을 최우선으로 로딩 요청
            RefreshScenes(grid.x, grid.y); //플레이어 주변 맵
        }
    }

    private void Update()
    {
        foreach (var index in loadWorkComplete)// 완료된 로드 작업은 로드워크에서 제거
        {
            loadWork.RemoveAll((x) => x == index);// loadWork에 있는 것들 중에서 x와 index가 같은 경우 removeAll 해라 지워라
        }
        loadWorkComplete.Clear();

        foreach(var index in loadWork)
        {
            AsyncSceneLoad(index);// loadWork에 있는 것들은 전부 비동기 로딩 시작
        }

        foreach (var index in unloadWorkComplete)
        {
            unloadWork.RemoveAll((x) => x == index);//  있는 것들 중에서 x와 index가 같은 경우 removeAll 해라 지워라
        }
        unloadWorkComplete.Clear();

        foreach (var index in unloadWork)
        {
            AsyncSceneUnLoad(index);// loadWork에 있는 것들은 전부 비동기 로딩 시작
        }
    }

    private void RequestAsyncSceneLoad(int x, int y)//씬을 비동기로 로딩할 것을 요청하는 함수 
    {
        int index = GetIndex(x, y);
        if (sceneLoadState[index] == SceneLoadState.UnLoad)
        {
            loadWork.Add(index);
        }
    }
    private void RequestAsyncSceneUnLoad(int x, int y)//씬을 비동기로 로딩 해제할 것을 요청하는 함수 
    {
        int index = GetIndex(x, y);
        if (sceneLoadState[index] == SceneLoadState.Loaded)
        {
            unloadWork.Add(index); //작업 리스트에 등록하고 
        }

        //서브맵에 있는 슬라임들을 찾아서 풀로 되돌리기
        Scene scene = SceneManager.GetSceneByName(sceneNames[index]);
        if (scene.isLoaded)
        {
            GameObject[] sceneRoots = scene.GetRootGameObjects();//해당 씬 안에 부모가 없는 모든 게임오브젝트들을 가져온다
            if (sceneRoots != null && sceneRoots.Length > 0)
            {
                Slime[] slimes = sceneRoots[0].GetComponentsInChildren<Slime>();//그중 첫번째 Grid
                foreach(Slime slime in slimes)
                {
                    slime.ReturnToPool();
                }
            }
        }
    }
    private void RefreshScenes(int gridX, int gridY)//지정된 좌표의 주변 맵은 로딩을 요청하고 그 외는 로딩 해제를 요청하는 함수 
    {
        int startX = Mathf.Max(0, gridX - 1);
        int endX = Mathf.Min(WidthCount, gridX + 2);// +2 가되는 이유는 for문에서  작다로 체크하기 위해
        int startY = Mathf.Max(0, gridY -1);
        int endY = Mathf.Min(HeightCount, gridY + 2);

        List<Vector2Int> open = new List<Vector2Int>(WidthCount * HeightCount);

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                RequestAsyncSceneLoad(x, y);
                open.Add(new Vector2Int(x, y));
            }
        }

        for (int y = 0; y < HeightCount; y++)
        {
            for (int x = 0; x < WidthCount; x++)
            {
                //contains = 값이 단순히 들어있는지 확인용
                //exist = 내가 설정한 조건에 맞는것이 있는지 확인하기 위한 것
                if (!open.Contains(new Vector2Int(x, y)))
                {
                    RequestAsyncSceneUnLoad(x, y);
                }
            }
        }
    }
    private void AsyncSceneLoad(int index) // 실제로 씬을 비동기로 로딩하는 함수 index = 해당 씬의 인덱스
    {
        if (sceneLoadState[index] == SceneLoadState.UnLoad) //파라미터로 받은 씬이 UnLoad상태라면 
        {
            sceneLoadState[index] = SceneLoadState.PendingLoad; //진행중이라고 표시

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[index], LoadSceneMode.Additive);//비동기 로딩 시작
            async.completed += (_) =>// 비동기 작업이 끝났을 때 실행되는 람다함수 추가
            {
                sceneLoadState[index] = SceneLoadState.Loaded;// 로드상태로 변경
                loadWorkComplete.Add(index);//로드 완료 목록에 추가
            };
        }
    }

    private void AsyncSceneUnLoad(int index)
    {
        if (sceneLoadState[index] == SceneLoadState.Loaded)//로딩완료상태일때만 진행
        {
            sceneLoadState[index] = SceneLoadState.PendingUnLoad; //언로드 진행중이라고 표시

            AsyncOperation async = SceneManager.UnloadSceneAsync(sceneNames[index]); //언로드 실행
            async.completed += (_) =>
            {
                sceneLoadState[index] = SceneLoadState.UnLoad;//언로드상태로 변경 
                unloadWorkComplete.Add(index);// 언로드 완료 목록에 추가
            };
        }
    }
   
    private int GetIndex(int x, int y)// z, y를(서브맵의 그리드 좌표) 파라미터로 주면 맵의 인덱스(배열에서 사용)로 바꿔주는 함수 @@@
    {
        return x + WidthCount * y;
    }

    public Vector2Int worldToGrid(Vector3 worldPos)// 월드좌표가 어떤 맵에 있는 , 어떤 좌표에있는지 리턴하는 함수
    {
        Vector2 offset = (Vector2)worldPos - worldOrigin;
        return new Vector2Int((int)(offset.x / mapwidthLength), (int)(offset.y / mapheightLength));
    }
//#if UNITY_EDITOR
    public void TestLoadScene(int x, int y)
    {
        RequestAsyncSceneLoad(x, y);
    }
    public void TestUnLoadScene(int x, int y)
    {
        RequestAsyncSceneUnLoad(x, y);
    }
    public void TestRefresh(int x, int y)
    {       
        RefreshScenes(x, y);
    }
//#endif
}
