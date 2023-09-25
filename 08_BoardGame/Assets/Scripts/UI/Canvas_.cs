using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_ : MonoBehaviour
{
    Button random;
    Button finish;
    private void Awake()
    {
        random = transform.GetChild(0).GetComponent<Button>();
        finish = transform.GetChild(1).GetComponent<Button>();
    }
}
