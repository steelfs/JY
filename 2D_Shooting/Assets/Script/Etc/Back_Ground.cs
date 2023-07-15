using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_Ground : MonoBehaviour
{
    public Transform[] bgslots; //배경 한 줄
    public float scrolling_Speed = 2.5f; //배경 이동속도

    public const float backGroundWidth = 13.5f; // 배경 한 변의 길이


    protected virtual void Awake()
    {

        bgslots = new Transform[transform.childCount]; //자식 갯수만큼의 크기를 가지는 배열 만들기
        for (int i = 0; i < bgslots.Length; i++)
        {
            bgslots[i] = transform.GetChild(i); //배열안에 트랜스폼 넣기
        }
    }
    void Start()
    {
        
    }
    void Update()
    {
        float baseLineX = transform.position.x - backGroundWidth; //배경이 충분히 이동했는지의 기준이되는 크기
        for (int i = 0; i< bgslots.Length; i++) //모든 슬롯 움직이기
        {
            bgslots[i].transform.Translate(Time.deltaTime * scrolling_Speed * -transform.right); //bgslot들을 왼쪼긍로 이동시키기
            if (bgslots[i].position.x < baseLineX) //기준선을 넘었을 때
            {
                MoveRightEnd(i); //오른쪽 끝으로 이동
            }
        }
    }

    protected virtual void MoveRightEnd(int index) //오른쪽 긑으로 보내는 함수
    {
  
        bgslots[index].Translate(backGroundWidth * bgslots.Length * transform.right);
    }
}
//백터
// v1 = (1,2,3)

//
/*
 * 방향백터 =  도착지점 - 출발지점
 * 
 * 백터의 곱하기
 * - 백터 * 백터 = 2종류 (내적, 외적)
 * - 백터 * 스칼라(백터의 크기 ) = 벡터
 * 
 * v1 * 5 = (5,10, 15)
 * v1 /2 = (0.5, 1, 1.5)
 * 
 *  내적 (Dot Product) 결과 : 스칼라 = (v1.x * v2.x) + (v1.y * v2.y) + (v1.z * v2.z) 캐릭터 대가리 돌리기
 *  
 *  외적 (cross Product) 결과는 백터 두 백터가 이루는 평면에 수직한 벡터(노멀 벡터)
 *  3D - 카메라에 보여지는 부분만 랜더링 한다.
 *  
 *  
 *  행렬 Transform 을 저장하는데 행렬이 사용된다.
 *  I, identify = 단위행렬 = 다른 행렬에 곱해도 값의 변화가 없다. 숫자에서 1과 같은 개념
 *                역행렬(-1) = 어떤 행렬에 역행렬을 곱하면 단위행렬이 도출된다.
 *                
 *  Transform = 변환행렬
 *  
 *              이동변환  -- (1, 0, 0, x)            
 *              회전변환
 *              스케일변환
 *              SRT - Scale, Rotate, Transform
 */