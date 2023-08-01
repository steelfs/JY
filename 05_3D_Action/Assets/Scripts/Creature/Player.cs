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

    //플레이어가 가진 돈
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
            if (IsAlive)// 살아있을때만 변경
            {
                hp = value;
                if (hp <= 0)
                {
                    Die();
                }
                hp = Mathf.Clamp(hp, 0, MaxHP);// hp는 항상 0 ~ 최대치까지
                onHealthChange?.Invoke(hp / MaxHP); //비율을 파라미터로 호출
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
        Debug.Log("플레이어 사망");
    }

    public void HealthRegenerate(float totalRegen, float duration)//duration동안 totalRegen만큼 회복하는 함수 
    {
        
    }
    



    public Action<int> onMoneyChange;

    PlayerInputController controller;
    private void Awake()
    {
        controller = GetComponent<PlayerInputController>();
        controller.onItemPickUp = OnItemPickUp;
    }
    private void OnItemPickUp() // 아이템 획득 
    {
        Collider[] itemColliders = Physics.OverlapSphere(transform.position, itemPickUpRange, LayerMask.GetMask("Item"));// Item이라는 레이어를 가진 Collider를 모두 가져온다
        foreach(Collider itemCollider in itemColliders)
        {
            ItemObject item = itemCollider.GetComponent<ItemObject>();//item object 컴포넌트 찾기

            IConsumeable consumable = item.ItemData as IConsumeable;
            if (consumable != null)
            {
                consumable.consume(this.gameObject);
                Destroy(item.gameObject);
                //즉시 소비가능
            }
            else if (inven.AddItem(item.ItemData.code))
            {
                Destroy(item.gameObject);
            }
    
        }
    }

    void Start()
    {
        inven = new Inventory(this);// Inventory 생성자에서  itemDataManager를 찾는거 ㅅ때문에 start에서 실행
        if (GameManager.Inst.InvenUI != null)
        {
            GameManager.Inst.InvenUI.InitializeInventory(inven); // 인벤토리와 인벤토리 UI 연결
        }
    }

    public void ShowWep_Shield(bool isShow) //무기와 방패를 표시, 해제하는 함수 
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
