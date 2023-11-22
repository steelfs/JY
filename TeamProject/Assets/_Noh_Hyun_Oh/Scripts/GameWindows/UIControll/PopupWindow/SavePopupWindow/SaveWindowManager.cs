
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ȭ�鿡 ���̴°͸� �Ű澲��
/// </summary>
public class SaveWindowManager : PopupWindowBase ,IPopupSortWindow ,IPointerDownHandler
{
    /// <summary>
    /// ȭ���� OnEnable �� �Ǿ� Start �� �����̵Ǽ� �ʱ�ȭ �Ǳ⶧���� 
    /// �ʱ�ȭ ���θ� üũ 
    /// </summary>
    bool isInit = false;

    /// <summary>
    /// ���� �������ѹ�
    /// �ش簪�� �⺻������ 1�� �������Է�ó���� ������ ���������� �����;��Ѵ�.
    /// �������ѹ��� 0���� �����ؾ��Ѵ�.
    /// </summary>
    [SerializeField]
    int pageIndex = 0;          // ���� �������ѹ� ù��° 0��
    public int PageIndex
    {
        get => pageIndex;
        set {
            pageIndex = value;
            if (pageIndex < 0) //���������� 0�� ù��°�̴� �׺�������������
            {
                pageIndex = 0;
            }
            else if (value > lastPageIndex) // �������� �ִ����������ٸ��Ե�����
            {
                pageIndex = lastPageIndex; //�������������� �����ش�.
            }

        }
    }
    [SerializeField]
    /// <summary>
    /// ���������Ʈ ����ũ��
    /// </summary>
    float saveObjectHeight = 150.0f;

    /// <summary>
    /// ���������� ���� �ִ� �������� ������Ʈ �ִ� ����
    /// </summary>
    [SerializeField]
    int pageMaxObject = 8;
    public int PageViewSaveObjectMaxSize => pageMaxObject;
    /// <summary>
    /// ���������� ���� �ִ� ����¡ ������Ʈ ���� 
    /// </summary>
    [SerializeField]
    int pagingMaxObject = 4;
    public int PagingMaxObject => pagingMaxObject;
    /// <summary>
    /// ������ ������ �� 
    /// </summary>
    int lastPageIndex = -1;
    public int LastPageIndex => lastPageIndex;

    /// <summary>
    /// �˾�â ���� �����ϱ������ Ŭ���̹�Ʈ �帮��
    /// </summary>
    public Action<IPopupSortWindow> PopupSorting { get; set; }

    /// <summary>
    /// �������������� ���� �������� ������Ʈ ����
    /// </summary>
    int lastPageObjectLength = -1;

    
    /// <summary>
    /// ���������� ������Ʈ ��ġ
    /// </summary>
    private GameObject saveWindowObject;

    /// <summary>
    /// ���������� ������Ʈ ��ġ
    /// ����¡ ó���Ұ��� ������Ʈ ��ġ
    /// </summary>
    private GameObject saveWindowPageObject;

    /// <summary>
    /// ������������ �˾������� ��ġ
    /// </summary>
    private ModalPopupWindow saveLoadPopupWindow;


    //int singletonCheck;

    //protected override void Awake()
    //{
    //    if (this.GetHashCode() == singletonCheck)
    //    {
    //        Debug.Log("�ؽ��ڵ尰�� ");
    //    }
    //    else 
    //    {
    //        singletonCheck = this.GetHashCode();
    //        Debug.Log($"�ΰ�����? {singletonCheck}");
    //    }
    //    base.Awake();
    //}

