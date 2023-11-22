using System;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections.Generic;
//����ȭ �ȵǴ�����
//1. MonoBehaviour �� ��ӹ����� ����ȭ �� �Ҽ����� 
//2. [Serializable] �Ӽ�(Attribute) �� Ŭ������ ��������� 
//
// -  Ŭ���� ������ private �� �����Ͽ����� [SerializeField] �� ������� JsonUtil���� ������ �����ϴ�.
//    �̱��濡 ���� Ŭ����Ÿ���� ���׸����� �����Ҽ�����.  gameObject.AddComponent<T>(); addComponent ���� ���׸�Ŭ������ ó���Ҽ�������
/// <summary>
/// 
/// ���� �ε� ���� ���̽� ���� �ۼ��� Ŭ���� 
/// try{}cahch(Exeption e){} �� ����������� ���������� ���ӻ󿡼� �ܵ����� �̷������ ����̰� 
/// ���⼭ ���������ٰ� ������ ���߸� �ȵǱ⶧���� �����߻��ϴ��� �������ʰ� �÷��� �ǵ��� �߰��Ͽ���.
/// </summary>
public class SaveLoadManager : ChildComponentSingeton<SaveLoadManager> {



    /// <summary>
    /// �������� ��ġ 
    /// project ���� �ٷιؿ����� Assets������ ������ġ
    /// </summary>
    string saveFileDirectoryPath;
    public String SaveFileDirectoryPath => saveFileDirectoryPath;


    /// <summary>
    /// �⺻ ������ �����̸�
    /// </summary>
    const string saveFileDefaultName = "SpacePirateSave";


    /// <summary>
    /// ������ ���� Ȯ���
    /// </summary>
    const string fileExpansionName = ".json ";


    /// <summary>����
    /// ���̺� ���ϸ� �о�������� ���� ���� 
    /// ? �� ���ڿ� �ϳ��� ���ϴ°��ε� �̰����ȴ�
    /// </summary>
    //string searchPattern = "SpacePirateSave[0-9][0-9][0-9].json"; //���ȵ�     
    //readonly string searchPattern = $"{saveFileDefaultName}???{fileExpansionName}"; // ��Ʈ������ �ּ�ġ�������� ����ȵǳ�...
    const string searchPattern = "SpacePirateSave???.json";



    /// <summary>
    /// Ǯ���� ����������� SaveData������Ʈ�� �ѱ������ ����ȭ�������� ���ӿ�����Ʈ
    /// </summary>
    GameObject saveLoadWindow;
    public GameObject SaveLoadWindow => saveLoadWindow;

    /// <summary>
    /// Ǯ���� ������������� PageObjtct �� �ѱ������ ������Ʈ��ġ
    /// </summary>
    GameObject saveLoadPagingWindow;
    public GameObject SaveLoadPagingWindow  => saveLoadPagingWindow;


    /// <summary>
    /// ����ȭ�� ���ε�
    /// </summary>
    public Action<JsonGameData,int> saveObjectReflash;
    
    

    /// <summary>
    /// ���ӵ����� �ε带 �ҽÿ� ���ӵ����͸� ������ �ε������ ������������ �Ѿ��. 
    /// </summary>
    public Action<JsonGameData> loadedSceanMove;


    /// <summary>
    /// �񵿱� ������ �����̳������� ȭ�鿡 �������Լ��Ϳ������� ��������Ʈ
    /// </summary>
    public Action<JsonGameData[]> isDoneDataLoaing;


    /// <summary>
    /// ���ӿ����� �����Ͱ� �Ľ̵Ǿ� ���⿡ �����Ѵ�.
    /// �ܺ�Ŭ�������� ������ �ؾ��ϳ�.?
    /// �����Ͱ� Ŀ���� �̸��̸� �����ϴ°�찡�ִ�.
    /// �ܺ�
    /// </summary>
    JsonGameData gameSaveData;
    public JsonGameData GameSaveData { 
        get => gameSaveData;
        set => gameSaveData = value;
    }
    /// <summary>
    /// ���� ������ �Ľ̰��� Ŭ���� 
    /// </summary>
    SaveDataParsing parsingProcess;
    public SaveDataParsing ParsingProcess => parsingProcess;

