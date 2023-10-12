using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GameManager : Singleton<GameManager>
{
    public GameObject player;
    public CinemachineVirtualCamera playerVcam;

    Vector3 zoomPos;
    Vector3 origin;
    Vector3 dirToZoomPos;
    Vector3 dirToOrigin;

    public Transform gunPos;
    private void Start()
    {
        zoomPos = new Vector3(0, -0.161f, 0.203f);
        origin = new Vector3(0, -0.32f, 0.446f);

        dirToZoomPos = (zoomPos - origin).normalized;
        dirToOrigin = (origin - zoomPos).normalized;
    }
    public void On_ZoomIn()
    {
        StartCoroutine(ZoomIn());
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

        while(playerVcam.m_Lens.FieldOfView > 20)
        {
            playerVcam.m_Lens.FieldOfView -= (Time.deltaTime * 20) / 0.1f;//0.5초 간 20 감소
            gunPos.localPosition += dirToZoomPos;
            yield return null;
        }
 
    }
    IEnumerator ZoomOut()
    {
        while(playerVcam.m_Lens.FieldOfView < 40)
        {
            playerVcam.m_Lens.FieldOfView += (Time.deltaTime * 20) / 0.1f;
            yield return null;
        }
    }
}
