using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_Ground_Planet : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float minRightEnd = 30.0f;
    public float maxRightEnd = 60.0f;
    public float minY = -8;
    public float maxY = -5;

    float baseLineX;

    void Start()
    {
        baseLineX = transform.position.x;
    }

    void Update()
    {
        if (transform.position.x < baseLineX)
        {
            Vector3 newPos = new Vector3(Random.Range(minRightEnd, maxRightEnd), Random.Range(minY, maxY), 0);
            transform.position = newPos;
        }
        else
        {
            transform.Translate(Time.deltaTime * moveSpeed * -transform.right);
        }
    }
    void MoveRight()
    {
        Debug.Log("ddd");
        Vector3 pos = new Vector3(Random.Range(minRightEnd, maxRightEnd), Random.Range(minY, maxY), 0);
        //transform.Translate(moveRangeX * transform.right);
        
    }
}
