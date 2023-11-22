using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTest : MonoBehaviour
{
    [SerializeField]
    Transform target;
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }
}
