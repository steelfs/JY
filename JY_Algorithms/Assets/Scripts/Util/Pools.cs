using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum PoolObjectType
{
    Cell
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
                obj.name = $"{prefab.name}_{j}";
                obj.AddComponent<Pooled_Obj>();
                obj.SetActive(false);
                Pooled_Obj poolObj = obj.GetComponent<Pooled_Obj>();
                poolObj.on_ReturnPool += ReturnPool;
                poolObj.poolIndex = i;// 비활성화시 다시 풀로 되돌릴때 사용
              
                pools[i].Enqueue(obj);
            }
        }
        int z = 0;
    }
    public GameObject GetObject(PoolObjectType type, Transform parent)
    {
        GameObject result = pools[(int)type].Dequeue();
        result.SetActive(true);
        result.transform.SetParent(parent);
        return result;
    }

    public void ReturnPool(Pooled_Obj obj)
    {
        Queue<GameObject> queue = pools[obj.poolIndex];
        queue.Enqueue(obj.gameObject);
        obj.transform.SetParent(parents[obj.poolIndex].transform);
       // StartCoroutine(SetParent(obj));
     
    }
    IEnumerator SetParent(Pooled_Obj obj)
    {
        obj.transform.SetParent(parents[obj.poolIndex].transform);
        yield return null;
    }
    void GenerateObject()
    {

    }
}
