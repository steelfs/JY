using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR //���� �����Ϳ��� �������̶�� �̰� ���ڴ�. �� ����ÿ��� ���Խ�Ű�� �ʰڴ�.//#define
using UnityEditor;
#endif
public class TurretChase : TurretBase
{
    //�÷��̾��� ������� transform(����Ÿ��)? or vector3(��Ÿ�� �÷��̾��� ��ġ�� �ٲ���� �ٽ� ó���������)
    Transform target;//�÷��̾�
    SphereCollider sightTrigger;

    public float sightRange = 10.0f;
    public float fireAngle = 10.0f;
    public float turnSpeed = 2.0f;
    bool IsFiring = false;


    protected override void Awake()
    {
        base.Awake();
        sightTrigger = GetComponent<SphereCollider>(); //�þ� ������ Ʈ����
    }
    private void Start()
    {
        sightTrigger.radius = sightRange;
    }
    private void Update()
    {
        LookTargetAndAttack();//����� �ִ��� Ȯ���ؼ� ������ Ÿ������ ȸ���� ����
    }
    void LookTargetAndAttack() //����� �ٶ󺸰� �����ȿ� ������ �����ϴ� �Լ�
    {
        if (target != null)//Ÿ���� �������� 
        {
            Vector3 dir = target.position - barrelBodyTransform.position;
            dir.y = 0;
           
            if (IsVisibleTarget(dir)) //Ÿ���� ���϶���
            {
                barrelBodyTransform.rotation = Quaternion.Slerp(barrelBodyTransform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * turnSpeed);
                //Quaternion.LookRotation(dir);// Ư�� (����)�� �ٶ󺸰��ϴ� �Լ�

                //barrelBodyTransform.LookAt(target);//Ʈ�������� Ư��(����)�� �ٶ󺸰� ����� �Լ�

                //���� ���ϱ� Angle, �Ķ���� = Vector3 from, to  ���̰� �� ���� ���� �������ִ� �Լ�
                //SignedAngle// �� ������ ���̰� �� ���̵Ǵ� ���͸� �������� ���
                float angle = Vector3.Angle(barrelBodyTransform.forward, dir);
                if (angle < fireAngle && IsVisibleTarget(dir)) // Ÿ�� ���̿� ��ֹ��� ���� Ray�� ��ĵ�� �Ǹ�
                {
                    StartFire(); //�߻簢 ���̸鼭  ���̸�
                }
                else
                {
                    StopFire();
                }
            }
            else
            {
                StopFire();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform; //�÷��̾ ������ Ÿ������ ����
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;// �÷��̾ ������ null;
            StopFire();
        }
    }

    bool IsVisibleTarget(Vector3 lookDir)
    {//ray 
        bool result = false;
        if (target != null)
        {
            Ray ray = new(barrelBodyTransform.position, lookDir);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, sightRange))
            {
                if (hitInfo.transform == target)
                {
                    result = true;
                }
                // ����� �Ÿ� �Է� ���� sightRange����
                //sightRange�� ������� Ray�� ��ĵ�� ��ġ�� �����ؾ���
                sightRange = (barrelBodyTransform.position - hitInfo.transform.position).sqrMagnitude;
            } 
        }
        return result;
    }

    void StartFire()
    {
        if (IsFiring)
        {
            StartCoroutine(fireCoroutine);
            IsFiring = false;
        }
    }
    void StopFire()
    {
        if (!IsFiring)
        {
            StopCoroutine(fireCoroutine);        
            IsFiring = true;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.up, sightRange,2);//�����Ҷ� �������

        if (barrelBodyTransform == null)
        {
            barrelBodyTransform = transform.GetChild(4);
        }
        Gizmos.DrawLine(barrelBodyTransform.position, barrelBodyTransform.position + barrelBodyTransform.forward * sightRange);

        Vector3 from = barrelBodyTransform.position;
        Vector3 to = barrelBodyTransform.position + barrelBodyTransform.forward * sightRange;
        Gizmos.color = IsFiring? Color.red : Color.green; // true�� ���� 
        Gizmos.DrawLine(from, to);

        Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, barrelBodyTransform.up) * barrelBodyTransform.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fireAngle, barrelBodyTransform.up) * barrelBodyTransform.forward;
        //drawLine = 2D���� vector3�� �ϵ��ڵ����� �־��� �Ͱ� ���¸� �ٸ� ���̴�. ���� ��ư� ������ �ʿ�� ����.
        //�߻簢
        Gizmos.color = Color.white;
        to = barrelBodyTransform.position + dir1 * sightRange;
        Gizmos.DrawLine(from, to);

        to = barrelBodyTransform.position + dir2 * sightRange;
        Gizmos.DrawLine(from, to);
    }
#endif

    // Line ��ġ 2��
    // Ray  ��ġ, ����
}
