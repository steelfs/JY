using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class OldSpawner : MonoBehaviour
{
    public Pool_Object_Type spawnType;
    public float interval = 0.5f;

    //������ ������Ʈ ������
    public GameObject[] spawnTarget;

    //���� ������
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
            yield return new WaitForSeconds(interval); // interval�� ������ �ð���ŭ ��ٸ���
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
