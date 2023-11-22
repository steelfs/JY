using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// ��Ʋ�� ���� �ʱ�ȭ �ϴ� ���۳�Ʈ
/// </summary>

public class InitCharcterSetting : MonoBehaviour
{
    /// <summary>
    /// ���� ���� 
    /// </summary>
    [SerializeField]
    int teamLength = 2;


    /// <summary>
    /// ���� ������ �� �迭
    /// </summary>
    ITurnBaseData[] teamArray;


    /// <summary>
    /// �̴ϸ� ī�޶� �������� �̿ϼ� - ���߿� Ÿ�ٵ� �ŰܾߵǼ� ������ �ʿ��ϴ� 
    /// </summary>
    MiniMapCamera miniCam;

    /// <summary>
    /// �̴ϸʰ� ���������� ���߿� Ÿ���� �ٲ�� �̰͵� �ٲ��ߵ����� ������ �ʿ� 
    /// </summary>
    CameraOriginTarget cameraOriginTarget;


    CinemachineBrain brainCam;

    Camera_Move cameraMoveComp;

    /// <summary>
    /// �ʱ�ȭ�ϱ����� ����
    /// </summary>
    Transform battleActionButtons;
    private void Awake()
    {
        miniCam = FindObjectOfType<MiniMapCamera>(true);
        cameraOriginTarget = FindObjectOfType<CameraOriginTarget>(true);
        cameraMoveComp = FindObjectOfType<Camera_Move>(true);
        brainCam = Camera.main.GetComponent<CinemachineBrain>();//�극�� ī�޶� ã�� 
        cameraMoveComp.GetCineBrainCam = () => brainCam; //�극��ī�޶� ã�Ƽ� �������ֱ�
        cameraMoveComp.gameObject.SetActive(true); //ī�޶� �̵��� ��Ʋ�ʿ����� ����ϱ⶧���� ���ξȻ���.


    }
    private void Start()
    {
        TestInit();
        
    }
    /// <summary>
    /// �׽�Ʈ�� ������ ���� 
    /// </summary>
    public void TestInit()
    {
        SpaceSurvival_GameManager.Instance.GetBattleMapInit = () => this; //��Ʋ�� ������ ����
        if (teamArray == null) 
        {
            teamArray = new ITurnBaseData[teamLength]; //�迭 ũ�����
        }
        if (TurnManager.Instance.IsViewGauge) // ������ �������� üũ�ؼ� 
        {
            WindowList.Instance.TurnGaugeUI.gameObject.SetActive(true); //�����ָ� ǥ��
        }
        ITurnBaseData tbo; //�ӽ÷� ���� ���� �����ϰ� 
        for (int i = 0; i < teamLength; i++)
        {
            if (i == 0) // ù��°�� ������ �Ʊ� ���� ����ְ� 
            {
                tbo = (PlayerTurnObject)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_PLAYER_POOL); //
                tbo.UnitBattleIndex = i; 
                tbo.gameObject.name = $"Player_Team_{i}";
                tbo.InitData();

                miniCam.player = tbo.CharcterList[0].transform; //�̴ϸ� Ȱ��ȭ�� �����ʿ� 
                cameraOriginTarget.Target = tbo.CharcterList[0].transform;
               
            }
            else //���̿ܿ��� ���ͳ� �߸� �� ������ȴ� ���ǹ� �߰��ʿ� 
            {
                tbo = (EnemyTurnObject)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_ENEMY_POOL); //��������
                tbo.UnitBattleIndex = i;
                tbo.gameObject.name = $"ENEMY_Team_{i}";
                tbo.InitData();
                
            }
            //tbo.TurnActionValue = 10.0f; // -�׽�Ʈ�� ����
            teamArray[i] = tbo;
        }

        //������ �ʱ�ȭ������ �Ͻ��� 

        battleActionButtons = WindowList.Instance.BattleActionButtons;

        int childCount = battleActionButtons.childCount;

        for (int i = 0; i < childCount; i++)
        {
            battleActionButtons.GetChild(i).gameObject.SetActive(true);
        }
        miniCam.gameObject.SetActive(true); //�̴ϸ� Ȱ��ȭ �����ʿ� 
        cameraOriginTarget.gameObject.SetActive(true);


        TurnManager.Instance.InitTurnData(teamArray);
        
        GameManager.QuickSlot_Manager.gameObject.SetActive(true);

        WindowList.Instance.Gyu_QuestManager.InitDataSetting();
    }

    /// <summary>
    /// ��Ʋ�� �����͸� �ʱ�ȭ �ϴ� �Լ�
    /// </summary>
    /// <param name="isBattleLoaded">���̵��� ��Ʋ�ʿ��� ��Ʋ������ ������ �ʱ�ȭ�� üũ�ϴ� ����</param>
    public void TestReset(bool isBattleLoaded = false) 
    {
        miniCam.gameObject.SetActive(false); //�̴ϸ� Ȱ��ȭ �����ʿ� 
        cameraOriginTarget.gameObject.SetActive(false);
        WindowList.Instance.TeamBorderManager.UnView(); //�� ��� ������ ���� 

        int childCount = battleActionButtons.childCount;
        for (int i = 0; i < childCount; i++)
        {
            battleActionButtons.GetChild(i).gameObject.SetActive(false); //��Ʋ�� �׼ǹ�ư ���� 
        }
     

        TurnManager.Instance.ResetBattleData(); //�ϵ����� �ʱ�ȭ 
        if (TurnManager.Instance.IsViewGauge) // ������ �������� üũ�ؼ� 
        {
            WindowList.Instance.TurnGaugeUI.gameObject.SetActive(false); //��Ȱ��ȭ ó��
        }
        SpaceSurvival_GameManager.Instance.ResetData(isBattleLoaded);
    }

}
     //UI ����
        //for (int i = 1; i<uiParent.childCount - 1; i++)
        //{
        //    for (int j = 0; j<uiParent.GetChild(i).childCount; j++)
        //    {
        //        uiParent.GetChild(i).GetChild(j).gameObject.SetActive(false); //�׼ǹ�ư ���� 
        //    }
        //}
        //turnGaugeUI.gameObject.SetActive(false); //�ϰ����� ����

