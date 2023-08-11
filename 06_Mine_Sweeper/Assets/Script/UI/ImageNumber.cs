using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    //이 UI로 표시할 숫자 범위는 (-99 ~ 999)
    private int number;
    public Sprite[] numberSprite;//사용할 숫자 스프라이트

    private Image[] RenderImage;// 화면에 표시될 이미지 0 : 

    Sprite ZeroSprite => numberSprite[0];
    Sprite MinusSprite => numberSprite[10];
    Sprite EmptySprite => numberSprite[11];

    public int Number
    {
        get => number;
        set
        {
            if (number != value)// 값의 변화가 있을때만
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
        int temp = Mathf.Abs(number);// number를 양수로
        Queue<int> digits = new Queue<int>(3);

        //자리수별로 나누어서 digits에 담기
        while(temp > 0)
        {
            digits.Enqueue(temp % 10); //1의 자리 값을 저장하고 
            temp /= 10; // 1의자리 제거
        }

        //digits 에 담겨진 데이터를 기반으로 이미지 표시하기
        int index = 0;
        while(digits.Count > 0)
        {
            int num = digits.Dequeue();                         //하나씩 큐에서 꺼내고 
            RenderImage[index].sprite = numberSprite[num];     //꺼낸값에 해당하는 인덱스에 
            index++;
        }

        for (int i = index; i < RenderImage.Length; i++)
        {
            RenderImage[i].sprite = ZeroSprite;
        }
        if (number < 0)//음수일 경우 
        {
            RenderImage[RenderImage.Length - 1].sprite = MinusSprite;
        }
    }
}
