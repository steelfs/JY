using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ����Ƽ Awake OnEnable OnDisable �߻� ���� �׽�Ʈ
/// ���� 
/// 1. Hierarchy â�� �θ������Ʈ�� Ȱ��ȭ ��Ȱ��ȭ �������� ���� �׾ȿ� �ڽĿ�����Ʈ�� ��Ȱ��ȭ Ȱ��ȭ �ϳ��� �д�. 
/// 2. Instantiate(obj,transform)  �׽�Ʈ   transform ��ü�� Ȱ��ȭ�϶� ��Ȱ��ȭ�� �� �׽�Ʈ 
///
/// ��� 
/// 1. Hierarchy â�� �̸� ���õ� ������Ʈ���� ��Ȱ��ȭ ���ִ� ��ü (�ڽİ�ü����) �� 
///     ���ε��� �Ǵ��� Ȱ��ȭ �ϱ������� Awake �����̺�Ʈ(enable,start��)�� �ߵ����Ѵ� 
/// 2. Instantiate �Լ��� ���������� ���ڰ� transform �� �����ҽ� transform �� ��ü�� ��Ȱ��ȭ ���ִ°�� Awake �� �ߵ��� ���Ѵ�.
/// 3. new �� ���ӿ�����Ʈ�� �����ҽ� 
/// </summary>
public class AwakeTimingTest : TestBase
{
    //�θ� Ŭ����
    public GameObject ondisable;
    public GameObject onenable;
    
    //�θ� ��Ȱ��ȭ�϶� �ڽ� 
    public GameObject disableChildEnable;  
    public GameObject disableChildDisable;
    
    //�θ� Ȱ��ȭ �϶� �ڽ�
    public GameObject enableChlidEnable;
    public GameObject enableChildDisable;

    protected override void Awake()
    {
        base.Awake();
        System.Type[] a = { typeof(OnEnableOnDisableTest), typeof(OnEnableOnDisableTest) }; //���۳�Ʈ������ Ŭ������ Ÿ�� �� �迭�� ��Ƽ� 
        GameObject go = new GameObject("Test", a); //���� �Ҷ� ���۳�Ʈ�� ����������ִ�. 
        Debug.Log(go.activeSelf);//new �� �����Ѱ͵��� ���� Ȱ��ȭ ���·� ����ȴ�.
    }

    /// <summary>
    /// �θ� ��Ȱ��ȭ�϶� �ڽ��� ���� Ȱ��ȭ ���Ѻ��� 
    /// �ڽ��� ��Ȱ��ȭ 1�� Ȱ��ȭ 1���� ���� 
    /// </summary>
    protected override void Test1(InputAction.CallbackContext context)
    {
        disableChildDisable.gameObject.SetActive(true);
        disableChildEnable.gameObject.SetActive(true);
        //�θ� ��Ȱ��ȭ�� �ƹ��� �ڽ��������ѵ� �̺�Ʈ��������
    }
    /// <summary>
    /// �θ� ��Ȱ��ȭ�϶� �ڽ��� ���� ��Ȱ��ȭ ���Ѻ��� 
    /// �ڽ��� ��Ȱ��ȭ 1�� Ȱ��ȭ 1���� ���� 
    /// </summary>
    protected override void Test2(InputAction.CallbackContext context)
    {
        disableChildDisable.gameObject.SetActive(false);
        disableChildEnable.gameObject.SetActive(false);
        //�θ� ��Ȱ��ȭ�� �ƹ��� �ڽ��������ѵ� �̺�Ʈ��������
    }
    /// <summary>
    /// �θ� Ȱ��ȭ�϶� �ڽ��� ���� Ȱ��ȭ ���Ѻ��� 
    /// �ڽ��� ��Ȱ��ȭ 1�� Ȱ��ȭ 1���� ���� 
    /// </summary>


    protected override void Test3(InputAction.CallbackContext context)
    {
        enableChildDisable.gameObject.SetActive(true);
        enableChlidEnable.gameObject.SetActive(true);
        //���������� ��Ȱ��ȭ �Ǽ� ���۵Ȱ�ü���� Awake ���� ����ȴ�.
    }
    /// <summary>
    /// �θ� Ȱ��ȭ�϶� �ڽ��� ���� ��Ȱ��ȭ ���Ѻ��� 
    /// �ڽ��� ��Ȱ��ȭ 1�� Ȱ��ȭ 1���� ���� 
    /// </summary>
    protected override void Test4(InputAction.CallbackContext context)
    {
        enableChildDisable.gameObject.SetActive(false);
        enableChlidEnable.gameObject.SetActive(false);
    }
    /// <summary>
    /// �θ� ��Ȱ��ȭ�϶� �ڽ��� Instantiate�� ���� ���Ѻ��� 
    /// </summary>
    /// 


    ///��Ȱ��ȭ �� ��ü�� instantiate �� ����Ͽ� ����־����� Awake OnEnable �� ����ǰ� ������ Ȯ�� 
    ///
    protected override void Test5(InputAction.CallbackContext context)
    {
       Instantiate(disableChildEnable,ondisable.transform); //�θ� ��Ȱ��ȭ ���¸� �ڽ����� �־����� Awake ���� �ߵ�����
       Instantiate(disableChildDisable,ondisable.transform);
    }
    /// <summary>
    /// �θ� ��Ȱ��ȭ�϶� �ڽ��� Instantiate�� ���� ���Ѻ��� 
    /// </summary>
    protected override void Test6(InputAction.CallbackContext context)
    {
        Instantiate(disableChildEnable, onenable.transform);
        Instantiate(disableChildDisable, onenable.transform);
    }
}

