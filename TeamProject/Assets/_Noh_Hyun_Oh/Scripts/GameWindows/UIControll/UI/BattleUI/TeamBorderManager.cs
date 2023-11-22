using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// �������� ������
/// </summary>
public class TeamBorderManager : MonoBehaviour
{
    CanvasGroup cg;
    TeamBorderStateUI[] teamState;
    public TeamBorderStateUI[] TeamStateUIs => teamState;
    private void Awake()
    {
        teamState = transform.GetComponentsInChildren<TeamBorderStateUI>(true);
        cg = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        UnView();
    }
    /// <summary>
    /// ��������ŭ ����â�� �����ش� 
    /// </summary>
    /// <param name="length">������</param>
    public void ViewTeamInfo(int length = 0) 
    {
        cg.alpha = 1.0f;
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }
    /// <summary>
    /// â�� �ݴ´� 
    /// </summary>
    public void UnView() 
    {
        cg.alpha = 0.0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;
       
    }
}
