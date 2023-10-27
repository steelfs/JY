using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int mazeWidth = 10;
    public int mazeHeight = 10;

    public int enemyCount = 10;
    public GameObject enemyPrefab;
    Player player;
    private void Start()
    {
        for(int i = 0; i < enemyCount; i++)
        {
            GameObject obj =  Instantiate(enemyPrefab, this.transform);
            obj.name = $"Enemy_{i}";
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.onDie += (target) =>
            {
                StartCoroutine(Respawn(target));
            };

            enemy.transform.position = GetRandomPos(true);
        }

    }

    private Vector3 GetRandomPos(bool init = false)
    {
        int size = CellVisualizer.CellSize;
        Vector2Int playerPos = new Vector2Int(-100, -100);
        if (!init)
        {
            player = GameManager.Inst.Player;
            playerPos = new((int)player.transform.position.x / size, -(int)player.transform.position.z / size);
        }
     
        int x = 0;
        int z = 0;
        //do
        //{
        //    x = Random.Range(0, mazeWidth);
        //}while (x < playerPos.x + 3 && x > playerPos.x - 3) ;

        //do
        //{
        //    z = Random.Range(0, mazeHeight);
        //}while (z < playerPos.y + 3 && z > playerPos.y - 3) ;

        do
        {
            int index = Random.Range(0, mazeWidth * mazeHeight);
            x = index % mazeWidth;
            z = index / mazeHeight;
        } while (x < playerPos.x + 3 && x > playerPos.x - 3 && z < playerPos.y + 3 && z > playerPos.y - 3);

        Vector3 pos = new Vector3(x * size + 2.5f, 0, -z * size - 2.5f);
        return pos;
    }

    IEnumerator Respawn(Enemy target)
    {
        yield return new WaitForSeconds(1);

        target.transform.position = GetRandomPos();
        target.gameObject.SetActive(true);
    }

    // 1. 적을 생성
    // 2. 항상 생성되어 있는 적은 10명 유지
    //  2.1. 시작할 때 10마리 생성
    //  2.2. 플레이어가 적을 죽이면 플레이어로 부터 떨어진 랜덤한 위치에 1초 뒤 생성
}
