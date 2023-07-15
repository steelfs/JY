using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Spawner : MonoBehaviour
{
    [Serializable]
    public struct SpawnData
    {
        public SpawnData(Pool_Object_Type type = Pool_Object_Type.Enemy_Fighter, float interval = 0.5f)
        {
            this.spawnType = type;
            this.interval = interval;
        }
        public Pool_Object_Type spawnType;
        public float interval;
    }
    //public List <SpawnData>spawnDatas;
    public SpawnData[] spawnDatas;

    Transform destination;
    Player player;
    public float rangeY = 4;
    public float rangeX = 0.5f;
    private void Awake()
    {
        destination = transform.GetChild(0);
    }
    void Start()
    {
        player = FindObjectOfType<Player>();

        foreach(var spawnData in spawnDatas)
        {
            StartCoroutine(SpawnCoroutine(spawnData));
        }    
    }
    protected virtual EnemyBase Spawn(Pool_Object_Type type)
    {
        GameObject obj = Factory.Inst.GetObject(type, new Vector3(transform.position.x, UnityEngine.Random.Range(-rangeY, rangeY), 0));

        EnemyBase enemy = obj.GetComponent<EnemyBase>();
        switch (type)
        {
            case Pool_Object_Type.Enemy_Asteroid:
                Vector3 destPos = destination.position;
                destPos.y = UnityEngine.Random.Range(-rangeY, rangeY);
                EnemyAsteroid asteroid = enemy as EnemyAsteroid;
                asteroid.Destination = destPos;
                break;
        }
        return enemy;
    }
    IEnumerator SpawnCoroutine(SpawnData data)
    {
        while (true)
        {
            Spawn(data.spawnType);
            yield return new WaitForSeconds(data.interval); // interval에 지정된 시간만큼 기다린다
        }

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(transform.position.x - rangeX, transform.position.y - rangeY, 0), new Vector3(transform.position.x - rangeX, transform.position.y + rangeY, 0));
        Gizmos.DrawLine(new Vector3(transform.position.x - rangeX, transform.position.y + rangeY, 0), new Vector3(transform.position.x + rangeX, transform.position.y + rangeY, 0));
        Gizmos.DrawLine(new Vector3(transform.position.x + rangeX, transform.position.y + rangeY, 0), new Vector3(transform.position.x + rangeX, transform.position.y - rangeY, 0));
        Gizmos.DrawLine(new Vector3(transform.position.x + rangeX, transform.position.y - rangeY, 0), new Vector3(transform.position.x - rangeX, transform.position.y - rangeY, 0));

    }
}
        


    
 


   





