using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 턴 순번을 관리해줄 클래스
/// 
/// 1.링크드 리스크사용시 Find 를 하기위해선 객체가 생성이되야하는데 
///     MonoBehaviour 를 상속받은 클래스는 new 키워드로 생성 이안되고  AddComponent 로 만들어야한다.
///     MonoBehaviour 테스트데이터 만들때 new 로 생성했더니 값이 null 이 들어가서 LinkedList Find 함수가 제대로 위치를 찾지 못하였음
/// 
/// TurnBaseData 상속받은 객체에서 턴끝났을경우 그개체의 값이 수정되고 수정된이후 그값을 가지고 다시 재정렬을 한다.
/// 
/// 링크드리스크를 다시 배열 혹은 리스트로 바꾸는법
/// List<ISortBase> b =  turnObjectList.ToList<ISortBase>(); 리스트로 바꾸는방법
/// ISortBase[] a =  turnObjectList.ToArray<ISortBase>();    배열로 바꾸는방법 
/// 
/// </summary>
/// 
/// <summary>
/// 정렬방법 오름차순(ASCENDING), 내림차순(DESCENDING)
/// </summary>
public enum SortType
{
    Ascending = 0, //오름차순
    Descending     //내림차순
}

public class TurnManager : ChildComponentSingeton<TurnManager>
{

    /// <summary>
    /// 현재 턴인 턴유닛
    /// </summary>
    ITurnBaseData currentTurn;
    public ITurnBaseData CurrentTurn => currentTurn;

    /// <summary>
    /// 다음턴유닛을 담아두기위한 변수 추가
    /// 한턴씩 진행하기위한 객체값
    /// </summary>
    ITurnBaseData nextTurn;

    /// <summary>
    /// 턴게이지 보여줄지 여부
    /// </summary>
    [SerializeField]
    private bool isViewGauge = false;
    public bool IsViewGauge => isViewGauge;

    /// <summary>
    /// 턴관리할 링크드 리스트
    /// </summary>
    LinkedList<ITurnBaseData> turnObjectList = null;
    public LinkedList<ITurnBaseData> TurnObjectList => turnObjectList;
    /// <summary>
    /// 전체리스트의 정렬방식을 정한다. 값이 큰것부터 턴이시작되야되면 Descending 작은것부터 시작되야되면 Ascending 을 넣어주면된다
    /// true = 오름차순 (- ~ +) , false 내림차순 (+ ~ -)
    /// </summary>
    [SerializeField]
    SortType isAscending = SortType.Descending;


    /// <summary>
    /// 현재까지 진행된 턴의값
    /// </summary>
    private int turnIndex = 0;
    public int TurnIndex => turnIndex;

    ///// <summary>
    ///// 턴시작의 최소값
    ///// </summary>
    //[SerializeField]
    //[Range(1.0f,10.0f)]
    //private float turnStartValue = 10.0f;

 
    /// <summary>
    /// 시간제한이 있을경우 제한턴의 값
    /// </summary>
    //private int maxTurnValue = 0;

    /// <summary>
    /// 배틀 맵일경우 데이터 초기화가 이루어진뒤에 호출이 되야된다 .
    /// 캐릭터 데이터가 전부생성이된상태일때 인자값으로받을지 결정한다.
    /// </summary>
    public void InitTurnData(ITurnBaseData[] teamList) {
        turnIndex = 0; //턴값 초기화
        

        turnObjectList = new LinkedList<ITurnBaseData>(teamList);//링크드 리스트 초기화

        foreach (ITurnBaseData team in turnObjectList) 
        {
            team.TurnEndAction = TurnEnd;
            team.TurnRemove = TurnListDeleteObj; //턴진행중 삭제될 유닛이 있으면 삭제함수를 연결시킨다.
        }
        nextTurn = turnObjectList.First.Value; //처음 턴유닛을 찾아와서 담아두고 

        TurnStart();//턴시작
    }

    

