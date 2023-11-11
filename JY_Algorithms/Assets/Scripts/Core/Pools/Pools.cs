using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum PoolObjectType
{
    Cell,
    SpawnEffect,
    Item
}
public class Pools : MonoBehaviour
{
    [System.Serializable]
    public class PoolPrefab
    {
        public GameObject prefab;
        [Range(1, 200)]
        public int poolSize;
    }

    public PoolPrefab[] PoolPrefabs;
    Queue<GameObject>[] pools;
    GameObject[] parents;

    private void Start()
    {
        Init();
    }
    void Init()
    {
        pools = new Queue<GameObject>[PoolPrefabs.Length];
        parents = new GameObject[PoolPrefabs.Length];

        for (int  i = 0; i < PoolPrefabs.Length; i++)
        {
            GameObject prefab = PoolPrefabs[i].prefab;
            Transform parent = new GameObject($"{PoolPrefabs[i].prefab.name}_Pool").transform;
            parents[i] = parent.gameObject;
            parent.transform.SetParent(this.transform);
            pools[i] = new Queue<GameObject>(PoolPrefabs[i].poolSize);
            for (int j = 0; j < PoolPrefabs[i].poolSize + 1; j++)
            {
                GameObject obj = Instantiate(prefab, parent);
                obj.name = $"{prefab.name}_{parent.childCount - 1}";
                Pooled_Obj poolObj = obj.AddComponent<Pooled_Obj>();
                obj.SetActive(false);
                poolObj.poolIndex = i;// 비활성화시 다시 풀로 되돌릴때 사용
              //  poolObj.on_ReturnPool += ReturnPool;
              
                pools[i].Enqueue(obj);
            }
        }
    }
    public GameObject GetObject(PoolObjectType type, Transform parent)
    {
        GameObject result = pools[(int)type].Dequeue();
        result.SetActive(true);
        result.transform.SetParent(parent);
        return result;
    }
    public GameObject GetObject(PoolObjectType type, Vector3 position)
    {
        Queue<GameObject> queue = pools[(int)type];
        GameObject result;
        if (queue.Count > 0)
        {
            result = queue.Dequeue();
        }
        else
        {
            ExpandPool(queue, type);
            //큐 확장
            result = queue.Dequeue();
        }
        result.transform.position = position;
        result.SetActive(true);
        return result;
    }
    void ExpandPool(Queue<GameObject> queue, PoolObjectType type)
    {
        PoolPrefab pool = PoolPrefabs[(int)type];
        Transform parent = parents[(int)type].transform;

        for (int i = 0; i < pool.poolSize; i++)
        {
            GameObject obj = Instantiate(pool.prefab, parent);
            obj.name = $"{pool.prefab.name}_{parent.childCount - 1}";
            Pooled_Obj poolObj = obj.AddComponent<Pooled_Obj>();
            poolObj.poolIndex = (int)type;
            obj.SetActive(false);
            queue.Enqueue(obj);
        }
        pool.poolSize *= 2;
    }
    public void ReturnPool(Pooled_Obj obj)
    {
        Queue<GameObject> queue = pools[obj.poolIndex];
        queue.Enqueue(obj.gameObject);

        obj.transform.SetParent(parents[obj.poolIndex].transform);
        //StartCoroutine(SetParent(obj));
    }
    

}
