using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyForTest : MonoBehaviour
{
    public float speed = 5.0f;
    float boost = 1.0f;
    EnemyInputAction enemyInputAction;
    Vector3 direction;
    Animator anim;
    readonly int InputY_String = Animator.StringToHash("InputY");
    public GameObject bullet;
    
    private void Awake()
    {
        enemyInputAction = new EnemyInputAction();
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        enemyInputAction.Enemy.Enable();
        enemyInputAction.Enemy.Move.performed += OnMove;
        enemyInputAction.Enemy.Move.canceled += OnMove;
        enemyInputAction.Enemy.Boost.performed += OnBoost;
        enemyInputAction.Enemy.Boost.canceled += OnBoost;
        enemyInputAction.Enemy.Fire.performed += OnFire;
    }

    private void OnFire(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        Instantiate(bullet);
        Transform child = transform.GetChild(0);
        bullet.transform.position = child.position;
        bullet.transform.rotation = child.rotation;
    }

    private void OnBoost(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            boost = 1.0f;
        }
        else
        {
            boost = 5.0f;
        }
    }

    private void OnDisable()
    {
        enemyInputAction.Enemy.Move.performed -= OnMove;
        enemyInputAction.Enemy.Move.canceled -= OnMove;
        enemyInputAction.Enemy.Boost.performed -= OnBoost;
        enemyInputAction.Enemy.Boost.canceled -= OnBoost;
        enemyInputAction.Enemy.Disable();
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        direction = value;
        anim.SetFloat(InputY_String, direction.y);
    }

    private void Update()
    {
        transform.position += Time.deltaTime * speed * boost * direction;
    }
}