    /// <summary>
    /// 턴시작할 오브젝트를 가져와서 시작함수를 호출한다.
    /// </summary>
    private void TurnStart() {

        turnIndex++; //턴시작마다 카운트 시킨다.

        currentTurn = nextTurn;     //다음 턴유닛을 현재턴 으로 변경시킨다.
        nextTurn = GetNextTurnObject(); //다음 턴유닛을 담아둔다 
        currentTurn.TurnStartAction();  //턴시작을 알린다

        //밑에는 한턴씩 주고받는게아니라 턴액션밸류별로 구분할때 사용 
        //if (turnStartValue < currentTurn.TurnActionValue) //턴진행 할수있는 값이 됬으면 턴진행
        //{
        //}
        //else  //아니면 턴을 종료해서 행동력값을 증가시킨다.
        //{
        //    TurnEnd();
        //}
        
    }

    /// <summary>
    /// 싱글 링크드 리스트라 노드가 2개면 다음노드를 못찾는다 그러니 체크추가
    /// 더블 링크드 리스트로 변경이 필요해보이는데 이건 만들어야되니 시간나면 하자.
    /// 링크드리스트가 두개인경우만 처리한다
    /// </summary>
    /// <returns>이전값 혹은 이후값 </returns>
    private ITurnBaseData GetNextTurnObject() 
    {
        LinkedListNode<ITurnBaseData> tempNode = turnObjectList.Find(currentTurn);
        if (tempNode.Next != null) 
        {
            return tempNode.Next.Value;
        }
        return tempNode.Previous.Value;
    }

    /// <summary>
    /// 턴종료시 실행할 내용
    /// </summary>
    /// <param name="turnEndObj">턴종료한 유닛</param>
    private void TurnEnd()
    {
        GameManager.Inst.ChangeCursor(false);
        currentTurn.IsTurn = false; //턴종료를 설정한다 

        SetTurnValue();// 턴종료시마다 리스트의 유닛들의 행동력 값을 추가해주는 기능

        SortComponent<ITurnBaseData>.BubbleSortLinkedList(turnObjectList , isAscending); //값이변경이 됬음으로 전체 재정렬
        //TurnSorting(currentTurn); // 값이 변경된 오브젝트의 정렬기능 실행 -- 턴종료마다 행동력증가폭이 같으면 해당함수가 실행되는의미가있다.
        
        

        //추가되는 행동력 값이 전부다르다는 전제하에 전체정렬을 재시도 


        TurnStart(); // 다음턴 실행
    }

    

    /// <summary>
    /// 턴이끝난뒤 턴리스트의 오브젝트들의 행동력을 추가시킨다.
    /// </summary>
    private void SetTurnValue() {

        foreach (ITurnBaseData node in turnObjectList) //턴이끝날때마다 
        {
            node.TurnActionValue += node.TurnEndActionValue;//리스트의 대기자들은 활동력을 추가 시킨다.
           
        }
    }


    /// <summary>
    /// 턴관리할 오브젝트가 사라질경우
    /// 턴진행리스트 에서 사라져야할 것들 제거
    /// </summary>
    /// <param name="deleteTurnObject">리스트에서 지울 턴</param>
    public void TurnListDeleteObj(ITurnBaseData deleteTurnObject)
    {
        if (deleteTurnObject.CharcterList.Count < 1) //턴오브젝트의 관리할유닛이 없는경우 
        {
            deleteTurnObject.ResetData();//턴오브젝트도 초기화 시키고 
            turnObjectList.Remove(deleteTurnObject);//리스트에서 삭제
        }
        else 
        {
            Debug.Log($"아직 유닛이 {deleteTurnObject.CharcterList.Count}명 남아있습니다 ");
        }
        
    }

    /// <summary>
    /// 턴관리할 오브젝트가 추가될경우 
    /// 턴리스트에 추가될 값으로 재정렬 
    /// </summary>
    /// <param name="addObject">턴이 새롭게 추가된 객체</param>
    public void TurnListAddObject(ITurnBaseData addObject)
    {
        if (addObject == null) return; // 추가할값이없으면 리턴 

        LinkedListNode<ITurnBaseData> checkNode = turnObjectList.First; //첫번째 노드 가져와서

        for (int i = 0; i < turnObjectList.Count; i++)//리스트 한번돌정도로 포문을돌리고
        {
            if (SortComponent<ITurnBaseData>.SortAscDesCheck(addObject, checkNode.Value, isAscending))//인자로받은 노드값이 리스트의 노드값과비교를해서 정렬기준을정한다. 
            {
                turnObjectList.AddBefore(checkNode, addObject); // 그앞단에 추가를해버린다.
                break;//추가했으면 빠져나간다.
            }
            else if (i == turnObjectList.Count - 1) //마지막까지 비교값이 정해지지않았으면  
            {
                turnObjectList.AddLast(addObject); //맨마지막에 추가를한다. 
                break;//추가했으면 빠져나간다.
            }
            checkNode = checkNode.Next; //값이 비교되지 않으면 다음 노드를 찾는다.
        }
    }

