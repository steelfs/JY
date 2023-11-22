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
    /// 턴 유닛 생성 테스트
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test1(InputAction.CallbackContext context)
    //{
    //    TurnBaseObject tbo = (TurnBaseObject)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_ENEMY_POOL); //가져오고
    //    RectTransform rt = tbo.gameObject.GetComponent<RectTransform>(); //랜덤으로 위치 뿌려주기위해 위치값정보가져오고
    //    rt = rt == null ? tbo.gameObject.AddComponent<RectTransform>() : rt; // 없으면 추가하고 
    //    GameObject parentObj = Instantiate(unit); // 테스트용 캐릭터 생성하고
    //    parentObj.transform.position = new Vector3(
    //                                    UnityEngine.Random.Range(-10.0f, 10.0f),
    //                                    0.0f,
    //                                    UnityEngine.Random.Range(-10.0f, 0.0f)
    //                                    );//랜덤위치로 뿌리고
    //    tbo.TurnActionValue = UnityEngine.Random.Range(0.0f, 8.0f); // -테스트값 설정
    //    tbo.transform.SetParent(parentObj.transform);//부모위치 옮기고 - 부모가 활성화 상태가 아닐경우 문제발생여부존재함
    //    rt.anchoredPosition3D = new Vector3(0.0f, 2.0f, 0.0f);// UI기본위치 살짝위로 옮기고 

    //    turnManager.TurnListAddObject(tbo); //턴리스트에 추가 
    //}

    /// <summary>
    /// 턴 유닛 삭제 테스트
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test2(InputAction.CallbackContext context)
    //{

    //    TurnBaseObject turnObj = GameObject.FindObjectOfType<TurnBaseObject>();// 유닛 찾아서 
    //    if (turnObj == null || turnObj.TurnEndAction != null) return; //유닛이 없거나  현재 진행중인 턴유닛이면 삭제가 안되야된다.
    //    GameObject obj = turnObj.transform.parent.gameObject; // 부모 찾아놓고
    //    turnObj.ResetData(); //데이터초기화 
    //    turnManager.TurnListDeleteObj(turnObj); //리스트 에서 삭제 
    //    GameObject.Destroy(obj); // 게임오브젝트 삭제 
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
    /// 턴 진행 
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test2(InputAction.CallbackContext context)
    //{
    //    ITurnBaseData node = TurnManager.Instance.CurrentTurn; //원래는 캐릭터 쪽에서 턴완료 버튼 호출해야하는데 캐릭터 가없음으로  테스트코드로 찾아온다.
    //    if (node == null) 
    //    {
    //        Debug.Log("왜못찾냐?");
    //        return;
    //    } 
    //    Debug.Log($"{node.gameObject.name} 옵젝 : 값 :{node.TurnActionValue} 함수가등록되있냐? : {node.TurnEndAction}");
    //    node.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f); //턴 진행 시 행동력 감소치 대충 때려넣는다.

    //    node.TurnEndAction(node); //턴완료 를 알린다.
    //}

    protected override void Test3(InputAction.CallbackContext context)
    {
        foreach (var charcter in TurnManager.Instance.CurrentTurn.CharcterList) 
        {
            Debug.Log($"{charcter.transform.name}의 타일은 :{charcter.CurrentTile.Width},{charcter.CurrentTile.length} "); 
        }
            
    }
    //protected override void Test3(InputAction.CallbackContext context)
    //{
    //    moveRange.MoveSizeDoubleView(TurnManager.Instance.CurrentTurn.CharcterList[0].CurrentTile, moveSize);
    //}
    /// <summary>
    /// 턴 내용 전부 초기화후 게임화면에있는 캐릭터 오브젝트 지우기
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test4(InputAction.CallbackContext context)
    //{
    //    turnManager.ResetBattleData(); //턴관리자의 데이터 리셋후
    //    TurnBaseObject[] ps = GameObject.FindObjectsOfType<TurnBaseObject>(); //대충만든 오브젝트 찾기
    //    foreach (TurnBaseObject p in ps)
    //    {
    //        GameObject.Destroy(p.gameObject); //오브젝트 삭제
    //    }
    //}
    /// <summary>
    /// 턴 초기화 함수 실행
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test5(InputAction.CallbackContext context)
    //{
    //    turnManager.ResetBattleData(); //리셋후
    //    TurnBaseObject[] ps = GameObject.FindObjectsOfType<TurnBaseObject>(); //대충만든 오브젝트 찾기
    //    foreach (TurnBaseObject p in ps)
    //    {
    //        GameObject.Destroy(p.gameObject); //오브젝트 삭제
    //    }
    //    turnManager.InitTurnData(ps);//초기데이터 셋팅 
    //}
    /// <summary>
    /// 턴리스트의 내용 출력 
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test6(InputAction.CallbackContext context)
    //{
    //    turnManager.ViewTurnList();//턴리스트 의 데이터를 정렬된 순서대로 출력 
    //}

    /// <summary>
    /// 랜덤한 캐릭터의 상태를 추가
    /// </summary>
    /// <param name="context"></param>
    //protected override void Test7(InputAction.CallbackContext context)
    //{
    //    TurnBaseObject tbo =  (TurnBaseObject)turnManager.RandomGetNode();
    //    //tbo.BattleUI.AddOfStatus(EnumList.StateType.Poison);//상태이상 추가해보기 
    //}



}
