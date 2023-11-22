using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;
using EnumList;

/// <summary>
/// �ε��� ������ Ŭ����
/// ������� :
///     1. ���̵��� �ߺ��� ������Ʈ�� ������������� 
///        �̱��� ���� Awake �Լ����� �ɹ� ������ �����Ͽ� ��������� ����� �ȵɼ��ִ�.
///     2. ���丮 ���� 
/// </summary>
public class LoadingScene : MonoBehaviour
{
    /// <summary>
    /// �ε���Ȳ�� üũ�ϴº���
    /// </summary>
    static bool isLoading = false;
    public static bool IsLoading => isLoading;
    
    /// <summary>
    /// ��Ʋ ������ üũ�ϴ� ���� 
    /// </summary>
    static bool isBattleMap = false;
    public static bool IsBattleMap => isBattleMap;

    /// <summary>
    /// ���������� �Ѿ ���̸�
    /// �������� �Է¾ȵǸ� Ÿ��Ʋ�γѾ��.
    /// </summary>
    //static  EnumList.SceanName nextSceanName = EnumList.SceanName.TITLE;
    static int nextSceanindex = -1;

     /// <summary>
    /// �ε� ���൵ �̹��� ����
    /// EnumList �������̽��� �����س��� ���� �����Ѵ�.
    /// </summary>
    static EnumList.ProgressType progressType;

    /// <summary>
    /// ���൵�� ó���� �̹�������
    /// </summary>
    Image progressImg;

    /// <summary>
    /// ����ũ �ε� �ð����� 
    /// �ε��� �ּҽð��̶󺸸�ȴ�.
    /// </summary>
    [Range(1.0f, 5.0f)]
    public float fakeTimer = 3.0f;

    /// <summary>
    /// ����ī�޶� �پ��ִ� �ó׸ӽ� �� ���������� ã�Ƶα�
    /// </summary>
    static CinemachineBrain brainCamera;

    /// <summary>
    /// ���ε��� ���൵�� �����ִ¾����� �Ѿ�� �Լ� �񵿱������
    /// �ε������� ��� �Ѿ�ٰ� �̵��Ѵ�.
    /// </summary>
    /// <param name="sceneName">�̵��� �� �̸�</param>
    /// <param name="type">���� ��Ȳ ǥ���� progressType  EnumList�� ����Ȯ��</param>
    public static void SceneLoading(EnumList.SceneName sceneName = EnumList.SceneName.TITLE, 
                EnumList.ProgressType type = EnumList.ProgressType.BAR)
    {
        if (sceneName != EnumList.SceneName.NONE) { //�� ������ �Ǿ��ְ�
            if (!isLoading) { //�ε��� �ȉ������ 
                isLoading = true;//�ε� �����÷���
                nextSceanindex = (int)sceneName; //������ �ε��� �����ϰ� 
                InputSystemController.Instance.DisableHotKey(HotKey_Use.None); //�����ִ� �׼� ���� �ݰ� �⺻������ 
                WindowList.Instance.PopupSortManager.CloseAllWindow(); //ȭ�� ��ȯ�� �����ִ�â ���δ���.  
                if (SceneManager.GetActiveScene().buildIndex != nextSceanindex) //������̾ƴ� �ٸ��������� �ε�â�� ������ ���� 
                {
                   
                    progressType = type; //���α׷��� Ÿ�Լ���.
                    SceneManager.LoadSceneAsync((int)EnumList.SceneName.LOADING);

                }
                else 
                {
                    isLoading = false; //�ε� ȭ����ȯ�̾������� �ٷ� ����
                    SetInputSetting();

                }

            }
        }
        
    }



    /// <summary>
    /// �ε�ȭ�� �ε��� �ٷ� �ڷ�ƾ �����Ͽ� ������������ �񵿱�� �ε��� �ϰ�
    /// �׿����� ������ �޾ƿ´�.
    /// </summary>
    void Start()
    {
        isLoading = true; // �ε�ȭ�鿡������ ���ӽ����ϸ� �ʿ��� ����
        SetDisavleObjects(); //�����ִ�â �ݾƹ�����
        StopAllCoroutines();//�ε��� �������� �̷����°�쿡 �����ڷ�ƾ�� ���߰� ���ν����Ѵ�.
        StartCoroutine(LoadSceneProcess());
        brainCamera = Camera.main.GetComponent<CinemachineBrain>();
    }


