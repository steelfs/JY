using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChoosePanel_Animal : MonoBehaviour
{
    float rotateSpeed = 90.0f;

    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime , 0);
    }
}
