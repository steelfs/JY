using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

/// <summary>
/// 포지션은 (0,20,-70)
/// 로테이션 (45,0,0)
/// </summary>
public class OpeningStart : MonoBehaviour
{ 

    /// <summary>
    /// 텍스트 기본 만들어둔 프리팹
    /// </summary>
    [SerializeField]
    TextMeshPro textPrefab;
    
    /// <summary>
    /// 텍스트 기본 사이즈
    /// </summary>
    const float textWidth = 80.0f;
    
    /// <summary>
    /// 텍스트 기본 사이즈
    /// </summary>
    const float textHeight = 5.0f;

    /// <summary>
    /// 줄간격 + 면 아래에서 위로  - 면 위에서 아래로 정렬
    /// </summary>
    [SerializeField]
    float textPadding = -10.0f;
    /// <summary>
    /// 그림자처럼 보이게하는 글자의 위치간격
    /// </summary>
    [SerializeField]
    float shadowPadding = -0.25f;

    /// <summary>
    /// 글자크기
    /// </summary>
    [SerializeField]
    float textSize = 23.0f;
    
    [SerializeField]
    Color shadowColor;
    /// <summary>
    /// 카메라 기본 위치
    /// </summary>
    Vector3 cameraPosition = new Vector3(0.0f,20.0f,0.0f);

    /// <summary>
    /// 카메라 기본 각도
    /// </summary>
    Vector3 cameraRotate = new Vector3(45.0f,0.0f,0.0f);

    [SerializeField]
    float cameraMoveSpeed = 3.0f;
    float elaspadSpeed = 0.0f;

    CinemachineVirtualCamera virtualCamera;

    int lineCount = -1;

    private void Awake()
    {

        lineCount = SetTextArray(TextFileRead()); //시작하면 바로 데이터 불러와서 셋팅

        OpeningButtonManager buttons = GameObject.FindObjectOfType<OpeningButtonManager>(); //버튼 관리해주는 컴퍼넌트 찾기 
        buttons.speedUpButton += (text) => {
            if (elaspadSpeed < cameraMoveSpeed + 0.1f) //이동속도 1배속 체크
            {
                text.text = ">>";
                elaspadSpeed = cameraMoveSpeed * 2; //두배로 늘린다.
            }
            else if (elaspadSpeed < cameraMoveSpeed * 2 + 0.1f) //이동속도 2배속 체크 
            {
                text.text = ">>>";
                elaspadSpeed = cameraMoveSpeed * 3; //세배로 늘린다.
            }
            else 
            {
                text.text = ">";
                elaspadSpeed = cameraMoveSpeed; // 원래 속도로 돌린다.
            }
        };

        buttons.skipButton += (text) => {
            LoadingScene.SceneLoading(EnumList.SceneName.TITLE); //스킵이면 씬이동
        };

        elaspadSpeed = cameraMoveSpeed; //이동속도 기본속도로 셋팅
        virtualCamera = GameObject.FindGameObjectWithTag("OpeningCam").GetComponent<CinemachineVirtualCamera>();
        virtualCamera.transform.position = cameraPosition;
        virtualCamera.transform.rotation = Quaternion.Euler(cameraRotate);
    }

    private void Start()
    {
        //카메라 불러와서 위치잡고
        //카메라 이동시작
        StartCoroutine(OpeningCameraMove(lineCount));
     
    }


    /// <summary>
    /// 카메라가 이동하여 도착지점까지 간다.
    /// </summary>
    /// <param name="endValue">도착할 위치값</param>
    IEnumerator OpeningCameraMove(int endValue) 
    {
        //음악 재생 추가 필요 

        float checkValue = virtualCamera.transform.position.z;
        float checkEnd = endValue * textPadding - 100.0f;
        while (checkValue > checkEnd) 
        {
            checkValue -= Time.deltaTime * elaspadSpeed;
            virtualCamera.transform.position = new Vector3(
                                                0,
                                                20,
                                                checkValue);
            yield return null;
        }
        //도착이니 다음 화면으로 이동
        LoadingScene.SceneLoading(EnumList.SceneName.TITLE); 
    }
    string enterText = "//";

