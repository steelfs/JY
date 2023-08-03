using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    Transform[] children;//각 웨이포인트의 배열
    int index = 0;//현재 찾아갈 인덱스

    public Transform Current => children[index];
    private void Awake()
    {
        children = new Transform[transform.childCount];
        for (int  i = 0; i < children.Length; i++)
        {
            children[i] = transform.GetChild(i);    //자식 찾아서 배열에 할당
        }
    }
    public Transform MoveNext()
    {
        index++;
        index %= children.Length;
        return children[index];
    }
}
