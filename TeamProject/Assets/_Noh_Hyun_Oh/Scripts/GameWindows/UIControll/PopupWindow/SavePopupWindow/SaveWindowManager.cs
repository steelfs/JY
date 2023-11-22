
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 화면에 보이는것만 신경쓰자
/// </summary>
public class SaveWindowManager : PopupWindowBase ,IPopupSortWindow ,IPointerDownHandler
{
    /// <summary>
    /// 화면이 OnEnable 이 되야 Start 가 실행이되서 초기화 되기때문에 
    /// 초기화 여부를 체크 
    /// </summary>
    bool isInit = false;

    /// <summary>
    /// 현재 페이지넘버
    /// 해당값은 기본적으로 1씩 페이지입력처리시 마지막 페이지값을 가져와야한다.
    /// 페이지넘버는 0부터 시작해야한다.
    /// </summary>
    [SerializeField]
    int pageIndex = 0;          // 현재 페이지넘버 첫번째 0번
    public int PageIndex
    {
        get => pageIndex;
        set {
            pageIndex = value;
            if (pageIndex < 0) //페이지수가 0이 첫번째이니 그보다작을순없다
            {
                pageIndex = 0;
            }
            else if (value > lastPageIndex) // 페이지가 최대페이지보다많게들어오면
            {
                pageIndex = lastPageIndex; //마지막페이지를 보여준다.
            }

        }
    }
    [SerializeField]
    /// <summary>
    /// 저장오브젝트 세로크기
    /// </summary>
    float saveObjectHeight = 150.0f;

    /// <summary>
    /// 한페이지에 보일 최대 저장파일 오브젝트 최대 갯수
    /// </summary>
    [SerializeField]
    int pageMaxObject = 8;
    public int PageViewSaveObjectMaxSize => pageMaxObject;
    /// <summary>
    /// 한페이지에 보일 최대 페이징 오브젝트 갯수 
    /// </summary>
    [SerializeField]
    int pagingMaxObject = 4;
    public int PagingMaxObject => pagingMaxObject;
    /// <summary>
    /// 마지막 페이지 값 
    /// </summary>
    int lastPageIndex = -1;
    public int LastPageIndex => lastPageIndex;

    /// <summary>
    /// 팝업창 순서 정렬하기용으로 클릭이밴트 드리븐
    /// </summary>
    public Action<IPopupSortWindow> PopupSorting { get; set; }

    /// <summary>
    /// 마지막페이지에 보일 저장파일 오브젝트 갯수
    /// </summary>
    int lastPageObjectLength = -1;

    
    /// <summary>
    /// 저장윈도우 오브젝트 위치
    /// </summary>
    private GameObject saveWindowObject;

    /// <summary>
    /// 저장페이지 오브젝트 위치
    /// 페이징 처리할곳의 오브젝트 위치
    /// </summary>
    private GameObject saveWindowPageObject;

    /// <summary>
    /// 저장로직실행시 팝업윈도우 위치
    /// </summary>
    private ModalPopupWindow saveLoadPopupWindow;


    //int singletonCheck;

    //protected override void Awake()
    //{
    //    if (this.GetHashCode() == singletonCheck)
    //    {
    //        Debug.Log("해시코드같다 ");
    //    }
    //    else 
    //    {
    //        singletonCheck = this.GetHashCode();
    //        Debug.Log($"두개생성? {singletonCheck}");
    //    }
    //    base.Awake();
    //}

