using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WorldManager : MonoBehaviour
{
    //������� ���� 
    const int HeightCount = 3;
    const int WidthCount = 3;

    const float mapheightLength = 20.0f; //����� �ϳ��� ���α���
    const float mapwidthLength = 20.0f;

    readonly Vector2 worldOrigin = new Vector2(-mapwidthLength * WidthCount * 0.5f, - mapheightLength * HeightCount * 0.5f);//��� ������� ���� (���� �Ʒ�)

    const string SceneNameBase = "SeemLess";//���̸� ���տ� �⺻ �̸�

    string[] sceneNames; //������ �Ϸ�� ��� ���� �̸� �迭
    
    enum SceneLoadState : byte// ���� �ε� ���¸� ��Ÿ�� enum
    {
        UnLoad = 0, //�ε��� �ȵǾ�����
        PendingUnLoad, // �ε� ���� ������
        PendingLoad,// �ε� ������
        Loaded//�ε� �Ϸ��
            
    }

    private void Start()
    {
        Debug.Log(worldToGrid(new Vector3(41, 30, 0)));
        
    }

    SceneLoadState[] sceneLoadState;

    public bool IsUnLoadAll// ��� ���� ��ε� �Ǿ����� Ȯ���ϱ� ���� ������Ƽ
    {
        get
        {
            bool result = true;
            foreach(SceneLoadState state in sceneLoadState)
            {
                if (state != SceneLoadState.UnLoad)
                {
                    result = false; //�ϳ��� UnLoad�� �ƴϸ� false
                    break;
                }
            }
            return result;
        }
    }

    List<int> loadWork = new List<int>(); //�ε��� �õ��� ���
    List<int> loadWorkComplete = new List<int>(); // �ε� �õ��� �Ϸ�� ���

    List<int> unloadWork = new List<int>(); //�ε������� �õ��� ���
    List<int> unloadWorkComplete = new List<int>(); // �ε������� �Ϸ�� ���
    public void PreInitialize()//�̱����� ����� ���� �� �ѹ��� ȣ��
    {
        sceneNames = new string[HeightCount * WidthCount ];  //�迭ũ�� Ȯ��
        sceneLoadState = new SceneLoadState[HeightCount * WidthCount];

        for (int y = 0; y < HeightCount; y++)
        {
            for (int x = 0; x < WidthCount; x++)
            {
                int index = GetIndex(x, y);
                sceneNames[index] = $"{SceneNameBase}{x}_{y}"; //�迭 ä���ֱ� 
                sceneLoadState[index] = SceneLoadState.UnLoad;
            }
        }
    }
    public void Initialize()// ���� single���� �ε��ɶ����� ȣ��Ǵ� �Լ� 
    {
        for (int i = 0; i < sceneLoadState.Length; i++)
        {
            sceneLoadState[i] = SceneLoadState.UnLoad; // ���� �ҷ����� �� ����ʵ��� �ε����� �ʱ�ȭ 
        }

        Player player = GameManager.Inst.Player;
        if (player != null)
        {
            player.onDie += (_, _) => //�÷��̾� �׾��� �� ����� ���� �Լ� , �Ķ���ʹ� ����
            {
                for (int y = 0; y < HeightCount; y++)
                {
                    for (int x = 0; x < WidthCount; x++)
                    {
                        RequestAsyncSceneUnLoad(x, y);//��� �� �ε� ���� ��û
                    }
                }
            };
            player.onMapMoved += (gridPos) =>// �÷��̾ ���� �̵����� �� ����� ���� �Լ� 
            {
                RefreshScenes(gridPos.x, gridPos.y);//�÷��̾� �ֺ� �� ���� ��û
            };

            Vector2Int grid = worldToGrid(player.transform.position); // �÷��̾ �ִ� ����� �׸��� ��������
            RequestAsyncSceneLoad(grid.x, grid.y); // �÷��̾ ��ġ�� ���� �ֿ켱���� �ε� ��û
            RefreshScenes(grid.x, grid.y); //�÷��̾� �ֺ� ��
        }
    }

    private void Update()
    {
        foreach (var index in loadWorkComplete)// �Ϸ�� �ε� �۾��� �ε��ũ���� ����
        {
            loadWork.RemoveAll((x) => x == index);// loadWork�� �ִ� �͵� �߿��� x�� index�� ���� ��� removeAll �ض� ������
        }
        loadWorkComplete.Clear();

        foreach(var index in loadWork)
        {
            AsyncSceneLoad(index);// loadWork�� �ִ� �͵��� ���� �񵿱� �ε� ����
        }

        foreach (var index in unloadWorkComplete)
        {
            unloadWork.RemoveAll((x) => x == index);//  �ִ� �͵� �߿��� x�� index�� ���� ��� removeAll �ض� ������
        }
        unloadWorkComplete.Clear();

        foreach (var index in unloadWork)
        {
            AsyncSceneUnLoad(index);// loadWork�� �ִ� �͵��� ���� �񵿱� �ε� ����
        }
    }

    private void RequestAsyncSceneLoad(int x, int y)//���� �񵿱�� �ε��� ���� ��û�ϴ� �Լ� 
    {
        int index = GetIndex(x, y);
        if (sceneLoadState[index] == SceneLoadState.UnLoad)
        {
            loadWork.Add(index);
        }
    }
    private void RequestAsyncSceneUnLoad(int x, int y)//���� �񵿱�� �ε� ������ ���� ��û�ϴ� �Լ� 
    {
        int index = GetIndex(x, y);
        if (sceneLoadState[index] == SceneLoadState.Loaded)
        {
            unloadWork.Add(index); //�۾� ����Ʈ�� ����ϰ� 
        }

        //����ʿ� �ִ� �����ӵ��� ã�Ƽ� Ǯ�� �ǵ�����
        Scene scene = SceneManager.GetSceneByName(sceneNames[index]);
        if (scene.isLoaded)
        {
            GameObject[] sceneRoots = scene.GetRootGameObjects();//�ش� �� �ȿ� �θ� ���� ��� ���ӿ�����Ʈ���� �����´�
            if (sceneRoots != null && sceneRoots.Length > 0)
            {
                Slime[] slimes = sceneRoots[0].GetComponentsInChildren<Slime>();//���� ù��° Grid
                foreach(Slime slime in slimes)
                {
                    slime.ReturnToPool();
                }
            }
        }
    }
    private void RefreshScenes(int gridX, int gridY)//������ ��ǥ�� �ֺ� ���� �ε��� ��û�ϰ� �� �ܴ� �ε� ������ ��û�ϴ� �Լ� 
    {
        int startX = Mathf.Max(0, gridX - 1);
        int endX = Mathf.Min(WidthCount, gridX + 2);// +2 ���Ǵ� ������ for������  �۴ٷ� üũ�ϱ� ����
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
                //contains = ���� �ܼ��� ����ִ��� Ȯ�ο�
                //exist = ���� ������ ���ǿ� �´°��� �ִ��� Ȯ���ϱ� ���� ��
                if (!open.Contains(new Vector2Int(x, y)))
                {
                    RequestAsyncSceneUnLoad(x, y);
                }
            }
        }
    }
    private void AsyncSceneLoad(int index) // ������ ���� �񵿱�� �ε��ϴ� �Լ� index = �ش� ���� �ε���
    {
        if (sceneLoadState[index] == SceneLoadState.UnLoad) //�Ķ���ͷ� ���� ���� UnLoad���¶�� 
        {
            sceneLoadState[index] = SceneLoadState.PendingLoad; //�������̶�� ǥ��

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[index], LoadSceneMode.Additive);//�񵿱� �ε� ����
            async.completed += (_) =>// �񵿱� �۾��� ������ �� ����Ǵ� �����Լ� �߰�
            {
                sceneLoadState[index] = SceneLoadState.Loaded;// �ε���·� ����
                loadWorkComplete.Add(index);//�ε� �Ϸ� ��Ͽ� �߰�
            };
        }
    }

    private void AsyncSceneUnLoad(int index)
    {
        if (sceneLoadState[index] == SceneLoadState.Loaded)//�ε��Ϸ�����϶��� ����
        {
            sceneLoadState[index] = SceneLoadState.PendingUnLoad; //��ε� �������̶�� ǥ��

            AsyncOperation async = SceneManager.UnloadSceneAsync(sceneNames[index]); //��ε� ����
            async.completed += (_) =>
            {
                sceneLoadState[index] = SceneLoadState.UnLoad;//��ε���·� ���� 
                unloadWorkComplete.Add(index);// ��ε� �Ϸ� ��Ͽ� �߰�
            };
        }
    }
   
    private int GetIndex(int x, int y)// z, y��(������� �׸��� ��ǥ) �Ķ���ͷ� �ָ� ���� �ε���(�迭���� ���)�� �ٲ��ִ� �Լ� @@@
    {
        return x + WidthCount * y;
    }

    public Vector2Int worldToGrid(Vector3 worldPos)// ������ǥ�� � �ʿ� �ִ� , � ��ǥ���ִ��� �����ϴ� �Լ�
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
