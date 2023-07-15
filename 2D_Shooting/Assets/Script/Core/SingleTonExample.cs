using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̱��� ��ü�� �ϳ��� ������ ������ ����
/// </summary>
public class SingleTonExample : MonoBehaviour
{
   

    /// <summary>
    /// �̹� ����ó���� ������ Ȯ���ϱ� ���� ����
    /// </summary>
    private static bool isShutDown = false;


    /// <summary>
    /// �̱����� ��ü
    /// </summary>
    private static SingleTonExample instance;

    /// <summary>
    /// �̱��� ��ü�� �������� ���� ������Ƽ ��ü�� ��������� �ʾ����� ���� �����.
    /// </summary>
    public static SingleTonExample Instance
    {
        get
        {
            if (isShutDown) //����ó���� �� ��Ȳ�̸� 
            {
                Debug.LogWarning("�̱����� �̹� �������̴�.");// ���޼��� ���
                return null;
            }
            if (instance == null)
            {
                //instance�� ������ ���� �����.
                SingleTonExample singleTon = FindObjectOfType<SingleTonExample>();
                if (singleTon == null) // �̹� ���� �̱����� �ִ��� Ȯ��
                {

                    GameObject gameObj = new GameObject();//�� ������Ʈ ����
                    gameObj.name = "SingleTon";             //�̸�����
                    gameObj.AddComponent<SingleTonExample>();// �̱��� ������Ʈ �߰�
                  
                }
                instance= singleTon; //instance�� ã�Ұų� ������� ��ü ����
                DontDestroyOnLoad(instance.gameObject); //���� �ٲ�ų� ������� ��ü�� �ı����� �ʴ´�.
            
            }
            return instance; //instance ���� (�̹� �ְų� ���� ��������ų�)
        }
    }
    public int testI = 0;

    private void Awake()
    {
        if (instance == null)
        {
            //���� ��ġ�Ǿ��ִ� ù��° �̱��� ���� ������Ʈ
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            //ù��° �̱��� ���Ӻ�����Ʈ�� ������� ���Ŀ� ������ �̱����̸� 
            if (instance != this)
            {
                Destroy(this.gameObject); // ù��° �̱���� �ٸ� ���̸� ���߿� ���������(�̰���) �����ض�
            }
        }
    }

    /// <summary>
    /// ���α׷��� ����ɶ� ����Ǵ� �Լ�
    /// </summary>
    private void OnApplicationQuit()
    {
        isShutDown = true;
    }
}
