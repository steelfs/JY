using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �� ������ �������� Ŭ����
/// 
/// 1.��ũ�� ����ũ���� Find �� �ϱ����ؼ� ��ü�� �����̵Ǿ��ϴµ� 
///     MonoBehaviour �� ��ӹ��� Ŭ������ new Ű����� ���� �̾ȵǰ�  AddComponent �� �������Ѵ�.
///     MonoBehaviour �׽�Ʈ������ ���鶧 new �� �����ߴ��� ���� null �� ���� LinkedList Find �Լ��� ����� ��ġ�� ã�� ���Ͽ���
/// 
/// TurnBaseData ��ӹ��� ��ü���� �ϳ�������� �װ�ü�� ���� �����ǰ� ���������� �װ��� ������ �ٽ� �������� �Ѵ�.
/// 
/// ��ũ�帮��ũ�� �ٽ� �迭 Ȥ�� ����Ʈ�� �ٲٴ¹�
/// List<ISortBase> b =  turnObjectList.ToList<ISortBase>(); ����Ʈ�� �ٲٴ¹��
/// ISortBase[] a =  turnObjectList.ToArray<ISortBase>();    �迭�� �ٲٴ¹�� 
/// 
/// </summary>
/// 
/// <summary>
/// ���Ĺ�� ��������(ASCENDING), ��������(DESCENDING)
/// </summary>
public enum SortType
{
    Ascending = 0, //��������
    Descending     //��������
}

public class TurnManager : ChildComponentSingeton<TurnManager>
{

    /// <summary>
    /// ���� ���� ������
    /// </summary>
    ITurnBaseData currentTurn;
    public ITurnBaseData CurrentTurn => currentTurn;

    /// <summary>
    /// ������������ ��Ƶα����� ���� �߰�
    /// ���Ͼ� �����ϱ����� ��ü��
    /// </summary>
    ITurnBaseData nextTurn;

    /// <summary>
    /// �ϰ����� �������� ����
    /// </summary>
    [SerializeField]
    private bool isViewGauge = false;
    public bool IsViewGauge => isViewGauge;

    /// <summary>
    /// �ϰ����� ��ũ�� ����Ʈ
    /// </summary>
    LinkedList<ITurnBaseData> turnObjectList = null;
    public LinkedList<ITurnBaseData> TurnObjectList => turnObjectList;
    /// <summary>
    /// ��ü����Ʈ�� ���Ĺ���� ���Ѵ�. ���� ū�ͺ��� ���̽��۵ǾߵǸ� Descending �����ͺ��� ���۵ǾߵǸ� Ascending �� �־��ָ�ȴ�
    /// true = �������� (- ~ +) , false �������� (+ ~ -)
    /// </summary>
    [SerializeField]
    SortType isAscending = SortType.Descending;


    /// <summary>
    /// ������� ����� ���ǰ�
    /// </summary>
    private int turnIndex = 0;
    public int TurnIndex => turnIndex;

    ///// <summary>
    ///// �Ͻ����� �ּҰ�
    ///// </summary>
    //[SerializeField]
    //[Range(1.0f,10.0f)]
    //private float turnStartValue = 10.0f;

 
    /// <summary>
    /// �ð������� ������� �������� ��
    /// </summary>
    //private int maxTurnValue = 0;

    /// <summary>
    /// ��Ʋ ���ϰ�� ������ �ʱ�ȭ�� �̷�����ڿ� ȣ���� �Ǿߵȴ� .
    /// ĳ���� �����Ͱ� ���λ����̵Ȼ����϶� ���ڰ����ι����� �����Ѵ�.
    /// </summary>
    public void InitTurnData(ITurnBaseData[] teamList) {
        turnIndex = 0; //�ϰ� �ʱ�ȭ
        

        turnObjectList = new LinkedList<ITurnBaseData>(teamList);//��ũ�� ����Ʈ �ʱ�ȭ

        foreach (ITurnBaseData team in turnObjectList) 
        {
            team.TurnEndAction = TurnEnd;
            team.TurnRemove = TurnListDeleteObj; //�������� ������ ������ ������ �����Լ��� �����Ų��.
        }
        nextTurn = turnObjectList.First.Value; //ó�� �������� ã�ƿͼ� ��Ƶΰ� 

        TurnStart();//�Ͻ���
    }

    

