using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public float speed = 3.0f;
    float rangeX = 8.5f;
    float rangeY = 4.5f;
    Vector3 direction;
    private void Awake()
    {
        direction = new Vector3(Random.Range(10f, -10f), Random.Range(6.5f, -6.5f), 0).normalized;
    }
    private void FixedUpdate()
    {
        transform.Translate(Time.fixedDeltaTime * speed * direction);
        if(transform.position.x > rangeX || transform.position.x < -rangeX || transform.position.y > rangeY || transform.position.y < -rangeY)
        {
            SetDirection();
        }
           
    }
    void SetDirection()
    {
        direction = new Vector3(Random.Range(10f, -10f), Random.Range(6.5f, -6.5f), 0).normalized;
         //transform.Rotate(0, 0, Random.Range(0, 360));
    }
}
