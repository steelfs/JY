using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class AttackArea : MonoBehaviour
{
    public SphereCollider coll; //Play상태가 아닐때도 참조하도록 public 

    public Action<IBattle> onPlayerIn;
    public Action<IBattle> onPlayerOut;
    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IBattle battle = other.GetComponent<IBattle>();
            onPlayerIn?.Invoke(battle);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IBattle battle = other.GetComponent<IBattle>();
            onPlayerOut?.Invoke(battle);
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        if (coll != null)
        {
            Handles.DrawWireDisc(transform.position, transform.up, coll.radius, 2.0f);
        }

    }
#endif
}
