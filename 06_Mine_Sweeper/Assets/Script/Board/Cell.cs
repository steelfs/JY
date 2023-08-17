using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public const int ID_NOT_VALID = -1;// Id가 정상이 아니라는 것을 알리기 위한 상수
    int id = ID_NOT_VALID;//셀의 ID 위치계산에도 사용가능 

    public Action onFlagUse;//깃발 설치시 실행될 델리게이트
    public Action onFlagReturn;// 깃발 해제시

    public int ID
    {
        get => id;
        set
        {
            if (id == ID_NOT_VALID)//아직 설정되지 않았을 때만 설정
            {
                id = value;
            }
        }
    }

    SpriteRenderer cover;//겉면 표시용 스프라이트
    SpriteRenderer inside;//안쪽 스프라이트

    public enum CellMarkState
    {
        None = 0,
        Flag,
        Question
    }
    CellMarkState markState = CellMarkState.None;//현재 표시된 마크
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


    int aroundMineCount = 0;//주변 8방향 지뢰 개수
    public int AroundMineCount => aroundMineCount;
    bool hasMine = false;//지뢰가 있는지
    public bool HasMine => hasMine;
    bool isOpen = false;//셀이 열려있는지 

    Board parentBoard = null;
    public Board Board
    {
        get => parentBoard;
        set
        {
            if (parentBoard == null)//한번만 설정 가능
            {
                parentBoard = value;

            }
        }
    }

    List<Cell> neighbors = null;
    List<Cell> pressedCells = null;// 이 셀에 의해 눌려진 셀의 목록

    public Action<int> onMineSet;

    private void Awake()
    {
        cover = transform.GetChild(0).GetComponent<SpriteRenderer>();
        inside = transform.transform.GetChild(1).GetComponent<SpriteRenderer>();

        pressedCells = new List<Cell>(9);
    }

    private void Start()
    {
        neighbors = Board.GetNeighbors(id);//주변 셀 미리 찾아놓기
    }
    public void ResetData()
    {
        markState = CellMarkState.None;
        isOpen = false;
        hasMine = false;
        aroundMineCount = 0;
        cover.sprite = Board[CloseCellType.Close];
        inside.sprite = Board[OpenCellType.Empty];
        cover.gameObject.SetActive(true);// 다시 닫는 함수는 없음
    }
    public void SetMine()
    {
        hasMine = true;  //지뢰설치 표시
        inside.sprite = Board[OpenCellType.Mine_NotFound];// 기본 지뢰스프라이트 설정
        onMineSet?.Invoke(ID);//신호보내기
    }
    public void IncreaseAroundMineCount()//이 셀 주변에 지뢰가 배치되면 AroundMineCount를 1증가 시키는 함수 (이 셀에 지뢰가 배치되어있지 않을 때만)
    {
        if (!hasMine)
        {
            aroundMineCount++;
            inside.sprite = Board[(OpenCellType)aroundMineCount];
        }
    }
    public void Open()//이 셀을 여는 함수 // 셀을 마우스 왼쪽버튼 눌렀다 땠을 때
    {
        if (!isOpen && !IsFlaged)
        {
            isOpen = true;
            cover.gameObject.SetActive(false);
            if (hasMine)
            {
                inside.sprite = Board[OpenCellType.Mine_Explosion];
                GameManager.Inst.GameOver();
                //게임오버
            }
            else if (aroundMineCount == 0)
            {
                foreach(Cell cell in neighbors)
                {
                    cell.Open();
                }
                //주변 셀을 모두 연다.
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
                //주변 깃발개수 확인
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
                    RestoreCovers();// 깃발갯수와 지뢰의 갯수가 다르면 되돌리기.
                }
                RestoreCovers();
            }
            else
            {
                Open();
            }
        }
    }

    //왼쪽버튼 누른상태에서 마우스 위치 변경시 셀의 상태 업데이트
    public void CellLeftPressed()
    {
        if (GameManager.Inst.IsPlaying)
        {
            pressedCells.Clear();// 새로 눌려졌으니 기존것들을 제거 
            if (isOpen)//이미 클릭이 된 것이라면
            {
                //주변 8개 중 닫혀있는 셀들만 누르는 표시를 한다.
                foreach (var cell in neighbors)//stack overflow
                {
                    if (!cell.isOpen && !cell.IsFlaged)//열리지 않은 것과 플레그가 없는것만
                    {
                        pressedCells.Add(cell);
                        cell.CellLeftPressed();
                    }
                }
            }
            else//아직 클릭이 안됐으면
            {
                //이 셀만 누르고있는 표시를 한다.
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
        //markState에 따라 cover의 스프라이트 변경
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
    public void RestoreCovers()//주위 눌려진 모든 셀들을 되돌리기
    {
        foreach (var cell in pressedCells)
        {
            cell.RestoreCover();
        }
        pressedCells.Clear();
    }
}
//release될 때 주변 지뢰 개수가 0이면 주변셀을 모두 연다.
// 열려있는 셀을 눌렀을 경우 주변 8개 셀 중 닫혀있는 셀은 모두 눌린표시
// 위 상태에서 마우스를 땠을 때 주변 깃발개수와 aroundMineCount가 같으면 깃발표시가된 셀을 제외하고 모두 연다
