using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PooledObject obj = collision.GetComponent<PooledObject>();
        if (obj != null)
        {
            collision.gameObject.SetActive(false); // PooledObject 를 상속받아 풀로 돌아가야하는 오브젝트라면 비활성화
        }
        else
        {
            Destroy(collision.gameObject); //아닐경우 파괴
        }
    
    }

}
