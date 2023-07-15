using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : PlatformBase, Iinteractable
{
    public bool IsDirectUse => true;
    bool isMoveStart = false;
    private void Start()
    {
        Target = targetWayPoints.GetNextWayPoint();// ù��° ��������Ʈ�� �����ϱ� ���ϸ� OnArrived ����Ǽ� �ȿ�����
    }
    private void FixedUpdate()
    {
        if (isMoveStart)
        {
            OnMove();
        }
    }
    protected override void OnArrived()
    {
        base.OnArrived();
        isMoveStart = false;
    }

    // �ߵ� ���� : �÷��̾� ���� F��ǲ�� �޾� �ִϸ��̼ǹߵ� -> �ִϸ��̼ǹߵ��Ǹ� �ݶ��̴� ������Ʈ Ȱ��ȭ ->  ItemUseChecker�� OntriggerEnter �ߵ�-> OntriggerEnter���� do while ������
    //Iinteractable �� ��ӹ��� �����ִ��� �ֻ���� �θ���� �˻�  ã�Ҵٸ� ã�� Iinteractable�� �Ķ���ͷ� ��������Ʈ ��ȣ�� ������ �÷��̾��� ItemUse�� �����Ŵ -> �÷��̾��� Use�Լ����� Iinteractable.Use����
    // bool ����  ���� -> �̵�
    public void Use()
    {
        isMoveStart = true;
    }
}
