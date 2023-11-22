
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// DontDestroyOnLoad �Լ��� �ֻ��� ���ӿ�����Ʈ���� �����Ҽ��ִٰ��Ѵ� 
/// �ڽĵ��� �޴� �̱����� ���Լ��� �������� 
/// </summary>
/// <typeparam name="T">����Ƽ ���ӿ�����Ʈ</typeparam>
public class ChildComponentSingeton<T> : MonoBehaviour where T : Component
{
    
    /// <summary>
    /// �̹� ����ó���� ������ Ȯ���ϱ� ���� ����
    /// </summary>
    private static bool isShutDown = false;

    /// <summary>
    /// �̱����� ��ü
    /// </summary>
    private static T instance;
    
    /// <summary>
    /// �̱��� ��ü�� �б� ���� ������Ƽ. ��ü�� ��������� �ʾ����� ���θ����.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (isShutDown) 
            {//����ó�������� Ȯ��
                Debug.LogWarning($"{typeof(T).Name}�� �̹� ���� ���̴�.");
                return null; //ó����������� null�� �ѱ��.
                
            }
            if (instance == null) 
            {
                if (FindObjectOfType<T>(true) == null)
                { //���� �̱������ִ��� Ȯ��
                    GameObject gameObj = new GameObject(); //������Ʈ���� 
                    gameObj.name = $"{typeof(T).Name} Singleton"; //�̸��߰��ϰ�
                    instance = gameObj.AddComponent<T>(); //�̱��水ü�� �߰��Ͽ� ����
                }
                else
                {
                    Debug.Log($"FindObjectOfType<T>(true) = {FindObjectOfType<T>(true)} ���� ����� �����°͵����ִ�");
                    Debug.Log("����κ����� �ϴ� ��Ȱ��ȭ�� ������Ʈ�� �̱����� �߰������ʾҳ� Ȯ���غ���.");

                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        { //���� ��ġ�Ǿ��ִ� ù��° �̱���  ���ӿ�����Ʈ
            instance = this as T;
        }
        else
        { //ù��° �̱��� ���� ������Ʈ�� ������� ���Ŀ� ������� �̱��� ���� ������Ʈ
            if (instance != this)
            { //�ΰ���������µ� �������ϼ����־ �ƴҰ�츸 ó���Ѵ�. 
                Destroy(this.gameObject);  //ù��° �̱���� �ٸ� ��ü�̸� ����
            }
        }
    }


    protected virtual void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
    }
    /// <summary>
    /// ���α׷��� ����ɶ� ����Ǵ� �Լ�.
    /// </summary>
    private void OnApplicationQuit()
    {
        isShutDown = true; //���� ǥ��
    }

    protected virtual void OnSceneLoaded(Scene scean, LoadSceneMode mode)
    {
        Init(scean,mode);
    }
    protected virtual void Init(Scene scene , LoadSceneMode mode) { 
        
    }
}

