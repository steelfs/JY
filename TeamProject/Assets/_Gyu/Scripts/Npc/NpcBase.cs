using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBase : MonoBehaviour
{
    public int Npcid;
    Animation anima;
    private void Awake()
    {
        anima = GetComponent<Animation>();
    }

    //public GameObject Button;
    //SphereCollider sphereCollider ;

    //public GameObject CanvasLocation;

    //private void Awake()
    //{
    //    CanvasLocation = GameObject.Find("Canvas");
    //    Button = CanvasLocation.transform.GetChild(3).gameObject;
    //    Button.gameObject.SetActive(false);
    //    sphereCollider = GetComponent<SphereCollider>();
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        Button.gameObject.SetActive(true);
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        Button.gameObject.SetActive(false);
    //    }
    //}
}
