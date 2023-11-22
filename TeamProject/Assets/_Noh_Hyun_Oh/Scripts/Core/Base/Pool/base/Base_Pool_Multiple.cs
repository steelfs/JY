using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 기본적으로 여러개 생산되는 오브젝트일경우 
/// 해당클래스를 상속받아서 사용 
/// </summary>
/// <typeparam name="T">ObjectIsPool을 상속받은 클래스</typeparam>
public class Base_Pool_Multiple<T> : MonoBehaviour where T : Base_PoolObj
{
    /// <summary>
    /// 풀에 담아 놓을 오브젝트의 프리팹
    /// </summary>
    public GameObject origianlPrefab;

    /// <summary>
    /// 풀의 크기. 처음에 생성하는 오브젝트의 갯수. 갯수는 2^n으로 잡는 것이 좋다.
    /// </summary>
    public int poolSize = 8;

    /// <summary>
    /// 풀이 생성한 모든 오브젝트가 들어있는 배열
    /// </summary>
    protected T[] pool;

    /// <summary>
    /// 사용가능한(비활성화되어있는) 오브젝트들이 들어있는 큐
    /// </summary>
    protected Queue<T> readyQueue;

    /// <summary>
    /// 오브젝트 생성위치 변경할 변수 추가 
    /// 이변수의 오브젝트가 비활성화되있으면 GenerateObjects 함수의 마지막 비활성화 시켜도 OnDisable함수가 호출이안된다.
    /// </summary>
    protected Transform setPosition;

    /// <summary>
    /// 화면전환시 계속 발생하여 버그가생긴다,.
    /// </summary>
    public void Initialize()
    {
        StartInitialize();  //객체 생성되기전에 처리할 내용이 있는경우를 위해 추가

        if (setPosition == null) {//오브젝트 생성위치 변경하는 로직을위해 조건문추가
            setPosition = transform;
        }

        
        if (pool == null)
        {
            pool = new T[poolSize];                 // 풀 전체 크기로 배열 할당
            readyQueue = new Queue<T>(poolSize);    // 레디큐 생성(capacity는 poolSize로 지정)
            //readyQueue.Count;       // 실제로 들어있는 갯수
            //readyQueue.Capatity;    // 현재 미리 준비해 놓은 갯수

            GenerateObjects(0, poolSize, pool);
        }
        else
        {
            
            // 두번째 씬이 불려져서 이미 풀은 만들어져 있는 상황
            foreach (T obj in pool)
            {
                obj.gameObject.SetActive(false);    // 전부 비활성화
            }
            
        }
        EndInitialize();//초기화 끝날때  처리 할내용 
    }

    /// <summary>
    /// 풀에서 오브젝트를 하나 꺼낸 후 돌려주는 함수
    /// </summary>
    /// <returns>레디큐에서 꺼내고 활성화시킨 오브젝트</returns>
    public T GetObject()
    {
        if (readyQueue.Count > 0)    // 레디큐에 남아있는 오브젝트가 있는지 확인
        {
            // 남아있으면
            T comp = readyQueue.Dequeue();      // 하나 꺼내고

            comp.onEnable_InitData?.Invoke();              // 초기화할 내용있으면 초기화 하고 

            comp.gameObject.SetActive(true);    // 활성화시킨 다음에 
            
            return comp;                        // 꺼낸 것 리턴

        }
        else
        {
            // 남은 오브젝트가 없으면
            ExpandPool();           // 풀 확장시키고
            return GetObject();     // 다시 요청
        }
    }

    /// <summary>
    /// 풀을 두배로 확장시키는 함수
    /// </summary>
    void ExpandPool()
    {
        Debug.LogWarning($"{gameObject.name} 풀 사이즈 증가. {poolSize} -> {poolSize * 2}");

        int newSize = poolSize * 2;     // 새로운 크기 구하기
        T[] newPool = new T[newSize];   // 새로운 크기만큼 새 배열 만들기
        for (int i = 0; i < poolSize; i++)     // 이전 배열에 있던 것을 새 배열에 복사
        {
            newPool[i] = pool[i];
        }

        GenerateObjects(poolSize, newSize, newPool);    // 새 배열에 남은 부분에 오브젝트 새엉해서 설정
        pool = newPool;     // 새 배열을 pool로 설정
        poolSize = newSize; // 새 크기를 크기로 설정
    }

