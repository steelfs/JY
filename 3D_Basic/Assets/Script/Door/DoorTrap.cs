using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : DoorAuto
{// 파티클 - CPU가 입자를 뿌리는 것 
    //많은 입자를 사용하면 부담이 된다.
    //VFX - GPU가 더 많은 파편을 만들 수 있다// floating 연산에 특화되어있다
    ParticleSystem ps;
    Player player = null;
    protected override void Awake()
    {
        base.Awake();
        Transform child = transform.GetChild(2);
        ps = child.GetComponent<ParticleSystem>();
        ps.gameObject.SetActive(false); //파티클시스템 처음에 꺼놓기
    }
    protected override void OnOpen()
    {
        ps.gameObject.SetActive(true);
        ps.Play();
        player.Die();

    }
    protected override void OnClose()
    {
        //ps.Stop();
        //ps.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<Player>(); //플레이어 찾기 
        if (player != null) // null이 아니면 player
        {
            Open();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Close(); //나갔으면 문 닫기
            player= null;
        }
    }
}