    /// <summary>
    /// ȭ����ȯ�� �ٽùߵ��ȵ� Ȯ�οϷ�.
    /// OnEnable ���� �ʰ� ����ȴ�.
    /// </summary>
    private void Start()
    {
        Oninitialize();
        InitLastPageIndex(); //����¡�� ���� �ʱⰪ����
        InitSaveObjects(); //����ȭ�� �ʱⰪ����
        isInit = true; //�����Ͱ� �ʱ�ȭ�ȵ� ȭ���� ��������������� �ʱ�ȭ����� üũ
        SetGameObjectList(SaveLoadManager.Instance.SaveDataList);//ó��ȭ������ﶧ ȭ����ÿ�
    }
    public void Oninitialize() 
    {
        //�̱��� �������� Awake ���� ��ã�´�. 
        saveWindowObject = SaveLoadManager.Instance.SaveLoadWindow;
        saveWindowPageObject = SaveLoadManager.Instance.SaveLoadPagingWindow;
        saveLoadPopupWindow = WindowList.Instance.IOPopupWindow;

        //��������Ʈ ���� 
        SaveLoadManager.Instance.saveObjectReflash = SetGameObject; //������� ��ɽ���� ������Ʈ�� �ٽñ׸���.
        SaveLoadManager.Instance.isDoneDataLoaing = SetGameObjectList; //������ �񵿱�� ó���� ������ ȭ�鸮�� �ڵ����� ���ֱ����� �߰�
        saveLoadPopupWindow.focusInChangeFunction = SetFocusView; //�����ưŬ���� ó���ϴ� �Լ�����
        
    }
    /// <summary>
    /// Ȱ��ȭ�� Ǯ���� ������ ������ ������Ʈ�� ����±���߰�
    /// Start�Լ����� ��������ȴ�.
    /// </summary>
    private void OnEnable()
    {

        if (isInit)//��ŸƮ�Լ����� ��������Ǽ� ó������ ������ �߻��Ѵ�.
        { 
            SetGameObjectList(SaveLoadManager.Instance.SaveDataList); //�ʱ�ȭ �۾��� �񵿱�� ���ϵ����͸� �о���⶧���� �����̾ȉ��������ִ� 
            SetPoolBug(saveWindowPageObject.transform, pagingMaxObject);
        }
    }

    
    /// <summary>
    /// ���� ���������� ȭ�鿡 ������ �������� ������Ʈ ������ ��ȯ�Ѵ�.
    /// </summary>
    /// <returns>������������ ���������Ʈ����</returns>
    private int GetGameObjectLength()
    {
        return pageIndex > lastPageIndex ? lastPageObjectLength : pageMaxObject;
    }

   
    /// <summary>
    /// ���� �ε����� ���� ȭ�鿡 ���������Ʈ�� ������ߵǴ��� ������Ʈ ��ġ�� ��ȯ�Ѵ�.
    /// ���������Ʈ �ѹ�(0 ~ pageMaxSize) =  �������Ϲ�ȣ(0 ~ �������ִ밪) - (�������ѹ�(������) 0�����ͽ��� * ���������� ���̴� ���� ) 
    /// </summary>
    /// <param name="fileIndex">�����ε���</param>
    /// <returns>���� �������� ������Ʈ �ε���</returns>
    private int GetGameObjectIndex(int fileIndex)
    {
        return fileIndex - (pageIndex * pageMaxObject);
    }


    /// <summary>
    /// �����ִ밹�� �� ���� �������� ���̴� ������Ʈ���� �� ������ �������������� �������������� ������ ������Ʈ������ �����´�.
    /// </summary>
    private void InitLastPageIndex()
    {
        lastPageIndex = (SaveLoadManager.Instance.MaxSaveDataLength / pageMaxObject);  //������ ���� ��������
        lastPageObjectLength = (SaveLoadManager.Instance.MaxSaveDataLength & pageMaxObject); //�������������� ������ ������Ʈ ����
    }

