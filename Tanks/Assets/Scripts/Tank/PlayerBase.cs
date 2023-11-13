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
    public void DamageTaken(float explosionForce, Vector3 pos, float explosionRadius)
    {

    }
}
