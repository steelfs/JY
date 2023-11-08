using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shell : MonoBehaviour
{
    //생성되면 즉시 앞으로 나간다.
    //포탄이 아닌 다른것과 부딫히면 폭발
    // 주변에 폭발력을 전달
    // 터지는 이펙트
    // 터지는 효과 VFX로 만들기
    // 포탄, 폭발이펙트는 팩토리, 오브젝트풀로 생성
    //

    Rigidbody rb;
    public float force = 50.0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        Shoot();
    }
    void Shoot()
    {
        Vector3 force_ = transform.forward * force;
        force_.y = 5;
        rb.AddForce(force_, ForceMode.Impulse);
    }
}
