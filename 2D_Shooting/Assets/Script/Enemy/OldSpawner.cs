using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class OldSpawner : MonoBehaviour
{
    public Pool_Object_Type spawnType;
    public float interval = 0.5f;

    //스폰할 오브젝트 프리팹
    public GameObject[] spawnTarget;

    //스폰 높이폭
    public float rangeY = 4;
    public float rangeX = 0.5f;


    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        StartCoroutine(SpawnCoroutine());
    }

    protected virtual EnemyBase Spawn()
    {
        GameObject obj = Factory.Inst.GetObject(spawnType);
        obj.transform.position = new Vector3(transform.position.x, Random.Range(rangeY, -rangeY), 0);

        EnemyBase enemy = obj.GetComponent<EnemyBase>();

        return enemy;
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(interval); // interval에 지정된 시간만큼 기다린다
        }
 
    }
    public void TestSpawn()
    {
        Spawn();
       
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color= Color.green;
        Gizmos.DrawLine(new Vector3( transform.position.x - rangeX, transform.position.y - rangeY, 0), new Vector3(transform.position.x - rangeX, transform.position.y + rangeY, 0));
        Gizmos.DrawLine(new Vector3(transform.position.x - rangeX, transform.position.y + rangeY, 0), new Vector3(transform.position.x + rangeX, transform.position.y + rangeY, 0));
        Gizmos.DrawLine(new Vector3(transform.position.x + rangeX, transform.position.y + rangeY, 0), new Vector3(transform.position.x + rangeX, transform.position.y - rangeY, 0));
        Gizmos.DrawLine(new Vector3(transform.position.x + rangeX, transform.position.y - rangeY, 0), new Vector3(transform.position.x - rangeX, transform.position.y - rangeY, 0));

    }
 

}
