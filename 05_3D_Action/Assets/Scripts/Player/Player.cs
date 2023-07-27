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
            if (inven.AddItem(item.ItemData.code))// 아이템 추가 시도, 성공시 아이템 오브젝트 파괴
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
