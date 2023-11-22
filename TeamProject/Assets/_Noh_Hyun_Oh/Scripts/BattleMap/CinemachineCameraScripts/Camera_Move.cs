using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// ���콺�� ȭ�� �����ڸ��� ��� ���ϴ��������� ������
/// </summary>
public enum Screen_Side_Mouse_Direction : byte
{
    None = 0,
    Left = 1,
    Right = 2,
    Top = 4,
    Bottom = 8,
}

/// <summary>
///  ��Ʋ��  �ó׸ӽ� ����� ī�޶� �̵� Ŭ����
/// </summary>
public class Camera_Move : MonoBehaviour
{
    /// <summary>
    /// ĳ���� ����ٴϴ� �⺻ī�޶�
    /// </summary>
    [SerializeField]
    CinemachineVirtualCamera originFollowCameraObject;
    /// <summary>
    /// ȭ�� ���� ���콺 �÷����� �̵��� ī�޶�
    /// </summary>
    [SerializeField]
    CinemachineVirtualCamera moveCam;
    /// <summary>
    /// ȭ������� ����� ��ġ���� ����� �극�� ī�޶�
    /// </summary>
    [SerializeField]
    CinemachineBrain brain;
    public CinemachineBrain Brain 
    {
        get 
        {
            if (brain == null) //ȣ��������� ������ 
            {
                brain = GetCineBrainCam?.Invoke(); //�ѹ� ã�´� .
            }
            return brain;
        }
    }
    /// <summary>
    /// �극�� ī�޶� ã�ƿ��� ��������Ʈ 
    /// �ʱ�ȭ ���۳�Ʈ InitCharcterSetting ���� ������
    /// </summary>
    public Func<CinemachineBrain> GetCineBrainCam;
    /// <summary>
    ///  ȭ���̵��Ҷ� �켱���� ������������ �������� ����
    /// </summary>
    readonly int viewIndex = 1000;
    /// <summary>
    /// ȭ�� �̵��������� �켱���� ��ȯ�� ������ ����
    /// </summary>
    readonly int closeIndex = 0;

    /// <summary>
    /// ī�޶� �̵��ӵ� 
    /// </summary>
    [SerializeField]
    float moveSpeed = 10.0f;

    /// <summary>
    /// ī�޶� �̵����� �޾ƿ� ��������Ʈ
    /// </summary>
    public Action<Screen_Side_Mouse_Direction> moveCamera;

    [SerializeField]
    /// <summary>
    /// �̵��� ���� ���� ����
    /// </summary>
    Vector3 tempMoveDir = Vector3.zero;

    /// <summary>
    /// ī�޶� ȸ�������� ������Ʈ
    /// </summary>
    Transform cameraOriginObject;

    private void Awake()
    {
        //���� ī�޶� �������°� ���⼭ �������� 
        //���߿� ī�޶� �Ƚ��Ǹ� ���⿡ ã�·����߰�
        moveCamera += OnMove;

        //Follow �������� awake ������ Vcam �� ����ε� ȸ������ ������ �ȵǱ⶧���̴�

    }
    private void OnEnable()
    {
        OnInitPos();
    }
    public void OnInitPos() 
    {
        moveCam.transform.position = originFollowCameraObject.transform.position; //�ϴ� ��ġ �⺻ī�޶��
        moveCam.transform.rotation = originFollowCameraObject.transform.rotation; //ī�޶��̵��� �̻������ʰ� ���������
        cameraOriginObject = originFollowCameraObject.Follow;
    }
    /// <summary>
    /// ī�޶� �̵� ���� ������
    /// </summary>
    /// <param name="dir">���콺�� ��ġ�� ��ũ�� ����</param>
    private void OnMove(Screen_Side_Mouse_Direction dir)
    {
        // ȸ�������̵Ǵ� ī�޶��� ȸ������ �����´�
        switch (dir)
        {
            case Screen_Side_Mouse_Direction.None:
                tempMoveDir = Vector3.zero;
                moveCam.Priority = closeIndex; //����ī�޶�� �켱�����ѱ��
                break;

            case Screen_Side_Mouse_Direction.Left | Screen_Side_Mouse_Direction.Top:
                moveCam.Priority = viewIndex; //�켱���� ��������
                tempMoveDir = -cameraOriginObject.transform.right;
                tempMoveDir += cameraOriginObject.transform.forward;
                break;
            case Screen_Side_Mouse_Direction.Right | Screen_Side_Mouse_Direction.Top:
                moveCam.Priority = viewIndex; //�켱���� ��������
                tempMoveDir = cameraOriginObject.transform.right;
                tempMoveDir += cameraOriginObject.transform.forward;
                break;

            case Screen_Side_Mouse_Direction.Left | Screen_Side_Mouse_Direction.Bottom:
                moveCam.Priority = viewIndex; //�켱���� ��������
                tempMoveDir = -cameraOriginObject.transform.right;
                tempMoveDir -= cameraOriginObject.transform.forward;
                break;
            case Screen_Side_Mouse_Direction.Right | Screen_Side_Mouse_Direction.Bottom:
                moveCam.Priority = viewIndex; //�켱���� ��������
                tempMoveDir = cameraOriginObject.transform.right;
                tempMoveDir -= cameraOriginObject.transform.forward;
                break;

            case Screen_Side_Mouse_Direction.Left:
                moveCam.Priority = viewIndex; //�켱���� ��������
                tempMoveDir = -cameraOriginObject.transform.right; 
                break;
            case Screen_Side_Mouse_Direction.Right:
                moveCam.Priority = viewIndex; //�켱���� ��������
                tempMoveDir = cameraOriginObject.transform.right;
                break;
            case Screen_Side_Mouse_Direction.Top:
                moveCam.Priority = viewIndex; //�켱���� ��������
                tempMoveDir = cameraOriginObject.transform.forward;
                break;
            case Screen_Side_Mouse_Direction.Bottom:
                moveCam.Priority = viewIndex; //�켱���� ��������
                tempMoveDir = -cameraOriginObject.transform.forward;
                break;
            default:
                break;
        }
        
        moveCam.transform.position = Brain.transform.position;
        //�⺻������ ���� �����̱⶧���� ȭ������� ����°��� �����ϱ����� �극���� ����ٴϰ� �����Ѵ�.

    }
    //�̵��� ȸ�� ����
    private void Update()
    {
        moveCam.transform.rotation = originFollowCameraObject.transform.rotation;
        moveCam.transform.position += Time.deltaTime * moveSpeed * tempMoveDir;
    }

}
