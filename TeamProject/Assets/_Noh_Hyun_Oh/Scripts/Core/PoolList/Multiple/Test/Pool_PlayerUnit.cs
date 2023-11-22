using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_PlayerUnit : Base_Pool_Multiple<BattleMapPlayerBase>
{
    protected override void EndInitialize()
    {
        //start ���� �ʱ�ȭ�ϱ����� Ȱ��ȭ ��Ų��.
        Base_PoolObj temp;
        foreach (var item in pool)  // Ǯ���ִ°� ���δ� �����ϱ����� ���ε�����.
        {
            temp = readyQueue.Dequeue();    //start ���� �ٽ� ��Ȱ��ȭ ��ų���̱⶧���� ��ť�� �̾Ƴ���.
            temp.gameObject.SetActive(true);  //Ȱ��ȭ ���Ѽ� start ������ ����ǵ��� �Ѵ�.
        }
    }
}
