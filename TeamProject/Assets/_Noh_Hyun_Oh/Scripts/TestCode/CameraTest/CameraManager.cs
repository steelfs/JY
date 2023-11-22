using EnumList;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 1. ī�޶� ����� ������Ʈ���� �̳Ѹ���� (�����������Ϸ� �����߰� �ʿ�)
/// 2. �̳����������� ���̳Ѻ� �ൿ���� ??
///  2-1. ī�޶��  
///       - �� Ȯ�� ���,  v
///       - �����Ÿ������ϸ� ���󰡱��� ,  v
///       - ������Ʈ ȸ�����⿡���� ȸ�����ִ±�� v
///      
/// UI ���� ���� ĳ���� ����ٴ� ī�޶� v
/// Texture ������ ī�޶� v
/// </summary>
public class CameraManager : TestBase 
{
    /// <summary>
    /// ��Ʈ���� ī�޶�
    /// </summary>
    public Camera actionCam = null;

    CameraBase camBase = null;

    [Header("ī�޶� ������ Ÿ��")]
    /// <summary>
    /// ī�޶� �� ����� ������Ʈ ����
    /// </summary>
    [SerializeField]
    protected EnumList.CameraFollowType cameraFollowType = EnumList.CameraFollowType.Custom;

    /// <summary>
    /// ī�޶� Ÿ���� �ٲ��� ���� �⺻�� ���� �ϱ����� ������Ƽ
    /// </summary>
    public virtual CameraFollowType CameraFollowType 
    {
        get => cameraFollowType;
        protected set 
        {
            ResetData();
            switch (value)
            {
                case CameraFollowType.Custom:
                    cameraMoveCoroutine = CustomCamera();
                    break;
                case CameraFollowType.MiniMap:
                    CustomCameraPos.y = 50.0f;
                    CustomCameraRotate = miniMapRotate;
                    actionCam.targetTexture = cameraTexture;
                    textureImage.texture = cameraTexture;
                    cameraMoveCoroutine = MiniMapCamera();
                    break;
                case CameraFollowType.UITexture:
                    CustomCameraPos.z = 2.0f;
                    actionCam.targetTexture = cameraTexture;
                    textureImage.texture = cameraTexture;
                    cameraMoveCoroutine = UITextureView();
                    break;
                case CameraFollowType.QuarterView:
                    waitForSeconds = new WaitForSeconds(followSpeed);
                    CustomCameraPos = quarterViewPos;
                    cameraMoveCoroutine = QuarterView();
                    break;
                default:
                    break;
            }
            cameraFollowType = value;   
        }
    }

    public Camera FollowCamera => actionCam;
    [Header("ī�޶� ������ ��ü")]

    [SerializeField]
    /// <summary>
    /// �������� ������Ʈ �� Ʈ������
    /// </summary>
    protected Transform target = null;

    [SerializeField]
    /// <summary>
    /// �������� ������Ʈ�� ������ ��ü��ġ
    /// </summary>
    protected Transform attackTarget = null;
   


    /// <summary>
    /// ������Ȳ�� �ϳ��� �ڵ�� �����Ű������ ���λ���.
    /// </summary>
    protected IEnumerator cameraMoveCoroutine;


    [Header("ī�޶��� ���� �ӵ�")]

    /// <summary>
    /// ī�޶� ĳ���͸� ����ٴϴ� �ӵ�
    /// </summary>
    [SerializeField]
    [Range(0.0f, 5.0f)]
    float followSpeed = 1.0f;

    /// <summary>
    /// �ڷ�ƾ�� ���� ��ٸ� �ð� 
    /// </summary>
    protected WaitForSeconds waitForSeconds;

    /// <summary>
    /// �������� ������ ó���ϱ� ���� �ʿ��� ��ü 
    /// </summary>
    protected WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();
    

    [Header("����ڰ� �����Է��ؼ� �����Ҷ� ����Ұ�")]
    /// <summary>
    /// �Է°����� ī�޶� ȸ�� �����Ҷ� ���
    /// </summary>
    [SerializeField]
    protected Quaternion CustomCameraRotate = Quaternion.identity;


