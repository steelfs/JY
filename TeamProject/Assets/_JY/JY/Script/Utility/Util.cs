using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class Util 
{
    public static void Shuffle<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > -1; i--)
        {
            int index = Random.Range(0, i);
            (arr[index], arr[i]) = (arr[i], arr[index]); 
        }
    }
}
