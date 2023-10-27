using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // 1. 플레이어의 총에 맞으면 죽는다.
    NavMeshAgent agent;
    Vector3 destination;
    int size = CellVisualizer.CellSize;

    public float walkSpeed = 5;
    public float runSpeed = 10;
    float speedPenalty = 0;

    public float hp = 30.0f;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if(hp <= 0)
            {
                Die();
            }
        }
    }
    public float maxHp = 30.0f;

    public Action<Enemy> onDie;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        HP = maxHp;
        agent.speed = walkSpeed;
        speedPenalty = 0;
        SetDestination();
    }

   
    void SetDestination()
    {
        Vector3 destination = NewDestination();
        agent.SetDestination(destination);
    }
    Vector3 NewDestination()
    {
        destination.x = (int)(transform.position.x + UnityEngine.Random.Range(-3, 3) * size);
        destination.z = (int)(transform.position.z + UnityEngine.Random.Range(-3, 3) * size);

        return destination;
    }
    //이동속도가지기
    // NavMeshAgent 를 이용해 이동을 한다.
    //3. 목적지 기준 +- 3칸 이내 목적지 설정
    // 도착시 목적지 재설정
    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.01f)
        {
            SetDestination();
        }
    }
    private void Die()
    {
        onDie?.Invoke(this);
        gameObject.SetActive(false);
    }
    public void OnAttacked(HitLocation hitLocation, float damage)
    {
        
        switch (hitLocation)
        {
            case HitLocation.Body:
                
                HP -= damage;
                break;
            case HitLocation.Head:
                HP -= damage * 2;
                break;
            case HitLocation.Arm:
                HP -= damage;
                break;
            case HitLocation.Leg:
                speedPenalty += 1;
                agent.speed = walkSpeed - speedPenalty;
                HP -= damage;
                break;
            default:
                break;
        }
      
    }
}
public enum HitLocation
{
    Body,
    Head,
    Arm,
    Leg
}