    /// <summary>
    /// 턴관련 데이터 초기화 
    /// </summary>
    public void ResetBattleData() 
    {
        if (turnObjectList == null) return; // 턴리스트가 존재하지 않으면 빠져나가기

        foreach (ITurnBaseData node in turnObjectList)  // 턴데이터 돌면서
        {
            node.ResetData(); //턴유닛의 데이터 초기화 
        }
        turnObjectList.Clear();//리스트 비우기 

        currentTurn = null; // 진행중인 턴유닛도 없애고 
        turnIndex = 0; //현재 진행된 턴값도 초기화 
    }

    /// <summary>
    /// 리스트와 Array 의 기본정렬기능을 이용한 정렬 
    /// 인자값의 자료형은 맞춰줘야한다 반환값은 int -1 0 1
    /// </summary>
    /// <param name="before">앞의값</param>
    /// <param name="after">뒤의값</param>
    /// <returns>비교 결과</returns>
    private int SortComparison(ITurnBaseData before, ITurnBaseData after)
    {
        if (before.TurnActionValue < after.TurnActionValue)  //두값을 비교 
        {
            return isAscending == SortType.Ascending ? -1 : 1;  //오름 차순이면 -1 내림차순이면 1 
        }
        else if (before.TurnActionValue > after.TurnActionValue)
        {
            return isAscending == SortType.Descending ? 1 : -1; //내림차순이면 1 오름 차순이면 -1
        }
        else
        {
            return 0;
        }
    }


    /// <summary>
    /// 제한 : 순차적으로 정렬된 상태여야 하고 이상황이 아닌상태에서 사용시 인덱스가 꼬일수있다.
    /// 리스트는 이미 정렬이된상태이기때문에 한쪽방향으로만 탐색하여 위치를교체한다. 
    /// 해당함수는 정렬이 다되있는 리스트에서 하나의 인덱스값이 변했을때 변한인덱스 를 정렬위치에 집어넣는기능이다 
    /// </summary>
    /// <param name="turnEndData">턴사용이 완료된 객체</param>
    private void TurnSorting(ITurnBaseData turnEndData)
    {

        LinkedListNode<ITurnBaseData> checkNode = turnObjectList.Find(turnEndData); //비교할 노드 가져와서

        LinkedListNode<ITurnBaseData> nextNode = checkNode.Next;//비교할 다음노드를 미리가져온다.
        for (int i = 0; i < turnObjectList.Count; i++)//리스트 크기만큼 돌리고 
        {
            //Debug.Log(nextNode);
            if (nextNode == null)
            {
                Debug.Log($"{turnEndData.transform.name} 가 턴끝난뒤 정렬시  : 비교값 :{nextNode} :: 포문횟수 : {i} ::  링크드리스트 전체크기 :{turnObjectList.Count}");
                return;
            }
            if (SortComponent<ITurnBaseData>.SortAscDesCheck(checkNode.Value, nextNode.Value, isAscending)) // 값비교 오름차순이냐 내림차순이냐에따라 달라진다.
            {                                       //교체가 필요할경우 
                turnObjectList.Remove(checkNode);   //일단 노드지우고
                                                    //안지우면 에러남 InvalidOperationException: The LinkedList node already belongs to a LinkedList.
                                                    //노드안에 이미 같은게있다고 추가할수없다고함.
                turnObjectList.AddAfter(nextNode, checkNode); //노드의 뒤에 추가
                break;//위치가 변동이 있으면 빠져나간다 .
            }
            //교체가 필요없는경우 
            nextNode = nextNode.Next;   //다음노드를 찾는다
        }

    }

   
}
