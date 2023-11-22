using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �ɼǿ��� ���α׷����̹��� ���� ��� �ֱ����� ���� ����  
/// ���̵��ÿ��� �����Ǳ⶧���� ���⼭ ó���Ѵ�.
/// </summary>
public class ProgressList : ChildComponentSingeton<ProgressList> 

    
    /// ���α׷����� ����� �̽�ũ��Ʈ�� �߰�
{
    /// <summary>
    /// prefab �����̶� transform ���� �����̾ȵȴ�.
    /// </summary>
    [SerializeField]
    private GameObject originBar;
    /// <summary>
    /// transform ���������� �����ص� �̹�������
    /// </summary>
    private GameObject progressBar;

    /// <summary>
    /// ProgressBar  
    /// </summary>
    /// <param name="type">Ÿ��</param>
    /// <param name="transform"></param>
    /// <returns></returns>
    public GameObject GetProgress(EnumList.ProgressType type, Transform transform) {
    
        switch (type) {
            case EnumList.ProgressType.BAR:
                if (progressBar == null) //���ʿ� ���� ������ �����Ѵ�.
                {
                    //�ε�â�� �׻� �ʱ�ȭ �Ǽ� �̸������ξ�� �Ź��ȸ����. 
                    //�ϳ������� ���Ǳ⶧���� ���丮�ȸ���� �ش�Ŭ�������� ó��                
                    progressBar = Instantiate(originBar, transform); //���οü���� �ʿ��Ҷ� �Ѱ��ش�.
                }
                return progressBar;
            default:
                Debug.LogWarning($"{type.ToString()} �� ���� �������� �ʽ��ϴ�");
                return null;
        }
    }
}