/*======================================== �׽�Ʈ�� ==================================================*/


/// <summary>
/// �ϸ���Ʈ�� ��Ȯ�ο�
/// </summary>
//public void ViewTurnList()
//{
//    foreach (ITurnBaseData j in turnObjectList)
//    {
//        Debug.Log($"{j} �� : {j.TurnActionValue}");
//    }
//    Debug.Log(turnObjectList.Count);
//}
///// <summary>
///// ���� �������� ������ ã�ƿ��� 
///// </summary>
///// <returns></returns>
//public ITurnBaseData GetNode()
//{
//    ITurnBaseData isTurnNode = null;
//    foreach (ITurnBaseData node in turnObjectList)
//    {
//        if (node.TurnEndAction != null) //���� ���������̸� endAction �� ��ϵ��ִ� 
//        {
//            Debug.Log(node.transform.name);
//            isTurnNode = node;//�������� ��� ��Ƽ� 
//        }
//    }
//    return isTurnNode;//��ȯ
//}
///// <summary>
///// �׽�Ʈ�� ������ ���� �������� 
///// </summary>
///// <returns></returns>
//public ITurnBaseData RandomGetNode()
//{
//    ITurnBaseData isRandNode = null;
//    int randomIndex = UnityEngine.Random.Range(0, turnObjectList.Count); //����Ʈ�� ������ �ε��� �� �������� 
//    LinkedListNode<ITurnBaseData> node = turnObjectList.First;
//    for (int i = 0; i < turnObjectList.Count; i++)
//    {
//        if (randomIndex == i)
//        {
//            isRandNode = node.Value;
//            break;
//        }
//        node = node.Next;
//    }
//    return isRandNode;
//}
//}
