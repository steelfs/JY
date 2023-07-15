using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//이 오브젝트와 닿으면 연결된 문이 열린다.
//플레이어와 이 오브젝트가 닿으면 이 오브젝트는 사라진다.
//이 오브젝트는 제자리에서 빙글빙글 돈다
public class DoorKey : MonoBehaviour
{
    public DoorBase target;
    public float rotateSpeed = 360.0f;

    Transform keyTransform;
    private void Awake()
    {
        keyTransform  = transform.GetChild(0);
    }
    private void Update()
    {
        keyTransform.Rotate(Time.deltaTime * rotateSpeed * Vector3.up);
        //keyTransform.RotateAround()
    }
    private void OnTriggerEnter(Collider other)
    {
        OnConsume();
    }

    private void OnValidate()
    {
        ResetTarget(target);
    }
    protected virtual void ResetTarget(DoorBase target)
    {
        if (target != null)
        {
            this.target = target as DoorAuto; // as : 왼쪽변수가 오른쪽 변수와 같으면 null이 아니고  같지 않으면 null을 리턴한다
                                         // is : 왼쪽변수가 오른쪽 변수와 같으면 true, 같지 않으면 false를 리턴한다
        }
    }
    protected virtual void OnConsume()
    {
        target.Open();
        Destroy(gameObject);
    }
}
