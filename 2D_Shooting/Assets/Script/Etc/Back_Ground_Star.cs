using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_Ground_Star : Back_Ground
{
    public float moveSpeed = 3.0f;
    public float starWidth = 13.5f;
    Transform[] starGroup;
    SpriteRenderer[] spriteRender;
    protected override void Awake()
    {
        spriteRender = GetComponentsInChildren<SpriteRenderer>(); // 나와 내 자식에 있는 Spriterender 를 모두 찾아 배열에 담아 리턴하는 함수
        starGroup = new Transform[transform.childCount];
        for (int i = 0; i < starGroup.Length; i++)
        {
            starGroup[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        float baseLine = transform.position.x - starWidth;
        for  (int i = 0; i <starGroup.Length; i++)
        {
            starGroup[i].Translate(Time.deltaTime * moveSpeed * -transform.right);
            if (starGroup[i].position.x < baseLine)
            {
                MoveRightEnd(i);
            }
        }
    }
    protected override  void MoveRightEnd(int i)
    {
        starGroup[i].Translate(starWidth * starGroup.Length * transform.right);


        //C#에서 숫자 앞에 0b_를 붙히면 2진수 라는 의미(0b_0100_1010) 10진수 74
        //C#에서 숫자 앞에 0x_를 붙히면 16진수 라는 의미(0x4a) 10진수 74

        //10 진수를 2진수로 바꾸는 법 - 어떤 수를 나눴을때 나머지가 0이 될 때 까지 계속나눈 후 나머지값
        int randomValue = Random.Range(0, 4);
        spriteRender[i].flipX = (randomValue & 0b_01) != 0; //randomvalue의 첫번째 비트가 0이냐 1이냐 true면 첫번째 비트는 1 false면 첫번째 비트는 0
        spriteRender[i].flipY = (randomValue & 0b_10) != 0;// randomvalue의 두번째 비트가 0이냐 1이냐 true면 두번째 비트는 1 false면 첫번째 비트는 0
        //true or false 에 해당하는 숫자를 저장해서 사용할 수 있다.

    }
}
