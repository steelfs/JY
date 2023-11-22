using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/*
  Ÿ�� 4(2X2) , 9(3X3) , 16(4X4) ĭ�� 1���� ������.
  
 */
/// <summary>
/// Ÿ�� �⺻���� 
/// </summary>
public class Base_TileCell : Base_PoolObj, ITileBase
{
    int index = -1;
    /// <summary>
    /// �ٴڿ� �� Ÿ�� Ÿ�ϱ�ü�� �ʿ� 
    /// </summary>
    [SerializeField]
    GameObject prefab;

    LineRenderer lr;

    [SerializeField]
    float tileModelSize = 1.0f;

    /// <summary>
    /// Ÿ�� �׸���  �ε��� ��ǥ :  x�� , y�� ,  z����
    /// </summary>
    Vector3Int tile3DPosIndex = Vector3Int.one;
    public Vector3Int Tile3DPos => tile3DPosIndex;

    /// <summary>
    /// Ÿ�� ������ üũ �⺻���� 1
    /// </summary>
    Vector3 tileSize = Vector3.one;
    public Vector3 TileSize => tileSize;

    /// <summary>
    /// ���� Ÿ������ ��ġ�� ��Ȳ
    /// </summary>
    [SerializeField]
    CurrentTileState currentTileState = CurrentTileState.None;
    public CurrentTileState CurruntTileState
    {
        get => currentTileState;
        set
        {
            //���� ���°� ��ֹ��̰ų� , �������������϶� ��
            if (currentTileState == CurrentTileState.Prop || currentTileState == CurrentTileState.InaccessibleArea)
            {
                return;//���þ��Ѵ�.
            }
            else if (currentTileState != value) //�ٸ����� ���ð�츸  
            {
                lr.enabled = false;
                currentTileState = value; //�������ϰ� 
                switch (currentTileState) //�����Ѱ������� ���� ó��
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

        defaultLinePos = new Vector3[lr.positionCount]; //�ν����� �����Ѱ�����ŭ ��� 
        for (int i = 0; i < lr.positionCount; i++)
        {
            defaultLinePos[i] = lr.GetPosition(i); //�����ѵ����� �⺻������ ���� 
        }

    }
  
    public void OnInitData(int index, Vector3Int gridPos, CurrentTileState currentTileState)
    {
        this.index = index;
        lr.enabled = false;
        Vector3 tempPos = Vector3.zero;
        // ��ġ���� Ÿ�ϻ���� �̿��ؼ� ����ε� ��ġ ���
        float tempX = gridPos.x * tileModelSize;
        float tempY = gridPos.y * tileModelSize;
        float tempZ = gridPos.z * tileModelSize;

        // ���η����� ��ġ ��� 
        for (int i = 0; i < lr.positionCount; i++)
        {
            tempPos = lr.GetPosition(i);
            tempPos = defaultLinePos[i]; //���ʷ� ������ �⺻������ �ʱ�ȭ 
            tempPos.x += tempX;
            tempPos.y += tempZ;
            tempPos.z += tempY;
            lr.SetPosition(i, tempPos);
        }

        tile3DPosIndex = gridPos; //�ε��� ���� ���

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
