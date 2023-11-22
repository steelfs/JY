using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// 팀관리용 제작중
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
    /// 팀원수만큼 상태창을 보여준다 
    /// </summary>
    /// <param name="length">팀원수</param>
    public void ViewTeamInfo(int length = 0) 
    {
        cg.alpha = 1.0f;
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }
    /// <summary>
    /// 창을 닫는다 
    /// </summary>
    public void UnView() 
    {
        cg.alpha = 0.0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;
       
    }
}
