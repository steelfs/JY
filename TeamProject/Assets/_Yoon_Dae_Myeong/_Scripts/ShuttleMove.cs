using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuttleMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private void Update()
    {
        transform . Translate ( Vector3 . forward * moveSpeed * Time . deltaTime );
    }
}
