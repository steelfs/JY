using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform target; // 추적할 대상
    public float initialRotationSpeed = 30.0f; // 초기 회전 속도 (1초 동안 적용됨)
    public float trackingRotationSpeed = 3.0f; // 추적 시 회전 속도

    private float elapsedTime = 0.0f;

    void Update ( )
    {
        elapsedTime += Time . deltaTime;

        if (elapsedTime < 2.0f)
        {
            // 초기 1초 동안은 y축 기준으로 회전
            transform . Rotate ( 0.0f , initialRotationSpeed * Time . deltaTime , 0.0f );
        }
        else if (elapsedTime < 4.0f) // 3초까지 추적
        {
            if (target != null)
            {
                // 대상을 추적하며 회전
                Vector3 direction = target . position - transform . position;
                Quaternion toRotation = Quaternion . LookRotation ( direction );
                transform . rotation = Quaternion . Lerp ( transform . rotation , toRotation , trackingRotationSpeed * Time . deltaTime );
            }
        }
        else // 3초 이후에 다시 y축 기준으로 회전
        {
            transform . Rotate ( 0.0f , initialRotationSpeed * Time . deltaTime , 0.0f );
        }
    }
}
