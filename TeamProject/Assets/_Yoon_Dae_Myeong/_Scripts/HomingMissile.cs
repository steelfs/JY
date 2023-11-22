using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private GameObject target;
    public float speed = 5f;
    public float rotateSpeed = 5f;


    private void Awake()
    {
        target = GameObject . Find ( "SM_Ship_Stealth_02" );
    }

    void Update ( )
    {
                Vector3 direction = ( target .transform. position - transform . position ) . normalized;
                Quaternion lookRotation = Quaternion . LookRotation ( direction );
                transform . rotation = Quaternion . Slerp ( transform . rotation , lookRotation , Time . deltaTime * rotateSpeed );

                transform . Translate ( Vector3 . forward * speed * Time . deltaTime );
            
       
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //if (other . CompareTag ( "Ship" ))
    //    //{
    //        Destroy ( this,0.1f );
    //    //}
    //}
}
