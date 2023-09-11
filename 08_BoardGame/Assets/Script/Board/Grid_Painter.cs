using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grid_Painter : MonoBehaviour
{
    public GameObject line_Prefab;
    public GameObject letter_Prefab;


    Vector3 origin;
    byte maxLine = 10;
    private void Start()
    {
        RenderLines();
    }
    void RenderLines()
    {
        Vector3 newPos = Vector3.zero;
        origin = new Vector3(0.5f, 0, 4.5f);
        for (int i = 0; i < maxLine; i++)
        {
            newPos.x = -i;
            Instantiate(line_Prefab, newPos, Quaternion.identity);
            Instantiate(line_Prefab, new Vector3(-4, 0, 5 - i), Quaternion.Euler(0, -90, 0));

            origin.z -= i;
            Instantiate(letter_Prefab, Vector2.zero, Quaternion.Euler(90,90,0));
           // Instantiate(letter_Prefab, origin, Quaternion.Euler(90, 90, 0));
            //Instantiate(line_Prefab, newPos,Quaternion.identity);
            //newPos.x = 4 + i;
            //newPos.z = 4 + i;
            //Instantiate(line_Prefab, newPos, Quaternion.Euler(0,-90,0));
        }
    }
}
