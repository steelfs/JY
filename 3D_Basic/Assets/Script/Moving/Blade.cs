using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : WayPointUser
{
    Transform bladeMesh;
    public float rotateSpeed = 720.0f;
    protected override Transform Target
    {
        //get => base.Target;
        set
        {
            base.Target = value;
            transform.LookAt(Target);
        }
    }

    private void Awake()
    {
        bladeMesh = transform.GetChild(0);
    }
    private void Update()
    {
        bladeMesh.Rotate(Time.deltaTime * rotateSpeed * Vector3.right);
    }
    //1.�̵������� �ٶ�����Ѵ�.
    //���� ȸ���ؾ��Ѵ�.
    //�÷��̾� �ǰݽ� ���
    //protected override void OnArrived()
    //{
    //    base.OnArrived();
    //    //targetWayPoints.currentWayPoint �� vector3�� �ٲ㼭 LookRotation�� �Ķ���ͷ� �Ѱܾ��ϴµ� ����� �������� �ʴ´�
    //    //targetWayPoints.currentWayPoint.position ���� ������ �ߴµ� ȸ�� ������ ���� �ʴ´�. 
    //    //������ ���� LookRotation�� �Ķ���ͷδ� Ÿ���� �������� �ѱ�°� �ƴ϶� Ÿ��������"����"�� �Ѱ�����Ѵ�.

    //    //transform,rotation ���� ���ʹϾ� Ÿ���� �־�����ϱ� ������ ���� ���ʹϾ� Ÿ���� ������ ���� ����� ����Ѵ�.
    //    Vector3 angle = targetWayPoints.currentWayPoint.position - transform.position;
    //    Quaternion rotation = Quaternion.LookRotation(angle, Vector3.up);
    //    transform.rotation = rotation;//������ ������ ������� ���������� ���� �ö󰣴�
    //}
    private void OnCollisionEnter(Collision collision)//���ʿ��� �ڵ������ �����ϱ� ���� �߰� ���̾�� ��) Ʈ���� �׶��� �浹�� ���� ���ڵ尡 �����
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.Die();
        }

    }
}
