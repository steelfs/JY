using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCanvasCamera : MonoBehaviour
{
    Canvas mainCanvas;

    private void Awake()
    {
        mainCanvas = GetComponent<Canvas>();    
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCanvas.worldCamera = Camera.main;
    }


}
