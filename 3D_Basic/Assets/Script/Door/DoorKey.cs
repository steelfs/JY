using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�� ������Ʈ�� ������ ����� ���� ������.
//�÷��̾�� �� ������Ʈ�� ������ �� ������Ʈ�� �������.
//�� ������Ʈ�� ���ڸ����� ���ۺ��� ����
public class DoorKey : MonoBehaviour
{
    public DoorBase target;
    public float rotateSpeed = 360.0f;

    Transform keyTransform;
    private void Awake()
    {
        keyTransform  = transform.GetChild(0);
    }
    private void Update()
    {
        keyTransform.Rotate(Time.deltaTime * rotateSpeed * Vector3.up);
        //keyTransform.RotateAround()
    }
    private void OnTriggerEnter(Collider other)
    {
        OnConsume();
    }

    private void OnValidate()
    {
        ResetTarget(target);
    }
    protected virtual void ResetTarget(DoorBase target)
    {
        if (target != null)
        {
            this.target = target as DoorAuto; // as : ���ʺ����� ������ ������ ������ null�� �ƴϰ�  ���� ������ null�� �����Ѵ�
                                         // is : ���ʺ����� ������ ������ ������ true, ���� ������ false�� �����Ѵ�
        }
    }
    protected virtual void OnConsume()
    {
        target.Open();
        Destroy(gameObject);
    }
}