    /// <summary>
    /// 1. Ǯ���� ������ ������Ʈ�� ������ ���缭 ���������ʱ⶧��(Ǯ���� �ι�� �ø����۾�)�� �۾��� �߰� - ���߿� Ǯ�� �����ؾߵɵ��ϴ�.
    /// 2. Ǯ���� ������ ������Ʈ�߿� �Ⱦ��� �κ��� �����ִ� �۾�
    /// </summary>
    private void InitSaveObjects()
    {
        //����¡
        int childCount = saveWindowPageObject.transform.childCount; //���� Ǯ���� ������ ������Ʈ ���� �� �����´�. (����¡)
        if (childCount < pagingMaxObject)//������ ������Ʈ�� ȭ�鿡 ������ �������� ������� 
        {
            PoolBugFunction(saveWindowPageObject.transform,  pagingMaxObject, EnumList.MultipleFactoryObjectList.SAVE_PAGE_BUTTON_POOL);//�����Ѻκа����ͼ� �ʿ���ºκа��߱�
        }

        childCount = saveWindowObject.transform.childCount; //���� Ǯ���� ������ ������Ʈ ���� �� �����´�. (���������Ʈ)
        int proccessLength = GetGameObjectLength(); //������������ ���������Ʈ ������ �����´�.
        if (childCount < proccessLength)//������ ������Ʈ�� ȭ�鿡 ������ �������� ������� 
        {
            PoolBugFunction(saveWindowObject.transform,  proccessLength, EnumList.MultipleFactoryObjectList.SAVE_DATA_POOL);//�����Ѻκа����ͼ� �ʿ���ºκа��߱�
        }
     
        for (int i = 0; i < proccessLength; i++)//����������ŭ�� ������
        {
            SaveFileRectSetting(saveWindowObject.transform.GetChild(i).GetComponent<RectTransform>(),i); //���� ������������ ��ġ��� 
        }
        SetListWindowSize(saveWindowObject.transform, proccessLength);//����ȭ�� ũ������

        SetPageList();//����¡ ������ ȭ�鿡�Ѹ���
    }
    /// <summary>
    /// ���̺����� ��ġ�� ũ�⼳�� 
    /// </summary>
    /// <param name="rt">���̺������Ʈ�� ��ƮƮ������</param>
    /// <param name="index">ȭ�鿡 ���� �ε���</param>
    private void SaveFileRectSetting(RectTransform rt , int index) {
        Vector2 tempV = rt.anchorMax; //��Ŀ ���� ���� ����
        tempV.x = 1.0f;
        tempV.y = 1.0f;
        rt.anchorMax = tempV;

        tempV = rt.anchorMin;       //width �� �θ��� �ޱ����� �ִ�ġ ���� 
        tempV.x = 0.0f;
        tempV.y = 1.0f;
        rt.anchorMin = tempV;

        tempV = rt.pivot;           //����������Ʈ�� ������ ��ġ�� ���߱����� �������� ��Ѵ�.
        tempV.x = 0.0f;
        tempV.y = 1.0f;
        rt.pivot = tempV;

        tempV =  rt.sizeDelta;      
        tempV.x = 0.0f;             //��ü�� ũ�� width �� 0���༭ �ִ밪�� �ְ� ���� anchorX������ ������ŭ ���� 
        tempV.y = saveObjectHeight; //��ü�� ũ�� height �� �������� ���.
        rt.sizeDelta = tempV;

        tempV = rt.localPosition;
        tempV.x = 0.0f;                     //�������� ���ʳ�
        tempV.y = -saveObjectHeight * index ; //�������� ��ü �� ���������� ���ϼ��ֵ��� ���� 
        rt.localPosition = tempV;

    }
    /// <summary>
    /// Ǯ���� ������ ������Ʈ ���� �����Ѻκ� �߰��ϰ� �ʿ���� �͵� ��Ȱ��ȭ�ϴ� �Լ� ȣ��
    /// </summary>
    /// <param name="position">Ǯ�� ������ġ�� ���� ������Ʈ</param>
    /// <param name="childCount">Ǯ���� ������ ������Ʈ ����</param>
    /// <param name="proccessLength">�ʿ��� ������Ʈ ����</param>
    /// <param name="type">������ ������Ʈ Ÿ��</param>
    private void PoolBugFunction(Transform position, int proccessLength, EnumList.MultipleFactoryObjectList type) {

        for (int i = 0; i < proccessLength; i++) //�ʿ��Ѹ�ŭ �߰��� �����Ѵ� 
        {
            Multiple_Factory.Instance.GetObject(type);//������Ʈ �߰��ؼ� ������ Ǯ�ǻ�����ø���.
        }
        SetPoolBug(position, proccessLength);//�ʿ���� ������Ʈ�� ��Ȱ��ȭ �ϴ� �Լ�
    }

    /// <summary>
    /// Ǯ���� 2�辿�ø��⶧���� �Ⱦ��������� ����� �װ͵��� ��Ȱ��ȭ �ϴ� �۾�.
    /// </summary>
    /// <param name="position">�ʱ�ȭ�� ��ġ</param>
    /// <param name="startIndex">���� �ε���</param>
    private void SetPoolBug(Transform position, int startIndex)
    {
        int lastIndex = position.childCount;// �������Ŀ� �߰��� ������ �ٽóѱ��.
        for (int i = 0; i < lastIndex; i++) {
            position.GetChild(i).gameObject.SetActive(true); //������Ʈ Ȱ��ȭ�ؼ� �ϴ� ���κ����ص�
        }
        for (int i = startIndex; i < lastIndex; i++)//�Ⱦ��� ���ϸ�ŭ�� ������.
        {
            position.GetChild(i).gameObject.SetActive(false); //�Ⱦ������� ����� 
        }
        
    }