    /// <summary>
    /// 화면전환시 다시발동안됨 확인완료.
    /// OnEnable 보다 늦게 실행된다.
    /// </summary>
    private void Start()
    {
        Oninitialize();
        InitLastPageIndex(); //페이징에 사용될 초기값셋팅
        InitSaveObjects(); //저장화면 초기값셋팅
        isInit = true; //데이터가 초기화된뒤 화면을 셋팅해줘야함으로 초기화됬는지 체크
        SetGameObjectList(SaveLoadManager.Instance.SaveDataList);//처음화면을띄울때 화면셋팅용
    }
    public void Oninitialize() 
    {
        //싱글톤 설정때매 Awake 에서 못찾는다. 
        saveWindowObject = SaveLoadManager.Instance.SaveLoadWindow;
        saveWindowPageObject = SaveLoadManager.Instance.SaveLoadPagingWindow;
        saveLoadPopupWindow = WindowList.Instance.IOPopupWindow;

        //델리게이트 연결 
        SaveLoadManager.Instance.saveObjectReflash = SetGameObject; //저장관련 기능실행시 오브젝트을 다시그린다.
        SaveLoadManager.Instance.isDoneDataLoaing = SetGameObjectList; //데이터 비동기로 처리시 끝나면 화면리셋 자동으로 해주기위해 추가
        saveLoadPopupWindow.focusInChangeFunction = SetFocusView; //저장버튼클릭시 처리하는 함수연결
        
    }
    /// <summary>
    /// 활성화시 풀에서 생성된 여분의 오브젝트를 숨기는기능추가
    /// Start함수보다 빨리실행된다.
    /// </summary>
    private void OnEnable()
    {

        if (isInit)//스타트함수보다 빨리실행되서 처음열때 오류가 발생한다.
        { 
            SetGameObjectList(SaveLoadManager.Instance.SaveDataList); //초기화 작업때 비동기로 파일데이터를 읽어오기때문에 셋팅이안됬을수도있다 
            SetPoolBug(saveWindowPageObject.transform, pagingMaxObject);
        }
    }

    
    /// <summary>
    /// 현재 페이지에서 화면에 보여줄 저장파일 오브젝트 갯수를 반환한다.
    /// </summary>
    /// <returns>현재페이지의 저장오브젝트갯수</returns>
    private int GetGameObjectLength()
    {
        return pageIndex > lastPageIndex ? lastPageObjectLength : pageMaxObject;
    }

   
    /// <summary>
    /// 파일 인덱스로 현재 화면에 어느오브젝트에 보여줘야되는지 오브젝트 위치를 반환한다.
    /// 저장오브젝트 넘버(0 ~ pageMaxSize) =  저장파일번호(0 ~ 설정한최대값) - (페이지넘버(설정값) 0번부터시작 * 한페이지에 보이는 갯수 ) 
    /// </summary>
    /// <param name="fileIndex">파일인덱스</param>
    /// <returns>현재 페이지의 오브젝트 인덱스</returns>
    private int GetGameObjectIndex(int fileIndex)
    {
        return fileIndex - (pageIndex * pageMaxObject);
    }


    /// <summary>
    /// 파일최대갯수 와 현재 페이지에 보이는 오브젝트갯수 를 가지고 최종페이지수와 마지막페이지에 보여질 오브젝트갯수를 가져온다.
    /// </summary>
    private void InitLastPageIndex()
    {
        lastPageIndex = (SaveLoadManager.Instance.MaxSaveDataLength / pageMaxObject);  //페이지 갯수 가져오기
        lastPageObjectLength = (SaveLoadManager.Instance.MaxSaveDataLength & pageMaxObject); //마지막페이지에 보여줄 오브젝트 갯수
    }

