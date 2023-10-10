using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsDisplay : MonoBehaviour
{
    private void Start()
    {
        int size = transform.childCount;
        for (int i = 0; i< size; i++)
        {
            Transform child = transform.GetChild(i);
            child.position = Vector3.right * (0.8f * i);
        }
    }
}
