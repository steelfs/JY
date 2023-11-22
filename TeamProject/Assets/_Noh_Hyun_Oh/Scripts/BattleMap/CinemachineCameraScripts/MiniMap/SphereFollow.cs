using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̴ϸʿ� ���� ������ �� ��Ʈ���� ��ũ��Ʈ
/// </summary>
public class SphereFollow : MonoBehaviour
{
    Transform followTarget;
    [SerializeField]
    Vector3 dir = new Vector3(0.0f,42.0f,0.0f);
    private void Awake()
    {
        followTarget = transform.parent;
    }
    private void Update()
    {
        transform.position = followTarget.position + dir;
    }
}
