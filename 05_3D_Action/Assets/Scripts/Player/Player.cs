using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Player : MonoBehaviour
{
    public Transform weaponParent;
    public Transform shieldParent;

    Inventory inven;

    public Inventory Inventory => inven;

    public float itemPickUpRange = 5.0f;

    PlayerInputController controller;
    private void Awake()
    {
        controller = GetComponent<PlayerInputController>();
        controller.onItemPickUp = OnItemPickUp;
    }

    private void OnItemPickUp() // ������ ȹ�� 
    {
        Collider[] itemColliders = Physics.OverlapSphere(transform.position, itemPickUpRange, LayerMask.GetMask("Item"));// Item�̶�� ���̾ ���� Collider�� ��� �����´�
        foreach(Collider itemCollider in itemColliders)
        {
            ItemObject item = itemCollider.GetComponent<ItemObject>();//item object ������Ʈ ã��
            if (inven.AddItem(item.ItemData.code))// ������ �߰� �õ�, ������ ������ ������Ʈ �ı�
            {
                Destroy(item.gameObject);
            }
        }
    }

    void Start()
    {
        inven = new Inventory(this);// Inventory �����ڿ���  itemDataManager�� ã�°� �������� start���� ����
        if (GameManager.Inst.InvenUI != null)
        {
            GameManager.Inst.InvenUI.InitializeInventory(inven); // �κ��丮�� �κ��丮 UI ����
        }
    }

    public void ShowWep_Shield(bool isShow) //����� ���и� ǥ��, �����ϴ� �Լ� 
    {
        weaponParent.gameObject.SetActive(isShow);
        shieldParent.gameObject.SetActive(isShow);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;

        Handles.DrawWireDisc(transform.position, Vector3.up, itemPickUpRange);
    }
#endif
}
