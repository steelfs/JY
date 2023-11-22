using EnumList;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// UI ���� ���� ĳ���� ����ٴ� ī�޶� 
/// Texture ������ ī�޶�
/// </summary>
public class UICamera : MonoBehaviour ,ICameraBase
{

    /// <summary>
    /// �����Ⱦ���
    /// </summary>
    EnumList.CameraFollowType cameraTarget = EnumList.CameraFollowType.Custom;
    public CameraFollowType TargetType 
    {
        get => cameraTarget; 
        set => cameraTarget =value; 
    }
    /// <summary>
    /// ���⼱ �Ⱦ���.
    /// </summary>
    public Vector3 Distance { get; set; }

    /// <summary>
    /// ���⼱ �Ƚ�
    /// </summary>
    public float FollowSpeed { get; set; }




    /// <summary>
    /// ĳ���� ���� ī�޶�
    /// </summary>
    Camera actionCam = null;
    public Camera FollowCamera => actionCam;
   

    [SerializeField]
    /// <summary>
    /// �������� ������Ʈ
    /// </summary>
    private Transform target = null;
    public Transform TargetObject 
    {
        get => target; 
        set => target = value; 
    }

    /// <summary>
    /// ī�޶�� ��ǥ���� ����
    ///  - 1 ĳ���� �󱼺���  1 ĳ���� ����� ����
    /// </summary>
    [SerializeField]
    [Range(-1.0f,1.0f)]
    float distance = 1.0f;

    /// <summary>
    /// ��ī�޶�� EtcObjects �ֻ��� ���ӿ�����Ʈ�ȿ��� ť�� ���������� ť�� ���� ��������Ʈ 
    /// </summary>
    public Action resetData;
   
    private void Awake()
    {
        //gameObject.layer = LayerMask.NameToLayer("UI");///������ ���̾� �˻��ؼ� ��ȣ ��������
        //gameObject.tag = "Respawn"; //�±� �����Ű��
        actionCam = GetComponent<Camera>();
        actionCam.targetTexture = new RenderTexture(512,512,16,RenderTextureFormat.ARGB32);
    }


    IEnumerator MoveCamera()
    {
        while (true)
        {
            TrackingValueSetting();
            yield return null;

        }
    }
    
    private void OnEnable()
    {
        if (target != null) 
        {
            StartCoroutine(MoveCamera());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        resetData?.Invoke(); //ť�ε�����.

    }

    /// <summary>
    /// ī�޶� Ư�� ��ǥ�� ���� x,y ������ z ���������� �ٶ󺸰Ը����.
    /// </summary>
    private void TrackingValueSetting() 
    {
        transform.position = target.transform.position  // ��ǥ �������� 
                            -(target.transform.forward * distance); // ��ǥ�� ������⿡  distance �� ���ѵ�  - �� �ؼ� �ٶ󺸴¹��� �ݴ������� ��ġ ��Ų��.

        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up); //������ͷ� �ٶ󺸰� ����� y��������� ������.
    }

}
