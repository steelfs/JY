using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //����
    // �÷��̾� ������� �� ���� 
    Transform meshTransform;
    public float spinSpeed = 360;
    public GunType gunType;
    private void Awake()
    {
        meshTransform = transform.GetChild(0);
    }
    void Update()
    {
        meshTransform.Rotate(Time.deltaTime * spinSpeed * Vector3.up, Space.World);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.GunChange(gunType);
                Destroy(this.gameObject);
            }
        }
    }
}
