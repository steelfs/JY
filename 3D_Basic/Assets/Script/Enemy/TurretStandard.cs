using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStandard : TurretBase
{

    void Start()
    {
        StartCoroutine(fireCoroutine);
    }


}
