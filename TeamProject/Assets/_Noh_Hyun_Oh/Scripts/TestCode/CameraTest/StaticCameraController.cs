
using System.Collections;
using UnityEngine;

public static class StaticCameraController 
{
    /// <summary>
    /// 물리연산끝난뒤 실행될수있도록 작성
    /// </summary>
    static WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();


    /// <summary>
    /// 캐릭터 의 foward 기준 줌인 값 설정
    /// </summary>
    static public readonly Vector3 zoomInPos = new Vector3(0.4f, 0.25f, -1.0f);


    /// <summary>
    /// 줌인을 하거나 줌아웃을할때 타겟을 기준으로 얼마나 떨어진지 계산해서 결과값을 반환하는 함수
    /// </summary>
    /// <param name="originPos">줌인 줌아웃할때의 타겟과의 카메라의 거리값</param>
    private static Vector3 GetZoomPos(Transform target , Vector3 originPos)
    {
        return target.position + //추적 대상 위치에서 
            (
                Quaternion.Euler(0, target.eulerAngles.y, 0) // 추적대상의 y축방향의 회전값 을 구하고  
                *
                originPos // 회전한 위치 기준으로 내가설정한 거리값만큼 곱해서 타겟의 회전에 상관없이
                          // 타겟의 회전이 적용되더라도 상대적으로 같은 위치에 갈수있게 값을 구한다.
            );
    }

    /// <summary>
    /// 줌인을 하기위해 타겟의 회전방향을 기준으로 pos 값만큼위치로 부드럽게 움직이기위한 로직
    /// 제대로 적용하기위해선 유닛이 타겟을 바라봐야한다.
    /// </summary>
    /// <param name="unit">현재 포커스잡힌 유닛</param>
    /// <param name="actionCam">움직일 카메라</param>
    /// <param name="followSpeed">카메라의 속도 기본은 5.0f</param>
    public static IEnumerator ZoomIn(Transform unit, Camera actionCam, Transform attackTarget , float followSpeed = 5.0f)
    {

        if (unit != null)
        {
            float timeElapsed = 0.0f; //시간누적용 

            Vector3 tempPos = attackTarget.position;    // 카메라가 바라볼 타겟의 위치
            
            Vector3 endPos = GetZoomPos(unit, zoomInPos); // 카메라의 최종위치를 계산해서 받아온다. 
            

            while ((endPos - actionCam.transform.position).sqrMagnitude > 0.04f) //도착할때까지 체크하고 돌린다
            {
                timeElapsed += Time.deltaTime * followSpeed; // 카메라 위치 이동시간 조절용 
                //Debug.Log($"{(endPos - gameObject.transform.position).sqrMagnitude} _ {endPos} _ {gameObject.transform.position}");
                actionCam.transform.position = Vector3.Lerp(actionCam.transform.position, endPos, timeElapsed); //카메라 보간으로 부드럽게 적용

                //Debug.Log(target.position - actionCam.transform.position);
                //Debug.Log(target.position + zoomFocusPos - actionCam.transform.position);
                actionCam.transform.rotation = Quaternion.Slerp(actionCam.transform.rotation, Quaternion.LookRotation(tempPos - actionCam.transform.position), timeElapsed); //항상 타겟바라보기

                yield return fixedWait;
            }
            actionCam.transform.rotation = Quaternion.LookRotation(tempPos - actionCam.transform.position); //반복문에서 싱크가안맞기때문에 맞추기

            //Debug.Log("끝나긴하냐?");
        }
    }
    /// <summary>
    /// 줌아웃을 해서 쿼터뷰 로 돌아가기위한 로직
    /// 제대로 적용하기위해선 유닛이 타겟을 바라봐야한다.
    /// </summary>
    /// <param name="unit">현재 포커스잡힌 유닛</param>
    /// <param name="actionCam">움직일 카메라</param>
    /// <param name="followSpeed">카메라의 속도 기본은 5.0f</param>
    /// <param name="attackTarget">유닛이 바라볼 타겟</param>
    public static IEnumerator ZoomOut(Transform unit, Camera actionCam, Vector3 quarterViewPos,  float followSpeed = 5.0f)
    {

        if (unit != null)
        {
            float timeElapsed = 0.0f;
            Vector3 tempPos = unit.position;    // 카메라 가 바라볼 유닛의 위치

            //카메라 회전시 방향처리 하기위해 quarterViewPos 값의 변화를 추가해야한다.

            Vector3 endPos = GetZoomPos(unit, quarterViewPos); // 내가 target 과의 같은방향으로 유지하고 오리진 포스 위치로 이동시킨다.
                ;

            while ((endPos - actionCam.transform.position).sqrMagnitude > 0.04f) //도착할때까지 체크하고 돌린다
            {
                timeElapsed += Time.deltaTime * followSpeed; // 카메라 위치 이동시간 조절용 
                //Debug.Log($"{(endPos - gameObject.transform.position).sqrMagnitude} _ {endPos} _ {gameObject.transform.position}");
                actionCam.transform.position = Vector3.Lerp(actionCam.transform.position, endPos, timeElapsed); //카메라 보간으로 부드럽게 적용

                //Debug.Log(target.position - actionCam.transform.position);
                //Debug.Log(target.position + zoomFocusPos - actionCam.transform.position);
                actionCam.transform.rotation = Quaternion.Slerp(actionCam.transform.rotation, Quaternion.LookRotation(tempPos - actionCam.transform.position), timeElapsed); //항상 타겟바라보기

                yield return fixedWait;
            }
            actionCam.transform.rotation = Quaternion.LookRotation(tempPos - actionCam.transform.position); //반복문에서 싱크가안맞기때문에 맞추기

            //Debug.Log("끝나긴하냐?");
        }
    }

    public static void SetCameraRotateValue(Camera originCam) 
    {
        // 회전기준이되는 카메라의 회전값을 가져온다
        float angle = originCam.transform.eulerAngles.y;
        Debug.Log(angle);
        ////Debug.Log(tempMoveDir);
        //if (angle > -1.0f && angle < 1.0f) //0도 회전
        //{
        //    Debug.Log("0도");
        //    tempMoveDir.x = dir.x;
        //    tempMoveDir.z = dir.y;
        //}
        //else
        //if (angle > 89.0f && angle < 91.0f) //90도 회전
        //{

        //    tempMoveDir.x = dir.y;
        //    tempMoveDir.z = -dir.x;
        //}
        //else
        //if (angle > 179.0f && angle < 181.0f) //180도 회전
        //{

        //    tempMoveDir.x = -dir.x;
        //    tempMoveDir.z = -dir.y;
        //    Debug.Log("180도");
        //}
        //else
        //if (angle > 269.0f && angle < 271.0f) //270도 회전
        //{
        //    tempMoveDir.x = -dir.y;
        //    tempMoveDir.z = dir.x;
        //}
    }
}
