using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPCTest : MonoBehaviour
{
    [SerializeField]
    GameObject questNPCPrefab;
    private void Start()
    {
        Tile[] battleMap = SpaceSurvival_GameManager.Instance.BattleMap;
        int maxX = SpaceSurvival_GameManager.Instance.MapSizeX;
        int maxY = SpaceSurvival_GameManager.Instance.MapSizeY;
        float a =  Random.Range(0.0f, 10.0f);
        Instantiate(questNPCPrefab, new Vector3(a,a,a), Quaternion.identity);
        //go.SetTile(SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster));
    }
}
