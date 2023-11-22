using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public GameObject missilePrefab; 
    public Transform launchPoint;
  

    void Start ( )
    {
        Invoke ( "LaunchMissile" , 2.5f ); 
    }

    void LaunchMissile ( )
    {
        GameObject missile = Instantiate ( missilePrefab , launchPoint . position , launchPoint . rotation );

    }
}