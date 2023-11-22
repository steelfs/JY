using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 오프닝 제작할때 사용할것
/// </summary>
public class LoadingOpening : MonoBehaviour
{
    private void Start() //로딩이되어 해당창으로 이동시 
    {
        StartCoroutine(returnTitle());
    }
    IEnumerator returnTitle() { 
        yield return new WaitForSeconds(1.0f);
        LoadingScene.SceneLoading(); //다시 타이틀로 이동시킨다.
    }
}
