
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// DontDestroyOnLoad 함수는 최상위 게임오브젝트에만 적용할수있다고한다 
/// 자식들이 받는 싱글톤은 저함수를 제거하자 
/// </summary>
/// <typeparam name="T">유니티 게임오브젝트</typeparam>
public class ChildComponentSingeton<T> : MonoBehaviour where T : Component
{
    
    /// <summary>
    /// 이미 종료처리에 들어갔는지 확인하기 위한 변수
    /// </summary>
    private static bool isShutDown = false;

    /// <summary>
    /// 싱글톤의 객체
    /// </summary>
    private static T instance;
    
    /// <summary>
    /// 싱글톤 객체를 읽기 위한 프로퍼티. 객체가 많들어지지 않았으면 새로만든다.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (isShutDown) 
            {//종료처리중인지 확인
                Debug.LogWarning($"{typeof(T).Name}은 이미 삭제 중이다.");
                return null; //처리하지말라고 null을 넘긴다.
                
            }
            if (instance == null) 
            {
                if (FindObjectOfType<T>(true) == null)
                { //씬에 싱글톤이있는지 확인
                    GameObject gameObj = new GameObject(); //오브젝트만들어서 
                    gameObj.name = $"{typeof(T).Name} Singleton"; //이름추가하고
                    instance = gameObj.AddComponent<T>(); //싱글톤객체에 추가하여 생성
                }
                else
                {
                    Debug.Log($"FindObjectOfType<T>(true) = {FindObjectOfType<T>(true)} 가끔 여기로 빠지는것들이있다");
                    Debug.Log("여기로빠지면 일단 비활성화된 오브젝트에 싱글톤을 추가하지않았나 확인해보라.");

                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        { //씬에 배치되어있는 첫번째 싱글톤  게임오브젝트
            instance = this as T;
        }
        else
        { //첫번째 싱글톤 게임 오브젝트가 만들어진 이후에 만들어진 싱글톤 게임 오브젝트
            if (instance != this)
            { //두개만들어졌는데 같은거일수도있어서 아닐경우만 처리한다. 
                Destroy(this.gameObject);  //첫번째 싱글톤과 다른 객체이면 삭제
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
    /// 프로그램이 종료될때 실행되는 함수.
    /// </summary>
    private void OnApplicationQuit()
    {
        isShutDown = true; //종료 표시
    }

    protected virtual void OnSceneLoaded(Scene scean, LoadSceneMode mode)
    {
        Init(scean,mode);
    }
    protected virtual void Init(Scene scene , LoadSceneMode mode) { 
        
    }
}

