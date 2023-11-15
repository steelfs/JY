using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_Clust : Shell
{
    public float upPower = 20.0f;
    public float guideHeight = 15.0f;
    bool isTracingStart = false;

    public int subCount = 30;
    private void Start()
    {

    }
    private void FixedUpdate()
    {
        if (!IsExplosion && transform.position.y < guideHeight)
        {
            rigid.AddForce(Vector3.up * upPower);
            // transform.forward = rigid.velocity;
            rigid.MoveRotation(Quaternion.LookRotation(rigid.velocity));

        }
        else if (!IsExplosion)
        {
            Explosion(transform.position, Vector3.up);
        }
    }
    protected override void OnExplosion()
    {
        rigid.AddForce(Vector3.down * 20, ForceMode.Impulse);
        for (int i = 0; i < subCount; i++)
        {
            Factory.Inst.GetClustBaby(transform.position);
        }
    }
}
