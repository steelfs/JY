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
        RenderLines();
        Draw_Grid_Letter();
    }
    void RenderLines()
    {
        for (int  i = 0; i < maxLine; i++)
        {
            GameObject line = Instantiate(line_Prefab, transform);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(i, 0, 1));
            lineRenderer.SetPosition(1, new Vector3(i, 0, 1 - maxLine));
        }

        for (int i = 0; i < maxLine; i++)
        {
            GameObject line = Instantiate(line_Prefab, transform);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(-1, 0, -i));
            lineRenderer.SetPosition(1, new Vector3(maxLine - 1, 0, -i));
        }
        //vertical_Origin = Vector3.zero;
        //horizontal_Origin = new Vector3(5, 0, 5);
        //for (int i = 0; i < maxLine; i++)
        //{
        //    vertical_Origin.x = i;
        //    horizontal_Origin.z = 5 - i;
        //    Instantiate(line_Prefab, vertical_Origin, Quaternion.identity, transform);
        //    Instantiate(line_Prefab, horizontal_Origin, Quaternion.Euler(0, -90, 0), transform);
        //}
    }

    private void Draw_Grid_Letter()
    {
        for (int i = 1; i < maxLine; i++)
        {
            GameObject letter = Instantiate(letter_Prefab, transform);
            letter.transform.position = new Vector3(i - 0.5f, 0, 0.5f);
            
            TextMeshPro text = letter.GetComponent<TextMeshPro>();
            char alphabet = (char)(64 + i); // 아스키코드로 65가 'A'
            text.text = alphabet.ToString();
        }
        for (int i = 1; i < maxLine; i++)
        {
            GameObject letter = Instantiate(letter_Prefab, transform);
            letter.transform.position = new Vector3(-0.5f, 0, 0.5f - i);
            TextMeshPro text = letter.GetComponent<TextMeshPro>();
            text.text = i.ToString();
            if (i > 9)
            {
                text.fontSize = 8;
            }
        }
  










        //horizontal_Letter_Origin = new Vector3(0.5f, 0, 5.5f);
        //vertical_Letter_Origin = new Vector3(-0.5f, 0, 4.5f);
        //char[] chars = new char[10] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
        //int j = 0;
        //while (j < 10)
        //{
        //    horizontal_Letter_Origin.x = 0.5f + j;
        //    vertical_Letter_Origin.z = 4.5f - j;
        //    GameObject horizontal_Letter = Instantiate(letter_Prefab, horizontal_Letter_Origin, Quaternion.Euler(90, 0, 0), transform);
        //    GameObject vertical_Letter = Instantiate(letter_Prefab, vertical_Letter_Origin, Quaternion.Euler(90, 0, 0), transform);

        //    letter = horizontal_Letter.GetComponent<TextMeshPro>();
        //    letter.text = chars[j].ToString();
        //    letter = vertical_Letter.GetComponent<TextMeshPro>();
        //    letter.text = $"{j}";
        //    j++;
        //}
    }
}