    /// <summary>
    /// ����ȭ�鿡 ���� �����͸���Ʈ
    /// ������ƮǮ �Ⱦ��� �׳� �����ͷθ� ������ ����.
    /// </summary>
    JsonGameData[] saveDataList;
    public JsonGameData[] SaveDataList => saveDataList;


    /// <summary>
    /// �������� �ִ밹�� 
    /// </summary>
    int maxSaveDataLength = 100;
    public int MaxSaveDataLength => maxSaveDataLength;



    /// <summary>
    /// ���� ���� ó���� ���������� üũ�ϴ� ���� 
    /// ����۾��� �����۾������ϸ�ȵȴ�.
    /// �񵿱�� ����� �渱 ������ �ֱ���ϴ�.
    /// </summary>
    bool isProcessing = false;

    
    protected override void Awake()
    {
        base.Awake();
        SaveWindowManager saveManager = FindObjectOfType<WindowList>().transform.GetComponentInChildren<SaveWindowManager>(true); // ã�� ������Ʈ���� ��Ȱ��ȸ�� �ڽĵ� ã�´�.

        saveLoadWindow = saveManager.transform.
                                    GetChild(0). //ContentParent
                                    GetChild(0). //Contents
                                    GetChild(0). //SaveLoadWindow
                                    GetChild(0). //SaveFileList
                                    GetChild(0). //Scroll View
                                    GetChild(0). //Viewport
                                    GetChild(0).gameObject;//Content 


        saveLoadPagingWindow = saveManager.transform.
                                        GetChild(0). //ContentParent
                                        GetChild(0). //Contents
                                        GetChild(0). //SaveLoadWindow
                                        GetChild(1). //PageListAndButton
                                        GetChild(1).gameObject; //PageNumber
        parsingProcess = GetComponent<SaveDataParsing>();
    }
   
    void Start (){
        SetDirectoryPath(); // ���� �����������ּҰ�����
        if (!isProcessing)
        {
            isProcessing = true;
            //Debug.LogWarning($"�񵿱� �ε��׽�Ʈ ���� {saveDataList}");
            FileListLoagind();//�񵿱�� ���Ϸε�
        }
    
    }

