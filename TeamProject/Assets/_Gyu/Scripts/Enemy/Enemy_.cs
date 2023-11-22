using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class Enemy_ : MonoBehaviour, IBattle
{
    Animator Anima;
    public AnimatorOverrideController EnemyAc_Riffle;
    public AnimatorOverrideController EnemyAc_Sword;
    int Go_Attack = Animator.StringToHash("Attack");
    int OnHit = Animator.StringToHash("Hit");
    int Moving = Animator.StringToHash("MoveSpeed");

    AudioSource Audio;
    public AudioClip RiffleAudio;
    public AudioClip SwordAudio;
    AudioClip SelectedAudio;

    public Transform GrapPosition;
    public GameObject Riffle;
    public GameObject Sword;

    public Monster_Type type = Monster_Type.Base;
    public Monster_Type mType
    {
        get => type;
        set
        {
            type = value;
            switch (type)
            {
                case Monster_Type.Base:
                    HP = 200;
                    enemyExp = 30.0f;
                    break;
                case Monster_Type.Size_S:
                    HP = 100;
                    attackPower += 10;
                    enemyExp = 10.0f;
                    break;
                case Monster_Type.Size_M:
                    HP = 200;
                    attackPower += 20;
                    enemyExp = 30.0f;
                    break;
                case Monster_Type.Size_L:
                    HP = 300;
                    attackPower += 30;
                    enemyExp = 80.0f;
                    break;
                case Monster_Type.Boss:
                    HP = 500;
                    enemyExp = 100.0f;
                    break;
                default:
                    HP = 200;
                    enemyExp = 50.0f;
                    break;
            }
        }
    }

    public enum WeaponType { None = 0, Riffle, Swrod }
    WeaponType weaponType = WeaponType.None;
    public WeaponType wType
    {
        get => weaponType;
        set
        {
            weaponType = value;
            switch (weaponType)
            {
                case WeaponType.None:
                    attackRange = 1;
                    SelectedAudio = Audio.clip;
                    break;
                case WeaponType.Riffle:
                    Anima.runtimeAnimatorController = EnemyAc_Riffle;
                    SelectedAudio = RiffleAudio;
                    Instantiate(Riffle.gameObject, GrapPosition.transform);
                    attackRange = 4;
                    attackPower += 10;
                    break;
                case WeaponType.Swrod:
                    attackRange = 1;
                    Anima.runtimeAnimatorController = EnemyAc_Sword;
                    SelectedAudio = SwordAudio;
                    Instantiate(Sword.gameObject, GrapPosition.transform);
                    attackPower += 15;
                    break;
                default:
                    break;
            }
        }
    }

    public Action AC_Attack;
    public Action<float> on_Enemy_Stamina_Change;
    public Action<float> on_Enemy_HP_Change;

    private void Awake()
    {
        Anima = GetComponent<Animator>();
        Audio = GetComponent<AudioSource>();
    }

    float hp = 200;
    float maxHP = 200;
    public float MaxHp
    {
        get => maxHP;
        set
        {
            maxHP = value;
        }
    }
    public float HP
    {
        get => hp;
        set
        {
            if(hp != value)
            {
                hp = value > maxHP ? maxHP : value;
                on_Enemy_HP_Change(hp);
            }
        }
    }

    float stamina = 10;
    const float maxStamina = 20;
    public float MaxStamina => maxStamina;
    public float Stamina
    {
        get => stamina;
        set
        {
            if (stamina != value)
            {
                stamina = Mathf.Clamp(value, 0, maxStamina);
                on_Enemy_Stamina_Change(stamina);
            }
        }
    }

    uint attackPower;
    public uint AttackPower
    {
        get => attackPower;
        set
        {
            if (attackPower != value)
            {
                attackPower = value;
            }
        }
    }


    uint defencePower;
    public uint DefencePower
    {
        get => defencePower;
        set
        {
            if (defencePower != value)
            {
                defencePower = value;
            }
        }
    }
    [SerializeField]
    uint attackRange = 1;
    public uint AttackRange=> attackRange;

    public float enemyExp = 50.0f;
    public float EnemyExp => enemyExp;
   

    private void Attack()
    {
        stamina--;
        Audio.PlayOneShot(SelectedAudio);
        Anima.SetTrigger(Go_Attack);
    }

    public void Attack_Enemy(IBattle target)
    {
        Attack();
        target.Defence(AttackPower);
    }

    public void Defence(float damage, bool isCritical = false)
    {
        Anima.SetTrigger(OnHit);
        HP -= Mathf.Max(0, damage - defencePower);
    }

    public void Move()
    {
        Anima.SetFloat(Moving, 1.0f);
    }

    public void Stop()
    {
        Anima.SetFloat(Moving, 0.0f);
    }

    public void onHit()
    {
        Anima.SetTrigger(OnHit);
    }
    public void OnInit() 
    {
        HP = maxHP;
    }
}
