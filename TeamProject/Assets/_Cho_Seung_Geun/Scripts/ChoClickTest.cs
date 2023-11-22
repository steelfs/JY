using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChoClickTest : MonoBehaviour
{
    /// <summary>
    /// ȭ���� ��� ī�޶�
    /// </summary>
    Camera mainCamera;
    /// <summary>
    /// ���� ��ġ�� �̵��� ĳ���� �ӵ�
    /// </summary>
    float speed = 4.0f;
    /// <summary>
    /// ȭ�鿡�� ��� ���� �ε����� �ڽ��ݶ��̴�
    /// </summary>
    BoxCollider target = null;
    /// <summary>
    /// ��ǲ�ý��� Ŭ��
    /// </summary>
    InputKeyMouse inputClick;

    MapTest map;
    List<Tile> path = null;

    Tile currentPos = null;
    public Tile CurrentPos
    {
        get => currentPos;
        set
        {
            if (currentPos != value)
            {
                if (currentPos != null)
                {
                    currentPos.ExistType = Tile.TileExistType.None;
                }
                else
                {
                    // �±پ��� �浹���ϱ� �±پ� ��ũ��Ʈ �����ؼ� ����������ð� 
                    // �����ؼ� �̸��ٲ㼭 ����ϵ����ϼ��� �Ƚ������̾ƴ϶� �����Ҷ����� �����������־�� 
                    transform.position = value.transform.position;
                }
                currentPos = value;
                currentPos.ExistType = Tile.TileExistType.Monster;
            }
        }
    }

    private void Awake()
    {
        inputClick = new InputKeyMouse();
    }
    private void OnEnable()
    {
        inputClick.BattleMap_Player.Enable();
        inputClick.BattleMap_Player.UnitMove.performed += onClick;

        map = FindObjectOfType<MapTest>();
        path = new List<Tile>();
    }

    private void OnDisable()
    {
        inputClick.BattleMap_Player.UnitMove.performed -= onClick;
        inputClick.BattleMap_Player.Disable();

        if (path != null )
            path.Clear();
        path = null;
    }

    /// <summary>
    /// ���콺�� Ŭ������ �� �Ͼ �Լ�
    /// </summary>
    /// <param name="context"></param>
    private void onClick(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());      // ȭ�鿡�� ���� ���콺�� ��ġ�� ��� ��
        Debug.DrawRay(ray.origin, ray.direction * 20.0f, Color.red, 1.0f);              // ����׿� ������

        if (Physics.Raycast(ray, out RaycastHit hitInfo))                       // ���� ���� �ε�����
        {
            if (hitInfo.transform.gameObject.CompareTag("Tile"))                // �±� "Ÿ��"�� �浹�ϸ�
            {
                target = (BoxCollider)hitInfo.collider;                         // Ÿ���� �ڽ��ݶ��̴� ��ȯ
                Tile tile = target.gameObject.GetComponent<Tile>();
                if (tile.ExistType == Tile.TileExistType.None)
                {
                    path = AStar.PathFind(map, CurrentPos, tile);
                }
                //CurrentPos = tile;
                Debug.Log($"Ÿ�� ��ġ : {tile.Width}, {tile.Length}");
            }
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;           //  ��� �ִ� ī�޶� ��������
    }

    private void FixedUpdate()
    {
        // Ÿ���� ������Ʈ�� �ƴϰ� Ÿ���� �������� �ʾ��� �� �̵�
        //if (target != null && (target.gameObject.transform.position - transform.position).sqrMagnitude > 0.01f)
        //{
        //    transform.Translate(Time.fixedDeltaTime * speed * (target.gameObject.transform.position - transform.position).normalized);
        //}

        if (path.Count > 0 )
        {
            Tile destPath = path[0];
        
            Vector3 dir = destPath.transform.position - transform.position;
        
            if (dir.sqrMagnitude < 0.01f )
            {
                transform.position = destPath.transform.position;
                CurrentPos = destPath;
                path.RemoveAt(0);
            }
            else
            {
                transform.Translate(Time.fixedDeltaTime * speed * dir.normalized);
            }
        }

    }
}
