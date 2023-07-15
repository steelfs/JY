using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifePanel : MonoBehaviour
{
    TextMeshProUGUI lifeText;
    private void Awake()
    {
        lifeText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        GameManager.Inst.Player.onLifeChange += (life) => lifeText.text = life.ToString();
    }
}