    /// <summary>
    /// ����� ������ġ�� ����
    /// </summary>
    private void SetDirectoryPath()
    {
        //����Ƽ �����Ͱ��ƴ� ����ȯ���� Applicaion���� �ڵ����� ������������ش�. 
#if UNITY_EDITOR //����Ƽ �����Ϳ������� ����
        saveFileDirectoryPath = $"{Application.dataPath}/SaveFile/";
        //Application.dataPath ��Ÿ�Ӷ� �����ȴ�.
#else //����Ƽ�����Ͱ� �ƴҶ��� ���� 
        saveFileDirectoryPath = Application.persistentDataPath + "/SaveFile/"; //����Ƽ �����Ͱ��ƴ� ����ȯ���� Applicaion���� �ڵ����� ������������ش�. 
#endif

    }
    /// <summary>
    /// �������� �������� Ȯ���ϰ� ������ �����ϴ·���
    /// </summary>
    /// <returns>���� ���� ���� ��ȯ</returns>
    private void ExistsSaveDirectory()
    {
        try
        {
            if (!Directory.Exists(saveFileDirectoryPath))//���� ��ġ�� ������ ������ 
            {
                Directory.CreateDirectory(saveFileDirectoryPath);//������ �����.
            }

        }
        catch (Exception e)
        {
            //���������� ������� �ü����ְڴ�.
            //��������� �������� �����𸣴� ��찡 ������������ �ϴ� �ɾ�д�.
            Debug.LogWarning(e);
        }
    }
    /// <summary>
    /// ���ӽ��۽� ����ȭ�� �����ٶ� ����� �����͵� ã�ƿ��� 
    /// List �����̾ƴ� �迭�������� �ۼ��Ͽ��� 
    /// ������ �迭�������� ������ ���缭 �˻��� ������ �̷������� �ϱ����ؼ���.
    /// </summary>
    private bool SetSaveFileList()
    {
        try
        {
            ExistsSaveDirectory();//����üũ�� ������ ���� 
            string[] jsonDataList = Directory.GetFiles(saveFileDirectoryPath, searchPattern); // �����ȿ� ���ϵ� ������ �о 

            // ����Ʈ ���� ����ȭ�鿡�� ���̺�â �ٲ𶧸��� �ð��� �ɸ����ɼ����ִ� ���������� �����ǵ��Եȴ�.  
            JsonGameData[] temp = new JsonGameData[maxSaveDataLength]; //�迭�� ó���� �����ѹ������ذ�ȴ�. 
            //temp������������ saveDataList �� null ���̸� ���⼭ó���ϸ�Ǳ⶧���� ����������� �־�J��.

            if (jsonDataList.Length == 0) //�о�������̾����� ����~
            {
                Debug.LogWarning($"{saveFileDirectoryPath}  ========    {searchPattern}     ������ ���� ������ �����ϴ� ");
                saveDataList = temp;//ȭ�鿡�� ��ü���ѷ��ش�.
                return false;
            }
           
            int checkDumyCount = 0;

            for (int i = 0; i < temp.Length; i++)
            { //�������� ã�ƿ� ���ϰ�����ŭ �ݺ���������

                if ((i - checkDumyCount) == jsonDataList.Length)    // ������Ƚ������ ���̰��������� �����о��ũ��Ͱ�����.
                {                                                   // ������ ���̻� ���������ʴ°�� ����������.
                    //Debug.Log($"���׽ȴ� . (i - checkDumyCount) = {(i - checkDumyCount)}  ,i = {i}  ,jsonDataList.Length = {jsonDataList.Length}");
                    break;
                }
                int tempIndex = GetIndexString(jsonDataList[i - checkDumyCount]);//�߰��� ������� üũ�ϱ����� checkDumyCount�� �����Ѵ�.
                if (tempIndex == i) // ���� �ε����� ���������� ������ �����Ѵ� 
                {
                    string jsonData = File.ReadAllText(jsonDataList[i - checkDumyCount]); // ���Ͽ� �����ؼ� �����ͻ̾ƺ��� 
                    if (jsonData.Length == 0 || jsonData == null)
                    {
                        //�⺻������ ����Ǹ�ȵȴ�. �̰��� ����Ǹ� ������������ ���ϳ����̾��ٴ°��̴�   
                        Debug.LogWarning($"{i - checkDumyCount} ��° ���� ������ �����ϴ� ���ϳ��� : {jsonDataList[i - checkDumyCount]} ");
                    }
                    else
                    {
                        //���������� �ƴѰ�� �۾�����
                        JsonGameData tempSaveData = JsonUtility.FromJson<JsonGameData>(jsonData); // ��ƿ����ؼ� �Ľ��۾��� ���� 

                        if (tempSaveData == null) //�Ľ̾ȉ�����  
                        {
                            //�⺻������ ����Ǹ�ȵȴ�.  ��������� Ȯ���غ��ų� ���嵥���Ͱ� ���������� ����������ʾƼ��߻��Ѵ�.
                            Debug.LogWarning($"{i - checkDumyCount} ��° ���� ������ Json���� ��ȯ�Ҽ� �����ϴ�  ���ϳ��� : {jsonDataList[i - checkDumyCount]} ");
                        }
                        else
                        {
                            temp[i] = tempSaveData;//�����ó���Ǹ� �����Ѵ�.

                        }
                    }
                }
                else
                {
                    checkDumyCount++;// �����ε�����ȣ���߱����� ��ã���� ���̰��߰�
                }
            }

            //�ʱ�ε�(���ӽ���)�ÿ� ó���ǰ��س��� ���ݽð��̰ɸ����۾��̶� �ش������� �����Ͽ���.
            saveDataList = temp;

            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            return false;
        }
    }
   
