using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 웨이포인트들을 관리하는 클래스
// 이 웨이포인트들을 사용하는 오브젝트에게 어디로 가야할지를 알려주는 역할
public class WayPoints : MonoBehaviour
{
    Transform[] wayPoints;

    int index = 0; //현재이동중인 웨이포인트의 인덱스
    public Transform currentWayPoint => wayPoints[index]; //현재이동중인 웨이포인트

    private void Awake()
    {
        wayPoints = new Transform[transform.childCount];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = transform.GetChild(i); //자식들 찾아놓기
        }
    }
    public Transform GetNextWayPoint()//다음 웨이포인트를 돌려주고 currentWayPoint를 지정하는 함수
    {
        index++;
        index %= wayPoints.Length; // 나머지는 반드시 나눈 수 보다 작을 수 밖에 없는 것을 이용한다. index가 length와 같아지면 0이 된다.

        return wayPoints[index];
    }
}
