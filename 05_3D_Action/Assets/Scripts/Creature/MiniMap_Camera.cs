using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniMap_Camera : MonoBehaviour
{
    Vector3 offset;
    Player player;
    public float smoothNess = 5.0f;
    private void Start()
    {
        player = GameManager.Inst.Player;
        offset = transform.position + player.transform.position + Vector3.up * 30;
    }
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, smoothNess * Time.deltaTime);
    }
}
