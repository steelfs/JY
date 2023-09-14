using System;
using UnityEngine;

public class Util 
{
    /// <summary>
    /// �Ǽſ����� �˰���
    /// </summary>
    /// <typeparam name="T">������ �ִ� Ÿ��</typeparam>
    /// <param name="source">������ ���� �迭</param>
    public static void Shuffle<T>(T[] source) 
    {
        for (int i = source.Length - 1; i > -1; i--)
        {
            int index = UnityEngine.Random.Range(0, i);
            (source[index], source[i]) = (source[i], source[index]);
        }
    }
}
