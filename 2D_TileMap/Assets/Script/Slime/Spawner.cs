using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Spawner : MonoBehaviour
{
    public Vector2 size;//스포너의 크기(소환 영역 position 에서 x, y)
    public float spawnInterval = 2.0f;

    float elapsed = 0.0f;//마지막 스폰에서 경과시간

    

    public int capacity = 2;//이 스포너가 최대로 유지 가능한 슬라임 수 
    public int count= 0;//현재 스폰된 슬라임 수 

    List<Node> spawnAreaList;//스폰이 가능한 지역들을 모아 리스트로 저장

    MapManager mapManager;
    Player player;

    private void Start()
    {
        mapManager = GetComponentInParent<MapManager>();
        spawnAreaList = mapManager.CalculateSpawnArea(this);
        player = GameManager.Inst.Player;
    }
    private void Update()
    {
        if (count < capacity)
        {
            elapsed += Time.deltaTime;
            if (elapsed > spawnInterval)
            {
                Spawn();
                elapsed = 0.0f;
            }
        }
    }

    private void Spawn()
    {
        if (GetSpawnPosition(out Vector3 spawnPos)) //랜덤한 위치가 파라미터 
        {
            Slime slime = Factory.Inst.GetSlime(); //슬라임 생성 

            slime.Initialize(mapManager.GridMap,spawnPos); //초기화
            slime.onDie += () =>
            {
                count--; //죽을때 카운트 감소 
                player.KillMonster(slime.lifeTimeBonus);
            };
            slime.transform.SetParent(transform); //슬라임을 풀에서 스포너의  자식으로 옮김
            count ++;
        }
    }

    /// <summary>
    /// 스폰할 위치 구하는 함수 
    /// </summary>
    /// <param name="spawnPos">출력용 파라미터. 스폰위치가 있으면 그중 하나로 설정</param>
    /// <returns>스폰할 위치가 있으면 true </returns>
    private bool GetSpawnPosition(out Vector3 spawnPos)
    {
        bool result = false;
        List<Node> spawns = new List<Node> ();

        foreach(Node node in spawnAreaList)// spawnList에서 평지인 지역 모두 찾기  =몬스터가 없는 위치
        {
            if (node.nodeType == Node.NodeType.Plain)
            {
                spawns.Add(node);
            }
        }
        if (spawns.Count > 0)//빈자리 있는지 확인
        {
            int index = UnityEngine.Random.Range(0, spawns.Count);
            Node target = spawns[index];
            spawnPos = mapManager.GridToWorld(target.x, target.y);
            result = true;
        }
        else//없으면 (0, 0, 0)
        {
            spawnPos = Vector3.zero;
        }
        return result;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 basePos = new Vector3(Mathf.Floor(transform.position.x) ,Mathf.Floor(transform.position.y) );
        Vector3 p0 = basePos;
        Vector3 p1 = basePos + Vector3.right * size.x;
        Vector3 p2 = basePos + new Vector3(size.x, size.y);
        Vector3 p3 = basePos + Vector3.up * size.y;

        Handles.color = Color.black;
        Handles.DrawLine(p0, p1, 5);
        Handles.DrawLine(p1, p2, 5);
        Handles.DrawLine(p2, p3, 5);
        Handles.DrawLine(p3, p0, 5);
        //Gizmos.color = Color.grey;
        //Color color = Gizmos.color;
        //color.a = 0.5f;
        //Gizmos.color = color;

        //Vector3 _size = Vector3.zero;
        //_size.x = size.x;
        //_size.y = size.y;
        //Gizmos.DrawCube(transform.position, _size);
        //Debug.Log(_size);


    }
#endif
}
    