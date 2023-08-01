using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Player : MonoBehaviour, IHealth
{
    public Transform weaponParent;
    public Transform shieldParent;

    Inventory inven;

    public Inventory Inventory => inven;

    public float itemPickUpRange = 5.0f;

    //�÷��̾ ���� ��
    int money = 0;
    public int Money
    {
        get => money;
        set
        {
            if (money != value)
            {
                money = value;
                onMoneyChange?.Invoke(money);
                Debug.Log(money);
            }
        }
    }


    float hp = 100.0f;
    public float HP
    {
        get => hp;
        set
        {
            if (IsAlive)// ����������� ����
            {
                hp = value;
                if (hp <= 0)
                {
                    Die();
                }
                hp = Mathf.Clamp(hp, 0, MaxHP);// hp�� �׻� 0 ~ �ִ�ġ����
                onHealthChange?.Invoke(hp / MaxHP); //������ �Ķ���ͷ� ȣ��
            }
        }
    }
    float maxHP = 100.0f;
    public float MaxHP => maxHP;

    public Action<float> onHealthChange { get; set; }
    public Action onDie { get; set; }

    public bool IsAlive => hp > 0.0f;

    public void Die()
    {
        onDie?.Invoke();
        Debug.Log("�÷��̾� ���");
    }

    public void HealthRegenerate(float totalRegen, float duration)//duration���� totalRegen��ŭ ȸ���ϴ� �Լ� 
    {
        
    }
    



    public Action<int> onMoneyChange;

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

            IConsumeable consumable = item.ItemData as IConsumeable;
            if (consumable != null)
            {
                consumable.consume(this.gameObject);
                Destroy(item.gameObject);
                //��� �Һ񰡴�
            }
            else if (inven.AddItem(item.ItemData.code))
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
