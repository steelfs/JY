using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데이터관련 최상위 오브젝트에사용
/// 데이터 사용시 공통된것을 넣을까생각중이다
/// </summary>
public class DataFactory : Singleton<DataFactory> {
    /// <summary>
    /// 퀘스트 원본이있는곳
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