    /// <summary>
    /// �Է°����� ī�޶� ��ġ �����Ҷ� ���
    /// </summary>
    [SerializeField]
    protected Vector3 CustomCameraPos = Vector3.zero;

    /// <summary>
    /// UI �� ����� ī�޶�� ����� �ؽ��� 
    /// </summary>
    [SerializeField]
    RenderTexture cameraTexture;

    /// <summary>
    /// �ؽ��İ� ���� UI RawImage 
    /// </summary>
    [SerializeField]
    RawImage textureImage;

    /// <summary>
    /// 90�� �Ʒ��� �ٶ󺸴� ���� �����صα� �̴ϸʿ�
    /// </summary>
    readonly Quaternion miniMapRotate = Quaternion.AngleAxis(90.0f, Vector3.right);

    /// <summary>
    /// ���ͺ� ���� ��ġ�� 
    /// </summary>
    readonly Vector3 quarterViewPos =  new Vector3(0.0f, 10.0f, -5.0f);

    /// <summary>
    /// ĳ���� �� foward ����  ���� �� ����
    /// </summary>
    [SerializeField]
    Vector3 zoomInPos = new Vector3(0.0f, 0.0f, -3.0f);

    protected override void Awake()
    {
        base.Awake();
        //gameObject.layer = LayerMask.NameToLayer("UI");///������ ���̾� �˻��ؼ� ��ȣ ��������
        //gameObject.tag = "Respawn"; //�±� �����Ű��
        if (actionCam == null)
        {
            actionCam = GetComponent<Camera>();
            camBase = actionCam.GetComponent<CameraBase>();
        }
        else 
        {
            camBase = actionCam.GetComponent<CameraBase>();
        }

        cameraTexture = new(256 ,256,16,RenderTextureFormat.ARGB32); //�ؽ��� �⺻�� ���� ����� 
        cameraTexture.name = $"{gameObject.name}_Texture";           //�̸��̾�� �ް����� �ϴ� �־����.
        cameraMoveCoroutine = CustomCamera();
    }

    /// <summary>
    /// ������ �ɹ����� �ʱ�ȭ �ϴ��Լ� 
    /// </summary>
    protected virtual void ResetData() 
    {
        actionCam.targetTexture = null;
        CustomCameraPos = Vector3.zero;
        CustomCameraRotate = Quaternion.identity;
        cameraMoveCoroutine = null;
        if(textureImage != null) textureImage.texture = null;
    }

    /// <summary>
    /// ī�޶� Ÿ���� �����ǰ� ȸ���� ���� ������ ���󰡴� ���� 
    /// </summary>
    /// <returns></returns>
    IEnumerator CustomCamera()
    {
        if (target != null) //Ÿ���� �������� ����
        {
            while (cameraFollowType == CameraFollowType.Custom)
            {
                actionCam.transform.position = target.position + CustomCameraPos;
                actionCam.transform.rotation = target.rotation * CustomCameraRotate;

                yield return fixedWait;
            }
        }

    }

    /// <summary>
    /// ������ ȸ�������� �����ϰ� ������ �Ÿ��� ������ä Ÿ���� �i�ƴٴϴ� ���� 
    /// �̴ϸʿ�
    /// </summary>
    IEnumerator MiniMapCamera()
    {
        if (target != null)
        {
            while (cameraFollowType == CameraFollowType.MiniMap)
            {
                actionCam.transform.position = CustomCameraPos + target.position;
                actionCam.transform.rotation = CustomCameraRotate;

                yield return fixedWait;
            }
        }


    }



