using UnityEngine;

public class CellVisualizer : MonoBehaviour
{
    GameObject[] walls;
    Renderer[] wall_Renderers;
    Renderer ground_Renderer;
    Renderer territory_Renderer;

    public Material default_Wall;
    public Material default_Ground;
    public Material path_Wall;
    public Material path_Ground;
    public Material confirmed_Ground;
    public Material confirmed_Wall;
    public Material next_Ground;
    public Material next_Wall;
    public Material player_Mat;
    public Material npc_01_Mat;
    public Material npc_02_Mat;
    public Material npc_03_Mat;

    PlayerType playerType = PlayerType.None;
    public PlayerType PlayerType
    {
        get => playerType;
        set
        {
            playerType = value;
            switch (playerType)
            {
                case PlayerType.None:
                    territory_Renderer.material = default_Ground;
                    break;
                case PlayerType.Player:
                    territory_Renderer.material = player_Mat;
                    break;
                case PlayerType.NPC01:
                    territory_Renderer.material = npc_01_Mat;
                    break;
                case PlayerType.NPC02:
                    territory_Renderer.material = npc_02_Mat;
                    break;
                case PlayerType.NPC03:
                    territory_Renderer.material = npc_03_Mat;
                    break;
                default:
                    break;
            }
        }
    }

    public int x { get; set; }
    public int y { get; set; }
    private void Awake()
    {
        territory_Renderer = transform.GetChild(5).GetComponent<Renderer>();
        ground_Renderer = transform.GetChild(0).GetComponent<Renderer>();
        int length = transform.childCount - 1;
        wall_Renderers = new Renderer[4];
        walls = new GameObject[4];
        for (int i = 1; i < 5; i++)
        {
            walls[i - 1] = transform.GetChild(i).gameObject;
            wall_Renderers[i - 1] = transform.GetChild(i).GetComponent<Renderer>();
        }

    }

    public void RefreshWalls(byte data)
    {
        for (int i = 0; i < walls.Length; i++)
        {
            int mask = 1 << i;
            walls[i].gameObject.SetActive(!((data & mask) != 0));//������ �Ǿ�������� SetActive(false);
        }
        //ex 2�� 2�� and�� �ϸ� 1�� �ƴ϶� 2�� ������ ������ 0�� �ƴ� �������� üũ�� �ؾ��Ѵ�. != 1 �� üũ�� �ϸ� ��Ʈ�� ������ �Ǿ��־ false�� ��ȯ�Ѵ�.
    }
    public void TestCoroutine()
    {

    }
  
    public void OnSet_Path_Material()
    {
        ground_Renderer.material = path_Ground;
        foreach(var renderer in wall_Renderers)
        {
            renderer.material = path_Wall;
        }
    }
    public void OnSet_Default_Material()
    {
        ground_Renderer.material = default_Ground;
        foreach (var renderer in wall_Renderers)
        {
            renderer.material = default_Wall;
        }
    }
    public void OnSet_Next_Material()
    {
        ground_Renderer.material = next_Ground;
        foreach (var renderer in wall_Renderers)
        {
            renderer.material = next_Wall;
        }
    }
    public void OnSet_Confirmed_Material()
    {
        ground_Renderer.material = confirmed_Ground;
        foreach (var renderer in wall_Renderers)
        {
            renderer.material = confirmed_Wall;
        }
    }
}
