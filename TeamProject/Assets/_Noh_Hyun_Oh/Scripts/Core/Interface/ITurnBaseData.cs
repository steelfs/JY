using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �� �޴������� �������Ȳ�� ó���ϱ����� ����ϴ� �������̽� 
/// ���� : �Ʊ� n�� , ���� n ��  ����  
/// 
/// </summary>
public interface ITurnBaseData
{
    GameObject gameObject { get; }
    /// <summary>
    /// �ϰ����� UI ĳ�̿� ������Ƽ
    /// </summary>
    TurnGaugeUnit GaugeUnit { get; }

    /// <summary>
    /// ������Ʈ�� ���ǵ� �Լ��� ����
    /// ������ UI���������� ������ ��ǥ���� �˾ƾ������� �߰�
    /// </summary>
    public Transform transform { get; }
    /// <summary>
    /// ���������� �ϰ����� ����� ��ȣ 
    /// </summary>
    public int UnitBattleIndex { get; set; }
    /// <summary>
    /// ������� �߰��� �ൿ�� 
    /// </summary>
    public float TurnEndActionValue { get; }
    /// <summary>
    /// �ൿ�� ���ĵ� ���ذ�
    /// </summary>
    public float TurnActionValue { get; set; }

    /// <summary>
    /// �ϿϷ�� �˷��� ��������Ʈ
    /// </summary>
    public Action TurnEndAction { get; set; }

    /// <summary>
    /// ���� ���� ����ִ��� üũ�� ����
    /// </summary>
    public bool IsTurn { get; set; }
    public bool IsMove { get; }

    /// <summary>
    /// ������ �ൿ�߿� Ư�������� �������� �޴����� ��ȣ�� �ִ� ��������Ʈ
    /// </summary>
    public Action<ITurnBaseData> TurnRemove { get; set; }

    /// <summary>
    /// �ش��Ͽ�����Ʈ���� ����� ĳ���� ����Ʈ
    /// </summary>
    public List<ICharcterBase> CharcterList { get; }

    /// <summary>
    /// ���� ��Ʈ���� ���� ��ȯ
    /// </summary>
    public ICharcterBase CurrentUnit { get; set; }

    /// <summary>
    /// �ʻ����� ȣ���� ������ �ʱ�ȭ �Լ�
    /// </summary>
    public void InitData();

    /// <summary>
    /// �Ͻ��۽� ������ �Լ�
    /// </summary>
    public void TurnStartAction();

    /// <summary>
    /// �������� ������� �ʱ�ȭ�� �Լ�
    /// </summary>
    public void ResetData();
}