    /// <summary>
    /// �ε�â������ �����ִ�â �ݾƹ��������� �Լ�
    /// �����߰� �ʿ�
    /// </summary>
    private void SetDisavleObjects() { 
        WindowList.Instance.MainWindow.gameObject.SetActive(false);
    }





    /// <summary>
    /// �ε�ȭ�鿡�� �������� �ε��� �Ϸ����� Ȯ���ϱ����� ó���ϴ��۾�
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadSceneProcess()
    {
        switch ((EnumList.SceneName)nextSceanindex)
        {
            case EnumList.SceneName.TITLE:
                Time.timeScale = 1.0f;
                TitleSceneMove();       //Ÿ��Ʋ�� ���� ������ �ʱ�ȭ�� �Լ�
                break;
            case EnumList.SceneName.ENDING:
                break;
            case EnumList.SceneName.TestBattleMap:
                break;
            case EnumList.SceneName.SpaceShip:
                GameManager.PlayerStatus.Base_Status.CurrentHP = GameManager.PlayerStatus.Base_Status.Base_MaxHP;
                break;
            default:
                break;
        }
        //�񵿱� ���ε������� �ޱ����� �������� ����
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceanindex, LoadSceneMode.Single); //�⺻ single => ������ �ε��Ϸ�Ȱ͸� ������
                                                                                                  // additive�� �������� ���̿�����..
        //AsyncOperation �� �Ѱ��������� �����������ص� �ϳ��� �����ؼ� 
        //allowSceneActivation ���� �����ȴ�.; 

        //op.allowSceneActivation ���� true �̸� �� �ε��� 90%(0.9f)�̻��� �Ǹ� �ڵ����� ���������� �Ѿ���� �߰�- ���ε尡�Ϸ�Ǹ� 0.9���� �����Ѵٰ��Ѵ�.
        op.allowSceneActivation = false;
        //���� ������� �ε��ð��� ª�Ƽ� �ε� ȭ���� ���İ��� ���������־ �ϴ� false �� 
        //�ε������� ���������� �̵��� ����ΰ� ����ũ�ε��� �ؿ� �����Ѵ�.

