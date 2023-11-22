using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject ������Ʈ�� �״�� �����ؼ� ����ҽ� 
/// ���� ������Ʈ�� �����ͱ��� �ٲ����� �����ؼ� ��ߵȴ� 
/// �����ϴ� ��ɸ� ���� ���� �����ϰ��ִ�
/// </summary>
/// <typeparam name="T">ScriptableObject ������Ʈ �� ��ӹ��� ���۳�Ʈ</typeparam>
public class BaseScriptableObjectGenerate<T> : MonoBehaviour where T : ScriptableObject
{
    /// <summary>
    /// ��ũ���ͺ� �����ʹ� �ٷ� ���������ؼ� ���� ������ ���������� �����ؼ� ����ؾ��Ѵ� 
    /// ��ũ���ͺ� ������Ʈ�� �����ϴ� ���� 
    /// ��ũ���ͺ� ������Ʈ�� Instantiate�� �Ͽ��� ���̶���Űâ�� ���ӿ�����Ʈ�� ������ �ȵȴ�. �׷����� Ʈ��������ġ�� ����Ƶ��ȴ�.
    /// </summary>
    /// <param name="originScriptableObjectArray">���� ��ũ���ͺ� �迭</param>
    /// <return>������ ��ũ���ͺ� �迭</return>
    public T[] QuestDataGenerate(T[] originScriptableObjectArray)
    {
        if(originScriptableObjectArray != null && originScriptableObjectArray.Length > 0) 
        {
            int arrayLength = originScriptableObjectArray.Length;
            T[] tempArray = new T[arrayLength];
            for (int i = 0; i < arrayLength; i++)
            {
                tempArray[i] = Instantiate(originScriptableObjectArray[i]);
            }
            return tempArray;
        }
        return null;
    }
}
