using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Spawner : MonoBehaviour
{
    public Vector2 size;//�������� ũ��(��ȯ ���� position ���� x, y)
    public float spawnInterval = 2.0f;

    float elapsed = 0.0f;//������ �������� ����ð�

    

    public int capacity = 2;//�� �����ʰ� �ִ�� ���� ������ ������ �� 
    public int count= 0;//���� ������ ������ �� 

    List<Node> spawnAreaList;//������ ������ �������� ��� ����Ʈ�� ����

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
        if (GetSpawnPosition(out Vector3 spawnPos)) //������ ��ġ�� �Ķ���� 
        {
            Slime slime = Factory.Inst.GetSlime(); //������ ���� 

            slime.Initialize(mapManager.GridMap,spawnPos); //�ʱ�ȭ
            slime.onDie += () =>
            {
                count--; //������ ī��Ʈ ���� 
                player.KillMonster(slime.lifeTimeBonus);
            };
            slime.transform.SetParent(transform); //�������� Ǯ���� ��������  �ڽ����� �ű�
            count ++;
        }
    }

    /// <summary>
    /// ������ ��ġ ���ϴ� �Լ� 
    /// </summary>
    /// <param name="spawnPos">��¿� �Ķ����. ������ġ�� ������ ���� �ϳ��� ����</param>
    /// <returns>������ ��ġ�� ������ true </returns>
    private bool GetSpawnPosition(out Vector3 spawnPos)
    {
        bool result = false;
        List<Node> spawns = new List<Node> ();

        foreach(Node node in spawnAreaList)// spawnList���� ������ ���� ��� ã��  =���Ͱ� ���� ��ġ
        {
            if (node.nodeType == Node.NodeType.Plain)
            {
                spawns.Add(node);
            }
        }
        if (spawns.Count > 0)//���ڸ� �ִ��� Ȯ��
        {
            int index = UnityEngine.Random.Range(0, spawns.Count);
            Node target = spawns[index];
            spawnPos = mapManager.GridToWorld(target.x, target.y);
            result = true;
        }
        else//������ (0, 0, 0)
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
    