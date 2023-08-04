using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle
{
    Transform transform { get; }//이 오브젝트의 트렌스폼(구현 필요없음)
    float AttackPower { get; }
    float DefencePower { get; }

    void Attack(IBattle target);//내가 공격할 대상 
    void defence(float damage);// 내가 받은 데미지 
}