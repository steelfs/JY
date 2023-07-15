using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePresentation : MonoBehaviour
{
    Player player;
    CinemachineVirtualCamera vCam;
    CinemachineDollyCart dollyCart;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        vCam = transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        dollyCart = transform.GetChild(2).GetComponent<CinemachineDollyCart>();

        player.onDie += OnDiePresentation;
    }
    void OnDiePresentation()
    {
        transform.position = player.transform.position;
        vCam.Priority = 101;
        dollyCart.m_Speed = 10;
    }
}
