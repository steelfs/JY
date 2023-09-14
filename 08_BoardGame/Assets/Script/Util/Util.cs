using System;
using UnityEngine;

public class Util 
{
    /// <summary>
    /// 피셔에이츠 알고리즘
    /// </summary>
    /// <typeparam name="T">순서가 있는 타입</typeparam>
    /// <param name="source">순서를 섞을 배열</param>
    public static void Shuffle<T>(T[] source) 
    {
        for (int i = source.Length - 1; i > -1; i--)
        {
            int index = UnityEngine.Random.Range(0, i);
            (source[index], source[i]) = (source[i], source[index]);
        }
    }
}
