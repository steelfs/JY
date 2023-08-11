using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ImageNumber))]//이 스크립트를 사용하기 위해 필수적으로 필요한 컴포넌트가 없으면 자동으로 추가됨
public class CounterBase : MonoBehaviour
{
    ImageNumber imageNumber;
    protected virtual void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
    }

    
    protected virtual void Refresh(int count)//델리게이트에 연결할 함수 
    {
        imageNumber.Number = count;
    }
}