    /// <summary>
    /// 1. 풀에서 생성된 오브젝트가 갯수에 맞춰서 생성되지않기때문(풀에서 두배로 늘리는작업)에 작업이 추가 - 나중에 풀을 수정해야될듯하다.
    /// 2. 풀에서 생성된 오브젝트중에 안쓰는 부분은 감춰주는 작업
    /// </summary>
    private void InitSaveObjects()
    {
        //페이징
        int childCount = saveWindowPageObject.transform.childCount; //현재 풀에서 생성된 오브젝트 갯수 를 가져온다. (페이징)
        if (childCount < pagingMaxObject)//생성된 오브젝트가 화면에 보여질 갯수보다 작을경우 
        {
            PoolBugFunction(saveWindowPageObject.transform,  pagingMaxObject, EnumList.MultipleFactoryObjectList.SAVE_PAGE_BUTTON_POOL);//부족한부분가져와서 필요없는부분감추기
        }

        childCount = saveWindowObject.transform.childCount; //현재 풀에서 생성된 오브젝트 갯수 를 가져온다. (저장오브젝트)
        int proccessLength = GetGameObjectLength(); //현재페이지의 저장오브젝트 갯수를 가져온다.
        if (childCount < proccessLength)//생성된 오브젝트가 화면에 보여질 갯수보다 작을경우 
        {
            PoolBugFunction(saveWindowObject.transform,  proccessLength, EnumList.MultipleFactoryObjectList.SAVE_DATA_POOL);//부족한부분가져와서 필요없는부분감추기
        }
     
        for (int i = 0; i < proccessLength; i++)//한페이지만큼만 돌린다
        {
            SaveFileRectSetting(saveWindowObject.transform.GetChild(i).GetComponent<RectTransform>(),i); //파일 사이즈조절및 위치잡기 
        }
        SetListWindowSize(saveWindowObject.transform, proccessLength);//저장화면 크기조절

        SetPageList();//페이징 데이터 화면에뿌리기
    }
    /// <summary>
    /// 세이브파일 위치및 크기설정 
    /// </summary>
    /// <param name="rt">세이브오브젝트의 렉트트랜스폼</param>
    /// <param name="index">화면에 보일 인덱스</param>
    private void SaveFileRectSetting(RectTransform rt , int index) {
        Vector2 tempV = rt.anchorMax; //앵커 위쪽 으로 몰빵
        tempV.x = 1.0f;
        tempV.y = 1.0f;
        rt.anchorMax = tempV;

        tempV = rt.anchorMin;       //width 는 부모값을 받기위해 최대치 설정 
        tempV.x = 0.0f;
        tempV.y = 1.0f;
        rt.anchorMin = tempV;

        tempV = rt.pivot;           //상위오브젝트와 기준점 위치를 맞추기위해 왼쪽위로 잡앗다.
        tempV.x = 0.0f;
        tempV.y = 1.0f;
        rt.pivot = tempV;

        tempV =  rt.sizeDelta;      
        tempV.x = 0.0f;             //객체의 크기 width 는 0을줘서 최대값을 주고 위에 anchorX값에서 잡은만큼 적용 
        tempV.y = saveObjectHeight; //객체의 크기 height 는 설정값을 줬다.
        rt.sizeDelta = tempV;

        tempV = rt.localPosition;
        tempV.x = 0.0f;                     //포지션은 왼쪽끝
        tempV.y = -saveObjectHeight * index ; //포지션은 객체 가 순차적으로 쌓일수있도록 설정 
        rt.localPosition = tempV;

    }
    /// <summary>
    /// 풀에서 생성된 오브젝트 에서 부족한부분 추가하고 필요없는 것들 비활성화하는 함수 호출
    /// </summary>
    /// <param name="position">풀의 생성위치로 잡힌 오브젝트</param>
    /// <param name="childCount">풀에서 생성된 오브젝트 갯수</param>
    /// <param name="proccessLength">필요한 오브젝트 갯수</param>
    /// <param name="type">생성할 오브젝트 타입</param>
    private void PoolBugFunction(Transform position, int proccessLength, EnumList.MultipleFactoryObjectList type) {

        for (int i = 0; i < proccessLength; i++) //필요한만큼 추가로 생성한다 
        {
            Multiple_Factory.Instance.GetObject(type);//오브젝트 추가해서 강제로 풀의사이즈를늘린다.
        }
        SetPoolBug(position, proccessLength);//필요없는 오브젝트를 비활성화 하는 함수
    }

