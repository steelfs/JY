using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    Player player;
    private void Start()
    {
        player = GameManager.Inst.Player;
    }
    private void Update()
    {
        transform.Translate(player.transform.position.x, player.transform.position.y + 30, player.transform.position.z);
    }
}
