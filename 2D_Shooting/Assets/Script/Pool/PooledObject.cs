using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ƮǮ�� �� ������Ʈ���� ��ӹ��� Ŭ����
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
        onDisable?.Invoke(); //��Ȱ��ȭ�ƴٰ� �˸�
    }
    protected IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
