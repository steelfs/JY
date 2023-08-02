using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Player : MonoBehaviour, IHealth,IMana,IEquipTarget
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
    public Action<float> onManaChange { get; set; }
    public Action onDie { get; set; }

    public bool IsAlive => hp > 0.0f;

    float mp = 200.0f;
    public float MP
    {
        get => mp;
        set
        {
            if (IsAlive)
            {
                mp = Mathf.Clamp(value, 0.0f, maxMP);
                onManaChange?.Invoke(mp / MaxMP);

            }
        }
    }
    float maxMP = 200.0f;
    public float MaxMP => maxMP;

    InvenSlot[] partsSlot;//장비아이템의 부위별 상태 (장착한 아이템이 있는 슬롯)
    public InvenSlot this[EquipType part] => partsSlot[(int)part];// return이 null 이면 장비가 안되어있음, null 이 아니면 이미 장비되어있음// parts = 확인할 장비의 종류 

    public void EquipItem(EquipType part, InvenSlot slot)
    {
        ItemData_Equip equip = slot.ItemData as ItemData_Equip;
        if (equip != null)//장비가능하다면
        {
            Transform partsTransform = GetEquipParentTransform(part); //부모가될 트렌스폼 가져와서
            GameObject obj = Instantiate(equip.equipPrefab, partsTransform);

            partsSlot[(int)part] = slot;// 어느슬롯에 아이템이 장착되었는지 기록
            slot.IsEquipped = true; // 장비되었다고 알림(프로퍼티 대리자 호출)
        }
    }

    public void UnEquipItem(EquipType part)
    {
        Transform partParent = GetEquipParentTransform(part);
        while (partParent.childCount > 0)
        {
            Transform child = partParent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
        partsSlot[(int)part].IsEquipped = false;
        partsSlot[(int)part] = null;
    }

    public Transform GetEquipParentTransform(EquipType part)
    {
        Transform result = null;
        switch (part)
        {
            case EquipType.Weapon:
                result = weaponParent;
                break;
            case EquipType.Shield:
                result = shieldParent;
                break;
        }
        return result;
    }
    public void Die()
    {
        onDie?.Invoke();
        Debug.Log("플레이어 사망");
    }

    public void HealthRegenerate(float totalRegen, float duration)//duration동안 totalRegen만큼 회복하는 함수 
    {
        StartCoroutine(RecoveryHealth(totalRegen, duration));       
    }
    public void HealthRegenerateByTick(float tickRegen, float tickTime, uint totalTickCount)
    {
        StartCoroutine(RecoveryHpByTick(tickRegen, tickTime, totalTickCount));
    }
    public void RegenerateMana(float totalRegen, float duration)
    {
        StartCoroutine(RecoveryMana(totalRegen, duration));
    }
    IEnumerator RecoveryHealth(float totalRegen, float duration)
    {
        float regenPerSec = totalRegen / duration;
        float time = 0.0f;
        while(time < duration)
        {
            //HP += totalRegen / duration * Time.deltaTime; 값을 미리 캐싱 해두는 것이 연산량을 더 줄일 수 있다.
            HP += Time.deltaTime * regenPerSec;
            time += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator RecoveryMana(float totalRegen, float duration)
    {
        float time = 0.0f;
        float regenPerSec = totalRegen / duration;
        while (time < duration)
        {
            MP += Time.deltaTime * regenPerSec;
            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator RecoveryHpByTick(float hpValuePerTick, float timePerTick, uint totalTick)
    {
        int tick = 0;
        WaitForSeconds tickValue = new WaitForSeconds(timePerTick);
        while(tick < totalTick)
        {
            HP += hpValuePerTick;
            yield return tickValue;
            tick++;
        }
    }



    public Action<int> onMoneyChange;

    PlayerInputController controller;
    private void Awake()
    {
        controller = GetComponent<PlayerInputController>();
        controller.onItemPickUp = OnItemPickUp;
        partsSlot = new InvenSlot[Enum.GetValues(typeof(EquipType)).Length];//EquipType의 Length만큼  만든다
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