    /// <summary>
    /// 풀에서 2배씩늘리기때문에 안쓰는파일이 생긴다 그것들을 비활성화 하는 작업.
    /// </summary>
    /// <param name="position">초기화할 위치</param>
    /// <param name="startIndex">시작 인덱스</param>
    private void SetPoolBug(Transform position, int startIndex)
    {
        int lastIndex = position.childCount;// 생성된후에 추가된 갯수를 다시넘긴다.
        for (int i = 0; i < lastIndex; i++) {
            position.GetChild(i).gameObject.SetActive(true); //오브젝트 활성화해서 일단 전부보여준뒤
        }
        for (int i = startIndex; i < lastIndex; i++)//안쓰는 파일만큼만 돌린다.
        {
            position.GetChild(i).gameObject.SetActive(false); //안쓰는파일 숨기기 
        }
        
    }


    /// <summary>
    /// 저장데이터 리스트의 윈도우 크기를 설정한다.
    /// </summary>
    private void SetListWindowSize(Transform position, int fileLength) {
        //트랜스폼을 변경해봤지만 사이즈델타값이 최종적으로바껴서 사이즈델타를 수정하였다.
        RectTransform rt = position.GetComponent<RectTransform>();
        Vector2 windowSize = rt.sizeDelta;
        windowSize.y = saveObjectHeight * fileLength;
        rt.sizeDelta = windowSize;
    }

    /// <summary>
    /// 한페이지에 보이는 오브젝트들의 데이터를 다시셋팅한다.
    /// <param name="saveDataList">화면에 뿌릴 데이터리스트</param>
    /// </summary>
    private void SetGameObjectList(JsonGameData[] saveDataList)
    {
        if (saveDataList == null)
        { // 읽어온 파일정보가없는경우 리턴
            Debug.LogWarning("읽어온 파일정보가없어?");
            return;
        }
        if (!isInit)//비동기로 처리하는것때문에 추가 
        {
            //Debug.LogWarning("아직 초기화 안됬다.");
            return;
        }
        int startIndex = pageIndex * pageMaxObject; //페이지시작오브젝트위치값 가져오기

        int lastIndex = (pageIndex + 1) * pageMaxObject > SaveLoadManager.Instance.MaxSaveDataLength ? //파일리스트 최대값 < 현재페이징값 * 화면에보여지는최대오브젝트수
                            SaveLoadManager.Instance.MaxSaveDataLength : //마지막페이지면 남은갯수만 셋팅
                            (pageIndex + 1) * pageMaxObject; //아니면 일반적인 페이징 

        for (int i = startIndex; i < lastIndex; i++)
        { //데이터를 한페이지만큼만 확인한다.
            SetGameObject(saveDataList[i], i); // 데이터를 셋팅하자 
            
        }
        int visibleEndIndex = lastIndex - startIndex; //페이지의 마지막 인덱스값을 준다.
        SetPoolBug(saveWindowObject.transform, visibleEndIndex);//풀은 오브젝트를 2배씩늘리는데 사용안하는것들은 비활성화작업이필요해서 추가했다.
        SetListWindowSize(saveWindowObject.transform, visibleEndIndex);
    }

    /// <summary>
    /// 값이 셋팅되야한다 
    /// lastPageIndex = 페이지 총갯수-1개 
    /// pageMaxSize = 한페이지에 보이는갯수
    /// 두값이 셋팅이되면 현재 페이지 기준으로 화면에 맨처음값을 반환한다.
    /// 해당함수는 pageMaxSize < lastPageIndex // 일때만 사용된다 일단 기본값 8로 설정하면 문제없음.
    /// 기능더추가할려면 내용수정해야함.
    /// </summary>
    /// <param name="viewLength">화면에 보일 페이지갯수</param>
    /// <returns>for문의 0번째에 셋팅될 값</returns>
    private int GetStartIndex(int viewLength)
    {
        int returnNum = 0; //반환값 이자 초기값 반절이상이안넘어가면 0을반환한다.
        int halfPageNum = viewLength / 2; //페이지의 반절
        int addPage = viewLength % 2; //이값으로 홀수와짝수를 구분하자
        if (pageIndex >= halfPageNum + addPage) // 현재페이지가 페이지의반절값(한페이지에보이는 갯수가 홀수면 +1) 보다 같거나 클때 
        { // 페이지가 중간값을 넘어갈때                      
            if (pageIndex + halfPageNum >= lastPageIndex) //마지막리스트(현재페이지+ 페이지의반절값)값이 페이지 최대값보다 같거나 클때 
            {
                returnNum = lastPageIndex - viewLength + 1; //마지막페이지에 근접할때 처리될값
            }
            else
            {
                returnNum = pageIndex - halfPageNum + (1 - addPage); // 중간페이지 일때 처리될값
            }

        }
        return returnNum + 1; //페이지는 0부터 가아니라 1부터 보여야하기때문에 +1
    }

