using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnTestCode : TestBase
{

    //Tile currentTile ;

    [SerializeField]
    InitCharcterSetting InitCharcterSetting;
    [SerializeField]
    MoveRange moveRange;

    protected override void Awake()
    {
        base.Awake();
        InitCharcterSetting = FindObjectOfType<InitCharcterSetting>();
        moveRange = FindObjectOfType<MoveRange>();
        
    }

    /// <summary>
    /// �� ���� ���� �׽�Ʈ
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test1(InputAction.CallbackContext context)
    //{
    //    TurnBaseObject tbo = (TurnBaseObject)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_ENEMY_POOL); //��������
    //    RectTransform rt = tbo.gameObject.GetComponent<RectTransform>(); //�������� ��ġ �ѷ��ֱ����� ��ġ��������������
    //    rt = rt == null ? tbo.gameObject.AddComponent<RectTransform>() : rt; // ������ �߰��ϰ� 
    //    GameObject parentObj = Instantiate(unit); // �׽�Ʈ�� ĳ���� �����ϰ�
    //    parentObj.transform.position = new Vector3(
    //                                    UnityEngine.Random.Range(-10.0f, 10.0f),
    //                                    0.0f,
    //                                    UnityEngine.Random.Range(-10.0f, 0.0f)
    //                                    );//������ġ�� �Ѹ���
    //    tbo.TurnActionValue = UnityEngine.Random.Range(0.0f, 8.0f); // -�׽�Ʈ�� ����
    //    tbo.transform.SetParent(parentObj.transform);//�θ���ġ �ű�� - �θ� Ȱ��ȭ ���°� �ƴҰ�� �����߻�����������
    //    rt.anchoredPosition3D = new Vector3(0.0f, 2.0f, 0.0f);// UI�⺻��ġ ��¦���� �ű�� 

    //    turnManager.TurnListAddObject(tbo); //�ϸ���Ʈ�� �߰� 
    //}

    /// <summary>
    /// �� ���� ���� �׽�Ʈ
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test2(InputAction.CallbackContext context)
    //{

    //    TurnBaseObject turnObj = GameObject.FindObjectOfType<TurnBaseObject>();// ���� ã�Ƽ� 
    //    if (turnObj == null || turnObj.TurnEndAction != null) return; //������ ���ų�  ���� �������� �������̸� ������ �ȵǾߵȴ�.
    //    GameObject obj = turnObj.transform.parent.gameObject; // �θ� ã�Ƴ���
    //    turnObj.ResetData(); //�������ʱ�ȭ 
    //    turnManager.TurnListDeleteObj(turnObj); //����Ʈ ���� ���� 
    //    GameObject.Destroy(obj); // ���ӿ�����Ʈ ���� 
    //}

    //protected override void Test1(InputAction.CallbackContext context)
    //{
    //    InitCharcterSetting.TestInit();
    //}
    //protected override void Test2(InputAction.CallbackContext context)
    //{
    //    InitCharcterSetting.TestReset();
    //}
    /// <summary>
    /// �� ���� 
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test2(InputAction.CallbackContext context)
    //{
    //    ITurnBaseData node = TurnManager.Instance.CurrentTurn; //������ ĳ���� �ʿ��� �ϿϷ� ��ư ȣ���ؾ��ϴµ� ĳ���� ����������  �׽�Ʈ�ڵ�� ã�ƿ´�.
    //    if (node == null) 
    //    {
    //        Debug.Log("�ָ�ã��?");
    //        return;
    //    } 
    //    Debug.Log($"{node.gameObject.name} ���� : �� :{node.TurnActionValue} �Լ�����ϵ��ֳ�? : {node.TurnEndAction}");
    //    node.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f); //�� ���� �� �ൿ�� ����ġ ���� �����ִ´�.

    //    node.TurnEndAction(node); //�ϿϷ� �� �˸���.
    //}

    protected override void Test3(InputAction.CallbackContext context)
    {
        foreach (var charcter in TurnManager.Instance.CurrentTurn.CharcterList) 
        {
            Debug.Log($"{charcter.transform.name}�� Ÿ���� :{charcter.CurrentTile.Width},{charcter.CurrentTile.length} "); 
        }
            
    }
    //protected override void Test3(InputAction.CallbackContext context)
    //{
    //    moveRange.MoveSizeDoubleView(TurnManager.Instance.CurrentTurn.CharcterList[0].CurrentTile, moveSize);
    //}
    /// <summary>
    /// �� ���� ���� �ʱ�ȭ�� ����ȭ�鿡�ִ� ĳ���� ������Ʈ �����
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test4(InputAction.CallbackContext context)
    //{
    //    turnManager.ResetBattleData(); //�ϰ������� ������ ������
    //    TurnBaseObject[] ps = GameObject.FindObjectsOfType<TurnBaseObject>(); //���游�� ������Ʈ ã��
    //    foreach (TurnBaseObject p in ps)
    //    {
    //        GameObject.Destroy(p.gameObject); //������Ʈ ����
    //    }
    //}
    /// <summary>
    /// �� �ʱ�ȭ �Լ� ����
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test5(InputAction.CallbackContext context)
    //{
    //    turnManager.ResetBattleData(); //������
    //    TurnBaseObject[] ps = GameObject.FindObjectsOfType<TurnBaseObject>(); //���游�� ������Ʈ ã��
    //    foreach (TurnBaseObject p in ps)
    //    {
    //        GameObject.Destroy(p.gameObject); //������Ʈ ����
    //    }
    //    turnManager.InitTurnData(ps);//�ʱⵥ���� ���� 
    //}
    /// <summary>
    /// �ϸ���Ʈ�� ���� ��� 
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test6(InputAction.CallbackContext context)
    //{
    //    turnManager.ViewTurnList();//�ϸ���Ʈ �� �����͸� ���ĵ� ������� ��� 
    //}

    /// <summary>
    /// ������ ĳ������ ���¸� �߰�
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test7(InputAction.CallbackContext context)
    //{
    //    TurnBaseObject tbo =  (TurnBaseObject)turnManager.RandomGetNode();
    //    //tbo.BattleUI.AddOfStatus(EnumList.StateType.Poison);//�����̻� �߰��غ��� 
    //}



}
