using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotater : MonoBehaviour
{
    //y축 기준 같은속도로 회전 
    public float rotateSpeed = 360.0f;
    public float moveSpeed = 2.0f;
    
    public float minHeight = 0.5f;
    public float maxHeight = 1.5f;

    float timeElapsed = 0.0f;

    float range = 0.0f;
    float timeValue = 0.0f;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
    }
    private void Update()
    {
        timeElapsed += Time.deltaTime;
        Vector3 pos;
        pos.x = transform.parent.position.x;
        pos.y = minHeight + (0.5f * (1 - Mathf.Cos(timeElapsed)) * (maxHeight - minHeight));
        pos.z = transform.parent.position.z;

        transform.position = pos;

        transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);

        // (Cos() + 1) * 0.5 => 1 ~ 0
        // 1 - (cos() + 1) * 0.5 => 0 ~ 1
        // min + (1 - (cos() + 1) * 0.5 )
        // min +  (1 - (cos() + 1) * 0.5 ) * (max - min) => min ~ max

        //min + (0.5 * (1- cos)) * (max -min)

        //transform.Rotate(0, rotateSpeed, 0);
        //timeElapsed += Time.deltaTime;
        //minHeight = MathF.Sin(timeValue);
        //transform.position = new Vector3(0, timeValue, 0); //내가쓴 코드
    }
}
