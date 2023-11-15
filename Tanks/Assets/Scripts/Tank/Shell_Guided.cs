using System.Collections;
using UnityEngine;

public class Shell_Guided : Shell
{
    public float upPower = 20.0f;
    public float guideHeight = 15.0f;
    bool isTracingStart = false;
    private void Start()
    {
        
    }
    private void FixedUpdate()
    {
        if(!isTracingStart && transform.position.y < guideHeight)
        {
            rigid.AddForce(Vector3.up * upPower);
            // transform.forward = rigid.velocity;
            rigid.MoveRotation(Quaternion.LookRotation(rigid.velocity));

        }
        else if (!isTracingStart)
        {
            StartCoroutine(StartTracing());
        }
    }
    IEnumerator StartTracing()
    {
        isTracingStart = true;

        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.useGravity = false;

        float timeElapsed = 0;

       // float angle = Vector3.SignedAngle(transform.forward, Vector3.down, transform.right);
        while(timeElapsed < 1)
        {
            timeElapsed += Time.deltaTime;
            transform.forward = Vector3.Slerp(transform.forward, Vector3.down, Time.deltaTime * 2);// forward를 돌리기
            transform.Rotate(0, 720 * Time.deltaTime, 0, Space.World);
            yield return null;
        }
        yield return new WaitForSeconds(1);

        Vector3 findCenter = transform.position;
        findCenter.y = 0;
        float radius = guideHeight * 2;
        Collider[] colliders = Physics.OverlapSphere(findCenter, radius, LayerMask.GetMask("Players")) ;
        if (colliders.Length > 0)
        {
            Collider target = colliders[Random.Range(0, colliders.Length)];
            transform.LookAt(target.transform.position);
        }
        rigid.useGravity = true;
        rigid.velocity = transform.forward * firePower;
    }
}
