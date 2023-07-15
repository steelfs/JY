using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCurve : EnemyBase
{
    [Header("Curve Enemy data")]
    public float rotateSpeed = 10.0f;
    float curveDir = 0.0f;
    float? startY = null;
 
    public float StartY
    {
        set
        {
            if (startY == null) //startY�� �̴ϼȶ������ null�� �ʱ�ȭ�ϰ�  ���϶��� ��������
            {
                startY = value;
                if (startY > 0)
                {
                    curveDir = 1.0f; // ������ �����ϸ� �Ʒ��� Ŀ��, ��ȸ��
                }
                else
                {
                    curveDir = -1.0f;// �Ʒ����� �����ϸ� ���� Ŀ��
                }
            }
   
        }
    }
    protected override void OnInitialize()
    {
        base.OnInitialize();
        startY = null;

    }
    protected override void OnMoveUpdate()
    {
        base.OnMoveUpdate();
        transform.Rotate(Time.deltaTime * rotateSpeed * curveDir * Vector3.forward);
    }
}
