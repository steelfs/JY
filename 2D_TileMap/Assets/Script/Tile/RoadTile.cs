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
    [Flags]// 이 enum은 비트플래그로 사용한다는 어트리뷰트
    enum AdjustTilePosition : byte//  이 enum의 크기는 1byte
    {
        None = 0,// 0000 0000
        North = 1,// 0000 0001
        East = 2,// 0000 0010
        South = 4,// 0000 0100
        West = 8,// 0000 1000
        All = North | East | South | West // 0000 1111
    }
    public Sprite[] sprites;// 구성 이미지 파일들 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">타일의 위치 (그리드의 좌표값)</param>
    /// <param name="tilemap">이 타일이 그려지는 타일맵</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)// 타일이 그려질 때 자동으로 호출되는 함수 
    {
        for(int y = -1; y < 2; y++)
        {
            for (int x = -1; x <2; x++)
            {
                Vector3Int location = new Vector3Int(position.x + x, position.y + y, position.z);
                if (HasThisTile(tilemap, location))
                {
                    tilemap.RefreshTile(location);// 같은 타일이면 갱신
                }
            }
        }
    }
    /// <summary>
    /// 타일맵의 refreshtile이 호출됐을 때 어떤 스프라이트를 그릴지 결정하는 함수  ref 참조만 넘김 초기화 없음 ,  out은 호출하는 순간 베이스 데이터 초기화 됨
    /// </summary>
    /// <param name="position">타일 데이터를 가져올 타일의 위치</param>
    /// <param name="tilemap">타일 데이터를 가져올 타일맵</param>
    /// <param name="tileData">가져온 타일데이터의 참조 (읽기, 쓰기 둘 다 가능)</param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        AdjustTilePosition mask = AdjustTilePosition.None;
        //position 주변에 타일이 어떻게 배치되어 있는가 
        //if ( HasThisTile(tilemap,position + new Vector3Int(0, 1, 0)))
        //{
        //    mask |= 
        //}
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)) ? AdjustTilePosition.North : 0; // 참이면 왼쪽 
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, -1, 0)) ? AdjustTilePosition.South : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(1, 0, 0)) ? AdjustTilePosition.East : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? AdjustTilePosition.West : 0;

        int index = GetIndex(mask); // 어떤 스프라이트를 그릴 것인지 결정
        if (index > -1)
        {
            tileData.sprite = sprites[index];
            tileData.color = Color.white;
            Matrix4x4 m = tileData.transform;
            m.SetTRS(Vector3.zero, GetRotation(mask), Vector3.one);
            tileData.transform = m;
            tileData.flags = TileFlags.LockTransform; //트렌스폼 잠금 (다른타일이 회전을 못시키도록 만들기 )
            tileData.colliderType = ColliderType.None;//컬라이더 없음
        }
        else
        {
            Debug.LogError($"잘못된 인덱스 : {index} mask : {mask}");
        }
    }

    /// <summary>
    /// 마스크 상황에 따라 몇 번째 스프라이트를 그려야 하는지 알려주는 함수 
    /// </summary>
    /// <param name="mask">주변타일상황 표시한 마스크</param>
    /// <returns>그려야하는 스프라이트의 인덱스</returns>
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
                index = 0; // 1자 모양 스프라이트
                break;

            case AdjustTilePosition.South | AdjustTilePosition.West:
            case AdjustTilePosition.North | AdjustTilePosition.West:
            case AdjustTilePosition.North | AdjustTilePosition.East:
            case AdjustTilePosition.South | AdjustTilePosition.East:
                index = 1;// ㄱ 자 모양 스프라이트
                break;

            case AdjustTilePosition.All & ~AdjustTilePosition.North:
            case AdjustTilePosition.All & ~AdjustTilePosition.East:
            case AdjustTilePosition.All & ~AdjustTilePosition.South:
            case AdjustTilePosition.All & ~AdjustTilePosition.West:
                index = 2;// ㅗ 모양  // 1111 & ~0001 = 1111 & 1110 = 1110
                break;
             
  
            case AdjustTilePosition.All:
                index = 3;//+자
                break;
            default:
                break;
        }
        return index;
    }

    /// <summary>
    /// 마스크 상황에 따라 스프라이트를 얼마나 회전시킬 것인지 결정
    /// </summary>
    /// <param name="mask"></param>
    /// <returns></returns>
    Quaternion GetRotation(AdjustTilePosition mask)
    {
        Quaternion rotate = Quaternion.identity;
        switch (mask)
        {
            case AdjustTilePosition.East:       // 1자
            case AdjustTilePosition.West:

            case AdjustTilePosition.East | AdjustTilePosition.West:
            case AdjustTilePosition.North | AdjustTilePosition.West: // ㄱ

            case AdjustTilePosition.All & ~AdjustTilePosition.West: // ㅗ
                rotate = Quaternion.Euler(0, 0, -90);
                break;

            case AdjustTilePosition.North | AdjustTilePosition.East: // ㄱ
            case AdjustTilePosition.All & ~AdjustTilePosition.North: // ㅗ
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
    bool HasThisTile(ITilemap tileMap, Vector3Int position)// 특정타일맵이 특정 위치에 이 타일과 같은 종류의 타입이 있는지 확인하는 함수
    {
        return tileMap.GetTile(position) == this; // 타일맵이 가지고 있는것은 원본타일의 참조
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Tiles/Custom/RoadTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject( // 파일 저장 창 열고 입력결과를 path 저장
            "Save Road Tile",   // 제목
            "New Road Tile",    // 파일의 기본 이름
            "Asset",            // 파일의 기본 확장자
            "Save Road Tile",   // 출력 메세지
            "Assets");          // 열리는 기본 폴더
        if (path != string.Empty)    // 뭔가 입력이 되었으면
        {
            AssetDatabase.CreateAsset(CreateInstance<RoadTile>(), path);    // RoadTile를 파일로 저장
        }
    }
# endif
}
