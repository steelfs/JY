using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��������Ʈ���� �����ϴ� Ŭ����
// �� ��������Ʈ���� ����ϴ� ������Ʈ���� ���� ���������� �˷��ִ� ����
public class WayPoints : MonoBehaviour
{
    Transform[] wayPoints;

    int index = 0; //�����̵����� ��������Ʈ�� �ε���
    public Transform currentWayPoint => wayPoints[index]; //�����̵����� ��������Ʈ

    private void Awake()
    {
        wayPoints = new Transform[transform.childCount];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = transform.GetChild(i); //�ڽĵ� ã�Ƴ���
        }
    }
    public Transform GetNextWayPoint()//���� ��������Ʈ�� �����ְ� currentWayPoint�� �����ϴ� �Լ�
    {
        index++;
        index %= wayPoints.Length; // �������� �ݵ�� ���� �� ���� ���� �� �ۿ� ���� ���� �̿��Ѵ�. index�� length�� �������� 0�� �ȴ�.

        return wayPoints[index];
    }
}