    /// <summary>
    /// 오브젝트 생성해서 배열에 추가해주는 함수
    /// </summary>
    /// <param name="start">배열의 시작 인덱스</param>
    /// <param name="end">배열의 마지막 인덱스-1</param>
    /// <param name="newArray">생성된 오브젝트가 들어갈 배열</param>
    void GenerateObjects(int start, int end, T[] newArray)
    {
        // 이 함수는 풀이 더필요할경우 (사용빈도가 낮다)만 사용됨으로 함수내부에서만 사용할수있게 자동변수로 지정
        // for문안에서 변수 메모리공간을 계속 추가로 생성해주는건 안좋을것같아서 로직문제없는 선에서 밖에다 선언하여 사용한다
        
        Transform tf = setPosition; //비활성화된 부모를 체크하기위한 객체
        
        GameObject obj; //추가될 게임오브젝트 선언

        bool isParentActive = true; //해당 함수는 부모들이 활성화 되었다는 전제하에 작동함으로 정상작동을하려면 true 가 되있어야한다. 


        //추가될위치의 오브젝트가 비활성화 되어있으면 SetActive(false)를해도 OnDisable 이 발동을안함으로 체크를해준다.
        while (tf != null || !isParentActive) //부모가 최상단이거나 부모가 비활성화 되있으면 빠져나간다
        {
            isParentActive = tf.gameObject.activeSelf; //부모의 활성화여부를 체크한다. 여기선 false 값만 있는지 확인한다.
            tf = tf.parent; //부모의 위치로 변경
        }

        for (int i = start; i < end; i++)    // 새로 만들어진 크기만큼 반복
        {
            //특정위치에 생성하기 기본적으로는 풀아래에 있다.
            obj = Instantiate(origianlPrefab, setPosition); //생성되면 일단 Awake =>  OnEnable 순으로 실행시킨다.
            obj.name = $"Pool_{origianlPrefab.name}_{i}";            // 이름 구분되도록 설정

            T comp = obj.GetComponent<T>();                     // PooledObject 컴포넌트 받아와서
            //Debug.Log($"{obj.name} _ {comp == null}");
            comp.onDisable += () =>
            {
                readyQueue.Enqueue(comp);   // PooledObject가 disable될 때 래디큐로 되돌리기
            };

            newArray[i] = comp;     // 배열에 저장

            comp.onGetPoolTransform?.Invoke(transform);

            obj.SetActive(false);   // 생성한 게임 오브젝트 비활성화(=>비활성화 되면서 레디큐에도 추가된다)
      
            if (!isParentActive) // 상위 오브젝트에서 비활성화되있으면 SetActive(false)를 해도 OnDisable 이 발동을안하여 큐값이 없을수가있다.
            { 
                readyQueue.Enqueue(comp);   // 상위 오브젝트가 비활성화 되있는경우  추가해주자
            }
            
        }
    }
    /// <summary>
    /// 풀에서 생성이되고 오브젝트가 해제될시에 다시 풀로돌리기위해 풀위치를 넘겨주기위한 함수
    /// </summary>
    /// <param name="comp">추가될 객체</param>
    protected virtual void ReturnPoolTransformSetting(T comp ,Transform poolObj) 
    {
        
    }
    /// <summary>
    /// 풀에서 객체 생성 전에 처리해야할 내용 
    /// </summary>
    protected virtual void StartInitialize() { }

    /// <summary>
    /// 풀에서 객체 생성 후에 처리해야 할 내용 상속받아서 각자사용
    /// </summary>
    protected virtual void EndInitialize()
    {
    }
}