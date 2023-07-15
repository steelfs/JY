using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//각종 문들의 기본 클래스
public class DoorBase : MonoBehaviour
{
    Animator anim;
    readonly int IsOpenHash = Animator.StringToHash("IsOpen"); //애니메이터에서 사용할 해시
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
    }
    protected virtual void OnOpen()
    {

    }
    protected virtual void OnClose()
    {

    }
    public void Open()
    {
        anim.SetBool(IsOpenHash, true);
        OnOpen();
    }
    public void Close()
    {
        anim.SetBool(IsOpenHash, false);
        OnClose();
    }
}
