using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �⺻������ ������ ����Ǵ� ������Ʈ�ϰ�� 
/// �ش�Ŭ������ ��ӹ޾Ƽ� ��� 
/// </summary>
/// <typeparam name="T">ObjectIsPool�� ��ӹ��� Ŭ����</typeparam>
public class Base_Pool_Multiple<T> : MonoBehaviour where T : Base_PoolObj
{
    /// <summary>
    /// Ǯ�� ��� ���� ������Ʈ�� ������
    /// </summary>
    public GameObject origianlPrefab;

    /// <summary>
    /// Ǯ�� ũ��. ó���� �����ϴ� ������Ʈ�� ����. ������ 2^n���� ��� ���� ����.
    /// </summary>
    public int poolSize = 8;

    /// <summary>
    /// Ǯ�� ������ ��� ������Ʈ�� ����ִ� �迭
    /// </summary>
    protected T[] pool;

    /// <summary>
    /// ��밡����(��Ȱ��ȭ�Ǿ��ִ�) ������Ʈ���� ����ִ� ť
    /// </summary>
    protected Queue<T> readyQueue;

    /// <summary>
    /// ������Ʈ ������ġ ������ ���� �߰� 
    /// �̺����� ������Ʈ�� ��Ȱ��ȭ�������� GenerateObjects �Լ��� ������ ��Ȱ��ȭ ���ѵ� OnDisable�Լ��� ȣ���̾ȵȴ�.
    /// </summary>
    protected Transform setPosition;

    /// <summary>
    /// ȭ����ȯ�� ��� �߻��Ͽ� ���װ������,.
    /// </summary>
    public void Initialize()
    {
        StartInitialize();  //��ü �����Ǳ����� ó���� ������ �ִ°�츦 ���� �߰�

        if (setPosition == null) {//������Ʈ ������ġ �����ϴ� ���������� ���ǹ��߰�
            setPosition = transform;
        }

        
        if (pool == null)
        {
            pool = new T[poolSize];                 // Ǯ ��ü ũ��� �迭 �Ҵ�
            readyQueue = new Queue<T>(poolSize);    // ����ť ����(capacity�� poolSize�� ����)
            //readyQueue.Count;       // ������ ����ִ� ����
            //readyQueue.Capatity;    // ���� �̸� �غ��� ���� ����

            GenerateObjects(0, poolSize, pool);
        }
        else
        {
            
            // �ι�° ���� �ҷ����� �̹� Ǯ�� ������� �ִ� ��Ȳ
            foreach (T obj in pool)
            {
                obj.gameObject.SetActive(false);    // ���� ��Ȱ��ȭ
            }
            
        }
        EndInitialize();//�ʱ�ȭ ������  ó�� �ҳ��� 
    }

    /// <summary>
    /// Ǯ���� ������Ʈ�� �ϳ� ���� �� �����ִ� �Լ�
    /// </summary>
    /// <returns>����ť���� ������ Ȱ��ȭ��Ų ������Ʈ</returns>
    public T GetObject()
    {
        if (readyQueue.Count > 0)    // ����ť�� �����ִ� ������Ʈ�� �ִ��� Ȯ��
        {
            // ����������
            T comp = readyQueue.Dequeue();      // �ϳ� ������

            comp.onEnable_InitData?.Invoke();              // �ʱ�ȭ�� ���������� �ʱ�ȭ �ϰ� 

            comp.gameObject.SetActive(true);    // Ȱ��ȭ��Ų ������ 
            
            return comp;                        // ���� �� ����

        }
        else
        {
            // ���� ������Ʈ�� ������
            ExpandPool();           // Ǯ Ȯ���Ű��
            return GetObject();     // �ٽ� ��û
        }
    }

    /// <summary>
    /// Ǯ�� �ι�� Ȯ���Ű�� �Լ�
    /// </summary>
    void ExpandPool()
    {
        Debug.LogWarning($"{gameObject.name} Ǯ ������ ����. {poolSize} -> {poolSize * 2}");

        int newSize = poolSize * 2;     // ���ο� ũ�� ���ϱ�
        T[] newPool = new T[newSize];   // ���ο� ũ�⸸ŭ �� �迭 �����
        for (int i = 0; i < poolSize; i++)     // ���� �迭�� �ִ� ���� �� �迭�� ����
        {
            newPool[i] = pool[i];
        }

        GenerateObjects(poolSize, newSize, newPool);    // �� �迭�� ���� �κп� ������Ʈ �����ؼ� ����
        pool = newPool;     // �� �迭�� pool�� ����
        poolSize = newSize; // �� ũ�⸦ ũ��� ����
    }

