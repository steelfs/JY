using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotater : MonoBehaviour
{
    public float minY = 0.5f;
    public float maxY = 1.5f;

    public float rotateSpeed = 90.0f;
    public float moveSpeed = 2.0f;

    float timeElapsed = 0.0f;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(-30, 0, 0);
    }
    private void Update()
    {
        timeElapsed += Time.deltaTime * moveSpeed;
        Vector3 pos = transform.position;
        pos.z = 0;
        pos.x = 0;
        pos.y = minY + (((1 - Mathf.Cos(timeElapsed)) * 0.5f) * (maxY - minY));

        transform.localPosition = pos;
        transform.Rotate(0,Time.deltaTime * rotateSpeed, 0, Space.World);
    }
}
