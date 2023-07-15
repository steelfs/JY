using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���� ������ �⺻ Ŭ����
public class DoorBase : MonoBehaviour
{
    Animator anim;
    readonly int IsOpenHash = Animator.StringToHash("IsOpen"); //�ִϸ����Ϳ��� ����� �ؽ�
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