    /// <summary>
    /// ���嵥���� ����Ʈ�� ������ ũ�⸦ �����Ѵ�.
    /// </summary>
    private void SetListWindowSize(Transform position, int fileLength) {
        //Ʈ�������� �����غ����� �����Ÿ���� ���������ιٲ��� �����Ÿ�� �����Ͽ���.
        RectTransform rt = position.GetComponent<RectTransform>();
        Vector2 windowSize = rt.sizeDelta;
        windowSize.y = saveObjectHeight * fileLength;
        rt.sizeDelta = windowSize;
    }

    /// <summary>
    /// ���������� ���̴� ������Ʈ���� �����͸� �ٽü����Ѵ�.
    /// <param name="saveDataList">ȭ�鿡 �Ѹ� �����͸���Ʈ</param>
    /// </summary>
    private void SetGameObjectList(JsonGameData[] saveDataList)
    {
        if (saveDataList == null)
        { // �о�� �������������°�� ����
            Debug.LogWarning("�о�� ��������������?");
            return;
        }
        if (!isInit)//�񵿱�� ó���ϴ°Ͷ����� �߰� 
        {
            //Debug.LogWarning("���� �ʱ�ȭ �ȉ��.");
            return;
        }
        int startIndex = pageIndex * pageMaxObject; //���������ۿ�����Ʈ��ġ�� ��������

        int lastIndex = (pageIndex + 1) * pageMaxObject > SaveLoadManager.Instance.MaxSaveDataLength ? //���ϸ���Ʈ �ִ밪 < ��������¡�� * ȭ�鿡���������ִ������Ʈ��
                            SaveLoadManager.Instance.MaxSaveDataLength : //�������������� ���������� ����
                            (pageIndex + 1) * pageMaxObject; //�ƴϸ� �Ϲ����� ����¡ 

        for (int i = startIndex; i < lastIndex; i++)
        { //�����͸� ����������ŭ�� Ȯ���Ѵ�.
            SetGameObject(saveDataList[i], i); // �����͸� �������� 
            
        }
        int visibleEndIndex = lastIndex - startIndex; //�������� ������ �ε������� �ش�.
        SetPoolBug(saveWindowObject.transform, visibleEndIndex);//Ǯ�� ������Ʈ�� 2�辿�ø��µ� �����ϴ°͵��� ��Ȱ��ȭ�۾����ʿ��ؼ� �߰��ߴ�.
        SetListWindowSize(saveWindowObject.transform, visibleEndIndex);
    }

    /// <summary>
    /// ���� ���õǾ��Ѵ� 
    /// lastPageIndex = ������ �Ѱ���-1�� 
    /// pageMaxSize = ���������� ���̴°���
    /// �ΰ��� �����̵Ǹ� ���� ������ �������� ȭ�鿡 ��ó������ ��ȯ�Ѵ�.
    /// �ش��Լ��� pageMaxSize < lastPageIndex // �϶��� ���ȴ� �ϴ� �⺻�� 8�� �����ϸ� ��������.
    /// ��ɴ��߰��ҷ��� ��������ؾ���.
    /// </summary>
    /// <param name="viewLength">ȭ�鿡 ���� ����������</param>
    /// <returns>for���� 0��°�� ���õ� ��</returns>
    private int GetStartIndex(int viewLength)
    {
        int returnNum = 0; //��ȯ�� ���� �ʱⰪ �����̻��̾ȳѾ�� 0����ȯ�Ѵ�.
        int halfPageNum = viewLength / 2; //�������� ����
        int addPage = viewLength % 2; //�̰����� Ȧ����¦���� ��������
        if (pageIndex >= halfPageNum + addPage) // ������������ �������ǹ�����(�������������̴� ������ Ȧ���� +1) ���� ���ų� Ŭ�� 
        { // �������� �߰����� �Ѿ��                      
            if (pageIndex + halfPageNum >= lastPageIndex) //����������Ʈ(����������+ �������ǹ�����)���� ������ �ִ밪���� ���ų� Ŭ�� 
            {
                returnNum = lastPageIndex - viewLength + 1; //�������������� �����Ҷ� ó���ɰ�
            }
            else
            {
                returnNum = pageIndex - halfPageNum + (1 - addPage); // �߰������� �϶� ó���ɰ�
            }

        }
        return returnNum + 1; //�������� 0���� ���ƴ϶� 1���� �������ϱ⶧���� +1
    }

