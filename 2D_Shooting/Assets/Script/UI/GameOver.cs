using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    Animator anim;
    Button reStart;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        reStart  = GetComponentInChildren<Button>();
        reStart.onClick.AddListener(OnReStart);
    }
    private void Start()
    {
        GameManager.Inst.Player.onDie += (_) => anim.SetTrigger("GameOver");
    }
    void OnReStart()
    {
        SceneManager.LoadScene(0);
    }
}