        float timer = 0.0f; //�ε� ���൵�� ���� ���� (�Լ��� ����߱⶧���� ���� ó���ϱ����� ����ߴ�.)
        float loadingTime = 0.0f; //���α׷����� ����ð�üũ
        switch (progressType)
        {
            case EnumList.ProgressType.BAR:
                //������̹��� ���������� ��������
                progressImg = ProgressList.Instance.GetProgress(EnumList.ProgressType.BAR, transform)
                                .transform.GetChild(0).GetComponent<Image>(); //Prefab �� ����1��°�� �����صξ���  

                while (!op.isDone)  //isDone ���� ������ �ε��� �������� üũ�Ҽ��ִ�.
                {
                    yield return null; //����ٰ� �ٲ���ְ� ������� �ѱ��.
                    loadingTime += Time.unscaledDeltaTime; //�ε��ð� üũ

                    // ���൵ ǥ�ø� �ٲٷ��� �ؿ� ������ �߰�
                    if (op.progress < 0.9f) //�ε� �Ϸ�Ǹ� 0.9 �������Ѵ� 1�� �������ȵȴ�.
                    {
                        //���� �ε� ���� 
                        progressImg.fillAmount = op.progress; //ȭ�� �̹����� �����Ȳ�� ����
                    }
                    else
                    {
                        //����ũ�ε� ����

                        timer += Time.unscaledDeltaTime; //Lerp �Լ��� ���� ��Ȳ����
                        progressImg.fillAmount = Mathf.Lerp(0.9f, 1f, timer); // 0.9 ~ 1.0 ������ ������ ǥ��

                        //�ε�â�� �ʹ������� �Ѿ�°��� �����ϱ����� ����ũ Ÿ�� üũ
                        if (fakeTimer < loadingTime) //�����Ϳ��� ����ũ�ε��ð��� �����Ѵ�.
                        {

                            isLoading = false;//�ε������ٰ� ����
                            op.allowSceneActivation = true; //�ش� ������ true�� progress ���� 0.9(90%)���� �Ѿ�¼��� �������� �ε��Ѵ�.
                            SetInputSetting();
                             yield break; //����ǳѱ��
                        }
                    }
                }
                break;
            //�ε��̹��� �߰��� ����ġ���� �ۼ�
            default: //Ÿ�԰��� �߸��Է�������� �̰����� �̵�
                Debug.LogWarning($"{this.name} �� ���α׷���(progress)�� Ÿ�Լ����� �߸��߽��ϴ�. ");
                yield break;
        }



    }

    /// <summary>
    /// ����ȯ�� �׼ǰ� ���ÿ� �Լ� 
    /// </summary>
    private static void SetInputSetting()
    {
        brainCamera.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;

        EnumList.SceneName nextSceanName = (EnumList.SceneName)nextSceanindex;
        switch (nextSceanName)
        {
            case EnumList.SceneName.TestBattleMap:
                Cursor.visible = true;
                InputSystemController.Instance.EnableHotKey(HotKey_Use.Use_BattleMap);
                InputSystemController.Instance.EnableHotKey(HotKey_Use.Use_InvenView);
                InputSystemController.Instance.EnableHotKey(HotKey_Use.QuickSlot);
                break;
            case EnumList.SceneName.SpaceShip:
                Cursor.visible = false;
                brainCamera.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
                InputSystemController.Instance.EnableHotKey(HotKey_Use.Use_TownMap);
                InputSystemController.Instance.EnableHotKey(HotKey_Use.Use_InvenView);
                break;
            case EnumList.SceneName.BattleShip:
                InputSystemController.Instance.EnableHotKey(HotKey_Use.Use_TownMap);
                InputSystemController.Instance.EnableHotKey(HotKey_Use.Use_InvenView);
                break;
            default:
                break;
        }

        Cursor.lockState = CursorLockMode.None;
    }
    /// <summary>
    /// Ÿ��Ʋ�� ���� �����͸� �ʱ�ȭ �ϱ����� �Լ� 
    /// </summary>
    private static void TitleSceneMove() 
    {
        SpaceSurvival_GameManager.Instance.ResetData();     //�������� ����ִ°� �ʱ�ȭ
        SpaceSurvival_GameManager.Instance.ShipStartPos = Vector3.zero; //����,�ε��ϰų� �Լ����� ��Ʋ�� �Ѿ�� ����Ȱ� �ʱ�ȭ

        SpaceSurvival_GameManager.Instance.PlayerQuest.ResetData(); //����Ʈ ������ �ʱ�ȭ 

        GameManager.EquipBox.ClearEquipBox();                       // ��� �ʱ�ȭ 

        SkillData[] skillDatas = GameManager.SkillBox.SkillDatas;
        foreach (var skillData in skillDatas)
        {
            skillData.InitSkillData(); //�⺻�� ������ 
        }

        GameManager.SlotManager.SaveFileLoadedResetSlots(); //�κ� ���� �ʱ�ȭ 


        GameManager.PlayerStatus.Base_Status.Init();                // ĳ���� �ɷ�ġ �ʱ�ȭ 
        GameManager.PlayerStatus.Reset_Status();                    // ĳ���� �ɷ�ġ �ʱ�ȭ 

        ///Ÿ��Ʋ�� ���ư��� ��Ʋ�� �������� �ʱ�ȭ 
        SpaceSurvival_GameManager.Instance.IsBattleMapClear = false;
        SpaceSurvival_GameManager.Instance.CurrentStage = StageList.None;
        SpaceSurvival_GameManager.Instance.StageClear = StageList.None; //�������� Ŭ�������� �ʱ�ȭ
        Cursor.visible = true;
    }
}
