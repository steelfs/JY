using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAsteroidMini : EnemyBase
{
    float baseSpeed = 7.0f;
    float baseRotateSpeed = 0.0f;
    float rotateSpeed = 0.0f;
    Vector3 direction = Vector3.zero;

    public Vector3 Direction //방향지정 프로퍼티
    {
        get => direction;
        set
        {
            if (direction == Vector3.zero) //활성화 직후 한번만 설정한다
            {
                direction = value;
            }
        }
    }
    protected override void OnInitialize() //처음 활성화시 방향은 0,0,0
    {
        base.OnInitialize();
        speed = baseSpeed + Random.Range(-1.0f, 1.0f); //이동속도
        rotateSpeed = Random.Range(0, 360); // 회전속도
        direction =Vector3.zero; // 방향
    }
    protected override void OnMoveUpdate()
    {
        //base.OnMoveUpdate();
        transform.Translate(Time.deltaTime * speed * direction, Space.World);
        transform.Rotate(Time.deltaTime * rotateSpeed * Vector3.forward);
    }
}
