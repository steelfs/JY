using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 유니티 Awake OnEnable OnDisable 발생 조건 테스트
/// 셋팅 
/// 1. Hierarchy 창에 부모오브젝트가 활성화 비활성화 두종류를 놓고 그안에 자식오브젝트로 비활성화 활성화 하나씩 둔다. 
/// 2. Instantiate(obj,transform)  테스트   transform 객체가 활성화일때 비활성화일 때 테스트 
///
/// 결과 
/// 1. Hierarchy 창에 미리 셋팅된 오브젝트들중 비활성화 되있는 객체 (자식객체포함) 는 
///     씬로딩이 되더라도 활성화 하기전까진 Awake 이하이벤트(enable,start등)가 발동안한다 
/// 2. Instantiate 함수도 마찬가지로 인자값 transform 을 셋팅할시 transform 의 객체가 비활성화 되있는경우 Awake 가 발동을 안한다.
/// 3. new 로 게임오브젝트를 생성할시 
/// </summary>
public class AwakeTimingTest : TestBase
{
    //부모 클래스
    public GameObject ondisable;
    public GameObject onenable;
    
    //부모가 비활성화일때 자식 
    public GameObject disableChildEnable;  
    public GameObject disableChildDisable;
    
    //부모가 활성화 일때 자식
    public GameObject enableChlidEnable;
    public GameObject enableChildDisable;

    protected override void Awake()
    {
        base.Awake();
        System.Type[] a = { typeof(OnEnableOnDisableTest), typeof(OnEnableOnDisableTest) }; //컴퍼넌트형식의 클래스의 타입 을 배열로 담아서 
        GameObject go = new GameObject("Test", a); //생성 할때 컴퍼넌트를 집어넣을수있다. 
        Debug.Log(go.activeSelf);//new 로 생성한것들은 전부 활성화 상태로 실행된다.
    }

    /// <summary>
    /// 부모가 비활성화일때 자식을 전부 활성화 시켜본다 
    /// 자식은 비활성화 1개 활성화 1개로 셋팅 
    /// </summary>
    protected override void Test1(InputAction.CallbackContext context)
    {
        disableChildDisable.gameObject.SetActive(true);
        disableChildEnable.gameObject.SetActive(true);
        //부모가 비활성화라서 아무리 자식을껏다켜도 이벤트가안켜짐
    }
    /// <summary>
    /// 부모가 비활성화일때 자식을 전부 비활성화 시켜본다 
    /// 자식은 비활성화 1개 활성화 1개로 셋팅 
    /// </summary>
    protected override void Test2(InputAction.CallbackContext context)
    {
        disableChildDisable.gameObject.SetActive(false);
        disableChildEnable.gameObject.SetActive(false);
        //부모가 비활성화라서 아무리 자식을껏다켜도 이벤트가안켜짐
    }
    /// <summary>
    /// 부모가 활성화일때 자식을 전부 활성화 시켜본다 
    /// 자식은 비활성화 1개 활성화 1개로 셋팅 
    /// </summary>


    protected override void Test3(InputAction.CallbackContext context)
    {
        enableChildDisable.gameObject.SetActive(true);
        enableChlidEnable.gameObject.SetActive(true);
        //마찬가지로 비활성화 되서 시작된객체들은 Awake 부터 실행된다.
    }
    /// <summary>
    /// 부모가 활성화일때 자식을 전부 비활성화 시켜본다 
    /// 자식은 비활성화 1개 활성화 1개로 셋팅 
    /// </summary>
    protected override void Test4(InputAction.CallbackContext context)
    {
        enableChildDisable.gameObject.SetActive(false);
        enableChlidEnable.gameObject.SetActive(false);
    }
    /// <summary>
    /// 부모가 비활성화일때 자식을 Instantiate로 생성 시켜본다 
    /// </summary>
    /// 


    ///비활성화 된 객체에 instantiate 를 사용하여 집어넣었을때 Awake OnEnable 이 실행되고 들어가는지 확인 
    ///
    protected override void Test5(InputAction.CallbackContext context)
    {
       Instantiate(disableChildEnable,ondisable.transform); //부모가 비활성화 상태면 자식으로 넣었을때 Awake 이하 발동안함
       Instantiate(disableChildDisable,ondisable.transform);
    }
    /// <summary>
    /// 부모가 비활성화일때 자식을 Instantiate로 생성 시켜본다 
    /// </summary>
    protected override void Test6(InputAction.CallbackContext context)
    {
        Instantiate(disableChildEnable, onenable.transform);
        Instantiate(disableChildDisable, onenable.transform);
    }
}

