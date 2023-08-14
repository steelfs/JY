using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public const int ID_NOT_VALID = -1;// Id�� ������ �ƴ϶�� ���� �˸��� ���� ���
    int id = ID_NOT_VALID;//���� ID ��ġ��꿡�� ��밡�� 

    public int ID
    {
        get => id;
        set
        {
            if (id == ID_NOT_VALID)//���� �������� �ʾ��� ���� ����
            {
                id = value;
            }
        }
    }

    SpriteRenderer cover;//�Ѹ� ǥ�ÿ� ��������Ʈ
    SpriteRenderer inside;//���� ��������Ʈ

    enum CellMarkState
    {
        None = 0,
        Flag,
        Question
    }
    CellMarkState markState = CellMarkState.None;//���� ǥ�õ� ��ũ

    int aroundMineCount = 0;//�ֺ� 8���� ���� ����
    bool hasMine = false;//���ڰ� �ִ���
    bool isOpen = false;//���� �����ִ��� 

    Board parentBoard = null;
    public Board Board
    {
        get => parentBoard;
        set
        {
            if (parentBoard == null)//�ѹ��� ���� ����
            {
                parentBoard = value;
            }
        }
    }
    public Action<int> onMineSet;

    private void Awake()
    {
        cover = transform.GetChild(0).GetComponent<SpriteRenderer>();
        inside = transform.transform.GetChild(1).GetComponent<SpriteRenderer>();
    }
    public void ResetData()
    {
        markState = CellMarkState.None;
        isOpen = false;
        hasMine = false;
        aroundMineCount = 0;
        cover.sprite = Board[CloseCellType.Close];
        inside.sprite = Board[OpenCellType.Empty];
        cover.gameObject.SetActive(true);// �ٽ� �ݴ� �Լ��� ����
    }
    public void SetMine()
    {
        hasMine = true;  //���ڼ�ġ ǥ��
        inside.sprite = Board[OpenCellType.Mine_NotFound];// �⺻ ���ڽ�������Ʈ ����
        onMineSet?.Invoke(ID);//��ȣ������
    }
    public void IncreaseAroundMineCount()//�� �� �ֺ��� ���ڰ� ��ġ�Ǹ� AroundMineCount�� 1���� ��Ű�� �Լ� (�� ���� ���ڰ� ��ġ�Ǿ����� ���� ����)
    {
        if (!hasMine)
        {
            aroundMineCount++;
            inside.sprite = Board[(OpenCellType)aroundMineCount];
        }
    }


}
