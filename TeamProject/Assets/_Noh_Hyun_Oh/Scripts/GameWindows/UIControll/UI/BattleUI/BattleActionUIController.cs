using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static BattleActionUI;

/// <summary>
/// 액션 UI Button들이 움직이는 기능
/// </summary>
public class BattleActionUIController : MonoBehaviour
{
    /// <summary>
    /// 움직일 버튼 배열 구하기 
    /// </summary>
    [SerializeField]
    RectTransform[] buttonRects;

    /// <summary>
    /// 버튼들이 열리면 크기조절 하기위해 필요한 렉투
    /// </summary>
    RectTransform controllerUI;

    /// <summary>
    /// 마우스 올렸을때 버튼이 펼쳐지는 속도 
    /// </summary>
    [SerializeField]
    private float viewSpeed = 1000.0f;

    /// <summary>
    /// 버튼들이 현재 위치값 계산용 
    /// </summary>
    float[] moveValues;

    /// <summary>
    /// 버튼들의 기본 높이값 구하기 
    /// </summary>
    private float viewHeight = 0.0f;

    /// <summary>
    /// 마우스 올렸을때 부드럽게 열리기위해 사용된 코루틴
    /// </summary>
    IEnumerator OnUpView;

    /// <summary>
    /// 마우스 올렸을때 부드럽게 닫히기 위해 사용된 코루틴
    /// </summary>
    IEnumerator OnDownView;

    /// <summary>
    /// 액션 버튼은 아무것도안하지만 버튼들 활성화될 때 이건 비활성화 시키는용으로 가져온다.
    /// </summary>
    GameObject actionButton;

    /// <summary>
    /// 위치 변경용 임시백터 두개.
    /// </summary>
    Vector2 tempMin = Vector2.zero;
    Vector2 tempMax = Vector2.zero;

    /// <summary>
    /// 부모가 활성화 상태인지 확인하는 프로퍼티
    /// </summary>
    bool IsParentVisible 
    {
        get 
        {
            bool isCheck = true;
            Transform parent = transform.parent;
            do
            {
                isCheck = parent.gameObject.activeSelf;
                parent = parent.parent;
            } while (parent != null && isCheck);

            return isCheck;
        }
    }
    private void Awake()
    {
       
        ///컨트롤러를 자식밑에 넣어보앗다.
        Transform parent = transform.parent; //그래서 부모를 찾고

        buttonRects = new RectTransform[parent.childCount - 2]; //Action버튼 과 마우스 핸들러 이벤트 오브젝트 두개 뺀나머지 가져오기
        moveValues = new float[parent.childCount - 2]; //진행값 담을 배열 선언하고
        for (int i = 0; i < buttonRects.Length; i++) //포문돌면서 
        {
            buttonRects[i] = parent.GetChild(i).GetComponent<RectTransform>(); // 찾아오고
            moveValues[i] = 0.0f; //값초기화
        }
        viewHeight = buttonRects[0].rect.height; //버튼의 높이들은 전부 동일하다는 전제 하에 버튼높이 가져오기  버튼높이 다다를경우 배열로 교체필요함.
        actionButton = parent.GetChild(parent.childCount-2).gameObject; // 찾아오고
        controllerUI = GetComponent<RectTransform>(); // 이것도 찾고
    }

    private void OnDisable()
    {
        ResetButtons();
    }

    /// <summary>
    /// 마우스 올라가면 배틀 액션버튼들 활성화 
    /// </summary>
    public void ViewButtons() 
    {
        actionButton.SetActive(false); //활성화전에 액션버튼가리고

        StopAllCoroutines(); // 활성화나 비활성화 상태 전부 멈추고 
        if (gameObject.activeSelf && IsParentVisible) //활성화 상태만 코루틴을 실행할수있다.
        {
            for (int i = 0; i < buttonRects.Length; i++)
            {
                OnUpView = SetUpView(i);    //개별로 담을경우 실행전에 담아줘야 한다. 
                StartCoroutine(OnUpView);   // 활성화 시작 
            }
        }
        else
        { 
            foreach (RectTransform rt in buttonRects)
            {
                SetTopBottomValue(rt, 0.0f); //위치 초기화
            }
        }

    }
    /// <summary>
    /// 마우스 벗어나면 배틀 액션버튼들 비활성화
    /// </summary>
    public void ResetButtons() 
    {
        StopAllCoroutines(); //마찬가지로 코루틴 다멈추고
        if (gameObject.activeSelf && IsParentVisible) //활성화 상태만 코루틴을 실행할수있다.
        {
            for (int i = 0; i < buttonRects.Length; i++)
            {
                OnDownView = SetDownView(i); //개별로 담을경우 실행전에 담아줘야 한다. 
                StartCoroutine(OnDownView);  //원래위치로 원복 로직 실행
            }
        }
        else 
        {
            foreach (RectTransform rt in buttonRects) 
            {
                SetTopBottomValue(rt, 0.0f); //위치 초기화
            }
        }
        actionButton.SetActive(true);  //원위치대면 액션버튼 활성화 
    
    }
    /// <summary>
    /// rectTransform 의 top 과 bottom 을 이용하여 위치 조절 시키는 로직
    /// </summary>
    /// <param name="index">버튼 순서</param>
    /// <returns></returns>
    private IEnumerator SetUpView(int index) 
    {
        if (buttonRects.Length > 0) 
        {
            
            //while문 조건을위해 
            float checkValue  = viewHeight * index; // 최대높이 만큼 잡아준다
            while (moveValues[index] <= checkValue)
            {
                moveValues[index] += Time.deltaTime * viewSpeed; //부드럽게 증가시키기위해 델타타임 누적시키고
                moveValues[index] = checkValue < moveValues[index] ? checkValue : moveValues[index]; //스피드를 너무높게잡으면 위치를벗어나는경우가생김으로 방지
                SetTopBottomValue(buttonRects[index], moveValues[index]);
                yield return null;
            }
        }
    }
    /// <summary>
    /// rectTransform 의 top bottom 을 이용하여 위치 조절 시키는 로직 
    /// </summary>
    /// <param name="index">버튼순서</param>
    /// <returns></returns>
    private IEnumerator SetDownView(int index) 
    {
        if (buttonRects.Length > 0)
        {

           
            while (moveValues[index] >= 0) //이값하고 
            {
                moveValues[index] -= Time.deltaTime * viewSpeed; //이값만 다르고 나머지 동일
                moveValues[index] = 0 > moveValues[index] ? 0 : moveValues[index]; //스피드를 너무높게잡으면 위치를벗어나는경우가생김으로 방지
                SetTopBottomValue(buttonRects[index], moveValues[index]);
                yield return null;
            }
            
        }
    }
    /// <summary>
    ///  rectTransform 의 top과 bottom 값을 
    ///  들어온인자값으로 적용한다  top 은 -인자값  bottom 은 인자값 그대로 
    ///<param name="value">적용할 값</param>
    ///<param name="rt">적용할 렉트트랜스폼</param>
    /// </summary>
    private void SetTopBottomValue(RectTransform rt , float value)
    {
        tempMin = rt.offsetMin; //bottom
        tempMax = rt.offsetMax; //top
        tempMin.y = value; //bottom 값은 양수값 그대로 들어간다
        tempMax.y = value; //top 값은 양수값 넣으면 음수값으로 변환되어 들어간다.
        rt.offsetMin = tempMin; //bottom
        rt.offsetMax = tempMax; //top
    }
    
}