    /// <summary>
    /// ������Ʈ �����ؼ� �迭�� �߰����ִ� �Լ�
    /// </summary>
    /// <param name="start">�迭�� ���� �ε���</param>
    /// <param name="end">�迭�� ������ �ε���-1</param>
    /// <param name="newArray">������ ������Ʈ�� �� �迭</param>
    void GenerateObjects(int start, int end, T[] newArray)
    {
        // �� �Լ��� Ǯ�� ���ʿ��Ұ�� (���󵵰� ����)�� �������� �Լ����ο����� ����Ҽ��ְ� �ڵ������� ����
        // for���ȿ��� ���� �޸𸮰����� ��� �߰��� �������ִ°� �������Ͱ��Ƽ� ������������ ������ �ۿ��� �����Ͽ� ����Ѵ�
        
        Transform tf = setPosition; //��Ȱ��ȭ�� �θ� üũ�ϱ����� ��ü
        
        GameObject obj; //�߰��� ���ӿ�����Ʈ ����

        bool isParentActive = true; //�ش� �Լ��� �θ���� Ȱ��ȭ �Ǿ��ٴ� �����Ͽ� �۵������� �����۵����Ϸ��� true �� ���־���Ѵ�. 


        //�߰�����ġ�� ������Ʈ�� ��Ȱ��ȭ �Ǿ������� SetActive(false)���ص� OnDisable �� �ߵ����������� üũ�����ش�.
        while (tf != null || !isParentActive) //�θ� �ֻ���̰ų� �θ� ��Ȱ��ȭ �������� ����������
        {
            isParentActive = tf.gameObject.activeSelf; //�θ��� Ȱ��ȭ���θ� üũ�Ѵ�. ���⼱ false ���� �ִ��� Ȯ���Ѵ�.
            tf = tf.parent; //�θ��� ��ġ�� ����
        }

        for (int i = start; i < end; i++)    // ���� ������� ũ�⸸ŭ �ݺ�
        {
            //Ư����ġ�� �����ϱ� �⺻�����δ� Ǯ�Ʒ��� �ִ�.
            obj = Instantiate(origianlPrefab, setPosition); //�����Ǹ� �ϴ� Awake =>  OnEnable ������ �����Ų��.
            obj.name = $"Pool_{origianlPrefab.name}_{i}";            // �̸� ���еǵ��� ����

            T comp = obj.GetComponent<T>();                     // PooledObject ������Ʈ �޾ƿͼ�
            //Debug.Log($"{obj.name} _ {comp == null}");
            comp.onDisable += () =>
            {
                readyQueue.Enqueue(comp);   // PooledObject�� disable�� �� ����ť�� �ǵ�����
            };

            newArray[i] = comp;     // �迭�� ����

            comp.onGetPoolTransform?.Invoke(transform);

            obj.SetActive(false);   // ������ ���� ������Ʈ ��Ȱ��ȭ(=>��Ȱ��ȭ �Ǹ鼭 ����ť���� �߰��ȴ�)
      
            if (!isParentActive) // ���� ������Ʈ���� ��Ȱ��ȭ�������� SetActive(false)�� �ص� OnDisable �� �ߵ������Ͽ� ť���� ���������ִ�.
            { 
                readyQueue.Enqueue(comp);   // ���� ������Ʈ�� ��Ȱ��ȭ ���ִ°��  �߰�������
            }
            
        }
    }
    /// <summary>
    /// Ǯ���� �����̵ǰ� ������Ʈ�� �����ɽÿ� �ٽ� Ǯ�ε��������� Ǯ��ġ�� �Ѱ��ֱ����� �Լ�
    /// </summary>
    /// <param name="comp">�߰��� ��ü</param>
    protected virtual void ReturnPoolTransformSetting(T comp ,Transform poolObj) 
    {
        
    }
    /// <summary>
    /// Ǯ���� ��ü ���� ���� ó���ؾ��� ���� 
    /// </summary>
    protected virtual void StartInitialize() { }

    /// <summary>
    /// Ǯ���� ��ü ���� �Ŀ� ó���ؾ� �� ���� ��ӹ޾Ƽ� ���ڻ��
    /// </summary>
    protected virtual void EndInitialize()
    {
    }
}