using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    Player player;
    Vector3 newPos;

    public float smooth = 2;
    private void Start()
    {
        player = GameManager.Inst.Player;
    }
    private void Update()
    {
        newPos.x = player.transform.position.x;
        newPos.y = player.transform.position.y + 40;
        newPos.z = player.transform.position.z;
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * smooth);
    }
    
}
