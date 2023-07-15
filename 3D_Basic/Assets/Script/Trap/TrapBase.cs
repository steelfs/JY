using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour
{
    SphereCollider sphereCollider;
    private void OnTriggerEnter(Collider other) //무언가가 트리거에 들어오면 발동
    {
        if (other.CompareTag("Player"))
        {
            OnTrapActivate(other.gameObject);
        }
        
    }
    protected virtual void OnTrapActivate(GameObject target) //충동한 오브젝트를 파라미터로  충돌시 실행할 함수 호출
    {

    }
}
