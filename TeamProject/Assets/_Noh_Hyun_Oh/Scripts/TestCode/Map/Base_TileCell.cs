using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/*
  타일 4(2X2) , 9(3X3) , 16(4X4) 칸이 1유닛 사이즈.
  
 */
/// <summary>
/// 타일 기본정보 
/// </summary>
public class Base_TileCell : Base_PoolObj, ITileBase
{
    int index = -1;
    /// <summary>
    /// 바닥에 깔릴 타일 타일교체시 필요 
    /// </summary>
    [SerializeField]
    GameObject prefab;

    LineRenderer lr;

    [SerializeField]
    float tileModelSize = 1.0f;

    /// <summary>
    /// 타일 그리드  인덱스 좌표 :  x좌 , y우 ,  z높이
    /// </summary>
    Vector3Int tile3DPosIndex = Vector3Int.one;
    public Vector3Int Tile3DPos => tile3DPosIndex;

    /// <summary>
    /// 타일 사이즈 체크 기본값은 1
    /// </summary>
    Vector3 tileSize = Vector3.one;
    public Vector3 TileSize => tileSize;

    /// <summary>
    /// 현재 타일위에 배치된 상황
    /// </summary>
    [SerializeField]
    CurrentTileState currentTileState = CurrentTileState.None;
    public CurrentTileState CurruntTileState
    {
        get => currentTileState;
        set
        {
            //현재 상태가 장애물이거나 , 갈수없는지역일때 는
            if (currentTileState == CurrentTileState.Prop || currentTileState == CurrentTileState.InaccessibleArea)
            {
                return;//셋팅안한다.
            }
            else if (currentTileState != value) //다른값이 들어올경우만  
            {
                lr.enabled = false;
                currentTileState = value; //값셋팅하고 
                switch (currentTileState) //셋팅한값에의해 각자 처리
                {
                    case CurrentTileState.None:
                        break;
                    case CurrentTileState.Charcter:
                        break;
                    case CurrentTileState.MoveArea:
                        lr.enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    Vector3[] defaultLinePos;

    public Action<int> OnClick { get; set; }

    protected override void Awake()
    {
        base.Awake();
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;

        defaultLinePos = new Vector3[lr.positionCount]; //인스팩터 설정한갯수만큼 잡고 
        for (int i = 0; i < lr.positionCount; i++)
        {
            defaultLinePos[i] = lr.GetPosition(i); //설정한데이터 기본값으로 셋팅 
        }

    }
  
    public void OnInitData(int index, Vector3Int gridPos, CurrentTileState currentTileState)
    {
        this.index = index;
        lr.enabled = false;
        Vector3 tempPos = Vector3.zero;
        // 위치값과 타일사이즈를 이용해서 제대로된 위치 잡기
        float tempX = gridPos.x * tileModelSize;
        float tempY = gridPos.y * tileModelSize;
        float tempZ = gridPos.z * tileModelSize;

        // 라인렌더러 위치 잡기 
        for (int i = 0; i < lr.positionCount; i++)
        {
            tempPos = lr.GetPosition(i);
            tempPos = defaultLinePos[i]; //최초로 설정한 기본값으로 초기화 
            tempPos.x += tempX;
            tempPos.y += tempZ;
            tempPos.z += tempY;
            lr.SetPosition(i, tempPos);
        }

        tile3DPosIndex = gridPos; //인덱스 정보 담기

        this.currentTileState = currentTileState;
        //Debug.Log($"{gridPos} _ {currentTileState}");
        transform.position = new Vector3(tempX, tempZ, tempY);
    }


    public void OnResetData()
    {
        tile3DPosIndex = Vector3Int.zero;
        currentTileState = CurrentTileState.None;
        gameObject.SetActive(false);
    }

   

    private void OnMouseDown()
    {
        Debug.Log(index);
    }

}
