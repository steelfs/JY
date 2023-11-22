using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle
{
    void Attack_Enemy(IBattle target);
    void Defence(float damage, bool isCritical = false);
}
