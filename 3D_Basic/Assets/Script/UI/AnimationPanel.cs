using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPanel : MonoBehaviour
{
    Animator clearAnim;
    Animator gameOverAnim;
    Player player;

    DoorAuto finalDoor;
    private void Start()
    {
        clearAnim = transform.GetChild(0).GetComponent<Animator>();
        gameOverAnim = transform.GetChild(1).GetComponent<Animator>();

        player = FindObjectOfType<Player>();
        player.onDie += GameOverAnim;

        finalDoor = FindObjectOfType<DoorAuto>();
        if (finalDoor != null)
        {
            finalDoor.gameClear += ClearAnim;
        }
    }
    void ClearAnim()
    {
        clearAnim.SetBool("Clear", true);
    }
    void GameOverAnim()
    {
        gameOverAnim.SetBool("GameOver", true);
    }
}
