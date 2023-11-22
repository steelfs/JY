using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ī�޶� �ھƵ� ������Ʈ
/// ī�޶� �� Ư�� Ÿ���� ���� �������� �̵��ϴ·����� 
/// ī�޶� Ư�� Ÿ���� ���� �ٶ󺸴� ���� �� ��Ƶ� ������Ʈ 
/// </summary>
public class CameraBase : MonoBehaviour
{
#if UNITY_EDITOR
    /// <summary>
    /// �����ͻ󿡼� ���൵�� ������ϱ����� ���� ���忡 ���ԾȵǴ� �̷������� �ص��������Ű���.
    /// </summary>
    [SerializeField]
    bool DEBUGCHECK = false;
#endif

    /// <summary>
    /// ī�޶� �̵��ӵ� 
    /// </summary>
    [SerializeField]
    float cameraMoveSpeed = 1.0f;

    /// <summary>
    /// ī�޶� ȸ���ӵ�
    /// </summary>
    [SerializeField]
    float cameraRotateSpeed = 1.0f;

    /// <summary>
    /// ī�޶� �̵��� ��ġ�� ������ ������Ʈ Ʈ������
    /// </summary>
    Transform followTarget;
    public Transform FollowTarget 
    {
        get => followTarget;
        set 
        {
            if (followTarget != value) // ���� �ٲ�� 
            {
                followTarget = value; //�����ϰ�  �̵������� �����Ѵ�

                StopCoroutine(cameraMoving); //���� �̵� ���߰�

                cameraMoving = StandardMove(); //���Ӱ� �������

                StartCoroutine(cameraMoving); // �����

            }
        }
    }

    /// <summary>
    /// ī�޶� �ٶ� ����� �ִ°�� ���� 
    /// </summary>
    Transform lookTarget;
    public Transform LookTarget 
    {
        get => lookTarget;
        set 
        {
            if (lookTarget != value) 
            {
                lookTarget = value;

                StopCoroutine(moveRotation); //���� ȸ�� ���߰�

                moveRotation = MoveRotation(); //���Ӱ� �������

                StartCoroutine(moveRotation); // �����
            }
        }
    }

    /// <summary>
    /// �̵� �ڷ�ƾ ��Ƶ� ����
    /// </summary>
    public IEnumerator cameraMoving;

    /// <summary>
    /// �̵� ���� ī�޶� �ٶ󺸴� Ÿ���������� �ٶ󺼼��ִ� ������ ��Ƶ� �ڷ�ƾ
    /// </summary>
    public IEnumerator moveRotation;

    /// <summary>
    /// �̵��� ������� Ÿ�ٸ� �ٶ󺸰� �Ҽ��ִ� ����
    /// </summary>
    public IEnumerator idleRotation;


    private void Awake()
    {
        cameraMoving = StandardMove();
        moveRotation = MoveRotation();
    }

    /// <summary>
    /// standardTarget ��ġ�� �̵��ϴ� ����
    /// �̵����Ҷ� Ÿ���� �����ϸ� �ٶ�����Ѵ�.
    /// </summary>
    /// <param name="timeElaspad">tlrk</param>
    IEnumerator StandardMove(float timeElaspad = 0.0f)
    {
        if (lookTarget != null) //�ٶ� ����� ������ 
        {
            StopCoroutine(moveRotation); //���� �ٶ󺸴��� ���߰� 
            moveRotation = MoveRotation(timeElaspad); //���� �ٶ󺸴� �������
            StartCoroutine(moveRotation); //�ٶ󺸴� ���� ���� 
        }
        while ((followTarget.position - transform.position).magnitude > 0.04) //�̵��̳��������� 
        {
            timeElaspad += cameraMoveSpeed * Time.deltaTime;// �ð������� 
            transform.position = Vector3.Lerp(transform.position, followTarget.position, timeElaspad); //���ݾ� �����δ�.
#if UNITY_EDITOR
            if (DEBUGCHECK) 
            {
                Debug.Log($"CameraBase.cs������ StandardMove �ڷ�ƾ�� �������Դϴ�. ��ġ :{transform.position}");
            }
#endif
            yield return null;
        }
    }

    /// <summary>
    /// �̵� ���߿� ��� �ٶ󺸴� ���� 
    /// ���� 1. �̵��Ҷ� �ٶ󺸴� Ÿ���� �ٲ�� �����̵� ��� �ٶ󺻴� ��ó���� ����.
    ///         �׷��ٰ� Slerp �ֱ⿡�� ȸ���ӵ��� �̵��ӵ��� �ȸ����� �̵��̳����� �ٶ󺸴� ������ ���������� ���������ִ�.
    /// <param name="timeElaspad"></param>
    /// </summary>
    IEnumerator MoveRotation(float timeElaspad = 0.0f)
    {
        while ((followTarget.position - transform.position).magnitude > 0.04) //�̵��� ���������� 
        {
            timeElaspad += cameraMoveSpeed + Time.deltaTime;  //�̵��ӵ��� ����ȭ  ��ŸŸ���� ���̸�ŭ ������ �߻��Ѵ�.
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookTarget.position - transform.position), timeElaspad); //��� �ٶ����
#if UNITY_EDITOR
            if (DEBUGCHECK)
            {
                Debug.Log($"CameraBase.cs������ MoveRotation �ڷ�ƾ�� �������Դϴ�. ȸ�� :{transform.rotation}");
            }
#endif
            yield return null;
        }
        transform.rotation = Quaternion.LookRotation(lookTarget.position - transform.position); //�ݺ��������� ��Ȯ�ϰ� ��ġ���������� �������� ������Ų��.
    }

    /// <summary>
    /// �̵� ���߿� ��� �ٶ󺸴� ���� 
    /// <param name="timeElaspad"></param>
    /// </summary>
    IEnumerator IdleRotation(float timeElaspad = 0.0f)
    {
        while (timeElaspad > 1.0f) //ȸ���� ���������� ������. 
        {
            timeElaspad += Time.deltaTime * cameraRotateSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookTarget.position - transform.position),timeElaspad); //��� �ٶ����
#if UNITY_EDITOR
            if (DEBUGCHECK)
            {
                Debug.Log($"CameraBase.cs������ MoveRotation �ڷ�ƾ�� �������Դϴ�. ȸ�� :{transform.rotation}");
            }
#endif
            yield return null;
        }
        transform.rotation = Quaternion.LookRotation(lookTarget.position - transform.position); //�ݺ��������� ��Ȯ��ó�����ȵǴ� �����Ϳ� �ѹ��� ����
    }

}
