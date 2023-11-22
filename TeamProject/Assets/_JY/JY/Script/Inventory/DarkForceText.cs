using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DarkForceText : MonoBehaviour
{
    TextMeshProUGUI darkForceText;
    private void Awake()
    {
        darkForceText = GetComponent<TextMeshProUGUI>();
    }
  
    public void Update_DarkForceText(uint darkForce)
    {
        darkForceText.text = $"{darkForce.ToString("N0")}";
    }
}