    /// <summary>
    /// �Ͻ����� ������Ʈ�� �����ͼ� �����Լ��� ȣ���Ѵ�.
    /// </summary>
    private void TurnStart() {

        turnIndex++; //�Ͻ��۸��� ī��Ʈ ��Ų��.

        currentTurn = nextTurn;     //���� �������� ������ ���� �����Ų��.
        nextTurn = GetNextTurnObject(); //���� �������� ��Ƶд� 
        currentTurn.TurnStartAction();  //�Ͻ����� �˸���

        //�ؿ��� ���Ͼ� �ְ�޴°Ծƴ϶� �Ͼ׼ǹ������ �����Ҷ� ��� 
        //if (turnStartValue < currentTurn.TurnActionValue) //������ �Ҽ��ִ� ���� ������ ������
        //{
        //}
        //else  //�ƴϸ� ���� �����ؼ� �ൿ�°��� ������Ų��.
        //{
        //    TurnEnd();
        //}
        
    }

    /// <summary>
    /// �̱� ��ũ�� ����Ʈ�� ��尡 2���� ������带 ��ã�´� �׷��� üũ�߰�
    /// ���� ��ũ�� ����Ʈ�� ������ �ʿ��غ��̴µ� �̰� �����ߵǴ� �ð����� ����.
    /// ��ũ�帮��Ʈ�� �ΰ��ΰ�츸 ó���Ѵ�
    /// </summary>
    /// <returns>������ Ȥ�� ���İ� </returns>
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
    /// ������� ������ ����
    /// </summary>
    /// <param name="turnEndObj">�������� ����</param>
    private void TurnEnd()
    {
        GameManager.Inst.ChangeCursor(false);
        currentTurn.IsTurn = false; //�����Ḧ �����Ѵ� 

        SetTurnValue();// ������ø��� ����Ʈ�� ���ֵ��� �ൿ�� ���� �߰����ִ� ���

        SortComponent<ITurnBaseData>.BubbleSortLinkedList(turnObjectList , isAscending); //���̺����� �������� ��ü ������
        //TurnSorting(currentTurn); // ���� ����� ������Ʈ�� ���ı�� ���� -- �����Ḷ�� �ൿ���������� ������ �ش��Լ��� ����Ǵ��ǹ̰��ִ�.
        
        

        //�߰��Ǵ� �ൿ�� ���� ���δٸ��ٴ� �����Ͽ� ��ü������ ��õ� 


        TurnStart(); // ������ ����
    }

    

    /// <summary>
    /// ���̳����� �ϸ���Ʈ�� ������Ʈ���� �ൿ���� �߰���Ų��.
    /// </summary>
    private void SetTurnValue() {

        foreach (ITurnBaseData node in turnObjectList) //���̳��������� 
        {
            node.TurnActionValue += node.TurnEndActionValue;//����Ʈ�� ����ڵ��� Ȱ������ �߰� ��Ų��.
           
        }
    }


    /// <summary>
    /// �ϰ����� ������Ʈ�� ��������
    /// �����ฮ��Ʈ ���� ��������� �͵� ����
    /// </summary>
    /// <param name="deleteTurnObject">����Ʈ���� ���� ��</param>
    public void TurnListDeleteObj(ITurnBaseData deleteTurnObject)
    {
        if (deleteTurnObject.CharcterList.Count < 1) //�Ͽ�����Ʈ�� ������������ ���°�� 
        {
            deleteTurnObject.ResetData();//�Ͽ�����Ʈ�� �ʱ�ȭ ��Ű�� 
            turnObjectList.Remove(deleteTurnObject);//����Ʈ���� ����
        }
        else 
        {
            Debug.Log($"���� ������ {deleteTurnObject.CharcterList.Count}�� �����ֽ��ϴ� ");
        }
        
    }

    /// <summary>
    /// �ϰ����� ������Ʈ�� �߰��ɰ�� 
    /// �ϸ���Ʈ�� �߰��� ������ ������ 
    /// </summary>
    /// <param name="addObject">���� ���Ӱ� �߰��� ��ü</param>
    public void TurnListAddObject(ITurnBaseData addObject)
    {
        if (addObject == null) return; // �߰��Ұ��̾����� ���� 

        LinkedListNode<ITurnBaseData> checkNode = turnObjectList.First; //ù��° ��� �����ͼ�

        for (int i = 0; i < turnObjectList.Count; i++)//����Ʈ �ѹ��������� ������������
        {
            if (SortComponent<ITurnBaseData>.SortAscDesCheck(addObject, checkNode.Value, isAscending))//���ڷι��� ��尪�� ����Ʈ�� ��尪���񱳸��ؼ� ���ı��������Ѵ�. 
            {
                turnObjectList.AddBefore(checkNode, addObject); // �׾մܿ� �߰����ع�����.
                break;//�߰������� ����������.
            }
            else if (i == turnObjectList.Count - 1) //���������� �񱳰��� ���������ʾ�����  
            {
                turnObjectList.AddLast(addObject); //�Ǹ������� �߰����Ѵ�. 
                break;//�߰������� ����������.
            }
            checkNode = checkNode.Next; //���� �񱳵��� ������ ���� ��带 ã�´�.
        }
    }