    /// <summary>
    /// �������� ��ġ�� �����̸��� Ȯ���� ������ �����´�.
    /// </summary>
    /// <param name="index">�������� ��ȣ</param>
    /// <returns>������ ���� ��ġ�� ���ϸ�,Ȯ���ڸ���ȯ�Ѵ�</returns>
    private string GetFileInfo(int index)
    {
        //�����̸� �ڿ� ������ ����("D3")�� 001 �̷��������� �ٲ㼭 �����Ѵ�. 3�� ����ǥ���ڸ����̴�
        return $"{saveFileDirectoryPath}{saveFileDefaultName}{index.ToString("D3")}{fileExpansionName}";
    }

    /// <summary>
    /// ���ϸ������� ī������ ���� 3�ڸ� ��������
    /// </summary>
    /// <param name="findText">�������Ͻ����ּҰ��� �̸� Ȯ���ڸ����</param>
    /// <returns>���ϸ��� �ε�������� ���ڸ� �̾Ƽ���ȯ -1�̸� ���Ͼ���</returns>
    private int GetIndexString(string findText)
    {
        if (findText != null)
        {
            int findDotPoint = findText.IndexOf('.'); //������ ***.json �̰� .�� �ϳ��ۿ������������� �ϴ� .��ġã�´�
            int temp = int.Parse(findText.Substring(findDotPoint - 3, 3));//����ġ���� 3ĭ������ �̵��� 3���� ���� �������� ���� 3�ڸ������ü��ִ�.
            return temp;
        }
        else
        {
            return -1;
        }
    }


    /// <summary>
    /// �񵿱� �Լ� �׽�Ʈ�� �����غ��Ѵ�
    /// �񵿱�μ����� async �Լ��� ��밡���� Thread �ϳ��� �� �Լ��������Ѵ�.
    /// ���α׷� �ȿ��� ������⶧���� ���α׷��� �����ֱ⸦ �����Ѵ� �Լ�����ó�����ȉ����� ���α׷�������Ǹ� ���� �������.
    /// </summary>
    private async void FileListLoagind() //�񵿱� �׽�Ʈ
    {

        //Debug.Log($"�񵿱� �ε��׽�Ʈ Ȯ��1�� {saveDataList}");

        await TestAsyncFunction(); //���Լ��� ���������� ��ٸ���.
        //await Task.Run(() => { isFilesLoading =  SetSaveFileList(); }); //���Լ��� ���������� ��� 
        if (saveDataList != null) { //�����ͷε��� ����Ή����� 
            isDoneDataLoaing?.Invoke(saveDataList);// �����ͷε��� �񵿱������� ó�������� ó���ؾߵ� �Լ�����  �������̸� �ʿ����.
            //Debug.LogWarning($"�񵿱� �ε��׽�Ʈ ��~ {saveDataList.Length}��  ���� �ε��Ϸ�"); // �Լ����������� ���Ÿ���� Ȯ���ϱ����� �ۼ�
        }
    }

    /// <summary>
    /// �񵿱� �Լ� �׽�Ʈ 
    /// ���������� �񵿱�� �ϱ����� �׽�Ʈ �ڵ��ۼ�
    /// �ڷ�ƾ�� ��������� �⺻������ ���� ������ �ٸ���.
    /// </summary>
    private async Task TestAsyncFunction() {
        //Debug.Log("�񵿱� �׽�Ʈ�Լ� ����");
        await Task.Run(() =>
        {
            SetSaveFileList();
            isProcessing = false; 

        }); //���Լ��� ���������� ��� 
        //Debug.Log("�񵿱� �׽�Ʈ�Լ� ����");
        await Task.Delay( 3000 ); //3�� ��ٸ���

    }


    /// <summary>
    /// �����̳� ���� ���� ���� �����̼����ɽ�
    /// ���̺굥���͸���Ʈ�� �����Ѵ�.
    /// </summary>
    /// <param name="gameData">�����Ǵ� ����</param>
    /// <param name="index">�����Ǵ� �ε���</param>
    private void SettingSaveDatas(JsonGameData gameData, int index)
    {
        if (saveDataList != null && index > -1)
        {
            saveDataList[index] = gameData;
        }
    }


