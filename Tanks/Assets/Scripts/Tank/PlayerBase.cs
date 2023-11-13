using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public Color baseColor;

    public float moveSpeed = 1.0f;
    public float rotateSpeed = 360.0f;
    protected Vector2 inputDir = Vector2.zero;
    protected Rigidbody rigid;
    protected PlayerInputActions inputActions;
    protected bool IsAlive = true;
    
    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();
    }
    protected void Start()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.SetColor("_BaseColor", baseColor);
        }
    }
    private void FixedUpdate()
    {
        rigid.MovePosition(transform.position + Time.fixedDeltaTime * moveSpeed * inputDir.y * transform.forward);
        rigid.MoveRotation(
            Quaternion.Euler(0, Time.fixedDeltaTime * rotateSpeed * inputDir.x, 0) * transform.rotation);
    }
    protected void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
    }
    public void DamageTaken(float damage, Vector3 hitDir)
    {
        //Vector3 dir = transform.position - hitDir;
        //float ratio = dir.magnitude / explosionRadius;

        Die(hitDir);
    }
    void Die(Vector3 hitDir)
    {
        if (IsAlive)
        {
            IsAlive = false;
            inputActions.Disable();
            //Debug.Log(dir);
            Vector3 explosionDir = hitDir + transform.up * 3;
            rigid.constraints = RigidbodyConstraints.None;

            Vector3 torqueAxis = Quaternion.Euler(0, Random.Range(80, 100), 0) * hitDir;


            rigid.AddForce(explosionDir, ForceMode.Impulse);
            rigid.AddTorque(torqueAxis, ForceMode.Impulse);
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsAlive && !collision.gameObject.CompareTag("Shell"))
        {
            rigid.drag = 5;
            rigid.angularDrag = 5;
        }
    }
}
