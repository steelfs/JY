using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorManual : DoorBase, Iinteractable
{
    TextMeshPro text;
    public bool IsDirectUse => true;
    public float closeTime = 3.0f;
    protected override void Awake()
    {
        base.Awake();
        text = GetComponentInChildren<TextMeshPro>(true);
    }
    public void Use()
    {
        Open();
        StartCoroutine(AutoClose(closeTime));
    }
    IEnumerator AutoClose(float time)
    {
        yield return new WaitForSeconds(time);
        Close();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(true);
        }  
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);
        }
    }
}
