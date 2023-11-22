using System;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections.Generic;
//직렬화 안되는이유
//1. MonoBehaviour 를 상속받으면 직렬화 를 할수없다 
//2. [Serializable] 속성(Attribute) 를 클래스에 붙혀줘야함 
//
// -  클래스 변수를 private 로 지정하였을땐 [SerializeField] 를 붙혀줘야 JsonUtil에서 접근이 가능하다.
//    싱글톤에 들어가는 클래스타입은 제네릭으로 설정할수없다.  gameObject.AddComponent<T>(); addComponent 에서 제네릭클래스를 처리할수가없다
/// <summary>
/// 
/// 저장 로드 관련 베이스 로직 작성할 클래스 
/// try{}cahch(Exeption e){} 를 사용한이유는 게임저장은 게임상에서 단독으로 이루어지는 기능이고 
/// 여기서 오류가난다고 게임이 멈추면 안되기때문에 에러발생하더라도 멈추지않고 플레이 되도록 추가하였다.
/// </summary>
public class SaveLoadManager : ChildComponentSingeton<SaveLoadManager> {



    /// <summary>
    /// 저장폴더 위치 
    /// project 폴더 바로밑에있음 Assets폴더랑 동급위치
    /// </summary>
    string saveFileDirectoryPath;
    public String SaveFileDirectoryPath => saveFileDirectoryPath;


    /// <summary>
    /// 기본 저장할 파일이름
    /// </summary>
    const string saveFileDefaultName = "SpacePirateSave";


    /// <summary>
    /// 저장할 파일 확장명
    /// </summary>
    const string fileExpansionName = ".json ";


    /// <summary>ㅐㅐ
    /// 세이브 파일만 읽어오기위한 패턴 정보 
    /// ? 은 문자열 하나를 비교하는것인데 이것은된다
    /// </summary>
    //string searchPattern = "SpacePirateSave[0-9][0-9][0-9].json"; //사용안됨     
    //readonly string searchPattern = $"{saveFileDefaultName}???{fileExpansionName}"; // 스트링버퍼 왜서치패턴으로 적용안되냐...
    const string searchPattern = "SpacePirateSave???.json";



    /// <summary>
    /// 풀에서 저장윈도우로 SaveData오브젝트를 넘기기위한 저장화면윈도우 게임오브젝트
    /// </summary>
    GameObject saveLoadWindow;
    public GameObject SaveLoadWindow => saveLoadWindow;

    /// <summary>
    /// 풀에서 페이지윈도우로 PageObjtct 를 넘기기위한 오브젝트위치
    /// </summary>
    GameObject saveLoadPagingWindow;
    public GameObject SaveLoadPagingWindow  => saveLoadPagingWindow;


    /// <summary>
    /// 게임화면 리로드
    /// </summary>
    public Action<JsonGameData,int> saveObjectReflash;
    
    

    /// <summary>
    /// 게임데이터 로드를 할시에 게임데이터를 가지고 로드씬으로 정보를가지고 넘어간다. 
    /// </summary>
    public Action<JsonGameData> loadedSceanMove;


    /// <summary>
    /// 비동기 데이터 저장이끝낫을때 화면에 보여줄함수와연결해줄 델리게이트
    /// </summary>
    public Action<JsonGameData[]> isDoneDataLoaing;


    /// <summary>
    /// 게임에사용된 데이터가 파싱되어 여기에 들어가야한다.
    /// 외부클래스에서 관리를 해야하나.?
    /// 데이터가 커지면 미리미리 저장하는경우가있다.
    /// 외부
    /// </summary>
    JsonGameData gameSaveData;
    public JsonGameData GameSaveData { 
        get => gameSaveData;
        set => gameSaveData = value;
    }
    /// <summary>
    /// 저장 데이터 파싱관련 클래스 
    /// </summary>
    SaveDataParsing parsingProcess;
    public SaveDataParsing ParsingProcess => parsingProcess;

    /// <summary>
    /// 저장화면에 사용될 데이터리스트
    /// 오브젝트풀 안쓰고 그냥 데이터로만 가지고 있자.
    /// </summary>
    JsonGameData[] saveDataList;
    public JsonGameData[] SaveDataList => saveDataList;


