using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ͱ��� �ֻ��� ������Ʈ�����
/// ������ ���� ����Ȱ��� ������������̴�
/// </summary>
public class DataFactory : Singleton<DataFactory> {
    /// <summary>
    /// ����Ʈ �������ִ°�
    /// </summary>
    [SerializeField]
    QuestScriptableGenerate questScriptableGenerate;
    public QuestScriptableGenerate QuestScriptableGenerate => questScriptableGenerate;

    protected override void Awake()
    {
        base.Awake();
        questScriptableGenerate = GetComponentInChildren<QuestScriptableGenerate>();
    }
}
