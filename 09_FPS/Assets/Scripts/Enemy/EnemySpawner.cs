using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //적 생성
    // 항상 생성되어있는 적은 10명을 유지해야 한다.
    // 플레이어가 적을 죽이면 플레이어로부터 떨어진 랜덤한 위치에 1초 뒤 생성

    public int enemyCount = 10;
    public GameObject enemyPrefab;

    public int mazeWidth = 10;
    public int mazeHeight = 10;

    private void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, this.transform);
            obj.name = $"Enemy_{i}";
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.on_Die += (target) =>
            {
                StartCoroutine(ReSpawn(target));
            };
            enemy.transform.position = GetRandomPos();
        }
    }

    Vector3 GetRandomPos()
    {
        int size = CellVisualizer.CellSize;
        return new(Random.Range(0, mazeWidth) * size + 2.5f, 0, -Random.Range(0, mazeHeight) * size - 2.5f);
    }

    IEnumerator ReSpawn(Enemy target)
    {
        yield return new WaitForSeconds(1);

        target.transform.position = GetRandomPos();
        target.gameObject.SetActive(true);

    }
}