    /// <summary>
    /// �����ε����� ������ �ش������Ʈ�� �ٽñ׸���.
    /// ���� ������Ʈ�� �����Ѵٴ� �����Ͽ� ����ȴ�.
    /// </summary>
    /// <param name="saveData">������ ���嵥����</param>
    /// <param name="index">������ �ε���</param>
    public void SetGameObject(JsonGameData saveData, int fileIndex) {
        if (!isInit) {
            Debug.Log("���� �ʱ�ȭ �ȵ̴ٰ�");
            return;
        }
        int viewObjectNumber = GetGameObjectIndex(fileIndex); //�������� ������Ʈ ��ġã��

        SaveGameObject sd = saveWindowObject.transform.GetChild(viewObjectNumber).GetComponent<SaveGameObject>(); //������ ������Ʈ �����´�.
        sd.gameObject.SetActive(true);
        sd.ObjectIndex = viewObjectNumber; //������Ʈ �ѹ����� ���ش� 

        if (saveData != null) { //���嵥���Ͱ� �ִ��� üũ

            //�����δ� ������ ���� ������Ƽ Set �Լ��� ȭ�鿡 �����ִ� ��ɳ־����.
            //���̴±�ɼ����� SaveDataObject Ŭ��������.
            sd.FileIndex = saveData.DataIndex;
            sd.CreateTime = saveData.SaveTime;
            sd.SceanName = saveData.SceanName;
            sd.Money = saveData.PlayerData.Base_DarkForce;
            sd.CharcterLevel = (int)saveData.PlayerData.Level;
            string text = "";
            if (saveData.QuestList.Length > 0)
            {
                text = saveData.QuestList[0].QuestInfo;
            }
            sd.EtcText = text;
        }
        else
        {
            //�⺻�� ����
            sd.FileIndex = fileIndex; //�⺻���� ���� �ѹ���
            sd.name = "";
            sd.CreateTime = "";
            sd.SceanName = EnumList.SceneName.NONE;
            sd.Money = 0;
            sd.CharcterLevel = 0;
            sd.EtcText = "";
        }
    }

    /// <summary>
    /// ������ ���ڸ� �糪���Ѵ�.
    /// </summary>
    public void SetPageList(int index = -1) {
        if (index > -1) PageIndex = index;
        SetGameObjectList(SaveLoadManager.Instance.SaveDataList); // ���ϸ���Ʈ �����ֱ�

        int startIndex = GetStartIndex(pagingMaxObject); // ����¡ó���� ó�� ǥ���Ұ��� �����´�  //10���� 0.1 ��
        
        //�����⿬�� ���̱�
        float[] arithmeticValue = new float[2];
        arithmeticValue[0] = 1.0f / pagingMaxObject;// ����¡ ��ư ��ĭ�� �����ϴ� ũ�� 
        arithmeticValue[1] = pagingMaxObject / 1000.0f;  //����¡ ��ư ������ ���������ϱ����� ��
        SavePageButton_PoolObj savePageBtn;
        for (int i = 0; i < pagingMaxObject; i++) { //�������� �ٽõ��鼭 �����Ѵ�
            PageNumRectSetting(saveWindowPageObject.transform.GetChild(i).GetComponent<RectTransform>(),i, arithmeticValue, pagingMaxObject);
            savePageBtn = saveWindowPageObject.transform.GetChild(i).GetComponent<SavePageButton_PoolObj>();
            savePageBtn.gameObject.SetActive(true);
            savePageBtn.PageIndex = startIndex + i; //������ �ε����� ǥ��
        }
        ResetSaveFocusing();//�������̵��� �ʱ�ȭ
        SetPoolBug(saveWindowPageObject.transform, pagingMaxObject);
    }
 
