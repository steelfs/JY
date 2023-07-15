using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAutoLock : DoorAuto
{
    public Color unLockColor;
    public Color LockColor;
    BoxCollider sensor;

    Material doorMaterial;
    protected override void Awake()
    {
        base.Awake();
        sensor= GetComponent<BoxCollider>();
        sensor.enabled = false;

        Transform door = transform.GetChild(1);
        door = door.GetChild(0);
        MeshRenderer meshRenderer = door.GetComponent<MeshRenderer>();
        doorMaterial = meshRenderer.material; //코드상에서 머티리얼을 조작하면 원본이 아니라 사본을 만들어서 조작하기 때문에 원본과는 독립적으로 존재한다
                                              //원본을 조작하면 연결된 모든 머티리얼이 영향을 받기 때문이다
    }
    private void Start()
    {
        doorMaterial.color = LockColor;
    }
    public void UnLock()
    {
        sensor.enabled = true;
        doorMaterial.color = unLockColor;
    }
}
