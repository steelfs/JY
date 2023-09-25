using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test11 : MonoBehaviour
{
    public GameState gameState = GameState.ShipDeployment;

    GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Inst;
    }
    private void OnValidate()
    {
        if (gameManager != null)
        {
            GameManager.Inst.GameState = gameState;
        }
    }
}