    /// <summary>
    /// ��������� �⺻������ �Է��Ѵ�.
    /// </summary>
    /// <param name="saveData">���� ����</param>
    /// <param name="index">���� ���Ϲ�ȣ</param>
    private void SetDefaultInfo(JsonGameData saveData , int index) {
        saveData.DataIndex = index;  ///�����ε��� ����
        saveData.SaveTime = DateTime.Now.ToString();// ����Ǵ� �ð��� �����Ѵ�
        saveData.SceanName = (EnumList.SceneName)SceneManager.GetActiveScene().buildIndex; //���̸��� �����Ѵ�.
    }

    /// <summary>
    /// ���� ����½� ������ �������ִµ� ����� ������ �߻��ϱ⶧���� 
    /// ���� �ִ��� üũ�ϰ� ������ ������ �������� ������ش�.
    /// <param name="filePath">����� �����ּ�</param>
    /// </summary>
    private void FileCreate(string filePath) {
        if (File.Exists(filePath)) //���� üũ
        {  
            File.Delete(filePath); // �������������� �����ϰ�                 
        }
        FileStream fs = File.Create(filePath); //���Ӱ� ���� ����
        fs.Close(); // ���������� �����Ѱ��� �ݾƼ� Ǯ���ش�.
    }

    /// <summary>
    /// ���ӿ��� ���̺�� �����͸� ���Ϸ� ���� �۾�
    /// </summary>
    /// <param name="index">�������� ��ȣ</param>
    /// <returns>�������� ��������</returns>
    public bool Json_Save(int selectFileIndex)
    {
        if (!isProcessing)
        {
            isProcessing = true;
            try
            {

                if (gameSaveData != null && selectFileIndex > -1) //���ӵ����Ͱ� ������ 
                {
                    SetDefaultInfo(gameSaveData, selectFileIndex);// ��������� �⺻������ �����Ѵ�.
                    string toJsonData = JsonUtility.ToJson(gameSaveData, true); //���嵥���͸� Json�������� ����ȭ �ϴ� �۾� ����Ƽ �⺻����
                                                                                //true�Է½� ���Ͽ뷮��Ŀ����. ��ź�����������.
                    string filePath = GetFileInfo(selectFileIndex);
                    FileCreate(filePath); //������ ���� ����
                    File.WriteAllText(filePath, toJsonData); //���Ͽ� �����ϱ� 
                    SettingSaveDatas(gameSaveData, selectFileIndex); //�����͸���Ʈ���� �����۾�
                    saveObjectReflash?.Invoke(gameSaveData, selectFileIndex); //������ ȭ�鿡 ����� �����ͷ� ǥ���ϱ����� ����
                    isProcessing = false;
                    return true;
                }
                Debug.Log($"���� �����ε��� :{selectFileIndex} , ���ӵ����͸���Ʈ : {gameSaveData} ������ Ȯ�����ʿ��մϴ�.");
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                isProcessing = false;
            }
        }
        return false;
    }
   /// <summary>
   /// �������� �о���� �Լ� 
   /// </summary>
   /// <returns>�о���� ��������</returns>
    public bool Json_Load(int selectFileIndex) 
    {
        if (!isProcessing)
        {
            isProcessing = true;
            try
            {

                string filePath = GetFileInfo(selectFileIndex);
                if (selectFileIndex > -1 &&  File.Exists(filePath))//������������ ������ �ִ��� üũ 
                {
                    string saveJsonData = File.ReadAllText(filePath); //���������� ��Ʈ������ �о���� 
                    //������ ����. ���߹迭 �����ȵǰ�, private �ɹ������� �����Ϸ���  [SerializeField] �����ؾ���. Ŭ������ [Serializable] ����Ǿ���.
                    gameSaveData = JsonUtility.FromJson<JsonGameData>(saveJsonData); //��ȯ�� ��Ʈ�� �����͸� JsonUtility ���������� ����ȭó���� Ŭ������ �ڵ��Ľ� 
                    loadedSceanMove?.Invoke(gameSaveData); //�ε�� �ش������ �̵��Ѵ�.
                    isProcessing = false;
                    return true;
                }
                else
                {
                    Debug.LogWarning($"{filePath} �ش���ġ�� ������ �������� �ʽ��ϴ�.");
                    isProcessing = false;
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e); // ����ġ ���� ���� �߻��� �����߰߸���. Ŭ���� ������ �߸��߸� ��ü�� �ߵȴ�.
                isProcessing = false;
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    
    /// <summary> �׽�Ʈ�����ȵ�
    /// ù��° �ε������� �ش��ϴ� ������ �ι�° �ε������� �ش��ϴ� �������������� �����ִ±��
    /// ���� ���� ���� 
    /// ����� ����������� �ڽ��� �ε����� �����ؾ��Ѵ�.
    /// </summary>
    /// <param name="oldIndex">������ ���Ϲ�ȣ</param>
    /// <param name="newIndex">����� ���Ϲ�ȣ</param>
    /// <returns>�����ϱ� ��������</returns>
    public bool Json_FileCopy(int oldIndex , int newIndex) 
    {
        if (!isProcessing)
        {
            isProcessing = true; //���� ���࿩��
            try
            {

                string oldFullFilePathAndName = GetFileInfo(oldIndex);// ������ ������ġ
                if (oldIndex > -1 && newIndex > -1 &&  File.Exists(oldFullFilePathAndName))//������ ������ġ�� �������ִ��� Ȯ��
                {
                    string newFullFilePathAndName = GetFileInfo(newIndex);// ����� ������ġ

                    FileCreate(newFullFilePathAndName);//���� ����

                    string tempa = File.ReadAllText(oldFullFilePathAndName);//������ ������ �о����

                    JsonGameData tempObject = JsonUtility.FromJson<JsonGameData>(tempa); //������Ʈ�����ϰ� 

                    tempObject.DataIndex = newIndex; //�ε����� �ٲ㼭 

                    File.WriteAllText(newFullFilePathAndName, JsonUtility.ToJson(tempObject, true));//����� ���Ͽ� �����߰�

                    SettingSaveDatas(tempObject, newIndex); //�����͸���Ʈ���� �����۾�

                    saveObjectReflash?.Invoke(tempObject, newIndex); //���ϳ����̹ٲ�� �ٽ�ȭ�鿡 �ٲﳻ�뺸����������� �߰�

                    isProcessing = false;

                    return true;
                }
                else
                {
                    Debug.LogWarning($"{oldFullFilePathAndName} �ش���ġ�� ������ ���ų� ����� ��ġ�� �߸�����ϴ� = {newIndex}.");
                    isProcessing = false;
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                isProcessing = false;
                return false;
            }

        }
        else
        {
            return false;
        }

    }

    /// <summary> �׽�Ʈ�����ȵ�
    /// ����� ���� ���� 
    /// </summary>
    /// <returns>���ϻ��� ��������</returns>
    public bool Json_FileDelete(int selectFileIndex)
    {
        if (!isProcessing)
        {
            isProcessing = true;
            try
            {
#if UNITY_EDITOR
                Debug.Log(selectFileIndex);
#endif
                string filePath = GetFileInfo(selectFileIndex);
                if (selectFileIndex > -1 && File.Exists(filePath))//�����ִ��� Ȯ�� 
                {
                    File.Delete(filePath);//������ ���� 
                    SettingSaveDatas(null, selectFileIndex); //�����͸���Ʈ���� �����۾�
                    saveObjectReflash?.Invoke(null, selectFileIndex); //������ ȭ�鿡 ������ �����ͷ� ǥ���ϱ����� ����
                    isProcessing = false;
                    return true;
                }
                else
                {
                    Debug.LogWarning($"{filePath} �ش���ġ�� ������ �������� �ʽ��ϴ�.");
                    isProcessing = false;
                    return false;
                }
            }
            catch (Exception e)
            {
                //���� IO������ �˼����� ������ �߻��Ҽ� ������ �ϴ� �ɾ�д�.
                Debug.LogWarning(e);
                isProcessing = false;
                return false;
            }
        }
        else
        {
            return false;
        }
    }




}
