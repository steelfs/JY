using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject 오브젝트는 그대로 참조해서 사용할시 
/// 원본 오브젝트의 데이터까지 바뀜으로 복사해서 써야된다 
/// 복사하는 기능만 따로 빼서 관리하고있다
/// </summary>
/// <typeparam name="T">ScriptableObject 오브젝트 를 상속받은 컴퍼넌트</typeparam>
public class BaseScriptableObjectGenerate<T> : MonoBehaviour where T : ScriptableObject
{
    /// <summary>
    /// 스크립터블 데이터는 바로 참조연결해서 사용시 원본이 수정됨으로 복사해서 사용해야한다 
    /// 스크립터블 오브젝트를 복사하는 로직 
    /// 스크립터블 오브젝트는 Instantiate를 하여도 하이라이키창에 게임오브젝트로 생성이 안된다. 그럼으로 트랜스폼위치를 안잡아도된다.
    /// </summary>
    /// <param name="originScriptableObjectArray">원본 스크립터블 배열</param>
    /// <return>복사한 스크립터블 배열</return>
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
