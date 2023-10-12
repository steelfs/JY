using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player;
    public GameObject bulletHolePrefab;//나중에 풀에서 가져오는것으로 변경


    CinemachineVirtualCamera vcamera;
    public CinemachineVirtualCamera Vcamera => vcamera;


    protected override void OnInitialize()
    {
        base.OnInitialize();
        player = FindAnyObjectByType<Player>();

        GameObject obj = GameObject.Find("PlayerFollowCamera");
        if (obj != null)
        {
            vcamera = obj.GetComponent<CinemachineVirtualCamera>();
        }
    }



    Vector3 zoomPos;
    Vector3 origin;
    public Transform gunPos;
    private void Start()
    {
        zoomPos = new Vector3(0, -0.161f, 0.203f);
        origin = new Vector3(0, -0.32f, 0.446f);

    }
    public void On_ZoomIn()
    {
        StartCoroutine(ZoomIn());
        gunPos.localPosition = zoomPos;

        // gunPos.localPosition = zoomPos;
    }
    public void On_ZoomOut() 
    {
        StopAllCoroutines();
        StartCoroutine(ZoomOut());
        gunPos.localPosition = origin;
    }
    IEnumerator ZoomIn()
    {
        while(vcamera.m_Lens.FieldOfView > 20)
        {
            vcamera.m_Lens.FieldOfView -= (Time.deltaTime * 20) / 0.1f;//0.5초 간 20 감소
            yield return null;
        }
    }
    IEnumerator ZoomOut()
    {
        while(vcamera.m_Lens.FieldOfView < 40)
        {
            vcamera.m_Lens.FieldOfView += (Time.deltaTime * 20) / 0.1f;
            yield return null;
        }
    }
}
