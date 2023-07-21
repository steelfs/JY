using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform weaponParent;
    public Transform shieldParent;

    public void ShowWep_Shield(bool isShow) //무기와 방패를 표시, 해제하는 함수 
    {
        weaponParent.gameObject.SetActive(isShow);
        shieldParent.gameObject.SetActive(isShow);
    }
}