    /// <summary>
    /// 파일인덱스를 넣으면 해당오브젝트를 다시그린다.
    /// 게임 오브젝트가 존재한다는 가정하에 실행된다.
    /// </summary>
    /// <param name="saveData">수정된 저장데이터</param>
    /// <param name="index">수정된 인덱스</param>
    public void SetGameObject(JsonGameData saveData, int fileIndex) {
        if (!isInit) {
            Debug.Log("아직 초기화 안됫다고");
            return;
        }
        int viewObjectNumber = GetGameObjectIndex(fileIndex); //페이지별 오브젝트 위치찾기

        SaveGameObject sd = saveWindowObject.transform.GetChild(viewObjectNumber).GetComponent<SaveGameObject>(); //수정된 오브젝트 가져온다.
        sd.gameObject.SetActive(true);
        sd.ObjectIndex = viewObjectNumber; //오브젝트 넘버링을 해준다 

        if (saveData != null) { //저장데이터가 있는지 체크

            //밑으로는 데이터 셋팅 프로퍼티 Set 함수에 화면에 보여주는 기능넣어놨다.
            //보이는기능수정시 SaveDataObject 클래스수정.
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
            //기본값 셋팅
            sd.FileIndex = fileIndex; //기본적인 파일 넘버링
            sd.name = "";
            sd.CreateTime = "";
            sd.SceanName = EnumList.SceneName.NONE;
            sd.Money = 0;
            sd.CharcterLevel = 0;
            sd.EtcText = "";
        }
    }

    /// <summary>
    /// 페이지 숫자를 재나열한다.
    /// </summary>
    public void SetPageList(int index = -1) {
        if (index > -1) PageIndex = index;
        SetGameObjectList(SaveLoadManager.Instance.SaveDataList); // 파일리스트 보여주기

        int startIndex = GetStartIndex(pagingMaxObject); // 페이징처리후 처음 표시할값을 가져온다  //10개면 0.1 값
        
        //나누기연산 줄이기
        float[] arithmeticValue = new float[2];
        arithmeticValue[0] = 1.0f / pagingMaxObject;// 페이징 버튼 한칸이 차지하는 크기 
        arithmeticValue[1] = pagingMaxObject / 1000.0f;  //페이징 버튼 끼리의 간격조절하기위한 값
        SavePageButton_PoolObj savePageBtn;
        for (int i = 0; i < pagingMaxObject; i++) { //한페이지 다시돌면서 셋팅한다
            PageNumRectSetting(saveWindowPageObject.transform.GetChild(i).GetComponent<RectTransform>(),i, arithmeticValue, pagingMaxObject);
            savePageBtn = saveWindowPageObject.transform.GetChild(i).GetComponent<SavePageButton_PoolObj>();
            savePageBtn.gameObject.SetActive(true);
            savePageBtn.PageIndex = startIndex + i; //페이지 인덱스값 표시
        }
        ResetSaveFocusing();//페이지이동시 초기화
        SetPoolBug(saveWindowPageObject.transform, pagingMaxObject);
    }
 
