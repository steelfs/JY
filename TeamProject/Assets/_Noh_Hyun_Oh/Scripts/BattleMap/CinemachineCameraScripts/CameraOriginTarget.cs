using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///  �ó׸ӽ� ����� ī�޶� Ÿ������ ���� ������Ʈ 
///  Ű�Է½� ȭ�� ȸ���Ҷ� ����Ѵ�.
/// </summary>
public class CameraOriginTarget : MonoBehaviour
{

    /// <summary>
    /// ȸ���ӵ�
    /// </summary>
    [SerializeField]
    float rotateSpeed = 100.0f;

    /// <summary>
    /// Ű�Է� �����
    /// </summary>
    bool isRotate = false;

    /// <summary>
    ///  �׽�Ʈ��  �ش�������� ���� �ƴ� �ٸ��������� ���� �����ѵ� ���ٹ���� �ٲܿ���
    /// </summary>
    [SerializeField]
    Transform target;
    public Transform Target 
    {
        get => target;
        set => target = value;
    }

    /// <summary>
    /// ȭ�������(�ȼ�����)�� �߰� ��ġ�� �������� 
    /// </summary>
    Vector3 screenHalfPosition = Vector3.zero;

    float rotateValue = 0.0f;
    float RotateValue 
    {
        get => rotateValue;
        set 
        {
            if (rotateValue != value) 
            {
                rotateValue = value > 359.0f ? 0.0f : value < 1.0f ? 0.0f : value; //���� 0~ 360 ������ ���õȴ�.
                onCameraRotateValue?.Invoke(rotateValue);
            } 
        }
    }

    /// <summary>
    /// ȸ������ ������ ��������Ʈ
    /// </summary>
    public Action<float> onCameraRotateValue;

    /// <summary>
    /// ī�޶� �̵��ӵ�
    /// </summary>
    [SerializeField]
    float followSpeed = 3.0f;

    /// <summary>
    /// wasd �� �����϶� ȸ�������� �������� ��������Ʈ
    /// </summary>
    public Action<Quaternion> cameraRotation;

    private void Awake()
    {
        screenHalfPosition.x = Screen.width * 0.5f;
        screenHalfPosition.z = 0.0f;
        screenHalfPosition.y = Screen.height * 0.5f;

    }
    private void Start()
    {
        InputSystemController.Instance.OnCamera_LeftRotate = OnLeftRotate;
        InputSystemController.Instance.OnCamera_RightRotate = OnRightRotate;
    }
    private void LateUpdate()
    {
        ///������ �����Ű����� �ϴ� ������ ���ϳ�..?
        transform.position = Vector3.Lerp(transform.position,target.position, followSpeed * Time.deltaTime); // ������ġ�� �׻�ٲ����� �ð����� ����.
        //transform.Translate(target.transform.position ,Space.World);
    }

    private void OnLeftRotate()
    {
        if (!isRotate)
        {
            StartCoroutine(RotateCourutine(-90.0f));

        }
    }

    private void OnRightRotate()
    {
        if (!isRotate)
        {
            StartCoroutine(RotateCourutine(90.0f));

        }
    }
    /// <summary>
    /// ȸ�����⿡���� õõ�� ȸ����Ű��
    /// </summary>
    /// <param name="rotateValue">ȸ�� ����� ����(90,-90)</param>
    /// <returns></returns>
    IEnumerator RotateCourutine(float rotateValue) 
    {
        RotateValue += rotateValue;// ������ ó��ȸ������ 0�� �� �����ѻ��·� �󸶳����ư����� üũ�ϱ����� ���� 
        isRotate = true;//ȸ������������ �Էµ��͵� ���¿�
        //Debug.Log(transform.rotation.eulerAngles.y);
        float time = transform.rotation.eulerAngles.y; //���۰� ����
        rotateValue += time; //������ ����
        if (rotateValue > time)//-�� + ��  ���� ������ üũ
        {
            while (time < rotateValue)//üŷ
            {
                time += Time.deltaTime * rotateSpeed;
                transform.rotation = Quaternion.Euler(0, time, 0);
                cameraRotation?.Invoke(transform.rotation);
                yield return null;
            }
            transform.rotation = Quaternion.Euler(0, rotateValue, 0);
            cameraRotation?.Invoke(transform.rotation);
            isRotate = false;//ȸ���������� üũ
        }
        else 
        {
            while (time > rotateValue)//üŷ
            {
                time -= Time.deltaTime * rotateSpeed;
                transform.rotation = Quaternion.Euler(0, time, 0);
                cameraRotation?.Invoke(transform.rotation);
                yield return null;
            }
            transform.rotation = Quaternion.Euler(0, rotateValue, 0);
            cameraRotation?.Invoke(transform.rotation);
            isRotate = false;//ȸ���������� üũ

        }


    }

}

