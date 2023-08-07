using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP_Bar : MonoBehaviour
{
    Transform fillPivot;// HP bar의 부모 트렌스폼
    Enemy target;
    private void Awake()
    {
        fillPivot = transform.GetChild(1).GetComponent<Transform>();
        target = GetComponentInParent<Enemy>();
        target.onHealthChange += Refresh; //적 HP가 줄어들면 스케일 조정
    }
    private void Start()
    {
        target.onDie += () => Destroy(this.gameObject);
    }
    //lossyScale 월드 기준, localScale
    private void Refresh(float ratio)
    {
        fillPivot.localScale = new Vector3(ratio, 1, 1);
    }
    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
