using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : PooledObject
{
    public GameObject originalPrefab;

    public int poolSize = 64;
    T[] pool;
    Queue<T> readyQueue; 

    public void Initialize()
    {
        if (pool == null)
        {
            pool = new T[poolSize];
            readyQueue = new Queue<T>(poolSize);

            //readyQueue.Count; //실제로 들어있는 갯수 
            //readyQueue.Capacity // 현재 미리 준비해둔 갯수 

            GenerateObjects(0, poolSize, pool);
        }
        else
        {
            foreach (T obj in pool) //만약 두번째 씬이 불러져서 풀이 존재하는 상황이라면 
            {
                obj.gameObject.SetActive(false); 
            }
        }
    }
    /// <summary>
    /// 풀에서 오브젝트를 하나 꺼낸 후 돌려주는 함수
    /// </summary>
    /// <returns>큐에서 꺼내고 활성화시킨 오브젝트</returns>

    public T GetObject(Transform spawnTransform = null) // 오브젝트 꺼낼 때 위치 회전 스케일 설정
    {
        if (readyQueue.Count > 0) // 큐에 남아있는 오브젝트가 있는지 확인
        {
            //남아있으면 
            T comp = readyQueue.Dequeue(); // 하나꺼내고
            if (spawnTransform != null)// 미리 설정할 트렌스폼이 있다면 적용
            {
                comp.transform.SetPositionAndRotation(spawnTransform.position, spawnTransform.rotation);
             
                comp.transform.localScale = spawnTransform.localScale;
            }
            else//없다면 
            {
                comp.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                comp.transform.localScale = Vector3.one;
            }
            comp.gameObject.SetActive(true);// 활성화시킨다음
            return comp; // 꺼낸것 리턴
        }
        else
        {
            //안남아있으면 
            ExpendPool(); //확장시키고
            return GetObject(spawnTransform);// 다시 요청
        }
    }
    private void ExpendPool()//풀의 크기를 두배로 확장시키는 함수
    {
        Debug.LogWarning($"{gameObject.name} 풀사이즈 증가 {poolSize} -> {poolSize * 2}");

        int newSize = poolSize * 2;    //새로운 크기 구하기
        T[] newPool = new T[newSize];  // 새로운 크기만큼 새 배열만들기
        for (int i = 0; i < poolSize; i++) // 이전 배열에 있던것을 새 배열에 복사
        {
            newPool[i] = pool[i]; 
        }
        GenerateObjects(poolSize, newSize, newPool); // 새 배열에 남은 부분에 오브젝트 생성해서 설정
        pool = newPool; // 새 배열을 풀로 설정
        poolSize = newSize; // 새 크기를 크기로 설정
    }

    /// <summary>
    /// // 오브젝트 생성해서 배열에 추가해주는 함수
    /// </summary>
    /// <param name="start">배열의 시작인덱스</param>
    /// <param name="end">배열의 마지막인덱스 -1</param>
    /// <param name="newArray">생성된 오브젝트가 들어갈 배열</param>
    void GenerateObjects(int start, int end, T[] newArray) 
    {
        for (int i = start; i < end; i++) //새로 만들어진 반복
        {
            GameObject obj = Instantiate(originalPrefab, transform);// 생성 후 풀을 부모 오브젝트로 설정 
            obj.name = $"{originalPrefab.name}_{i}"; //이름 구분하기위해 설정

            T comp = obj.GetComponent<T>();  // pooledObject 컴포넌트가져와서 
            comp.onDisable += () => readyQueue.Enqueue(comp); // disable될때 큐로 되돌리기 

            OnGenerateObjects(comp);

            newArray[i] = comp; // 배열에 저장하고 
            obj.SetActive(false);//비활성화 시키기 비활성화될때 위에서 추가한 람다식이 실행되면서 큐로 다시 돌아가게된다.
            
        }
    }

    /// <summary>
    /// 각 T타입 별로 필요한 추가 작업을 처리하는 함수 
    /// </summary>
    /// <param name="comp">작업할 컴포넌트</param>
    protected virtual void OnGenerateObjects(T comp)
    {

    }
}