    /// <summary>
    /// 연속된 글자의 배열을 가지고 TextMeshPro 를 만들어 화면에뿌리는로직
    /// </summary>
    /// <param name="fileText">읽어온 파일의 연속된글자의 배열</param>
    /// <returns>글자의 라인수</returns>
    private int SetTextArray(string[] fileText) 
    {
        int textLength = fileText.Length; //글자 구간 총갯수 
        RectTransform rt; //위치조절할 렉트 변수 선언해두고 
        RectTransform shadowRt;
        int textLineLength = 0;
        int lineCount = 1;      //몇줄째인지 체크용 포문돌기전에 하나생성하고시작하기때문에 1이 초기값

        TextMeshPro textObject = null;
        TextMeshPro shadowObject = null;
        for (int i = 0; i < textLength ; i++) //글자 구간갯수 만큼 돌리고 
        {
            if (fileText[i].Contains(enterText) 
                )//|| textLineLength + fileText[i].Length > fileLineSize)           //한줄에 보여줄 길이 넘어가면 
            {
                fileText[i] =  fileText[i].Replace(enterText,"");                   //
                textObject = Instantiate(textPrefab,transform);                     //새롭게 프리팹 내용 복사하고 
                rt = textObject.GetComponent<RectTransform>();                      //위치 조절할 렉트 찾아오고 
                rt.anchoredPosition3D = new Vector3(0, 0, lineCount * textPadding); //위치 조절하고 
                textObject.fontSize = textSize;
                textObject.text = fileText[i];                                      //내용 담고 

                shadowObject = Instantiate(textPrefab, transform); //프리팹 생성해서 담기 시작
                shadowRt = shadowObject.GetComponent<RectTransform>(); //위치 조절할 렉트 찾아오고 
                shadowRt.anchoredPosition3D = new Vector3(0.0f, shadowPadding, lineCount * textPadding); //처음 위치 조절하고 
                shadowObject.color = shadowColor;
                shadowObject.fontSize = textSize;
                shadowObject.text = fileText[i];
                

                //textLineLength = fileText[i].Length;                                //글자 길이 수정 
                lineCount++;
            }
            else //한줄에 보여줄 길이 안넘어가면 다시 추가하고 
            {
                textLineLength += fileText[i].Length+1; //글자 길이 추가해두고 
                textObject.text += $" {fileText[i]}"; //내용도 추가 
                shadowObject.text += $" {fileText[i]}";
            }
      
        }

        return lineCount;
    }


    /// <summary>
    /// txt 파일 읽어서 string에 담는 로직 
    /// </summary>
    /// <returns>연속된 글자의 배열</returns>
    private string[] TextFileRead() 
    {
        //StreamingAssets 폴더를 사용하는방법으로 변경 
        string filePath = Path.Combine(Application.streamingAssetsPath, "TextFile"); // 상대 경로 설정

        string fileFullPath = filePath + "/Synopsis.txt";
        string[] textArray = null; //반환할값 선언
        
        try
        {
            string readText = string.Empty;
            if (!Directory.Exists(filePath))
            {
                Debug.Log(filePath);
                Directory.CreateDirectory(filePath);
            }
            readText = File.ReadAllText(fileFullPath);
            textArray = readText.Split(' ','\r','\n'); //구간별로 나눠서 배열로 담는다.
            //foreach (string text in textArray) 
            //{
            //    Debug.Log($"텍스트 :{text}  ==  길이 :{text.Length} "); //체크용 출력 
            //}
        }
        catch (Exception e)
        {
            Debug.LogWarning($"파일 생성 실패  \r\n {e.Message}");
        }
        return textArray;
    }

}
