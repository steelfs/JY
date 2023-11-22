using System . Collections;
using System . Collections . Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float spiralForce = 10f;
    public float rotationSpeed = 5f;
    public float spiralRadius = 1f;
    public float spiralHeight = 0.1f;

    private bool isHit = false;
    private float time = 0f;

    void OnTriggerEnter ( Collider other )
    {
        if (other . CompareTag ( "Missile" ))
        {
            isHit = true;
            StartSpiralDescent ( ); // Missile에 맞았을 때 Spiral 추락
            Explode ( ); // 폭발 효과 생성
            Destroy ( other . gameObject );
        }
    }

    void Explode ( )
    {
        Instantiate ( explosionPrefab , transform . position , Quaternion . identity );
        //Destroy ( gameObject );
    }

    void StartSpiralDescent ( )
    {
        Rigidbody rb = GetComponent<Rigidbody> ( );
        rb . useGravity = true;
        rb . velocity = Vector3 . zero; 
    }

    void Update ( )
    {
        if (isHit)
        {
            time += Time . deltaTime;

            float x = spiralRadius * Mathf . Cos ( time );
            float z = spiralRadius * Mathf . Sin ( time );
            float y = time * spiralHeight;

            //Vector3 spiralPosition = new Vector3 ( x , y , z );
            //transform . position = transform . position + spiralPosition * Time . deltaTime;
            Vector3 spiralPosition = new Vector3 ( x , y , z ) * Time . deltaTime;
            transform . position += spiralPosition;


            transform . Rotate ( Vector3 . up , rotationSpeed * Time . deltaTime );
        }
    }
}