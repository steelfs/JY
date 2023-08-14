using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public const int ID_NOT_VALID = -1;// Id가 정상이 아니라는 것을 알리기 위한 상수
    int id = ID_NOT_VALID;//셀의 ID 위치계산에도 사용가능 

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

    enum CellMarkState
    {
        None = 0,
        Flag,
        Question
    }
    CellMarkState markState = CellMarkState.None;//현재 표시된 마크

    int aroundMineCount = 0;//주변 8방향 지뢰 개수
    bool hasMine = false;//지뢰가 있는지
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


}
