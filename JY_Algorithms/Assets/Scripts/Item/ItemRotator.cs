using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotator : MonoBehaviour
{
    float time = 0;
    float elapsed = 0;
    public float rotateSpeed = 180.0f;
    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(30, 0, 30);
    }
    void Update()
    {
        transform.Rotate(Time.deltaTime * rotateSpeed * Vector3.up, Space.World);
        time += Time.deltaTime;
        elapsed = Mathf.Sin(time);
        //Debug.Log(elapsed);
    }
}
