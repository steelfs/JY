using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grid_Painter : MonoBehaviour
{
    public GameObject line_Prefab;
    public GameObject letter_Prefab;
    TextMeshPro letter;

    Vector3 vertical_Origin;
    Vector3 horizontal_Origin;

    Vector3 horizontal_Letter_Origin;
    Vector3 vertical_Letter_Origin;
    byte maxLine;
    private void Start()
    {
        maxLine = 11;
        vertical_Origin = Vector3.zero;
        horizontal_Origin = new Vector3(5, 0, 5);

        horizontal_Letter_Origin = new Vector3(0.5f, 0, 5.5f);
        vertical_Letter_Origin = new Vector3(-0.5f, 0, 4.5f);
        RenderLines();
    }
    void RenderLines()
    {
        char[] chars = new char[10] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'};

        for (int i = 0; i < maxLine; i++)
        {
            vertical_Origin.x = i;
            horizontal_Origin.z = 5 - i;
            Instantiate(line_Prefab, vertical_Origin, Quaternion.identity, transform);
            Instantiate(line_Prefab, horizontal_Origin, Quaternion.Euler(0, -90, 0), transform);
        }
        int j = 0;
        while(j < 10)
        {
            horizontal_Letter_Origin.x = 0.5f + j;
            vertical_Letter_Origin.z = 4.5f - j;
            GameObject horizontal_Letter = Instantiate(letter_Prefab, horizontal_Letter_Origin, Quaternion.Euler(90, 0, 0), transform);
            GameObject vertical_Letter = Instantiate(letter_Prefab, vertical_Letter_Origin, Quaternion.Euler(90, 0, 0), transform);

            letter = horizontal_Letter.GetComponent<TextMeshPro>();
            letter.text = chars[j].ToString();
            letter = vertical_Letter.GetComponent<TextMeshPro>();
            letter.text = $"{j}";
            j++;
        }

    }
}
