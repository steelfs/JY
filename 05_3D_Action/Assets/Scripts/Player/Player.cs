using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform weaponParent;
    public Transform shieldParent;

    public void ShowWep_Shield(bool isShow) //����� ���и� ǥ��, �����ϴ� �Լ� 
    {
        weaponParent.gameObject.SetActive(isShow);
        shieldParent.gameObject.SetActive(isShow);
    }
}
