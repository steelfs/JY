using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component // where T : Component   T = component�̴�. �ٸ����� ���� �� ���� 
{
    // <> �ȿ��� �ݵ�� ������Ʈ�� �־�����Ѵ�.
    private static bool isShutDown = false;
    private static T instance;
    public static T Inst
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
                T singleTon = FindObjectOfType<T>();
                if (singleTon == null) // �̹� ���� �̱����� �ִ��� Ȯ��
                {

                    GameObject gameObj = new GameObject();//�� ������Ʈ ����
                    gameObj.name = $"{typeof(T).Name} : SingleTon";             //�̸�����
                    singleTon = gameObj.AddComponent<T>();// �̱��� ������Ʈ �߰�

                }
                instance = singleTon; //instance�� ã�Ұų� ������� ��ü ����
                DontDestroyOnLoad(instance.gameObject); //���� �ٲ�ų� ������� ��ü�� �ı����� �ʴ´�.

            }
            return instance; //instance ���� (�̹� �ְų� ���� ��������ų�)
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            //���� ��ġ�Ǿ��ִ� ù��° �̱��� ���� ������Ʈ
            instance = this as T;
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
 
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnsceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnsceneLoaded;
    }
    private void OnsceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnInitialize();
    }

    protected virtual void OnInitialize()
    {

    }


    private void OnApplicationQuit()
    {
        isShutDown = true;
    }
  
}
