
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
//유니티에서 싱글톤패턴 만들시 고려해야할 부분 
/*
    1. 싱글톤클래스가 닫히는 중일때 Thread 처럼 여러곳에서 동시다발적으로 접근하는경우 처리가 필요
    2. unity GameObject에 들어가야 씬에 올라갈수있기에 오브젝트 하나당 하나씩해서 중복생성에 관한 처리필요 
        2-1 . Awake 에서 생성될때 확인
        2-2 . 싱글톤클래스 호출시 확인
    3. 씬 변환 시 오브젝트가 초기화되니 초기화 안되게 DontDestroyOnLoad(instance.gameObject); 함수를 사용 
    4. 싱글톤 상속받은 클래스에서 InputSystem 을 사용하여 OnEnable 과 OnDisable 쓸경우  
        - Destroy 함수 실행시 OnEnable은 실행안되고  OnDisable 먼저 발동하기때문에 InputSystem 의 활성화 비활성화를 OnEnable 과 OnDisable 를사용하여 이벤트처리했다면 수정해야한다.
    5. 상속받은 클래스가 제네릭이면 AddComponent에서 자료의 형태를 찾을수가없다.
    - 유니티에서의 싱글톤은 나중에 생성된것을 사용하는것이 낫다.?
    - 게으른 할당?
 */
//T 는 반드시 컴퍼넌트 여야 한다
//where 는 조건 걸기위해 작성한다.
public class Singleton<T> : MonoBehaviour where T : Component
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
                    //Debug.Log($"instance = {instance} 생성순서확인");
                    DontDestroyOnLoad(instance.gameObject); //씬이 사라질때 게임오브젝트가 삭제되지 안하게하는 함수
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
    /// <summary>
    /// 해당 컴포넌트가 사용되는 첫씬에서 한번 Awake 가 발동한다. 
    /// 대신 해당오브젝트 그리고 부모오브젝트들이 모두 활성화 되어있어야 발동을한다.
    /// 아니면 활성화 될때 OnEnable 실행되기전에 발동을한다
    /// 씬이동시 고려해야할점은 Awake 에서 맴버변수로 처리해야되는 로직이있는경우 
    /// 씬이동시 중복 오브젝트의 Awake 는 내용이 전부 실행이 됨으로 맴버변수로 처리하는로직을 넣으면 안된다.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
        { //씬에 배치되어있는 첫번째 싱글톤  게임오브젝트
            instance = this as T;
    
            DontDestroyOnLoad(instance.gameObject); //씬이 사라질때 게임오브젝트가 삭제되지 안하게하는 함수
        }
        else
        { //첫번째 싱글톤 게임 오브젝트가 만들어진 이후에 만들어진 싱글톤 게임 오브젝트
            if (instance != this)
            { //두개만들어졌는데 같은거일수도있어서 아닐경우만 처리한다. 
                Destroy(this.gameObject);  //첫번째 싱글톤과 다른 객체이면 삭제
                //Awake 에서 맴버변수로 초기화하는건 비추천
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

