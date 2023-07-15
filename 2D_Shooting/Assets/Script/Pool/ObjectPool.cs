using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : PooledObject
{
    public GameObject originalPrefab;

    public int poolSize = 64;
    T[] pool;
    Queue<T> readyQueue; 

    public void Initialize()
    {
        if (pool == null)
        {
            pool = new T[poolSize];
            readyQueue = new Queue<T>(poolSize);

            //readyQueue.Count; //������ ����ִ� ���� 
            //readyQueue.Capacity // ���� �̸� �غ��ص� ���� 

            GenerateObjects(0, poolSize, pool);
        }
        else
        {
            foreach (T obj in pool) //���� �ι�° ���� �ҷ����� Ǯ�� �����ϴ� ��Ȳ�̶�� 
            {
                obj.gameObject.SetActive(false); 
            }
        }
    }
    /// <summary>
    /// Ǯ���� ������Ʈ�� �ϳ� ���� �� �����ִ� �Լ�
    /// </summary>
    /// <returns>ť���� ������ Ȱ��ȭ��Ų ������Ʈ</returns>
    public T GetObject()
    {
        if (readyQueue.Count > 0) // ť�� �����ִ� ������Ʈ�� �ִ��� Ȯ��
        {
            //���������� 
            T comp = readyQueue.Dequeue(); // �ϳ�������
            comp.gameObject.SetActive(true);// Ȱ��ȭ��Ų����
            return comp; // ������ ����
        }
        else
        {
            //�ȳ��������� 
           ExpendPool(); //Ȯ���Ű��
            return GetObject();// �ٽ� ��û
        }
    }
    private void ExpendPool()//Ǯ�� ũ�⸦ �ι�� Ȯ���Ű�� �Լ�
    {
        Debug.LogWarning($"{gameObject.name} Ǯ������ ���� {poolSize} -> {poolSize * 2}");

        int newSize = poolSize * 2;    //���ο� ũ�� ���ϱ�
        T[] newPool = new T[newSize];  // ���ο� ũ�⸸ŭ �� �迭�����
        for (int i = 0; i < poolSize; i++) // ���� �迭�� �ִ����� �� �迭�� ����
        {
            newPool[i] = pool[i]; 
        }
        GenerateObjects(poolSize, newSize, newPool); // �� �迭�� ���� �κп� ������Ʈ �����ؼ� ����
        pool = newPool; // �� �迭�� Ǯ�� ����
        poolSize = newSize; // �� ũ�⸦ ũ��� ����
    }

    /// <summary>
    /// // ������Ʈ �����ؼ� �迭�� �߰����ִ� �Լ�
    /// </summary>
    /// <param name="start">�迭�� �����ε���</param>
    /// <param name="end">�迭�� �������ε��� -1</param>
    /// <param name="newArray">������ ������Ʈ�� �� �迭</param>
    void GenerateObjects(int start, int end, T[] newArray) 
    {
        for (int i = start; i < end; i++) //���� ������� �ݺ�
        {
            GameObject obj = Instantiate(originalPrefab, transform);// ���� �� Ǯ�� �θ� ������Ʈ�� ���� 
            obj.name = $"{originalPrefab.name}_{i}"; //�̸� �����ϱ����� ����

            T comp = obj.GetComponent<T>();  // pooledObject ������Ʈ�����ͼ� 
            comp.onDisable += () => readyQueue.Enqueue(comp); // disable�ɶ� ť�� �ǵ����� 

            newArray[i] = comp; // �迭�� �����ϰ� 
            obj.SetActive(false);//��Ȱ��ȭ ��Ű�� ��Ȱ��ȭ�ɶ� ������ �߰��� ���ٽ��� ����Ǹ鼭 ť�� �ٽ� ���ư��Եȴ�.
            
        }
    }
}
