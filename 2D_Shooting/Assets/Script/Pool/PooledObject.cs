using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트풀에 들어갈 오브젝트들이 상속받을 클래스
/// </summary>
public class PooledObject : MonoBehaviour
{
    public Action onDisable;

    protected virtual void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
  
    protected virtual void OnDisable()
    {
        onDisable?.Invoke(); //비활성화됐다고 알림
    }
    protected IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
