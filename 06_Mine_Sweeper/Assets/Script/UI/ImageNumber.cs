using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    //�� UI�� ǥ���� ���� ������ (-99 ~ 999)
    private int number;
    public Sprite[] numberSprite;//����� ���� ��������Ʈ

    private Image[] RenderImage;// ȭ�鿡 ǥ�õ� �̹��� 0 : 

    Sprite ZeroSprite => numberSprite[0];
    Sprite MinusSprite => numberSprite[10];
    Sprite EmptySprite => numberSprite[11];

    public int Number
    {
        get => number;
        set
        {
            if (number != value)// ���� ��ȭ�� ��������
            {
                number = Mathf.Clamp(value, -99, 999);
                Refresh();
            }
        }
    }
    private void Awake()
    {
        RenderImage = GetComponentsInChildren<Image>();
    }
    void Refresh()
    {
        int temp = Mathf.Abs(number);// number�� �����
        Queue<int> digits = new Queue<int>(3);

        //�ڸ������� ����� digits�� ���
        while(temp > 0)
        {
            digits.Enqueue(temp % 10); //1�� �ڸ� ���� �����ϰ� 
            temp /= 10; // 1���ڸ� ����
        }

        //digits �� ����� �����͸� ������� �̹��� ǥ���ϱ�
        int index = 0;
        while(digits.Count > 0)
        {
            int num = digits.Dequeue();                         //�ϳ��� ť���� ������ 
            RenderImage[index].sprite = numberSprite[num];     //�������� �ش��ϴ� �ε����� 
            index++;
        }

        for (int i = index; i < RenderImage.Length; i++)
        {
            RenderImage[i].sprite = ZeroSprite;
        }
        if (number < 0)//������ ��� 
        {
            RenderImage[RenderImage.Length - 1].sprite = MinusSprite;
        }
    }
}
