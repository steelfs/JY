using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//WayPoints Ŭ������ ����ϴ� Ŭ���� //��� or etc
//��������Ʈ�� ���� �̵���
public class WayPointUser : MonoBehaviour
{
    public WayPoints targetWayPoints;// �� ������Ʈ�� ��������� ��������Ʈ���� �����ϴ� Ŭ����

    public float moveSpeed = 5.0f;

    Transform target; //���� ��ǥ���ϴ� Ʈ������
    Vector3 moveDir; // target���� ���� ����

    protected Vector3 moveDelta = Vector3.zero; //�̹� ���������ӿ� �̵��� ����
    protected virtual Transform Target //������ ���� ������Ƽ
    {
        get => target;
        set
        {
            target = value;
            moveDir = (target.position - transform.position).normalized; //���⼳��
        }
    }
    bool IsArrived//������ġ�� ���������� ���������� true
    {
        get
        {
            return (target.position - transform.position).sqrMagnitude < 0.02f;
        }
    }
    private void Start()
    {
        Target = targetWayPoints.currentWayPoint;
    }
    private void FixedUpdate()
    {
        OnMove();
    }
    protected virtual void OnArrived()//�������� �� ����Ǵ� �Լ�
    {
        Target = targetWayPoints.GetNextWayPoint();
    }
    protected virtual void OnMove() //�̵�ó���� �Լ� FixedUpdate���� ȣ��
    {
        moveDelta = Time.fixedDeltaTime * moveSpeed * moveDir;
        transform.Translate(moveDelta, Space.World);
        if (IsArrived)
        {
            OnArrived();
        }
    }
    
}