    /// <summary>
    /// 저장파일 최대갯수 
    /// </summary>
    int maxSaveDataLength = 100;
    public int MaxSaveDataLength => maxSaveDataLength;



    /// <summary>
    /// 파일 관련 처리가 진행중인지 체크하는 변수 
    /// 모든작업은 동시작업진행하면안된다.
    /// 비동기로 진행시 뚧릴 위험이 있기는하다.
    /// </summary>
    bool isProcessing = false;

    
    protected override void Awake()
    {
        base.Awake();
        SaveWindowManager saveManager = FindObjectOfType<WindowList>().transform.GetComponentInChildren<SaveWindowManager>(true); // 찾은 오브젝트에서 비활성회된 자식도 찾는다.

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
        SetDirectoryPath(); // 게임 저장할폴더주소값셋팅
        if (!isProcessing)
        {
            isProcessing = true;
            //Debug.LogWarning($"비동기 로딩테스트 시작 {saveDataList}");
            FileListLoagind();//비동기로 파일로딩
        }
    
    }

    /// <summary>
    /// 저장될 폴더위치값 셋팅
    /// </summary>
    private void SetDirectoryPath()
    {
        //유니티 에디터가아닌 실행환경은 Applicaion에서 자동으로 폴더를만들어준다. 
#if UNITY_EDITOR //유니티 에디터에서만의 설정
        saveFileDirectoryPath = $"{Application.dataPath}/SaveFile/";
        //Application.dataPath 런타임때 결정된다.
#else //유니티에디터가 아닐때의 설정 
        saveFileDirectoryPath = Application.persistentDataPath + "/SaveFile/"; //유니티 에디터가아닌 실행환경은 Applicaion에서 자동으로 폴더를만들어준다. 
#endif

    }
    /// <summary>
    /// 저장폴더 생성여부 확인하고 없으면 생성하는로직
    /// </summary>
    /// <returns>폴더 존재 여부 반환</returns>
    private void ExistsSaveDirectory()
    {
        try
        {
            if (!Directory.Exists(saveFileDirectoryPath))//폴더 위치에 폴더가 없으면 
            {
                Directory.CreateDirectory(saveFileDirectoryPath);//폴더를 만든다.
            }

        }
        catch (Exception e)
        {
            //상위폴더가 없을경우 올수도있겠다.
            //여기올일은 없겠지만 내가모르는 경우가 있을수있으니 일단 걸어둔다.
            Debug.LogWarning(e);
        }
    }
    /// <summary>
    /// 게임시작시 저장화면 보여줄때 사용할 데이터들 찾아오기 
    /// List 형식이아닌 배열형식으로 작성하였다 
    /// 이유는 배열형식으로 순서를 맞춰서 검색이 빠르게 이뤄지도록 하기위해서다.
    /// </summary>
    private bool SetSaveFileList()
    {
        try
        {
            ExistsSaveDirectory();//폴더체크후 없으면 생성 
            string[] jsonDataList = Directory.GetFiles(saveFileDirectoryPath, searchPattern); // 폴더안에 파일들 정보를 읽어서 

            // 리스트 사용시 게임화면에서 세이브창 바뀔때마다 시간이 걸릴가능성이있다 이중포문이 무조건들어가게된다.  
            JsonGameData[] temp = new JsonGameData[maxSaveDataLength]; //배열로 처리시 포문한번으로해결된다. 
            //temp에담은이유는 saveDataList 가 null 값이면 여기서처리하면되기때문에 버그잡기위해 넣어놧다.

            if (jsonDataList.Length == 0) //읽어올파일이없으면 리턴~
            {
                Debug.LogWarning($"{saveFileDirectoryPath}  ========    {searchPattern}     폴더에 저장 파일이 없습니다 ");
                saveDataList = temp;//화면에는 빈객체만뿌려준다.
                return false;
            }
           
            int checkDumyCount = 0;

            for (int i = 0; i < temp.Length; i++)
            { //폴더에서 찾아온 파일갯수만큼 반복문돌리고

                if ((i - checkDumyCount) == jsonDataList.Length)    // 포문돈횟수에서 더미값을뺀값이 파일읽어온크기와같을때.
                {                                                   // 파일이 더이상 존재하지않는경우 빠져나간다.
                    //Debug.Log($"버그싫다 . (i - checkDumyCount) = {(i - checkDumyCount)}  ,i = {i}  ,jsonDataList.Length = {jsonDataList.Length}");
                    break;
                }
                int tempIndex = GetIndexString(jsonDataList[i - checkDumyCount]);//중간에 빈공간을 체크하기위해 checkDumyCount를 적용한다.
                if (tempIndex == i) // 파일 인덱스와 포문순서가 같으면 셋팅한다 
                {
                    string jsonData = File.ReadAllText(jsonDataList[i - checkDumyCount]); // 파일에 접근해서 데이터뽑아보고 
                    if (jsonData.Length == 0 || jsonData == null)
                    {
                        //기본적으로 실행되면안된다. 이것이 실행되면 파일은있으나 파일내용이없다는것이다   
                        Debug.LogWarning($"{i - checkDumyCount} 번째 파일 내용이 없습니다 파일내용 : {jsonDataList[i - checkDumyCount]} ");
                    }
                    else
                    {
                        //더미파일이 아닌경우 작업시작
                        JsonGameData tempSaveData = JsonUtility.FromJson<JsonGameData>(jsonData); // 유틸사용해서 파싱작업후 저장 

                        if (tempSaveData == null) //파싱안됬을때  
                        {
                            //기본적으로 실행되면안된다.  저장로직을 확인해보거나 저장데이터가 정상적으로 만들어지지않아서발생한다.
                            Debug.LogWarning($"{i - checkDumyCount} 번째 파일 내용을 Json으로 변환할수 없습니다  파일내용 : {jsonDataList[i - checkDumyCount]} ");
                        }
                        else
                        {
                            temp[i] = tempSaveData;//제대로처리되면 저장한다.

                        }
                    }
                }
                else
                {
                    checkDumyCount++;// 파일인덱스번호맞추기위해 못찾으면 더미값추가
                }
            }

            //초기로딩(게임시작)시에 처리되게해놔서 조금시간이걸리는작업이라도 해당방법으로 진행하였다.
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
    /// 저장파일 위치와 파일이름및 확장자 정보를 가져온다.
    /// </summary>
    /// <param name="index">저장파일 번호</param>
    /// <returns>파일이 생성 위치와 파일명,확장자를반환한다</returns>
    private string GetFileInfo(int index)
    {
        //파일이름 뒤에 숫자의 형태("D3")를 001 이런형식으로 바꿔서 저장한다. 3은 숫자표시자릿수이다
        return $"{saveFileDirectoryPath}{saveFileDefaultName}{index.ToString("D3")}{fileExpansionName}";
    }

    /// <summary>
    /// 파일명마지막에 카운팅한 숫자 3자리 가져오기
    /// </summary>
    /// <param name="findText">저장파일실제주소값과 이름 확장자명까지</param>
    /// <returns>파일명의 인덱스로줬던 숫자를 뽑아서반환 -1이면 파일없음</returns>
    private int GetIndexString(string findText)
    {
        if (findText != null)
        {
            int findDotPoint = findText.IndexOf('.'); //무조건 ***.json 이고 .은 하나밖에있을수없으니 일단 .위치찾는다
            int temp = int.Parse(findText.Substring(findDotPoint - 3, 3));//점위치에서 3칸앞으로 이동후 3개의 값을 가져오면 숫자 3자리가져올수있다.
            return temp;
        }
        else
        {
            return -1;
        }
    }


    /// <summary>
    /// 비동기 함수 테스트겸 적용해보앗다
    /// 비동기로선언한 async 함수는 사용가능한 Thread 하나를 얻어서 함수를실행한다.
    /// 프로그램 안에서 선언됬기때문에 프로그램과 생명주기를 같이한다 함수내용처리가안됬을때 프로그램이종료되면 같이 사라진다.
    /// </summary>
    private async void FileListLoagind() //비동기 테스트
    {

        //Debug.Log($"비동기 로딩테스트 확인1번 {saveDataList}");

        await TestAsyncFunction(); //이함수가 끝날때까지 기다린다.
        //await Task.Run(() => { isFilesLoading =  SetSaveFileList(); }); //이함수가 끝날때까지 대기 
        if (saveDataList != null) { //데이터로딩이 제대로됬으면 
            isDoneDataLoaing?.Invoke(saveDataList);// 데이터로딩이 비동기로진행시 처리끝날때 처리해야될 함수실행  동기방식이면 필요없다.
            //Debug.LogWarning($"비동기 로딩테스트 끝~ {saveDataList.Length}개  파일 로딩완료"); // 함수끝날때까지 대기타는지 확인하기위해 작성
        }
    }

    /// <summary>
    /// 비동기 함수 테스트 
    /// 파일저장을 비동기로 하기위해 테스트 코드작성
    /// 코루틴과 비슷하지만 기본적으로 동작 개념은 다르다.
    /// </summary>
    private async Task TestAsyncFunction() {
        //Debug.Log("비동기 테스트함수 시작");
        await Task.Run(() =>
        {
            SetSaveFileList();
            isProcessing = false; 

        }); //이함수가 끝날때까지 대기 
        //Debug.Log("비동기 테스트함수 진행");
        await Task.Delay( 3000 ); //3초 기다리기

    }


    /// <summary>
    /// 저장이나 복사 삭제 같이 내용이수정될시
    /// 세이브데이터리스트도 갱신한다.
    /// </summary>
    /// <param name="gameData">수정되는 파일</param>
    /// <param name="index">수정되는 인덱스</param>
    private void SettingSaveDatas(JsonGameData gameData, int index)
    {
        if (saveDataList != null && index > -1)
        {
            saveDataList[index] = gameData;
        }
    }


    /// <summary>
    /// 파일저장시 기본정보를 입력한다.
    /// </summary>
    /// <param name="saveData">저장 파일</param>
    /// <param name="index">저장 파일번호</param>
    private void SetDefaultInfo(JsonGameData saveData , int index) {
        saveData.DataIndex = index;  ///파일인덱스 저장
        saveData.SaveTime = DateTime.Now.ToString();// 저장되는 시간을 저장한다
        saveData.SceanName = (EnumList.SceneName)SceneManager.GetActiveScene().buildIndex; //씬이름을 저장한다.
    }

    /// <summary>
    /// 파일 입출력시 기존에 파일이있는데 쓸경우 오류가 발생하기때문에 
    /// 파일 있는지 체크하고 있으면 삭제후 빈파일을 만들어준다.
    /// <param name="filePath">저장될 파일주소</param>
    /// </summary>
    private void FileCreate(string filePath) {
        if (File.Exists(filePath)) //파일 체크
        {  
            File.Delete(filePath); // 기존파일있으면 삭제하고                 
        }
        FileStream fs = File.Create(filePath); //새롭게 파일 생성
        fs.Close(); // 생성한파일 접근한것을 닫아서 풀어준다.
    }

    /// <summary>
    /// 게임에서 세이브시 데이터를 파일로 빼는 작업
    /// </summary>
    /// <param name="index">저장파일 번호</param>
    /// <returns>파일저장 성공여부</returns>
    public bool Json_Save(int selectFileIndex)
    {
        if (!isProcessing)
        {
            isProcessing = true;
            try
            {

                if (gameSaveData != null && selectFileIndex > -1) //게임데이터가 있을때 
                {
                    SetDefaultInfo(gameSaveData, selectFileIndex);// 파일저장시 기본정보를 저장한다.
                    string toJsonData = JsonUtility.ToJson(gameSaveData, true); //저장데이터를 Json형식으로 직렬화 하는 작업 유니티 기본제공
                                                                                //true입력시 파일용량이커진다. 대신보기좋아진다.
                    string filePath = GetFileInfo(selectFileIndex);
                    FileCreate(filePath); //저장할 파일 생성
                    File.WriteAllText(filePath, toJsonData); //파일에 저장하기 
                    SettingSaveDatas(gameSaveData, selectFileIndex); //데이터리스트에도 수정작업
                    saveObjectReflash?.Invoke(gameSaveData, selectFileIndex); //저장후 화면에 저장된 데이터로 표시하기위해 실행
                    isProcessing = false;
                    return true;
                }
                Debug.Log($"선택 파일인덱스 :{selectFileIndex} , 게임데이터리스트 : {gameSaveData} 데이터 확인이필요합니다.");
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
   /// 저장파일 읽어오는 함수 
   /// </summary>
   /// <returns>읽어오기 성공여부</returns>
    public bool Json_Load(int selectFileIndex) 
    {
        if (!isProcessing)
        {
            isProcessing = true;
            try
            {

                string filePath = GetFileInfo(selectFileIndex);
                if (selectFileIndex > -1 &&  File.Exists(filePath))//저장폴더에서 파일이 있는지 체크 
                {
                    string saveJsonData = File.ReadAllText(filePath); //저장파일을 스트링으로 읽어오기 
                    //제약이 많음. 다중배열 지원안되고, private 맴버변수들 접근하려면  [SerializeField] 선언해야함. 클래스가 [Serializable] 선언되야함.
                    gameSaveData = JsonUtility.FromJson<JsonGameData>(saveJsonData); //변환된 스트링 데이터를 JsonUtility 내부적으로 직렬화처리후 클래스에 자동파싱 
                    loadedSceanMove?.Invoke(gameSaveData); //로드시 해당씬으로 이동한다.
                    isProcessing = false;
                    return true;
                }
                else
                {
                    Debug.LogWarning($"{filePath} 해당위치에 파일이 존재하지 않습니다.");
                    isProcessing = false;
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e); // 예상치 못한 오류 발생시 아직발견못함. 클래스 구조만 잘맞추면 대체로 잘된다.
                isProcessing = false;
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    
    /// <summary> 테스트아직안됨
    /// 첫번째 인덱스값에 해당하는 파일을 두번째 인덱스값에 해당하는 파일을내용으로 붙혀넣는기능
    /// 저장 파일 복사 
    /// 저장시 복사된파일은 자신의 인덱스를 유지해야한다.
    /// </summary>
    /// <param name="oldIndex">복사할 파일번호</param>
    /// <param name="newIndex">복사될 파일번호</param>
    /// <returns>복사하기 성공여부</returns>
    public bool Json_FileCopy(int oldIndex , int newIndex) 
    {
        if (!isProcessing)
        {
            isProcessing = true; //로직 실행여부
            try
            {

                string oldFullFilePathAndName = GetFileInfo(oldIndex);// 복사할 파일위치
                if (oldIndex > -1 && newIndex > -1 &&  File.Exists(oldFullFilePathAndName))//복사할 파일위치에 파일이있는지 확인
                {
                    string newFullFilePathAndName = GetFileInfo(newIndex);// 복사될 파일위치

                    FileCreate(newFullFilePathAndName);//파일 생성

                    string tempa = File.ReadAllText(oldFullFilePathAndName);//복사할 파일을 읽어오기

                    JsonGameData tempObject = JsonUtility.FromJson<JsonGameData>(tempa); //오브젝트생성하고 

                    tempObject.DataIndex = newIndex; //인덱스값 바꿔서 

                    File.WriteAllText(newFullFilePathAndName, JsonUtility.ToJson(tempObject, true));//복사될 파일에 내용추가

                    SettingSaveDatas(tempObject, newIndex); //데이터리스트에도 수정작업

                    saveObjectReflash?.Invoke(tempObject, newIndex); //파일내용이바뀌면 다시화면에 바뀐내용보여줘야함으로 추가

                    isProcessing = false;

                    return true;
                }
                else
                {
                    Debug.LogWarning($"{oldFullFilePathAndName} 해당위치에 파일이 없거나 복사될 위치가 잘못됬습니다 = {newIndex}.");
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

    /// <summary> 테스트아직안됨
    /// 저장된 파일 삭제 
    /// </summary>
    /// <returns>파일삭제 성공여부</returns>
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
                if (selectFileIndex > -1 && File.Exists(filePath))//파일있는지 확인 
                {
                    File.Delete(filePath);//있으면 삭제 
                    SettingSaveDatas(null, selectFileIndex); //데이터리스트에도 수정작업
                    saveObjectReflash?.Invoke(null, selectFileIndex); //삭제후 화면에 삭제된 데이터로 표시하기위해 실행
                    isProcessing = false;
                    return true;
                }
                else
                {
                    Debug.LogWarning($"{filePath} 해당위치에 파일이 존재하지 않습니다.");
                    isProcessing = false;
                    return false;
                }
            }
            catch (Exception e)
            {
                //파일 IO관련은 알수없는 오류가 발생할수 있으니 일단 걸어둔다.
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
