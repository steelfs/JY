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

    public Action<bool> onWeaponBladeEnable;// Į�� �ݶ��̴�, Ȱ��ȭ Ÿ�̹� �˸���
    public Action<bool> onWeaponEffectEnable; // ��ƼŬ�ý���

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

    InvenSlot[] partsSlot;//���������� ������ ���� (������ �������� �ִ� ����)
    public InvenSlot this[EquipType part] => partsSlot[(int)part];// return�� null �̸� ��� �ȵǾ�����, null �� �ƴϸ� �̹� ���Ǿ�����// parts = Ȯ���� ����� ���� 

    public void EquipItem(EquipType part, InvenSlot slot)
    {
        ItemData_Equip equip = slot.ItemData as ItemData_Equip;
        if (equip != null)//��񰡴��ϴٸ�
        {
            Transform partsTransform = GetEquipParentTransform(part); //�θ𰡵� Ʈ������ �����ͼ�
            GameObject obj = Instantiate(equip.equipPrefab, partsTransform);

            partsSlot[(int)part] = slot;// ������Կ� �������� �����Ǿ����� ���
            slot.IsEquipped = true; // ���Ǿ��ٰ� �˸�(������Ƽ �븮�� ȣ��)

            if (part == EquipType.Weapon)
            {
                Weapon weapon = obj.GetComponent<Weapon>();
                onWeaponBladeEnable = weapon.BladeColliderEnable;
                onWeaponEffectEnable = weapon.EffectEnable;
            }
        }
    }

    public void UnEquipItem(EquipType part)
    {
        Transform partParent = GetEquipParentTransform(part);// �����θ� �ڽ��� ���������� �����
        while (partParent.childCount > 0) // �����θ� �ڽ��� ������ ������ ��� ����
        {
            Transform child = partParent.GetChild(0); 
            child.SetParent(null);
            Destroy(child.gameObject);
        }
        partsSlot[(int)part].IsEquipped = false;
        partsSlot[(int)part] = null;

        if (part == EquipType.Weapon)
        {
            onWeaponBladeEnable = null;
            onWeaponEffectEnable = null;
        }
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
        Debug.Log("�÷��̾� ���");
    }

    public void HealthRegenerate(float totalRegen, float duration)//duration���� totalRegen��ŭ ȸ���ϴ� �Լ� 
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
            //HP += totalRegen / duration * Time.deltaTime; ���� �̸� ĳ�� �صδ� ���� ���귮�� �� ���� �� �ִ�.
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
        partsSlot = new InvenSlot[Enum.GetValues(typeof(EquipType)).Length];//EquipType�� Length��ŭ  �����
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
    public void WeaponEffectEnable(bool enable)
    {
        onWeaponEffectEnable?.Invoke(enable);
    }
    public void WeaponBladeEnable()
    {
        onWeaponBladeEnable?.Invoke(true);
    }
    public void WeaponBladedisable()
    {
        onWeaponBladeEnable?.Invoke(false);
    }
    public void SetPartsSlot(EquipType type, InvenSlot slot)//�������� ������� �������� ���ǰ��ִ����� �����ϴ� �Լ� 
    {
        partsSlot[(int)type] = slot;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;

        Handles.DrawWireDisc(transform.position, Vector3.up, itemPickUpRange);
    }

  






#endif
}
