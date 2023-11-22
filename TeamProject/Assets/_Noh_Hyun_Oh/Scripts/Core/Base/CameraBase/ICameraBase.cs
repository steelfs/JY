using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraBase
{
    /// <summary>
    /// 움직일 카메라
    /// </summary>
    Camera FollowCamera { get; }
    /// <summary>
    /// 카메라가 따라다닐 목표
    /// </summary>
    Transform TargetObject { get; set; }

    /// <summary>
    /// 따라다니는 목표의 타입
    /// </summary>
    EnumList.CameraFollowType TargetType { get; set; }

    /// <summary>
    /// 따라다닐 거리
    /// </summary>
    Vector3 Distance { get; set; }

    /// <summary>
    /// 카메라 이동속도
    /// </summary>
    float FollowSpeed { get; set; }
}
