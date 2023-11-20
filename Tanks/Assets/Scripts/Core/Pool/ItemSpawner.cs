using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public float sizeX = 40;
    public float sizeZ = 40;

    public float baseInterval = 0.1f;
    public float randomRange = 0.1f;

    float current = 0;
    public GameObject itemPrefab;

    private void Start()
    {
        current = GetRandomInterval();
    }
    private void Update()
    {
        current -= Time.deltaTime;
        if (current < 0)
        {
            GameObject obj = Instantiate(itemPrefab);
            obj.transform.position = transform.position + new Vector3(Random.Range(0, sizeX), transform.position.y, Random.Range(0, sizeZ));
            
            
            Item item = obj.GetComponent<Item>();
            count[(int)item.shellType]++;
            current = GetRandomInterval();

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 p0 = transform.position;
        Vector3 p1 = new Vector3(transform.position.x + sizeX, transform.position.y, transform.position.z);
        Vector3 p2 = new Vector3(transform.position.x + sizeX, transform.position.y, transform.position.z + sizeZ);
        Vector3 p3 = new Vector3(transform.position.x, transform.position.y, transform.position.z + sizeZ);

        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p0);
    }
    float GetRandomInterval()
    {
        return baseInterval; //+ Random.Range(randomRange, -randomRange);
    }

    int[] count = new int[3];
    public void TestCounter()
    {
        Debug.Log(count[0]);
        Debug.Log(count[1]);
        Debug.Log(count[2]);
    }
}
