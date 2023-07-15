using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCamera : TestBase
{
    //�÷��̾ õõ�� ���󰡴� ������ ī�޶�
    // Slerp�̿� �÷��̾ ���󰡱�
    //����ٴϴ� ��� �ʿ�
    //ī�޶��� ������ġ�� ó�������Ҷ� �÷��̾���� �Ÿ��� �����ؾ��Ѵ�.
    //�յ� ���� ���� ��  ȸ��
    float length;
    public Transform target;
    public float speed = 3.0f;
    Vector3 offset;
    private void Start()
    {
        if (target == null)
        {
            Player player = FindObjectOfType<Player>();     //ã�¹�� Find Tag, Type(�� �Ѱ��� �ִٴ� ���� �Ͽ� Type)
            target = player.transform;
        }
        offset = transform.position - target.position;// Ÿ�� ��ġ���� ī�޶� �� ������Ʈ�� ���� ���⺤��;
        length= offset.magnitude;// �Ÿ� ���ϱ�
    }
    private void FixedUpdate()
    {//offset��ŭ ������ ��ġ�� ����
        transform.position = Vector3.Slerp(transform.position, target.position + Quaternion.LookRotation(target.forward) * offset, Time.fixedDeltaTime * speed);
        transform.LookAt(target);// ȸ�� �������Ʈ ������ �ν����Ϳ��� �Ҵ� ����


        // Ÿ��(�÷��̾�)���� ī�޶�� ���� ����
        Ray ray = new Ray(target.position, transform.position - target.position);// ���̽� ��� ���� �÷��̾�� ī�޶� ���� ���� ������ ���̰� �浹�� �������� ī�޶� ��ġ�� �����ϱ�
        if (Physics.Raycast(ray, out RaycastHit hitInfo, length))
        {
            transform.position = hitInfo.point;
        }
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
       
    }
}
