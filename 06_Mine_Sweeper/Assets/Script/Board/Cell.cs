using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public const int ID_NOT_VALID = -1;// Id�� ������ �ƴ϶�� ���� �˸��� ���� ���
    int id = ID_NOT_VALID;//���� ID ��ġ��꿡�� ��밡�� 

    public Action onFlagUse;//��� ��ġ�� ����� ��������Ʈ
    public Action onFlagReturn;// ��� ������

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

    public enum CellMarkState
    {
        None = 0,
        Flag,
        Question
    }
    CellMarkState markState = CellMarkState.None;//���� ǥ�õ� ��ũ
    public CellMarkState MarkState
    {
        get => markState;
        set
        {
  
                markState = value;
                switch (markState)
                {
                    case CellMarkState.None:
                        cover.sprite = Board[CloseCellType.Close];
                        break;
                    case CellMarkState.Flag:
                        cover.sprite = Board[CloseCellType.Flag];
                        break;
                    case CellMarkState.Question:
                        cover.sprite = Board[CloseCellType.Question];
                        break;
                    default:
                        break;
                }
            
        }
    }
    public bool IsFlaged => markState == CellMarkState.Flag;


    int aroundMineCount = 0;//�ֺ� 8���� ���� ����
    public int AroundMineCount => aroundMineCount;
    bool hasMine = false;//���ڰ� �ִ���
    public bool HasMine => hasMine;
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

    List<Cell> neighbors = null;
    List<Cell> pressedCells = null;// �� ���� ���� ������ ���� ���

    public Action<int> onMineSet;

    private void Awake()
    {
        cover = transform.GetChild(0).GetComponent<SpriteRenderer>();
        inside = transform.transform.GetChild(1).GetComponent<SpriteRenderer>();

        pressedCells = new List<Cell>(9);
    }

    private void Start()
    {
        neighbors = Board.GetNeighbors(id);//�ֺ� �� �̸� ã�Ƴ���
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
    public void Open()//�� ���� ���� �Լ� // ���� ���콺 ���ʹ�ư ������ ���� ��
    {
        if (!isOpen && !IsFlaged)
        {
            isOpen = true;
            cover.gameObject.SetActive(false);
            if (hasMine)
            {
                inside.sprite = Board[OpenCellType.Mine_Explosion];
                GameManager.Inst.GameOver();
                //���ӿ���
            }
            else if (aroundMineCount == 0)
            {
                foreach(Cell cell in neighbors)
                {
                    cell.Open();
                }
                //�ֺ� ���� ��� ����.
            }
        }
    }
    public Action<int> onOpenAroundCell;
    public Action<int> onOpenAroundCell_Request;
    public void CellLeftRelease()
    {
        if (GameManager.Inst.IsPlaying)
        {
            if (isOpen)
            {
                //�ֺ� ��߰��� Ȯ��
                int flagCount = 0;
                foreach (Cell cell in neighbors)
                {
                    if (cell.IsFlaged)
                    {
                        flagCount++;
                    }
                }
                if (flagCount == aroundMineCount)
                {
                    foreach (Cell cell in pressedCells)
                    {
                        cell.Open();
                    }
                }
                else
                {
                    RestoreCovers();// ��߰����� ������ ������ �ٸ��� �ǵ�����.
                }
                RestoreCovers();
            }
            else
            {
                Open();
            }
        }
    }

    //���ʹ�ư �������¿��� ���콺 ��ġ ����� ���� ���� ������Ʈ
    public void CellLeftPressed()
    {
        if (GameManager.Inst.IsPlaying)
        {
            pressedCells.Clear();// ���� ���������� �����͵��� ���� 
            if (isOpen)//�̹� Ŭ���� �� ���̶��
            {
                //�ֺ� 8�� �� �����ִ� ���鸸 ������ ǥ�ø� �Ѵ�.
                foreach (var cell in neighbors)//stack overflow
                {
                    if (!cell.isOpen && !cell.IsFlaged)//������ ���� �Ͱ� �÷��װ� ���°͸�
                    {
                        pressedCells.Add(cell);
                        cell.CellLeftPressed();
                    }
                }
            }
            else//���� Ŭ���� �ȵ�����
            {
                //�� ���� �������ִ� ǥ�ø� �Ѵ�.
                switch (markState)
                {
                    case CellMarkState.None:
                        cover.sprite = Board[CloseCellType.Close_Press];
                        break;
                    case CellMarkState.Question:
                        cover.sprite = Board[CloseCellType.Question_Press];
                        break;
                    case CellMarkState.Flag:
                        break;
                    default:
                        break;
                }
                pressedCells.Add(this);
            }
        }
       
    }
 
    public void CellRightPressed()
    {
        //markState�� ���� cover�� ��������Ʈ ����
        if (GameManager.Inst.IsPlaying)
        {
            switch (markState)
            {
                case CellMarkState.None:
                    MarkState = CellMarkState.Flag;
                    onFlagUse?.Invoke();
                    break;
                case CellMarkState.Flag:
                    MarkState = CellMarkState.Question;
                    onFlagReturn?.Invoke();
                    break;
                case CellMarkState.Question:
                    MarkState = CellMarkState.None;
                    break;
                default:
                    break;
            }
        }
    }

    public void RestoreCover()
    {
        switch (markState)
        {
            case CellMarkState.None:
                cover.sprite = Board[CloseCellType.Close];
                break;
            case CellMarkState.Flag:
                
                break;
            case CellMarkState.Question:
                break;
            default:
                break;
        }
    }
    public void RestoreCovers()//���� ������ ��� ������ �ǵ�����
    {
        foreach (var cell in pressedCells)
        {
            cell.RestoreCover();
        }
        pressedCells.Clear();
    }
}
//release�� �� �ֺ� ���� ������ 0�̸� �ֺ����� ��� ����.
// �����ִ� ���� ������ ��� �ֺ� 8�� �� �� �����ִ� ���� ��� ����ǥ��
// �� ���¿��� ���콺�� ���� �� �ֺ� ��߰����� aroundMineCount�� ������ ���ǥ�ð��� ���� �����ϰ� ��� ����
