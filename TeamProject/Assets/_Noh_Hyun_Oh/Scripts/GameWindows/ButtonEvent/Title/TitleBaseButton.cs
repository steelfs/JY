using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBaseButton : MonoBehaviour
{
    Button bt;
    private void Awake()
    {
        bt = GetComponent<Button>();
    }
}
