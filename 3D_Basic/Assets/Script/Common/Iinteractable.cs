using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�������̽� : � Ŭ������ �ݵ�� ��� ��ɵ��� �����־���Ѵٰ� ������ ���� ��.
//�������̽��� �⺻������ public 
//�������̽��� ����ϴµ� ������ ������ ����.
//�������̽��� ���� �ִ�.(������ ���ԵǾ����� �ʴ�)
//�������̽����� ������ �� �� ����.
//�������̽��� ��ӹ��� Ŭ������ �ݵ�� �������̽� ������� �����ؾ� �Ѵ�.

public interface Iinteractable
{
    bool IsDirectUse // ��ȣ�ۿ밡���� ������Ʈ�� ������밡���� ������, ������밡���� ������ǥ���ϱ� ���� ������Ƽ
    {
        get;
    }
    void Use();//����ϴ� ����� �ִٰ� ������ ������
    
}