using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject gunCamera;
    
    GunBase gun;
    private void Awake()
    {
        gunCamera = transform.GetChild(2).gameObject;
        gun = gunCamera.GetComponentInChildren<GunBase>();
    }
    private void Start()
    {
        gun.Equip();
        int result = MinCostClimbingStairs(new int[] { 1, 100, 1, 1, 1, 100, 1, 1, 100, 1 });
        Debug.Log(result);
    }

    public void ShowGunCamera(bool show = true)
    {
        gunCamera.SetActive(show);
    }
    public void GunFire()
    {
        gun.Fire();
    }

    public int MinCostClimbingStairs(int[] cost)
    {
        int result = 0;
        int current = 0;
        int next = 0;
        int next_next = 0;
        for(int  i = 0; i < cost.Length; i++)
        {
            current = cost[i];
            next = cost[i + 1];
            next_next = cost[i + 2];
            if (i > cost.Length - 3)
            {
                result += Mathf.Min(next, next_next);
                return result;
            }
       
            if (next >= next_next)
            {
                i++;
            }
            else
            {
                result += next;

            }
            result += current;
        }

        return result;
    }
}
