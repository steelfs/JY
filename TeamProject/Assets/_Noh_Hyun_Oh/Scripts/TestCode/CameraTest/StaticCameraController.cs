
using System.Collections;
using UnityEngine;

public static class StaticCameraController 
{
    /// <summary>
    /// �������곡���� ����ɼ��ֵ��� �ۼ�
    /// </summary>
    static WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();


    /// <summary>
    /// ĳ���� �� foward ���� ���� �� ����
    /// </summary>
    static public readonly Vector3 zoomInPos = new Vector3(0.4f, 0.25f, -1.0f);


    /// <summary>
    /// ������ �ϰų� �ܾƿ����Ҷ� Ÿ���� �������� �󸶳� �������� ����ؼ� ������� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="originPos">���� �ܾƿ��Ҷ��� Ÿ�ٰ��� ī�޶��� �Ÿ���</param>
    private static Vector3 GetZoomPos(Transform target , Vector3 originPos)
    {
        return target.position + //���� ��� ��ġ���� 
            (
                Quaternion.Euler(0, target.eulerAngles.y, 0) // ��������� y������� ȸ���� �� ���ϰ�  
                *
                originPos // ȸ���� ��ġ �������� ���������� �Ÿ�����ŭ ���ؼ� Ÿ���� ȸ���� �������
                          // Ÿ���� ȸ���� ����Ǵ��� ��������� ���� ��ġ�� �����ְ� ���� ���Ѵ�.
            );
    }

    /// <summary>
    /// ������ �ϱ����� Ÿ���� ȸ�������� �������� pos ����ŭ��ġ�� �ε巴�� �����̱����� ����
    /// ����� �����ϱ����ؼ� ������ Ÿ���� �ٶ�����Ѵ�.
    /// </summary>
    /// <param name="unit">���� ��Ŀ������ ����</param>
    /// <param name="actionCam">������ ī�޶�</param>
    /// <param name="followSpeed">ī�޶��� �ӵ� �⺻�� 5.0f</param>
    public static IEnumerator ZoomIn(Transform unit, Camera actionCam, Transform attackTarget , float followSpeed = 5.0f)
    {

        if (unit != null)
        {
            float timeElapsed = 0.0f; //�ð������� 

            Vector3 tempPos = attackTarget.position;    // ī�޶� �ٶ� Ÿ���� ��ġ
            
            Vector3 endPos = GetZoomPos(unit, zoomInPos); // ī�޶��� ������ġ�� ����ؼ� �޾ƿ´�. 
            

            while ((endPos - actionCam.transform.position).sqrMagnitude > 0.04f) //�����Ҷ����� üũ�ϰ� ������
            {
                timeElapsed += Time.deltaTime * followSpeed; // ī�޶� ��ġ �̵��ð� ������ 
                //Debug.Log($"{(endPos - gameObject.transform.position).sqrMagnitude} _ {endPos} _ {gameObject.transform.position}");
                actionCam.transform.position = Vector3.Lerp(actionCam.transform.position, endPos, timeElapsed); //ī�޶� �������� �ε巴�� ����

                //Debug.Log(target.position - actionCam.transform.position);
                //Debug.Log(target.position + zoomFocusPos - actionCam.transform.position);
                actionCam.transform.rotation = Quaternion.Slerp(actionCam.transform.rotation, Quaternion.LookRotation(tempPos - actionCam.transform.position), timeElapsed); //�׻� Ÿ�ٹٶ󺸱�

                yield return fixedWait;
            }
            actionCam.transform.rotation = Quaternion.LookRotation(tempPos - actionCam.transform.position); //�ݺ������� ��ũ���ȸ±⶧���� ���߱�

            //Debug.Log("�������ϳ�?");
        }
    }
    /// <summary>
    /// �ܾƿ��� �ؼ� ���ͺ� �� ���ư������� ����
    /// ����� �����ϱ����ؼ� ������ Ÿ���� �ٶ�����Ѵ�.
    /// </summary>
    /// <param name="unit">���� ��Ŀ������ ����</param>
    /// <param name="actionCam">������ ī�޶�</param>
    /// <param name="followSpeed">ī�޶��� �ӵ� �⺻�� 5.0f</param>
    /// <param name="attackTarget">������ �ٶ� Ÿ��</param>
    public static IEnumerator ZoomOut(Transform unit, Camera actionCam, Vector3 quarterViewPos,  float followSpeed = 5.0f)
    {

        if (unit != null)
        {
            float timeElapsed = 0.0f;
            Vector3 tempPos = unit.position;    // ī�޶� �� �ٶ� ������ ��ġ

            //ī�޶� ȸ���� ����ó�� �ϱ����� quarterViewPos ���� ��ȭ�� �߰��ؾ��Ѵ�.

            Vector3 endPos = GetZoomPos(unit, quarterViewPos); // ���� target ���� ������������ �����ϰ� ������ ���� ��ġ�� �̵���Ų��.
                ;

            while ((endPos - actionCam.transform.position).sqrMagnitude > 0.04f) //�����Ҷ����� üũ�ϰ� ������
            {
                timeElapsed += Time.deltaTime * followSpeed; // ī�޶� ��ġ �̵��ð� ������ 
                //Debug.Log($"{(endPos - gameObject.transform.position).sqrMagnitude} _ {endPos} _ {gameObject.transform.position}");
                actionCam.transform.position = Vector3.Lerp(actionCam.transform.position, endPos, timeElapsed); //ī�޶� �������� �ε巴�� ����

                //Debug.Log(target.position - actionCam.transform.position);
                //Debug.Log(target.position + zoomFocusPos - actionCam.transform.position);
                actionCam.transform.rotation = Quaternion.Slerp(actionCam.transform.rotation, Quaternion.LookRotation(tempPos - actionCam.transform.position), timeElapsed); //�׻� Ÿ�ٹٶ󺸱�

                yield return fixedWait;
            }
            actionCam.transform.rotation = Quaternion.LookRotation(tempPos - actionCam.transform.position); //�ݺ������� ��ũ���ȸ±⶧���� ���߱�

            //Debug.Log("�������ϳ�?");
        }
    }

    public static void SetCameraRotateValue(Camera originCam) 
    {
        // ȸ�������̵Ǵ� ī�޶��� ȸ������ �����´�
        float angle = originCam.transform.eulerAngles.y;
        Debug.Log(angle);
        ////Debug.Log(tempMoveDir);
        //if (angle > -1.0f && angle < 1.0f) //0�� ȸ��
        //{
        //    Debug.Log("0��");
        //    tempMoveDir.x = dir.x;
        //    tempMoveDir.z = dir.y;
        //}
        //else
        //if (angle > 89.0f && angle < 91.0f) //90�� ȸ��
        //{

        //    tempMoveDir.x = dir.y;
        //    tempMoveDir.z = -dir.x;
        //}
        //else
        //if (angle > 179.0f && angle < 181.0f) //180�� ȸ��
        //{

        //    tempMoveDir.x = -dir.x;
        //    tempMoveDir.z = -dir.y;
        //    Debug.Log("180��");
        //}
        //else
        //if (angle > 269.0f && angle < 271.0f) //270�� ȸ��
        //{
        //    tempMoveDir.x = -dir.y;
        //    tempMoveDir.z = dir.x;
        //}
    }
}
