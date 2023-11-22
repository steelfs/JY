using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoTestCamera : MonoBehaviour
{
    public Transform player;

    Vector3 offset = Vector3.zero;

    private void Start()
    {
        Vector3 playerPos = player.position;
        transform.position = new Vector3(playerPos.x + 1.0f, playerPos.y + 10.0f, playerPos.z - 5.0f);      // 카메라 위치 조정
        transform.rotation = Quaternion.Euler(45.0f, 0.0f, 0.0f);                                           // 카메라 각도 조절
        offset = transform.position - player.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, player.position + offset, 0.2f); 
        transform.LookAt(player);
    }
}