    /// <summary>
    /// UI�� ī�޶� ����
    /// </summary>
    IEnumerator UITextureView()
    {
        if (target != null)
        {

            while (cameraFollowType == CameraFollowType.UITexture)
            {
                actionCam.transform.position = target.position // ��ǥ �������� 
                                    - (target.forward * CustomCameraPos.z); // �ٶ󺸴� ���� �ݴ������� �����Ÿ� ���������� ī�޶���ġ���Ѵ�.

                actionCam.transform.rotation = Quaternion.LookRotation(target.position - actionCam.transform.position, target.up); //������ͷ� �ٶ󺸰� ����� y��������� ������.
                yield return fixedWait;
            }
        }
    }

    /// <summary>
    ///  ���ͺ� ������ ī�޶� ��ġ���ð� Ÿ�� �ٶ󺸱�
    /// </summary>
    IEnumerator QuarterView()
    {
        if (target != null)
        {

            while (cameraFollowType == CameraFollowType.QuarterView)
            {
                //Ÿ���� Y ���� ȸ������ �����´� .
                //�⺻������  Ÿ���� y�� ȸ�����Ѵٰ� ����  y ���� 0���� �����ɰ��̴�.  
                //Quaternion.Euler �Լ��� x y z  �� �� �������� ������� ȸ���ߴ��������� ������� ��ȯ�ϴ� �Լ��̴�.
                //target.eulerAngles �� ���� ����ϸ� �ش� target �� ��������� ������� ȸ���ߴ��� ������� ���Ҽ��ִ� �̰��� �ٷ� rotation ������ ������ �����ϴ�
                Quaternion rotation = Quaternion.Euler(0, target.eulerAngles.y, 0);  // y ������ Ÿ���� ������� ȸ���ߴ��������� ���� �޾ƿ´� 
                actionCam.transform.position = target.position + (rotation * CustomCameraPos); //ȸ�����⿡ �Ÿ��� ���ؼ� ���ͺ� ó�����̰� ��ġ�� ��´�

                // Ÿ���� ������ �ٶ󺸵��� ī�޶� ȸ��
                actionCam.transform.rotation = Quaternion.LookRotation(target.position - actionCam.transform.position);
                yield return null; // �Ƚõ� ������Ʈ���� �ߵ��ϵ��� �ɾ��ش�.
            }
        }

    }
    /// <summary>
    /// ���� �Ҷ� Ÿ���� ���߾��� �ƴ� ��¦ ���ܳ����� ��������� �Ÿ���
    /// </summary>
    [SerializeField]
    Vector3 zoomFocusPos = Vector3.zero;

    /// <summary>
    /// 
    /// </summary>
    private Vector3 tempPos = Vector3.zero;

   

   

   

    /// <summary>
    /// ī�޶� Ư����ġ�� �̵���ų�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="pos">ī�޶� �̵��� ��ġ��</param>
    public virtual void SetRotation(Vector3 pos) 
    {
        CustomCameraPos = pos;
    }
    /// <summary>
    /// ī�޶� Ư�� �������� ȸ����ų�� ����ϴ� �Լ� 
    /// </summary>
    /// <param name="rotate">ȸ�������Ұ�</param>
    public virtual void SetPosition(Quaternion rotate) 
    {
        CustomCameraRotate = rotate;
    }

    /// <summary>
    /// �����ѷ��� ����� 
    /// </summary>
    public virtual void CameraMoveStart() 
    {
        StartCoroutine(cameraMoveCoroutine);
    }

    /// <summary>
    /// �������� ��� ���߱�
    /// </summary>
    public virtual void CameraMoveEnd() 
    {
        StopCoroutine(cameraMoveCoroutine);
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        CameraFollowType = cameraFollowType;
        StartCoroutine(cameraMoveCoroutine);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        StopCoroutine(cameraMoveCoroutine);
        StartCoroutine(StaticCameraController.ZoomIn(target,actionCam,attackTarget.transform,followSpeed));
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        StopCoroutine (cameraMoveCoroutine);
        StartCoroutine(StaticCameraController.ZoomOut(target, actionCam, quarterViewPos, followSpeed));
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        camBase.FollowTarget = target;
        camBase.LookTarget  = attackTarget;
    }
}
