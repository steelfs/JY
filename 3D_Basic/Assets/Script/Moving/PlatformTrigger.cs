using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾ �ö󰡸� ���� �÷������� �����̴� �÷���
public class PlatformTrigger : PlatformBase
{
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
    private void OnTriggerEnter(Collider other) // �÷��̾ Ʈ���� �����ȿ� �������� �����̱� ����
    {
        isMoveStart = other.CompareTag("Player");
    }
    protected override void OnArrived()// �����ϸ� ����
    {
        base.OnArrived();
        isMoveStart = false;
    }

}