    /// <summary>
    /// 버튼의 갯수가 늘어나더라도 버튼의 사이즈 자동 조절하기위한 계산식 
    /// </summary>
    /// <param name="rt">렉터 트랜스폼</param>
    /// <param name="index">페이징버튼 순번</param>
    /// <param name="arithmeticValue">나누기연산이필요해서 반복문밖에서 처리한값을 배열로받아서 곱셈으로 사용</param>
    /// <param name="length">화면에보일 버튼의 갯수</param>
    private void PageNumRectSetting(RectTransform rt , int index , float[] arithmeticValue, int length) 
    {
        int doubleCheck = index < 1 ? 0 : 1;
        Vector2 tempV = rt.anchorMin;
        tempV.x = index * arithmeticValue[0] + arithmeticValue[1] +  arithmeticValue[1] * doubleCheck; //페이징버튼  연산 맨앞에값은 0 그다음은 1칸크기+ 패딩의2배값
        tempV.y = 0.25f;
        rt.anchorMin = tempV;

        tempV = rt.anchorMax;      
        
        tempV.x =   (index+1) * arithmeticValue[0]; //한칸의 크기를 잡아준다
        tempV.y = 0.75f;
        rt.anchorMax = tempV;

        tempV = rt.pivot;           //상위오브젝트와 기준점 위치를 맞추기위해 왼쪽위로 잡앗다.
        tempV.x = 0.0f;
        tempV.y = 1.0f;
        rt.pivot = tempV;
        
       
        Rect rect = rt.rect;

        rect.xMin = 0.0f; //에디터의 Left   값을 수정한다.
        rect.yMax = 0.0f; //에디터의 Top    값을 수정한다.
        rect.xMax = 0.0f; //에디터의 Right  값을 수정한다.
        rect.yMin = 0.0f; //에디터의 Bottom 값을 수정한다.

        // 수정된 위치와 크기 정보를 RectTransform에 적용합니다.
        rt.sizeDelta = new Vector2(rect.width, rect.height);
        rt.anchoredPosition = new Vector2(rect.x, rect.y);

    }



    /// <summary>
    /// 세이브파일 포커싱 리셋하기 
    /// </summary>
    public void ResetSaveFocusing() {
        int initLength = GetGameObjectLength();
        for (int i = 0; i < initLength; i++)
        {
            saveWindowObject.transform.GetChild(i).GetComponent<Image>().color = Color.black;
        }
    }

    /// <summary>
    /// 세이브 파일 포커싱 하기
    /// </summary>
    /// <param name="fileIndex">선택한 파일 인덱스</param>
    /// <param name="isCopy">현재 복사상태인지확인</param>
    public void SetFocusView(int fileIndex,bool isCopy)
    {
        if (fileIndex > -1) //선택파일이 제대로된 데이터가있는경우 
        {
            int pageIndexObejct = GetGameObjectIndex(fileIndex); //게임오브젝트위치가져오기
            if (!isCopy) //복사가 아닌경우 
            {
                ResetSaveFocusing(); //기존 포커싱 해제하고
                saveWindowObject.transform.GetChild(pageIndexObejct).GetComponent<Image>().color = Color.white; //새로 포커싱
            }
            else // 복사상태인경우
            {
                if (saveLoadPopupWindow.OldIndex == saveLoadPopupWindow.NewIndex)
                { //저장버튼 눌렀을시에 같은값을 가지고 이리온다.
                    saveWindowObject.transform.GetChild(GetGameObjectIndex(saveLoadPopupWindow.OldIndex)).GetComponent<Image>().color = Color.red;
                }
                else //저장버튼누른후 파일을 선택할시  
                {
                    saveWindowObject.transform.GetChild(GetGameObjectIndex(saveLoadPopupWindow.NewIndex)).GetComponent<Image>().color = Color.blue;
                }
            }
        } 
    }


    /// <summary>
    /// 화면 정렬용 이벤트함수 추가 
    /// </summary>
    /// <param name="eventData">사용안함</param>
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
