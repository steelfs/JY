using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP_Bar : MonoBehaviour
{
    Transform fillPivot;// HP bar�� �θ� Ʈ������
    Enemy target;
    private void Awake()
    {
        fillPivot = transform.GetChild(1).GetComponent<Transform>();
        target = GetComponentInParent<Enemy>();
        target.onHealthChange += Refresh; //�� HP�� �پ��� ������ ����
    }
    private void Start()
    {
        target.onDie += () => Destroy(this.gameObject);
    }
    //lossyScale ���� ����, localScale
    private void Refresh(float ratio)
    {
        fillPivot.localScale = new Vector3(ratio, 1, 1);
    }
    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
