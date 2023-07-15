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
        spriteRender = GetComponentsInChildren<SpriteRenderer>(); // ���� �� �ڽĿ� �ִ� Spriterender �� ��� ã�� �迭�� ��� �����ϴ� �Լ�
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


        //C#���� ���� �տ� 0b_�� ������ 2���� ��� �ǹ�(0b_0100_1010) 10���� 74
        //C#���� ���� �տ� 0x_�� ������ 16���� ��� �ǹ�(0x4a) 10���� 74

        //10 ������ 2������ �ٲٴ� �� - � ���� �������� �������� 0�� �� �� ���� ��ӳ��� �� ��������
        int randomValue = Random.Range(0, 4);
        spriteRender[i].flipX = (randomValue & 0b_01) != 0; //randomvalue�� ù��° ��Ʈ�� 0�̳� 1�̳� true�� ù��° ��Ʈ�� 1 false�� ù��° ��Ʈ�� 0
        spriteRender[i].flipY = (randomValue & 0b_10) != 0;// randomvalue�� �ι�° ��Ʈ�� 0�̳� 1�̳� true�� �ι�° ��Ʈ�� 1 false�� ù��° ��Ʈ�� 0
        //true or false �� �ش��ϴ� ���ڸ� �����ؼ� ����� �� �ִ�.

    }
}
