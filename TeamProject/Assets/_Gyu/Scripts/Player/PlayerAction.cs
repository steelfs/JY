using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public GameObject Button;
    SphereCollider sphereCollider;

    public GameObject CanvasLocation;
    GameObject Npc;
    NpcBase NpcBase;

    private void Awake()
    {
        CanvasLocation = GameObject.Find("Canvas");
        Button = CanvasLocation.transform.GetChild(3).gameObject;
        Button.gameObject.SetActive(false);
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Npc"))
        {
            Npc = other.gameObject;
            Button.gameObject.SetActive(true);
            NpcBase = Npc.GetComponent<NpcBase>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Npc"))
        {
            Npc = null;
            Button.gameObject.SetActive(false);
        }
    }
    
    public void playac()
    {
        QuestManager.instance.Action(NpcBase.Npcid);
    }
}
