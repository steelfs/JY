using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������ �����Ҷ� ����Ұ�
/// </summary>
public class LoadingOpening : MonoBehaviour
{
    private void Start() //�ε��̵Ǿ� �ش�â���� �̵��� 
    {
        StartCoroutine(returnTitle());
    }
    IEnumerator returnTitle() { 
        yield return new WaitForSeconds(1.0f);
        LoadingScene.SceneLoading(); //�ٽ� Ÿ��Ʋ�� �̵���Ų��.
    }
}