    /// <summary>
    /// �ϰ��� ������ �ʱ�ȭ 
    /// </summary>
    public void ResetBattleData() 
    {
        if (turnObjectList == null) return; // �ϸ���Ʈ�� �������� ������ ����������

        foreach (ITurnBaseData node in turnObjectList)  // �ϵ����� ���鼭
        {
            node.ResetData(); //�������� ������ �ʱ�ȭ 
        }
        turnObjectList.Clear();//����Ʈ ���� 

        currentTurn = null; // �������� �����ֵ� ���ְ� 
        turnIndex = 0; //���� ����� �ϰ��� �ʱ�ȭ 
    }

    /// <summary>
    /// ����Ʈ�� Array �� �⺻���ı���� �̿��� ���� 
    /// ���ڰ��� �ڷ����� ��������Ѵ� ��ȯ���� int -1 0 1
    /// </summary>
    /// <param name="before">���ǰ�</param>
    /// <param name="after">���ǰ�</param>
    /// <returns>�� ���</returns>
    private int SortComparison(ITurnBaseData before, ITurnBaseData after)
    {
        if (before.TurnActionValue < after.TurnActionValue)  //�ΰ��� �� 
        {
            return isAscending == SortType.Ascending ? -1 : 1;  //���� �����̸� -1 ���������̸� 1 
        }
        else if (before.TurnActionValue > after.TurnActionValue)
        {
            return isAscending == SortType.Descending ? 1 : -1; //���������̸� 1 ���� �����̸� -1
        }
        else
        {
            return 0;
        }
    }


    /// <summary>
    /// ���� : ���������� ���ĵ� ���¿��� �ϰ� �̻�Ȳ�� �ƴѻ��¿��� ���� �ε����� ���ϼ��ִ�.
    /// ����Ʈ�� �̹� �����̵Ȼ����̱⶧���� ���ʹ������θ� Ž���Ͽ� ��ġ����ü�Ѵ�. 
    /// �ش��Լ��� ������ �ٵ��ִ� ����Ʈ���� �ϳ��� �ε������� �������� �����ε��� �� ������ġ�� ����ִ±���̴� 
    /// </summary>
    /// <param name="turnEndData">�ϻ���� �Ϸ�� ��ü</param>
    private void TurnSorting(ITurnBaseData turnEndData)
    {

        LinkedListNode<ITurnBaseData> checkNode = turnObjectList.Find(turnEndData); //���� ��� �����ͼ�

        LinkedListNode<ITurnBaseData> nextNode = checkNode.Next;//���� ������带 �̸������´�.
        for (int i = 0; i < turnObjectList.Count; i++)//����Ʈ ũ�⸸ŭ ������ 
        {
            //Debug.Log(nextNode);
            if (nextNode == null)
            {
                Debug.Log($"{turnEndData.transform.name} �� �ϳ����� ���Ľ�  : �񱳰� :{nextNode} :: ����Ƚ�� : {i} ::  ��ũ�帮��Ʈ ��üũ�� :{turnObjectList.Count}");
                return;
            }
            if (SortComponent<ITurnBaseData>.SortAscDesCheck(checkNode.Value, nextNode.Value, isAscending)) // ���� ���������̳� ���������̳Ŀ����� �޶�����.
            {                                       //��ü�� �ʿ��Ұ�� 
                turnObjectList.Remove(checkNode);   //�ϴ� ��������
                                                    //������� ������ InvalidOperationException: The LinkedList node already belongs to a LinkedList.
                                                    //���ȿ� �̹� �������ִٰ� �߰��Ҽ����ٰ���.
                turnObjectList.AddAfter(nextNode, checkNode); //����� �ڿ� �߰�
                break;//��ġ�� ������ ������ ���������� .
            }
            //��ü�� �ʿ���°�� 
            nextNode = nextNode.Next;   //������带 ã�´�
        }

    }

   
}
