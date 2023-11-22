using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��Ÿ ���̵��ÿ��� �������� ���Ƿο�����Ʈ
/// </summary>
public class EtcObjects : Singleton<EtcObjects>
{
    /// <summary>
    /// ī�޶� ������ ť
    /// </summary>
    Queue<UICamera> cameraQueue;

    /// <summary>
    /// ĳ���� ��û����ʿ� ����ī�޶� 3��
    /// </summary>
    UICamera[] teamCharcterView; 
    public UICamera TeamCharcterView => cameraQueue.Dequeue(); //ī�޶� �ٶ� ť���� �������ش�.

    
    protected override void Awake()
    {
        base.Awake();
        teamCharcterView = GetComponentsInChildren<UICamera>(true); //EtcObject �ؿ��� �׽� 3���� ���� ��ġ�ٲ���ã������ �� �̷��� ã�´�.
        cameraQueue = new Queue<UICamera>(teamCharcterView.Length); //ã�� ������ ť�����ΰ� 
    }
    private void Start()
    {
        TeamBorderManager teamBorderManager = WindowList.Instance.GetComponentInChildren<TeamBorderManager>(true);
        RawImage[] rawImages = teamBorderManager.GetComponentsInChildren<RawImage>(true);
        int i = 0;
        foreach (UICamera camera in teamCharcterView) //���鼭
        {
            rawImages[i].texture = camera.FollowCamera.activeTexture; //�ý��� ����ֱ�
            camera.resetData = () =>  //���µɶ� 
            {
                cameraQueue.Enqueue(camera);//�ٽ� �����ְ� �������ش�.
                //Debug.Log(camera.GetHashCode()); //camera ������ ����ִ°��� ����� ������� ���� Ȯ�οϷ� 
            };
            i++;
            camera.gameObject.SetActive(false);
        }
    }
}
