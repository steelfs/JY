using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUsechecker : MonoBehaviour
{
    public Action<Iinteractable> onItemUse;
    private void OnTriggerEnter(Collider other)
    {
        //체커의 트리거 영역에 다른 콜라이더가 들어왔을 때
        Transform target = other.transform;
        Iinteractable obj = null;
        do
        {
            obj = target.GetComponent<Iinteractable>();//충돌한 물체의 Iinteractable 이있다면 찾아오기
            target = target.parent;                     // target은 부모로 변경
        }
        while (obj == null && target != null);// obj를 찾거나 더이상 부모가 없으면 종료
    

        if (obj != null)
        {
            onItemUse?.Invoke(obj);//Iinteractable 을 상속받은 컴포넌트가 있으면 실행
        }
    }
}
