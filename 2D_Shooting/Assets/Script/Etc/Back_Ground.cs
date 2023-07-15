using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_Ground : MonoBehaviour
{
    public Transform[] bgslots; //��� �� ��
    public float scrolling_Speed = 2.5f; //��� �̵��ӵ�

    public const float backGroundWidth = 13.5f; // ��� �� ���� ����


    protected virtual void Awake()
    {

        bgslots = new Transform[transform.childCount]; //�ڽ� ������ŭ�� ũ�⸦ ������ �迭 �����
        for (int i = 0; i < bgslots.Length; i++)
        {
            bgslots[i] = transform.GetChild(i); //�迭�ȿ� Ʈ������ �ֱ�
        }
    }
    void Start()
    {
        
    }
    void Update()
    {
        float baseLineX = transform.position.x - backGroundWidth; //����� ����� �̵��ߴ����� �����̵Ǵ� ũ��
        for (int i = 0; i< bgslots.Length; i++) //��� ���� �����̱�
        {
            bgslots[i].transform.Translate(Time.deltaTime * scrolling_Speed * -transform.right); //bgslot���� ���ɱ�� �̵���Ű��
            if (bgslots[i].position.x < baseLineX) //���ؼ��� �Ѿ��� ��
            {
                MoveRightEnd(i); //������ ������ �̵�
            }
        }
    }

    protected virtual void MoveRightEnd(int index) //������ �P���� ������ �Լ�
    {
  
        bgslots[index].Translate(backGroundWidth * bgslots.Length * transform.right);
    }
}
//����
// v1 = (1,2,3)

//
/*
 * ������� =  �������� - �������
 * 
 * ������ ���ϱ�
 * - ���� * ���� = 2���� (����, ����)
 * - ���� * ��Į��(������ ũ�� ) = ����
 * 
 * v1 * 5 = (5,10, 15)
 * v1 /2 = (0.5, 1, 1.5)
 * 
 *  ���� (Dot Product) ��� : ��Į�� = (v1.x * v2.x) + (v1.y * v2.y) + (v1.z * v2.z) ĳ���� �밡�� ������
 *  
 *  ���� (cross Product) ����� ���� �� ���Ͱ� �̷�� ��鿡 ������ ����(��� ����)
 *  3D - ī�޶� �������� �κи� ������ �Ѵ�.
 *  
 *  
 *  ��� Transform �� �����ϴµ� ����� ���ȴ�.
 *  I, identify = ������� = �ٸ� ��Ŀ� ���ص� ���� ��ȭ�� ����. ���ڿ��� 1�� ���� ����
 *                �����(-1) = � ��Ŀ� ������� ���ϸ� ��������� ����ȴ�.
 *                
 *  Transform = ��ȯ���
 *  
 *              �̵���ȯ  -- (1, 0, 0, x)            
 *              ȸ����ȯ
 *              �����Ϻ�ȯ
 *              SRT - Scale, Rotate, Transform
 */