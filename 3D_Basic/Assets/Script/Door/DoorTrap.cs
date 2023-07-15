using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : DoorAuto
{// ��ƼŬ - CPU�� ���ڸ� �Ѹ��� �� 
    //���� ���ڸ� ����ϸ� �δ��� �ȴ�.
    //VFX - GPU�� �� ���� ������ ���� �� �ִ�// floating ���꿡 Ưȭ�Ǿ��ִ�
    ParticleSystem ps;
    Player player = null;
    protected override void Awake()
    {
        base.Awake();
        Transform child = transform.GetChild(2);
        ps = child.GetComponent<ParticleSystem>();
        ps.gameObject.SetActive(false); //��ƼŬ�ý��� ó���� ������
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
        player = other.GetComponent<Player>(); //�÷��̾� ã�� 
        if (player != null) // null�� �ƴϸ� player
        {
            Open();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Close(); //�������� �� �ݱ�
            player= null;
        }
    }
}
