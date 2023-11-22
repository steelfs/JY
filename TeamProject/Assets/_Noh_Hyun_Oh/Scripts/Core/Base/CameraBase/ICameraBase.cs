using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraBase
{
    /// <summary>
    /// ������ ī�޶�
    /// </summary>
    Camera FollowCamera { get; }
    /// <summary>
    /// ī�޶� ����ٴ� ��ǥ
    /// </summary>
    Transform TargetObject { get; set; }

    /// <summary>
    /// ����ٴϴ� ��ǥ�� Ÿ��
    /// </summary>
    EnumList.CameraFollowType TargetType { get; set; }

    /// <summary>
    /// ����ٴ� �Ÿ�
    /// </summary>
    Vector3 Distance { get; set; }

    /// <summary>
    /// ī�޶� �̵��ӵ�
    /// </summary>
    float FollowSpeed { get; set; }
}
