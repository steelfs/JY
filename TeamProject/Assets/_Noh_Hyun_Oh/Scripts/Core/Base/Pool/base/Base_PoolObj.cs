using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ǯ���� �⺻ Ŭ����
/// </summary>
public class Base_PoolObj : MonoBehaviour
{
    /// <summary>
    /// Ȱ��ȭ �ϱ����� ó���� ���� (������ �ʱⰪ ���ÿ�)
    /// </summary>
    public Action onEnable_InitData;

    /// <summary>
    /// ��Ȱ��ȭ �ϱ����� ó���� ���� (������ ���� ��)
    /// </summary>
    public Action<Transform> onGetPoolTransform;

    /// <summary>
    /// ������ Ǯ��ġ�� ����صд�.
    /// </summary>
    protected Transform poolTransform;

    /// <summary>
    /// ��Ȱ��ȭ�� Queue�� ��ȯ ó���� �̷�������Ѵ�.
    /// ������Ʈ Ǯ�� �� ������Ʈ���� ��ӹ��� Ŭ���� 
    /// </summary>
    public Action onDisable;

    protected virtual void Awake() 
    {
        onGetPoolTransform = (pool_Transform) => 
                            { 
                                //Ǯ��ġ �����ϱ� 
                                this.poolTransform = pool_Transform; 
                            };
    }

    protected virtual void OnEnable()
    {
        //��ġ�� �ʱ�ȭ �⺻������ ������Ʈ�����Ҷ� Ʈ�������� 0,0,0 ���� �����ؾ��Ѵ�.
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    /// <summary>
    /// ��Ȱ��ȭ�� �θ� ����� ���������Ͽ� ť�� �ʱ�ȭ�Ҽ�����.
    /// </summary>
    protected virtual void OnDisable() 
    {
        onDisable?.Invoke(); //Queue �ʱ�ȭ�Ѵ�.
    }

    /// <summary>
    /// ���� �ð� �Ŀ� �� ���ӿ�����Ʈ�� ��Ȱ��ȭ ��Ű�� �ڷ�ƾ
    /// </summary>
    /// <param name="delay">��Ȱ��ȭ�� �ɶ����� �ɸ��� �ð�(�⺻ = 0.0f)</param>
    /// <returns></returns>
    protected virtual IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay); // delay��ŭ ����ϰ�
        gameObject.SetActive(false);            // ���� ������Ʈ ��Ȱ��ȭ
    }

}
