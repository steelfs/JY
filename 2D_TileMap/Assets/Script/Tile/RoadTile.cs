using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class RoadTile : Tile
{
    [Flags]// �� enum�� ��Ʈ�÷��׷� ����Ѵٴ� ��Ʈ����Ʈ
    enum AdjustTilePosition : byte//  �� enum�� ũ��� 1byte
    {
        None = 0,// 0000 0000
        North = 1,// 0000 0001
        East = 2,// 0000 0010
        South = 4,// 0000 0100
        West = 8,// 0000 1000
        All = North | East | South | West // 0000 1111
    }
    public Sprite[] sprites;// ���� �̹��� ���ϵ� 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">Ÿ���� ��ġ (�׸����� ��ǥ��)</param>
    /// <param name="tilemap">�� Ÿ���� �׷����� Ÿ�ϸ�</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)// Ÿ���� �׷��� �� �ڵ����� ȣ��Ǵ� �Լ� 
    {
        for(int y = -1; y < 2; y++)
        {
            for (int x = -1; x <2; x++)
            {
                Vector3Int location = new Vector3Int(position.x + x, position.y + y, position.z);
                if (HasThisTile(tilemap, location))
                {
                    tilemap.RefreshTile(location);// ���� Ÿ���̸� ����
                }
            }
        }
    }
    /// <summary>
    /// Ÿ�ϸ��� refreshtile�� ȣ����� �� � ��������Ʈ�� �׸��� �����ϴ� �Լ�  ref ������ �ѱ� �ʱ�ȭ ���� ,  out�� ȣ���ϴ� ���� ���̽� ������ �ʱ�ȭ ��
    /// </summary>
    /// <param name="position">Ÿ�� �����͸� ������ Ÿ���� ��ġ</param>
    /// <param name="tilemap">Ÿ�� �����͸� ������ Ÿ�ϸ�</param>
    /// <param name="tileData">������ Ÿ�ϵ������� ���� (�б�, ���� �� �� ����)</param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        AdjustTilePosition mask = AdjustTilePosition.None;
        //position �ֺ��� Ÿ���� ��� ��ġ�Ǿ� �ִ°� 
        //if ( HasThisTile(tilemap,position + new Vector3Int(0, 1, 0)))
        //{
        //    mask |= 
        //}
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)) ? AdjustTilePosition.North : 0; // ���̸� ���� 
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, -1, 0)) ? AdjustTilePosition.South : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(1, 0, 0)) ? AdjustTilePosition.East : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? AdjustTilePosition.West : 0;

        int index = GetIndex(mask); // � ��������Ʈ�� �׸� ������ ����
        if (index > -1)
        {
            tileData.sprite = sprites[index];
            tileData.color = Color.white;
            Matrix4x4 m = tileData.transform;
            m.SetTRS(Vector3.zero, GetRotation(mask), Vector3.one);
            tileData.transform = m;
            tileData.flags = TileFlags.LockTransform; //Ʈ������ ��� (�ٸ�Ÿ���� ȸ���� ����Ű���� ����� )
            tileData.colliderType = ColliderType.None;//�ö��̴� ����
        }
        else
        {
            Debug.LogError($"�߸��� �ε��� : {index} mask : {mask}");
        }
    }

    /// <summary>
    /// ����ũ ��Ȳ�� ���� �� ��° ��������Ʈ�� �׷��� �ϴ��� �˷��ִ� �Լ� 
    /// </summary>
    /// <param name="mask">�ֺ�Ÿ�ϻ�Ȳ ǥ���� ����ũ</param>
    /// <returns>�׷����ϴ� ��������Ʈ�� �ε���</returns>
    int GetIndex(AdjustTilePosition mask)
    {
        //swi tab, tab, enter,enter

        int index = -1;
        switch (mask)
        {
            case AdjustTilePosition.None:
            case AdjustTilePosition.North:
            case AdjustTilePosition.East:
            case AdjustTilePosition.South:
            case AdjustTilePosition.West:
            case AdjustTilePosition.North | AdjustTilePosition.South:
            case AdjustTilePosition.East | AdjustTilePosition.West:
                index = 0; // 1�� ��� ��������Ʈ
                break;

            case AdjustTilePosition.South | AdjustTilePosition.West:
            case AdjustTilePosition.North | AdjustTilePosition.West:
            case AdjustTilePosition.North | AdjustTilePosition.East:
            case AdjustTilePosition.South | AdjustTilePosition.East:
                index = 1;// �� �� ��� ��������Ʈ
                break;

            case AdjustTilePosition.All & ~AdjustTilePosition.North:
            case AdjustTilePosition.All & ~AdjustTilePosition.East:
            case AdjustTilePosition.All & ~AdjustTilePosition.South:
            case AdjustTilePosition.All & ~AdjustTilePosition.West:
                index = 2;// �� ���  // 1111 & ~0001 = 1111 & 1110 = 1110
                break;
             
  
            case AdjustTilePosition.All:
                index = 3;//+��
                break;
            default:
                break;
        }
        return index;
    }

    /// <summary>
    /// ����ũ ��Ȳ�� ���� ��������Ʈ�� �󸶳� ȸ����ų ������ ����
    /// </summary>
    /// <param name="mask"></param>
    /// <returns></returns>
    Quaternion GetRotation(AdjustTilePosition mask)
    {
        Quaternion rotate = Quaternion.identity;
        switch (mask)
        {
            case AdjustTilePosition.East:       // 1��
            case AdjustTilePosition.West:

            case AdjustTilePosition.East | AdjustTilePosition.West:
            case AdjustTilePosition.North | AdjustTilePosition.West: // ��

            case AdjustTilePosition.All & ~AdjustTilePosition.West: // ��
                rotate = Quaternion.Euler(0, 0, -90);
                break;

            case AdjustTilePosition.North | AdjustTilePosition.East: // ��
            case AdjustTilePosition.All & ~AdjustTilePosition.North: // ��
                rotate = Quaternion.Euler(0, 0, -180);
                break;

            case AdjustTilePosition.South | AdjustTilePosition.East:
            case AdjustTilePosition.All & ~AdjustTilePosition.East:
                rotate = Quaternion.Euler(0, 0, -270);
                break;
            default:
                break;
        }
        return rotate;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tileMap"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    bool HasThisTile(ITilemap tileMap, Vector3Int position)// Ư��Ÿ�ϸ��� Ư�� ��ġ�� �� Ÿ�ϰ� ���� ������ Ÿ���� �ִ��� Ȯ���ϴ� �Լ�
    {
        return tileMap.GetTile(position) == this; // Ÿ�ϸ��� ������ �ִ°��� ����Ÿ���� ����
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Tiles/Custom/RoadTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject( // ���� ���� â ���� �Է°���� path ����
            "Save Road Tile",   // ����
            "New Road Tile",    // ������ �⺻ �̸�
            "Asset",            // ������ �⺻ Ȯ����
            "Save Road Tile",   // ��� �޼���
            "Assets");          // ������ �⺻ ����
        if (path != string.Empty)    // ���� �Է��� �Ǿ�����
        {
            AssetDatabase.CreateAsset(CreateInstance<RoadTile>(), path);    // RoadTile�� ���Ϸ� ����
        }
    }
# endif
}