    /// <summary>
    /// ��ư�� ������ �þ���� ��ư�� ������ �ڵ� �����ϱ����� ���� 
    /// </summary>
    /// <param name="rt">���� Ʈ������</param>
    /// <param name="index">����¡��ư ����</param>
    /// <param name="arithmeticValue">�����⿬�����ʿ��ؼ� �ݺ����ۿ��� ó���Ѱ��� �迭�ι޾Ƽ� �������� ���</param>
    /// <param name="length">ȭ�鿡���� ��ư�� ����</param>
    private void PageNumRectSetting(RectTransform rt , int index , float[] arithmeticValue, int length) 
    {
        int doubleCheck = index < 1 ? 0 : 1;
        Vector2 tempV = rt.anchorMin;
        tempV.x = index * arithmeticValue[0] + arithmeticValue[1] +  arithmeticValue[1] * doubleCheck; //����¡��ư  ���� �Ǿտ����� 0 �״����� 1ĭũ��+ �е���2�谪
        tempV.y = 0.25f;
        rt.anchorMin = tempV;

        tempV = rt.anchorMax;      
        
        tempV.x =   (index+1) * arithmeticValue[0]; //��ĭ�� ũ�⸦ ����ش�
        tempV.y = 0.75f;
        rt.anchorMax = tempV;

        tempV = rt.pivot;           //����������Ʈ�� ������ ��ġ�� ���߱����� �������� ��Ѵ�.
        tempV.x = 0.0f;
        tempV.y = 1.0f;
        rt.pivot = tempV;
        
       
        Rect rect = rt.rect;

        rect.xMin = 0.0f; //�������� Left   ���� �����Ѵ�.
        rect.yMax = 0.0f; //�������� Top    ���� �����Ѵ�.
        rect.xMax = 0.0f; //�������� Right  ���� �����Ѵ�.
        rect.yMin = 0.0f; //�������� Bottom ���� �����Ѵ�.

        // ������ ��ġ�� ũ�� ������ RectTransform�� �����մϴ�.
        rt.sizeDelta = new Vector2(rect.width, rect.height);
        rt.anchoredPosition = new Vector2(rect.x, rect.y);

    }



    /// <summary>
    /// ���̺����� ��Ŀ�� �����ϱ� 
    /// </summary>
    public void ResetSaveFocusing() {
        int initLength = GetGameObjectLength();
        for (int i = 0; i < initLength; i++)
        {
            saveWindowObject.transform.GetChild(i).GetComponent<Image>().color = Color.black;
        }
    }

    /// <summary>
    /// ���̺� ���� ��Ŀ�� �ϱ�
    /// </summary>
    /// <param name="fileIndex">������ ���� �ε���</param>
    /// <param name="isCopy">���� �����������Ȯ��</param>
    public void SetFocusView(int fileIndex,bool isCopy)
    {
        if (fileIndex > -1) //���������� ����ε� �����Ͱ��ִ°�� 
        {
            int pageIndexObejct = GetGameObjectIndex(fileIndex); //���ӿ�����Ʈ��ġ��������
            if (!isCopy) //���簡 �ƴѰ�� 
            {
                ResetSaveFocusing(); //���� ��Ŀ�� �����ϰ�
                saveWindowObject.transform.GetChild(pageIndexObejct).GetComponent<Image>().color = Color.white; //���� ��Ŀ��
            }
            else // ��������ΰ��
            {
                if (saveLoadPopupWindow.OldIndex == saveLoadPopupWindow.NewIndex)
                { //�����ư �������ÿ� �������� ������ �̸��´�.
                    saveWindowObject.transform.GetChild(GetGameObjectIndex(saveLoadPopupWindow.OldIndex)).GetComponent<Image>().color = Color.red;
                }
                else //�����ư������ ������ �����ҽ�  
                {
                    saveWindowObject.transform.GetChild(GetGameObjectIndex(saveLoadPopupWindow.NewIndex)).GetComponent<Image>().color = Color.blue;
                }
            }
        } 
    }


    /// <summary>
    /// ȭ�� ���Ŀ� �̺�Ʈ�Լ� �߰� 
    /// </summary>
    /// <param name="eventData">������</param>
    public void OnPointerDown(PointerEventData _)
    {
        PopupSorting(this);
    }

    public void OpenWindow()
    {
        this.gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }
}
